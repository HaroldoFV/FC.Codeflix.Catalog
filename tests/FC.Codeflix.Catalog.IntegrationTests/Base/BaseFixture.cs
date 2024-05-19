using Bogus;

namespace FC.Codeflix.Catalog.IntegrationTests.Base;

public class BaseFixture
{
    public Faker Faker { get; set; } = new("pt_BR");
    
    public bool GetRandomBoolean()
        => new Random().NextDouble() < 0.5;
}