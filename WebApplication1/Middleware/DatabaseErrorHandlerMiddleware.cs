using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Npgsql;
using System.Net;
using System.Threading.Tasks;
using WebApplication1.Models;
using System.Text.Json;

namespace WebApplication1.Middleware
{
    public class DatabaseErrorHandlerMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<DatabaseErrorHandlerMiddleware> _logger;

        public DatabaseErrorHandlerMiddleware(RequestDelegate next, ILogger<DatabaseErrorHandlerMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (PostgresException ex)
            {
                var response = ex.SqlState switch
                {
                    "23505" => new ApiResponse(409, "Duplicate key violation", new { Field = ex.ConstraintName }),
                    "23503" => new ApiResponse(400, "Foreign key violation", new { Constraint = ex.ConstraintName }),
                    "23502" => new ApiResponse(400, "Null value violation", new { Column = ex.ColumnName }),
                    "40001" => new ApiResponse(409, "Transaction conflict, please retry"),
                    _ => new ApiResponse(500, "Database error occurred", new { Error = ex.Message })
                };
                
                await WriteJsonResponse(context, response);
            }
            catch (DbUpdateException ex) when (ex.InnerException is PostgresException pgEx)
            {
                await HandlePostgresException(context, pgEx);
            }
        }

        private async Task HandlePostgresException(HttpContext context, PostgresException ex)
        {
            ApiResponse response = ex.SqlState switch
            {
                "23505" => new ApiResponse(409, "Duplicate key violation", new { Field = ex.ConstraintName }),
                "23503" => new ApiResponse(400, "Foreign key violation", new { Constraint = ex.ConstraintName }),
                "23502" => new ApiResponse(400, "Null value violation", new { Column = ex.ColumnName }),
                "40001" => new ApiResponse(409, "Transaction conflict, please retry"),
                _ => new ApiResponse(500, "Database error occurred", new { Error = ex.Message })
            };
            
            await WriteJsonResponse(context, response);
        }

        private static async Task WriteJsonResponse(HttpContext context, ApiResponse response)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = response.StatusCode;
            await context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }
    }
}
