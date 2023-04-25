using System.ComponentModel.Composition;

namespace NuGetSpy;
internal class MefAdapter<T>
    where T : new()
{
    public MefAdapter()
    {
        Export = new T();
    }

    [Export]
    public T Export
    {
        get;
    }
}
