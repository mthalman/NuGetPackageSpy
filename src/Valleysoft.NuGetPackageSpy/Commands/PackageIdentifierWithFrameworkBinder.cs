using NuGet.Frameworks;
using NuGetSpy.Core;
using System.CommandLine;
using System.CommandLine.Parsing;

namespace NuGetSpy.Commands;
internal class PackageIdentifierWithFrameworkBinder
{
    private readonly Option<string> _framework;
    private readonly PackageIdentifierBinder _packageIdentifierBinder;

    public PackageIdentifierWithFrameworkBinder(Argument<string> name, Argument<string> version, Option<string> framework)
    {
        _packageIdentifierBinder = new PackageIdentifierBinder(name, version);
        _framework = framework;
    }

    public PackageFramework GetBoundValue(ParseResult parseResult) =>
        new(_packageIdentifierBinder.GetBoundValue(parseResult), GetNuGetFramework(parseResult));

    private NuGetFramework? GetNuGetFramework(ParseResult parseResult)
    {
        string? fwk = parseResult.GetValueForOption(_framework);
        return fwk is null ? null : NuGetFramework.Parse(fwk);
    }
}
