using NuGet.Packaging.Core;

namespace NuGetSpy.Core;
public class PackageGraphNode
{
    public PackageGraphNode(PackageIdentity package)
    {
        Package = package;
    }

    public PackageIdentity Package { get; }
    public IList<PackageGraphNode> Dependencies { get; } = new List<PackageGraphNode>();
}
