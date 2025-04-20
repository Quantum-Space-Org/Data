using Quantum.Configurator;

namespace Quantum.DataBase.EntityFramework.Configurator;

public static class ConfigQuantumDatabaseBuilderExtensions
{
    public static ConfigQuantumDatabaseBuilder
        ConfigQuantumDatabase(this QuantumServiceCollection collection)
        => new(collection);
}