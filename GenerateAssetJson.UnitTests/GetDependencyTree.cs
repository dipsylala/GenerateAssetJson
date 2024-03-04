using GenerateAssetJson;

namespace GenerateAssetJson.UnitTests
{
    [TestFixture]
    public class Tests
    {

        [Test]
        public void Should_Parse_Valid_Packages_Config()
        {
            var sut = new GetDependencyTree();

            var list = sut.ParsePackageConfig("./artifacts/packages.config");
            Assert.That(actual: list, Has.Count.EqualTo(18));
        }

        [Test]
        public void Should_Call_Nuget_For_Dependencies()
        {
            var sut = new GetDependencyTree();

            var list = sut.GetNugetDependencies("Newtonsoft.Json", "12.0.3", "net45").Result;
            Assert.That(actual: list, Has.Count.GreaterThan(0));
        }
    }
}