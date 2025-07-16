using System.Text.Json;

namespace WebApplication1.Services
{
    public class QueryService
    {
        private readonly Dictionary<string, Dictionary<string, string>> _queries;

        public QueryService(string jsonPath)
        {
            var json = File.ReadAllText(jsonPath);
            _queries = JsonSerializer.Deserialize<Dictionary<string, Dictionary<string, string>>>(json)
                ?? throw new InvalidOperationException("Failed to load queries");
        }

        public string GetQuery(string category, string queryName)
        {
            if (_queries.TryGetValue(category, out var categoryQueries) && 
                categoryQueries.TryGetValue(queryName, out var query))
            {
                return query;
            }
            throw new KeyNotFoundException($"Query '{category}.{queryName}' not found");
        }
    }
}
