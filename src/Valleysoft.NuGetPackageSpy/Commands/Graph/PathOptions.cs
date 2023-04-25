using NuGet.Frameworks;
using NuGet.Packaging.Core;
using System.CommandLine;

namespace NuGetSpy.Commands.Graph;
internal class PathOptions : OptionsBase
{
    private readonly Argument<string> _frameworkArg;
    private readonly PackageIdentifierBinder _srcPackageIdentifierBinder;
    private readonly PackageIdentifierBinder _targetPackageIdentifierBinder;

    public NuGetFramework Framework { get; set; } = default!;
    public PackageIdentity SourcePackage { get; set; } = default!;
    public PackageIdentity TargetPackage { get; set; } = default!;

    public PathOptions()
    {
        _frameworkArg = Add(new Argument<string>("framework", "Target framework used to resolve dependencies"));

        Argument<string> srcPkgNameArg = new("source-name", "Name of the source package");
        Argument<string> srcPkgVerArg = new("source-version", "Version of the source package");
        _srcPackageIdentifierBinder = new PackageIdentifierBinder(Add(srcPkgNameArg), Add(srcPkgVerArg));

        Argument<string> targetPkgNameArg = new("target-name", "Name of the target package");
        Argument<string> targetPkgVerArg = new("target-version", "Version of the target package");
        _targetPackageIdentifierBinder = new PackageIdentifierBinder(Add(targetPkgNameArg), Add(targetPkgVerArg));
    }

    protected override void GetValues()
    {
        Framework = NuGetFramework.Parse(ParseResult.GetValueForArgument(_frameworkArg));
        SourcePackage = _srcPackageIdentifierBinder.GetBoundValue(ParseResult);
        TargetPackage = _targetPackageIdentifierBinder.GetBoundValue(ParseResult);
    }
}
