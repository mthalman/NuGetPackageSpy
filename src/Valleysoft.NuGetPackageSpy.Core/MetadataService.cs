using NuGet.Common;
using NuGet.Packaging.Core;
using NuGet.Protocol;
using NuGet.Protocol.Core.Types;
using System.ComponentModel.Composition;

namespace NuGetSpy.Core;

public interface IMetadataService
{
    Task<IPackageSearchMetadata> GetAsync(PackageIdentity packageId, CancellationToken cancellationToken = default);
}

[Export(typeof(IMetadataService))]
internal class MetadataService : IMetadataService
{
    private readonly SourceCacheContext _sourceCacheContext;

    [ImportingConstructor]
    public MetadataService(SourceCacheContext sourceCacheContext)
    {
        _sourceCacheContext = sourceCacheContext;
    }

    public async Task<IPackageSearchMetadata> GetAsync(PackageIdentity packageId, CancellationToken cancellationToken = default)
    {
        SourceRepository repository = Repository.Factory.GetCoreV3("https://api.nuget.org/v3/index.json");
        PackageMetadataResource resource = await repository.GetResourceAsync<PackageMetadataResource>();

        return await resource.GetMetadataAsync(packageId, _sourceCacheContext, NullLogger.Instance, cancellationToken) ??
            throw new Exception($"Package '{packageId}' is not found.");
    }
}
