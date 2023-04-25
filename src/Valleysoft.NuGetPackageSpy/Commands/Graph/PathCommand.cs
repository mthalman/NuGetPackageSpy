using NuGetSpy.Core;
using Spectre.Console;
using System.CommandLine;
using System.ComponentModel.Composition;

namespace NuGetSpy.Commands.Graph;

[Export(typeof(Command))]
[ExportMetadata("CommandType", nameof(GraphCommand))]
internal class PathCommand : CommandWithOptions<PathOptions>
{
    private readonly IGraphService _graphService;

    [ImportingConstructor]
    public PathCommand(IGraphService graphService)
        : base("path", "Calculates the path in a dependency graph between two packages")
    {
        _graphService = graphService;
    }

    protected override async Task ExecuteAsync()
    {
        var result = await _graphService.GetPathAsync(Options.Framework, Options.SourcePackage, Options.TargetPackage);

        if (result is not null)
        {
            Tree root = new(result.Package.ToString());
            GenerateConsoleTree(result, root);
            AnsiConsole.Write(root);
        }
        else
        {
            Console.WriteLine("Nothing found");
        }
    }

    private void GenerateConsoleTree(PackageGraphNode packageNode, IHasTreeNodes parent)
    {
        foreach (var dependency in packageNode.Dependencies)
        {
            GenerateConsoleTree(dependency, parent.AddNode(dependency.Package.ToString()));
        }
    }
}
