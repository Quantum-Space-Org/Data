using System.Threading.Tasks;

namespace Quantum.DataBase.EntityFramework.Interceptor;

public interface IDbContextInterceptor
{
    Task Start();
    Task Commit();
    Task RoleBack();
}