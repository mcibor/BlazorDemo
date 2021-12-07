using BlazorApp.Client;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

namespace BlazorApp
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebAssemblyHostBuilder.CreateDefault(args);
            builder.RootComponents.Add<App>("#app");
            builder.RootComponents.Add<HeadOutlet>("head::after");

            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

            if (builder.HostEnvironment.IsDevelopment())
            {
                builder.Logging.SetMinimumLevel(LogLevel.Debug);
                builder.Logging.AddFilter("Microsoft.*", LogLevel.Information);
            }   

            await builder.Build().RunAsync();

        }
    }
}
