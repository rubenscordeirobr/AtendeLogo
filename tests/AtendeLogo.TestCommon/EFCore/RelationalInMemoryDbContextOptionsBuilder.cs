﻿using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AtendeLogo.TestCommon.EFCore;

public class RelationalInMemoryDbContextOptionsBuilder : InMemoryDbContextOptionsBuilder,
    IRelationalDbContextOptionsBuilderInfrastructure
{
    public RelationalInMemoryDbContextOptionsBuilder(DbContextOptionsBuilder optionsBuilder)
        : base(optionsBuilder)
    {
    }

    DbContextOptionsBuilder IRelationalDbContextOptionsBuilderInfrastructure.OptionsBuilder
        => OptionsBuilder;
}
