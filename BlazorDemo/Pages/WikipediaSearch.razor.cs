using BlazorDemo.Clients;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BlazorDemo.Pages
{
    public partial class WikipediaSearchComponent : ComponentBase, IAsyncDisposable
    {
        [Inject]
        public IWikipediaApiClient WikipediaApiClient { get; set; }


        public WikipediaSearchQuery Model { get; set; } = new WikipediaSearchQuery();

        public List<WikipediaSearchResult> Result { get; private set; }

        [Inject]
        public IJSRuntime JS { get; set; }

        private IJSObjectReference module { get; set; }

        protected override async Task OnInitializedAsync()
        {
            module = await JS.InvokeAsync<IJSObjectReference>("import", "./Pages/WikipediaSearch.razor.js");
        }

        public async Task HandleSubmit()
        {
            await module.InvokeVoidAsync("myLog", "test JS interop message");

            Result = await WikipediaApiClient.OpenSearch(Model);
        }

        public async ValueTask DisposeAsync()
        {
            if (module is not null)
            {
                await module.DisposeAsync();
            }
        }
    }
}
