using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using CloudNotes.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");

// Change this to your server API base URL (adjust port if needed)
builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5182/") });

await builder.Build().RunAsync();
