using NuGet.Protocol;
using NuGetSpy.Core;
using System.CommandLine;
using System.ComponentModel.Composition;

namespace NuGetSpy.Commands.Package;
[Export(typeof(Command))]
[ExportMetadata("CommandType", nameof(PackageCommand))]
internal class InfoCommand : CommandWithOptions<InfoOptions>
{
    private readonly IMetadataService _metadataService;

    [ImportingConstructor]
    public InfoCommand(IMetadataService metadataService)
        : base("info", "Get details of a package")
    {
        _metadataService = metadataService;
    }

    protected override async Task ExecuteAsync()
    {
        var package = await _metadataService.GetAsync(Options.PackageIdentity);
        Console.WriteLine(package.ToJson(Newtonsoft.Json.Formatting.Indented));
        //Console.WriteLine($"Name: {package.Identity.Id}");
        //Console.WriteLine($"Version: {package.Identity.Version}");
        //Console.WriteLine($"Listed: {package.IsListed}");
        //Console.WriteLine($"Tags: {package.Tags}");
        //Console.WriteLine($"Description: {package.Description}");

        //Console.WriteLine("Dependencies:");
        //foreach (var set in package.DependencySets)
        //{
        //    Console.WriteLine($" - {set.TargetFramework}");
        //    foreach (var dep in set.Packages)
        //    {
        //        Console.WriteLine($"   - {dep.Id}");
        //    }
        //}
    }
}
