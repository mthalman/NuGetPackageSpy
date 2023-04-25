using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace NuGetSpy.Commands.Graph;

[Export(typeof(IBaseCommand))]
internal class GraphCommand : BaseCommand
{
    [ImportingConstructor]
    public GraphCommand(CompositionContainer compositionContainer)
        : base(compositionContainer, "graph", "Commands for computing NuGet package graphs")
    {
    }
}
