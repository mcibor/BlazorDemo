using BlazorDemo.Clients;
using Microsoft.AspNetCore.Components;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorDemo.Pages
{
    public partial class WikipediaSearchComponent : ComponentBase
    {
        [Inject]
        public IWikipediaApiClient WikipediaApiClient { get; set; }

        public string SearchQuery { get; set; }
        public List<WikipediaSearchResult> Result { get; private set; }

        protected override void OnInitialized()
        {
        }

        public async Task Search()
        {
            Result = await WikipediaApiClient.OpenSearch(SearchQuery);

        }
    }
}
