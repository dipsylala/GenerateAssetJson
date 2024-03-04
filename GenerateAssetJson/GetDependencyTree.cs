using NuGet.Common;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using System.Xml.Linq;

namespace GenerateAssetJson
{
    public class GetDependencyTree
    {
        public List<PackageReference> ParsePackageConfig (string packageConfigPath)
        {
            var document = XDocument.Load(packageConfigPath);
            var packages = document.Descendants("package")
                .Select(x => new PackageReference
                {
                    Id = x.Attribute("id").Value,
                    Version = x.Attribute("version").Value,
                    TargetFramework = x.Attribute("targetFramework").Value
                })
                .ToList();

            return packages;
        }

        public async Task<List<NugetDependency>> GetNugetDependencies (string packageId, string version, string targetFramework)
        {
            var cache = new SourceCacheContext();
            var repositories = Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");

            var resource = await repositories.GetResourceAsync<FindPackageByIdResource>();
            var dependencyInfo = await resource.GetDependencyInfoAsync(packageId, new NuGet.Versioning.NuGetVersion(version), cache, NullLogger.Instance, CancellationToken.None);

            var dependencies = dependencyInfo.DependencyGroups.SelectMany(group => group.Packages)
                .Select(dep => new NugetDependency { Id = dep.Id, VersionRange = dep.VersionRange.ToString() })
                .ToList();

            return dependencies;

        }
    }
}
