using AtendeLogo.TestCommon.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AtendeLogo.TestCommon.EFCore;

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
