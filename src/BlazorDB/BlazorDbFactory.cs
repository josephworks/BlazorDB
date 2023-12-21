using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.JSInterop;

namespace BlazorDB;

public class BlazorDbFactory(IServiceProvider serviceProvider, IJSRuntime jSRuntime) : IBlazorDbFactory
{
    private readonly IDictionary<string, IndexedDbManager> _dbs = new Dictionary<string, IndexedDbManager>();

    public async Task<IndexedDbManager> GetDbManager(string dbName)
    {
        if(!_dbs.Any())
            await BuildFromServices();
        return _dbs.TryGetValue(dbName, out var db) ? db : null;
    }

    public Task<IndexedDbManager> GetDbManager(DbStore dbStore)
        => GetDbManager(dbStore.Name);

    /// <summary>
    /// Gets DbStore objects from service provider. Uses information users configured in program.cs.  
    /// </summary>
    /// <returns></returns>
    async Task BuildFromServices()
    {
        var dbStores = serviceProvider.GetServices<DbStore>();
        foreach(var dbStore in dbStores)
        {
            Console.WriteLine($"{dbStore.Name}{dbStore.Version}{dbStore.StoreSchemas.Count}");
            var db = new IndexedDbManager(dbStore, jSRuntime);
            await db.OpenDb();
            _dbs.Add(dbStore.Name, db);
        }
    }
}