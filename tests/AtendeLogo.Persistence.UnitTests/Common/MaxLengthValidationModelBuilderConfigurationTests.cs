using AtendeLogo.Domain.Primitives;
using AtendeLogo.Persistence.Common.Configurations;
using AtendeLogo.Persistence.Common.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;


namespace AtendeLogo.Persistence.UnitTests.Common;

public class MaxLengthValidationModelBuilderConfigurationTests

{
    [Fact]
    public void EntityBuilderConfiguration_ShouldTrowMaxLengthNotDefinedException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<MaxLengthTestDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        // Act
        Action act = () =>
        {
            using var context = new MaxLengthTestDbContext(options);
            context.Model.FindEntityType(typeof(MaxLengthTestEntity));
        };

        // Assert
        act.Should()
            .Throw<MaxLengthNotDefinedException>();

    }

    public class MaxLengthTestDbContext : DbContext
    {
        public DbSet<MaxLengthTestEntity> MaxLengthTestEntities { get; set; }

        public MaxLengthTestDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureModelDefaultConfiguration<MaxLengthTestDbContext>();
            base.OnModelCreating(modelBuilder);
        }
    }

    public class MaxLengthTestEntity : EntityBase
    {
        public required string Name { get; set; }
    }
}
