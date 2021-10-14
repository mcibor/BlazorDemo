using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace BlazorDemo.Clients
{
    public interface IWikipediaApiClient
    {
        Task<List<WikipediaSearchResult>> OpenSearch(string query);
    }

    public class WikipediaApiClient : IWikipediaApiClient
    {
        private readonly HttpClient _http;
        private readonly ILogger<WikipediaApiClient> _logger;

        public WikipediaApiClient(HttpClient http, ILogger<WikipediaApiClient> logger)
        {
            _http = http;
            _logger = logger;
        }

        public async Task<List<WikipediaSearchResult>> OpenSearch(string query)
        {
            using var httpResponse = await _http.GetAsync(
                "?action=opensearch&format=json&origin=" + Uri.EscapeDataString("*") +
                "&search=" + Uri.EscapeDataString(query));

            if (httpResponse.StatusCode != HttpStatusCode.OK)
                throw new InvalidOperationException(httpResponse.ToString());

            var content = await httpResponse.Content.ReadAsStringAsync();
            using var jsonResult = JsonDocument.Parse(content);
            var articles = jsonResult.RootElement[1];
            var links = jsonResult.RootElement[3];

            var searchResults = articles.EnumerateArray()
                .Select((x, i) => new WikipediaSearchResult
                {
                    Article = x.GetString(),
                    Uri = links[i].GetString()
                })
                .ToList();

            return searchResults;
        }

    }

    public class WikipediaSearchResult
    {
        public string Article { get; set; }
        public string Uri { get; set; }
    }
}
