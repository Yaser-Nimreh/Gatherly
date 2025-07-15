namespace Domain.Abstractions;

public interface IDataSeeder
{
    Task SeedAsync();
}