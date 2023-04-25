using NuGetSpy;
using NuGetSpy.Commands;
using System.CommandLine;
using System.ComponentModel.Composition.Hosting;

CompositionContainer compositionContainer = new(new TypeCatalog(typeof(AppCompositionContainer)));
compositionContainer = compositionContainer.GetExportedValue<CompositionContainer>()!;

RootCommand rootCmd = new("CLI for querying NuGet information");
var vals = compositionContainer.GetExportedValues<IBaseCommand>();
foreach (IBaseCommand baseCmd in vals)
{
    rootCmd.AddCommand(baseCmd.Command);
}

return rootCmd.Invoke(args);
