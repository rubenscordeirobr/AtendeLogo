using AtendeLogo.Application.UnitTests.Mocks.Extensions;
using AtendeLogo.Persistence.Common.Configurations;
using AtendeLogo.Persistence.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AtendeLogo.Application.UnitTests.Mocks.EFCore;

public class InMemoryIdentityDbContextModelCustomizer : IModelCustomizer
{
    public void Customize(ModelBuilder modelBuilder, DbContext context)
    {
        //TODO in memory database configuration
        if (context is IdentityDbContext identityDb)
        {
            modelBuilder
                .ConfigureModelDefaultConfiguration<IdentityDbContext>(isInMemory: true)
                .ConfigureInMemoryEntities();
        }
    }
}
