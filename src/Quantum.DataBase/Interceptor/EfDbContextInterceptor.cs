using System;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Quantum.DataBase.EntityFramework.Interceptor;

public class EfDbContextInterceptor : IDbContextInterceptor
{
    private readonly QuantumDbContext _quantumDbContext;

    public EfDbContextInterceptor(QuantumDbContext quantumDbContext)
        => _quantumDbContext = quantumDbContext;

    public Task Start()
    {
        //
        return Task.CompletedTask;
    }

    public async Task Commit()
        => await _quantumDbContext.SaveChangesAsync();

    public async Task RoleBack()
    {
        var context = _quantumDbContext;
        var changedEntries = context.ChangeTracker.Entries()
            .Where(x => x.State != EntityState.Unchanged).ToList();

        foreach (var entry in changedEntries)
        {
            switch (entry.State)
            {
                case EntityState.Modified:
                    entry.CurrentValues.SetValues(entry.OriginalValues);
                    entry.State = EntityState.Unchanged;
                    break;
                case EntityState.Added:
                    entry.State = EntityState.Detached;
                    break;
                case EntityState.Deleted:
                    entry.State = EntityState.Unchanged;
                    break;
                case EntityState.Detached:
                case EntityState.Unchanged:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }
    }
}