using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Quantum.DataBase.EntityFramework;

public class QuantumDbContext : DbContext
{
    private readonly DbContextConfig _config;

    public QuantumDbContext(DbContextConfig config)
       : base(config.OptionsBuilder.Options)
    {
        _config = config;

        _config.OnIniDbContext?.Invoke(this);
    }

    public DbSet<T> Get<T>() where T : class
    {
        var dbSet = Set<T>();
        return dbSet;
    }

    public object Get(Type type)
    {
        var dbContextType = GetType();
        var dbSetMethodInfo = dbContextType.GetMethods().FirstOrDefault(_ => _.Name == "Set");

        var dbSet = dbSetMethodInfo.MakeGenericMethod(type).Invoke(this, null);

        return dbSet;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        if (_config.Assemblies is null || _config.Assemblies.Length == 0)
        {
            base.OnModelCreating(modelBuilder);
            return;
        }

        var entityMethod = (MethodInfo)typeof(ModelBuilder)
            .GetMethod("Entity", new Type[] { });

        foreach (var assembly in _config.Assemblies)
        {
            var loadedAssemblyExportedTypes = LoadedAssemblyExportedTypes(assembly);

            var exportedDbSetTypes = ExportedDbSetTypes(loadedAssemblyExportedTypes);

            if (exportedDbSetTypes != null)
                foreach (var type in (from type in exportedDbSetTypes
                             where type.BaseType is System.Object
                             select type))
                {
                    entityMethod.MakeGenericMethod(type)
                        .Invoke(modelBuilder, new object[] { });
                }
        }

        base.OnModelCreating(modelBuilder);

        IEnumerable<Type> LoadedAssemblyExportedTypes(Assembly assembly)
        {
            var loadedAssembly = Assembly.Load(new AssemblyName(assembly.FullName));
            var loadedAssemblyExportedTypes = loadedAssembly.ExportedTypes;
            return loadedAssemblyExportedTypes;
        }

         IEnumerable<Type> ExportedDbSetTypes(IEnumerable<Type> loadedAssemblyExportedTypes)
        {
            IEnumerable<Type> exportedDbSetTypes;

            if (_config.FilterDbSetTypesFunction is not null)
            {
                exportedDbSetTypes = _config.FilterDbSetTypesFunction.Invoke(loadedAssemblyExportedTypes);
            }
            else exportedDbSetTypes = loadedAssemblyExportedTypes
                .Where(t => t
                                .GetCustomAttributes()
                                .Any(a=>a.GetType() == typeof(ItIsADbEntityAttribute)));

            return exportedDbSetTypes;
        }
    }
}
