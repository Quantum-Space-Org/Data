using System;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace Quantum.DataBase.EntityFramework;

public class DbContextConfig
{
    public Assembly[] Assemblies { get; set; }
    public DbContextOptionsBuilder<QuantumDbContext> OptionsBuilder { get; set; }
    public string Application { get; set; }
    public string DefaultSchema { get; set; } = "db";
    public Action<QuantumDbContext> OnIniDbContext { get; set; }
    public Func<IEnumerable<Type>, IEnumerable<Type>> FilterDbSetTypesFunction { get; set; }

    public DbContextConfig(DbContextOptionsBuilder<QuantumDbContext> optionsBuilder)
    {
        OptionsBuilder = optionsBuilder;
    }
    
    public DbContextConfig(DbContextOptionsBuilder<QuantumDbContext>
            optionsBuilder, params Assembly[] assemblies)
    {
        OptionsBuilder = optionsBuilder;
        Assemblies = assemblies;
    }
}