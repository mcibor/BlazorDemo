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

        public WikipediaSearchQuery Model { get; set; } = new WikipediaSearchQuery();

        public List<WikipediaSearchResult> Result { get; private set; }

        protected override void OnInitialized()
        {
        }

        public async Task HandleSubmit()
        {
            Result = await WikipediaApiClient.OpenSearch(Model);

        }
    }
}
