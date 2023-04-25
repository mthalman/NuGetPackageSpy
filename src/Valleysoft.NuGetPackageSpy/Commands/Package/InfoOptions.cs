using NuGet.Packaging.Core;

namespace NuGetSpy.Commands.Package;

internal class InfoOptions : OptionsBase
{
    private readonly PackageIdentifierBinder _packageIdentifierBinder;

    public PackageIdentity PackageIdentity { get; set; } = default!;

    public InfoOptions()
    {
        _packageIdentifierBinder =
            new PackageIdentifierBinder(Add(PackageIdentifierBinder.PackageNameArg), Add(PackageIdentifierBinder.PackageVersionArg));
    }

    protected override void GetValues()
    {
        PackageIdentity = _packageIdentifierBinder.GetBoundValue(ParseResult);
    }
}
