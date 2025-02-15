﻿using AtendeLogo.Domain.Primitives;
using AtendeLogo.Persistence.Common.Configurations;
using AtendeLogo.Persistence.Common.Exceptions;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;


namespace AtendeLogo.Persistence.UnitTests.Common;

public class EnumNotMappedValidationModelBuilderConfigurationTests

{
    [Fact]
    public void EntityBuilderConfiguration_ShouldTrowEnumTypeNotMappedException()
    {
        // Arrange
        var options = new DbContextOptionsBuilder<EnumTestDbContext>()
            .UseInMemoryDatabase(databaseName: "TestDb")
            .Options;

        // Act
        Action act = () =>
        {
            using var context = new EnumTestDbContext(options);
            context.Model.FindEntityType(typeof(EnumTestEntity));
        };

        // Assert
        act.Should()
            .Throw<EnumTypeNotMappedException>();

    }

    public class EnumTestDbContext : DbContext
    {
        public DbSet<EnumTestEntity> MaxLengthTestEntities { get; set; }

        public EnumTestDbContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ConfigureModelDefaultConfiguration<EnumTestDbContext>();
            base.OnModelCreating(modelBuilder);
        }
    }

    public class EnumTestEntity : EntityBase
    {
        public required TestEnum Name { get; set; }
    }

    public enum TestEnum
    {
        Value1,
        Value2,
        Value3
    }
}
