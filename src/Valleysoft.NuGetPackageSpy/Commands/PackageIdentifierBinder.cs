using NuGet.Packaging.Core;
using NuGet.Versioning;
using System.CommandLine;
using System.CommandLine.Parsing;

namespace NuGetSpy.Commands;
internal class PackageIdentifierBinder
{
    private readonly Argument<string> _name;
    private readonly Argument<string> _version;

    public static Argument<string> PackageNameArg => new("name", "Name of the package");
    public static Argument<string> PackageVersionArg => new("version", "Version of the package");

    public PackageIdentifierBinder(Argument<string> name, Argument<string> version)
    {
        _name = name;
        _version = version;
    }

    public PackageIdentity GetBoundValue(ParseResult parseResult) =>
        new(
            parseResult.GetValueForArgument(_name),
            GetNuGetVersion(parseResult));

    private NuGetVersion GetNuGetVersion(ParseResult parseResult)
    {
        string arg = parseResult.GetValueForArgument(_version);
        if (!NuGetVersion.TryParse(arg, out NuGetVersion ver))
        {
            throw new Exception($"The value for option '{_version.Name}' is not a valid NuGet version: '{arg}'");
        }

        return ver;
    }
}
