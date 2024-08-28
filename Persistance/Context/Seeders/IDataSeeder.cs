using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace MyAtelier.DAL.Context.Seeders;

public interface IDataSeeder<T> where T : class
{
    void Seed(EntityTypeBuilder<T> entityTypeBuilder);
}