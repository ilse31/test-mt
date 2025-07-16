using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using WebApplication1.Models;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace WebApplication1.Data;

public class ApplicationDbContext : DbContext, IDisposable
{
    private readonly ILogger<ApplicationDbContext>? _logger;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options, 
        ILogger<ApplicationDbContext>? logger = null)
        : base(options)
    {
        _logger = logger;
    }

    #region DbSets
    public DbSet<Player> Players { get; set; } = null!;
    #endregion

    #region Configuration
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder
            .EnableDetailedErrors()
            .EnableSensitiveDataLogging();
            
        base.OnConfiguring(optionsBuilder);
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
        
        base.OnModelCreating(modelBuilder);
    }
    #endregion

    #region Transactions
    public async Task ExecuteInTransactionAsync(Func<Task> operation, 
        CancellationToken cancellationToken = default)
    {
        await using var transaction = await Database.BeginTransactionAsync(cancellationToken);
        try
        {
            await operation();
            await transaction.CommitAsync(cancellationToken);
        }
        catch (Exception ex)
        {
            _logger?.LogError(ex, "Transaction failed");
            await transaction.RollbackAsync(cancellationToken);
            throw;
        }
    }
    #endregion

    #region Disposal
    private bool _disposed;

    protected virtual void Dispose(bool disposing)
    {
        if (!_disposed)
        {
            if (disposing)
            {
                _logger?.LogInformation("DbContext disposed");
            }
            _disposed = true;
        }
    }

    public override void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }
    #endregion
}
