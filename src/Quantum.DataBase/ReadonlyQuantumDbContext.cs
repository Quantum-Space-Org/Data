using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Quantum.DataBase.EntityFramework;

public sealed class ReadonlyQuantumDbContext : QuantumDbContext
{
    public new IQueryable<T> Get<T>() where T : class
        => Set<T>().AsNoTracking();

    public new IQueryable<T> Set<T>() where T : class
    {
        var dbSet = base.Set<T>().AsNoTracking();
        return dbSet;
    }

    public ReadonlyQuantumDbContext(
        DbContextConfig config)
        : base(config)
    {
        ChangeTracker.QueryTrackingBehavior = QueryTrackingBehavior.NoTracking;
    }

   
}