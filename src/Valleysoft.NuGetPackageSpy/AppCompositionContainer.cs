using NuGet.Protocol.Core.Types;
using NuGetSpy.Core;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace NuGetSpy;
[Export(typeof(CompositionContainer))]
internal class AppCompositionContainer : CompositionContainer
{
    public AppCompositionContainer()
        : base(CreateCatalog())
    {
    }

    private static AggregateCatalog CreateCatalog()
    {
        AggregateCatalog catalog = new();
        catalog.Catalogs.Add(new AssemblyCatalog(typeof(Program).Assembly));
        catalog.Catalogs.Add(new AssemblyCatalog(typeof(IGraphService).Assembly));
        catalog.Catalogs.Add(new TypeCatalog(typeof(MefAdapter<SourceCacheContext>)));

        return catalog;
    }
}
