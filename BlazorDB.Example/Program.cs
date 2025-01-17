using BlazorDB.Example;
using BlazorDB;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddBlazorDB(options =>
{
    options.Name = "Test";
    options.Version = 1;
    options.StoreSchemas =
    [
        new StoreSchema
        {
            Name = "Person",
            PrimaryKey = "id",
            PrimaryKeyAuto = true,
            UniqueIndexes = ["guid"],
            Indexes = ["name"]
        }
    ];
});
builder.Services.AddBlazorDB(options =>
{
    options.Name = "Test2";
    options.Version = 1;
    options.StoreSchemas =
    [
        new StoreSchema
        {
            Name = "Item",
            PrimaryKey = "id",
            PrimaryKeyAuto = true,
            UniqueIndexes = ["guid"],
            Indexes = ["name"]
        }
    ];
});

await builder.Build().RunAsync();
