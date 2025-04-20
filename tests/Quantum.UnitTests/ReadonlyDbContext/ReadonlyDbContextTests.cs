using System.Data.Common;
using System.Threading.Tasks;
using FluentAssertions;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;
using Quantum.DataBase.EntityFramework;
using Xunit;

namespace Quantum.UnitTests.ReadonlyDbContext;

public class ReadonlyDbContextTests
{ 
    [Fact]
    public async Task register_db_entities_automatically()
    {
        var dbContextConfig = new DbContextConfig(QuantumDbContextOptions4Sqlite
            , this.GetType().Assembly);

        var readonlyQuantumDbContext = new ReadonlyQuantumDbContext(dbContextConfig);

        var myDbEntities = readonlyQuantumDbContext.Get<MyDbEntity>();

        myDbEntities.Should().NotBeNull();

        readonlyQuantumDbContext.ChangeTracker
            .QueryTrackingBehavior.Should().Be(QueryTrackingBehavior.NoTracking);

    }
        
    public static DbContextOptionsBuilder<QuantumDbContext> QuantumDbContextOptions4Sqlite =>
        new DbContextOptionsBuilder<QuantumDbContext>()
            .UseSqlite(CreateInMemoryDatabase());

    private static DbConnection CreateInMemoryDatabase()
    {
        var connection = new SqliteConnection("Filename=:memory:");

        connection.Open();

        return connection;
    }
}