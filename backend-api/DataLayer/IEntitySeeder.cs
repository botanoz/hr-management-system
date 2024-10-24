using Microsoft.EntityFrameworkCore;

namespace Hr.DL
{
    public interface IEntitySeeder
    {
        void SeedData(ModelBuilder modelBuilder);
    }
}