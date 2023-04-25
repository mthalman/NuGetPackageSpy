using NuGet.Frameworks;
using NuGet.Packaging;
using NuGet.Packaging.Core;
using NuGet.Protocol.Core.Types;
using System.ComponentModel.Composition;

namespace NuGetSpy.Core;

public interface IGraphService
{
    Task<PackageGraphNode?> GetPathAsync(
        NuGetFramework framework, PackageIdentity srcPackage, PackageIdentity targetPackage, CancellationToken cancellationToken = default);
}

[Export(typeof(IGraphService))]
internal class GraphService : IGraphService
{
    private readonly IMetadataService _metadataService;
    private readonly FrameworkReducer _frameworkReducer = new();

    [ImportingConstructor]
    public GraphService(IMetadataService metadataService)
    {
        _metadataService = metadataService;
    }

    public Task<PackageGraphNode?> GetPathAsync(
        NuGetFramework framework, PackageIdentity srcPackage, PackageIdentity targetPackage, CancellationToken cancellationToken = default) =>
        GetPathAsync(framework, srcPackage, targetPackage, new Dictionary<PackageIdentity, PackageGraphNode>(), cancellationToken);

    private async Task<PackageGraphNode?> GetPathAsync(
        NuGetFramework framework,
        PackageIdentity srcPackage,
        PackageIdentity targetPackage,
        Dictionary<PackageIdentity, PackageGraphNode> processedPackages,
        CancellationToken cancellationToken)
    {
        // https://learn.microsoft.com/en-us/nuget/concepts/dependency-resolution
        //DefaultCompatibilityProvider.Instance.IsCompatible()

        if (processedPackages.TryGetValue(srcPackage, out PackageGraphNode? srcNode))
        {
            return srcNode.Dependencies.Any() ? srcNode : null;
        }

        IPackageSearchMetadata srcPkgMetadata = await _metadataService.GetAsync(srcPackage, cancellationToken);

        PackageDependencyGroup? dependencies = ResolvePackageDependencyGroup(srcPkgMetadata, framework);
        if (dependencies is null)
        {
            return null;
        }

        PackageGraphNode result = new(srcPackage);
        bool isMatch = IsTargetDependency(targetPackage, dependencies);
        if (isMatch)
        {
            result.Dependencies.Add(new PackageGraphNode(targetPackage));
        }
        else
        {
            List<PackageGraphNode> dependencyNodes = new();

            foreach (var pkg in dependencies.Packages)
            {
                if (pkg.VersionRange.IsMinInclusive)
                {
                    if (pkg.VersionRange.IsFloating)
                    {
                        throw new NotSupportedException($"Package {srcPackage} has a dependency on {pkg.Id} with a floating version range: {pkg.VersionRange}");
                    }

                    PackageIdentity newSrc = new(pkg.Id, pkg.VersionRange.MinVersion);
                    var node = await GetPathAsync(framework, newSrc, targetPackage, processedPackages, cancellationToken);
                    if (node is not null)
                    {
                        dependencyNodes.Add(node);
                    }
                }
                else
                {
                    // An exclusive minimum version requires searching for the next available version.
                    throw new NotSupportedException($"Package {srcPackage} has a dependency on {pkg.Id} with an exclusive minimum version: {pkg.VersionRange}");
                }
            }

            result.Dependencies.AddRange(dependencyNodes);
            processedPackages.Add(srcPackage, result);
        }

        if (result.Dependencies.Any())
        {
            return result;
        }
        else
        {
            return null;
        }
    }

    private PackageDependencyGroup? ResolvePackageDependencyGroup(IPackageSearchMetadata pkgMetadata, NuGetFramework framework)
    {
        NuGetFramework nearestTarget = _frameworkReducer.GetNearest(framework, pkgMetadata.DependencySets.Select(ds => ds.TargetFramework));
        return pkgMetadata.DependencySets.FirstOrDefault(ds => ds.TargetFramework == nearestTarget);
    }

    private static bool IsTargetDependency(PackageIdentity targetPackage, PackageDependencyGroup dependencyGroup) =>
        dependencyGroup.Packages.Any(pkg => pkg.Id == targetPackage.Id && pkg.VersionRange.Satisfies(targetPackage.Version));
}
