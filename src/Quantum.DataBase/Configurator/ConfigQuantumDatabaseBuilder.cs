using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Quantum.Configurator;
using Quantum.DataBase.EntityFramework.Interceptor;

namespace Quantum.DataBase.EntityFramework.Configurator;

public class ConfigQuantumDatabaseBuilder
{
    private readonly QuantumServiceCollection _quantumServiceCollection;

    public ConfigQuantumDatabaseBuilder(QuantumServiceCollection quantumServiceCollection)
        => _quantumServiceCollection = quantumServiceCollection;

    public ConfigQuantumDatabaseBuilder AddQuantumDbContext<T>(ServiceLifetime serviceLifetime)
        where T : QuantumDbContext
    {
        _quantumServiceCollection.Collection.Add(new ServiceDescriptor(typeof(QuantumDbContext), typeof(T), serviceLifetime));
        return this;
    }

    public ConfigQuantumDatabaseBuilder AddReadonlyQuantumDbContext<T>(ServiceLifetime serviceLifetime
        , Assembly assembly)
        where T : DbContext
    {
        var dbContextConfig = new DbContextConfig(null, assembly);

        _quantumServiceCollection.Collection.Add(new ServiceDescriptor(typeof(ReadonlyQuantumDbContext), typeof(T), serviceLifetime));
        return this;
    }

    public ConfigQuantumDatabaseBuilder WithOptions(DbContextOptions<QuantumDbContext> options)
    {
        _quantumServiceCollection.Collection.AddSingleton(sp => options);

        return this;
    }

    public ConfigQuantumDatabaseBuilder RegisterRepositoriesInAssembly(Assembly assembly)
    {
        const string repoPostfix = "Repository";

        var types = assembly.GetTypes().Where(t => t.Name.EndsWith(repoPostfix)
                                                   && t.GetInterfaces() != null
                                                   && t.IsClass);

        foreach (var type in types)
            _quantumServiceCollection.Collection.AddTransient(type.GetInterfaces()[0], type);

        return this;
    }

    public ConfigQuantumDatabaseBuilder RegisterDbContextInterceptorAsScoped<T>()
        where T : class, IDbContextInterceptor
    {
        _quantumServiceCollection.Collection.AddScoped<IDbContextInterceptor, T>();

        return this;
    }

    public QuantumServiceCollection and()
        => _quantumServiceCollection;
}