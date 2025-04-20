using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Quantum.Domain;

namespace Quantum.DataBase.EntityFramework.Repository;

public class AbstractRepository<TAggregateRoot, TAggregateRootId> :
    IAbstractRepository<TAggregateRoot, TAggregateRootId>
    where TAggregateRoot : IsAnAggregateRoot<TAggregateRootId>
    where TAggregateRootId : IsAnIdentity<TAggregateRootId>
{
    private readonly QuantumDbContext _quantumDbContext;

    public AbstractRepository(QuantumDbContext quantumDbContext)
        => _quantumDbContext = quantumDbContext;
    public async Task Add(TAggregateRoot aggregateRoot)
        => await GetDbSet().AddAsync(aggregateRoot);

    public async Task Update(TAggregateRoot aggregateRoot)
        => GetDbSet().Update(aggregateRoot);
    public async Task Delete(TAggregateRoot aggregateRoot)
        => GetDbSet().Remove(aggregateRoot);

    public async Task<TAggregateRoot> Fetch(Expression<Func<TAggregateRoot, bool>> predicate)
        => await GetDbSet().FirstOrDefaultAsync(predicate);

    private DbSet<TAggregateRoot> GetDbSet()
        => _quantumDbContext.Get<TAggregateRoot>();
}