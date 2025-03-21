using AtendeLogo.TestCommon.Extensions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;

namespace AtendeLogo.TestCommon.EFCore;

public class InMemoryIdentityDbContextModelCustomizer : IModelCustomizer
{
    public void Customize(ModelBuilder modelBuilder, DbContext context)
    {
        if (context is IdentityDbContext _)
        {
            modelBuilder
                .ConfigureModelDefaultConfiguration<IdentityDbContext>(isInMemory: true)
                .ConfigureInMemoryEntities();
        }
    }
}
