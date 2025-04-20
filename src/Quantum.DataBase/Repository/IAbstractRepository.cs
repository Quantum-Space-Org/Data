using System;
using System.Linq.Expressions;
using System.Threading.Tasks;
using Quantum.Domain;

namespace Quantum.DataBase.EntityFramework.Repository;

public interface IAbstractRepository<TAggregateRoot, TAggregateRootId>
    where TAggregateRoot : IsAnAggregateRoot<TAggregateRootId>
    where TAggregateRootId : IsAnIdentity<TAggregateRootId>
{
    Task Add(TAggregateRoot aggregateRoot);
    Task Update(TAggregateRoot aggregateRoot);
    Task Delete(TAggregateRoot aggregateRoot);
    Task<TAggregateRoot> Fetch(Expression<Func<TAggregateRoot, bool>> predicate);
}