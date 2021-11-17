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
        Task<List<WikipediaSearchResult>> OpenSearch(WikipediaSearchQuery query);
    }

    public class WikipediaApiClient : IWikipediaApiClient
    {
        private const string _baseAddress = "https://{0}.wikipedia.org/w/api.php";
        
        private readonly static Dictionary<WikipediaSearchLanguage, string> _languagePrefixes = new() 
        {
            { WikipediaSearchLanguage.English, "en" }, 
            { WikipediaSearchLanguage.Polish, "pl" } 
        };

        private readonly HttpClient _http;

        public WikipediaApiClient(HttpClient http)
        {
            _http = http;
        }

        private static string GetBaseAddress(WikipediaSearchLanguage language) => string.Format(_baseAddress, _languagePrefixes[language]);

        public async Task<List<WikipediaSearchResult>> OpenSearch(WikipediaSearchQuery queryDto)
        {
            using var httpResponse = await _http.GetAsync(
                GetBaseAddress(queryDto.Language) +
                "?action=opensearch&format=json&origin=" + Uri.EscapeDataString("*") +
                "&search=" + Uri.EscapeDataString(queryDto.Query));

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

    public class WikipediaSearchQuery
    {
        public string Query { get; set; }
        public WikipediaSearchLanguage Language { get; set; }
    }

    public enum WikipediaSearchLanguage
    {
        English,
        Polish
    }

    public class WikipediaSearchResult
    {
        public string Article { get; set; }
        public string Uri { get; set; }
    }
}
