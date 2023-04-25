using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace NuGetSpy.Commands.Package;

[Export(typeof(IBaseCommand))]
internal class PackageCommand : BaseCommand
{
    [ImportingConstructor]
    public PackageCommand(CompositionContainer compositionContainer)
        : base(compositionContainer, "package", "Commands for querying a package")
    {
    }
}
