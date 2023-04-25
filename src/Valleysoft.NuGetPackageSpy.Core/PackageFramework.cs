using NuGet.Frameworks;
using NuGet.Packaging.Core;

namespace NuGetSpy.Core;
public class PackageFramework : IEquatable<PackageFramework>
{
    public PackageFramework(PackageIdentity packageId, NuGetFramework? framework)
    {
        PackageId = packageId;
        Framework = framework;
    }
    public PackageIdentity PackageId { get; }
    public NuGetFramework? Framework { get; }

    public bool Equals(PackageFramework? other)
    {
        if (other is null)
        {
            return false;
        }

        return PackageId == other.PackageId && Framework == other.Framework;
    }

    public override bool Equals(object? obj)
    {
        return Equals(obj as PackageFramework);
    }

    public override int GetHashCode()
    {
        return (PackageId.ToString() + Framework?.ToString()).GetHashCode();
    }
}
