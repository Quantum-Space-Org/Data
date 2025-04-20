using System.Threading.Tasks;

namespace Quantum.DataBase.EntityFramework.Interceptor;

public class NullIDbContextInterceptor : IDbContextInterceptor
{
    public static NullIDbContextInterceptor New()
        => new NullIDbContextInterceptor();

    public Task Start() => Task.CompletedTask;

    public Task Commit() => Task.CompletedTask;

    public Task RoleBack() => Task.CompletedTask;
}