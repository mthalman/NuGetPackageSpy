using System.CommandLine;
using System.ComponentModel.Composition;
using System.ComponentModel.Composition.Hosting;

namespace NuGetSpy.Commands;
internal interface IBaseCommand
{
    Command Command { get; }
}

internal abstract class BaseCommand : Command, IBaseCommand
{
    protected BaseCommand(CompositionContainer compositionContainer, string name, string? description = null)
        : base(name, description)
    {
        
        IEnumerable<Command> subCommands = compositionContainer.GetExports<Command, IDictionary<string, object>>()
            .Where(e => e.Metadata["CommandType"].ToString() == GetType().Name)
            .Select(e => e.Value);
        foreach (var subCommand in subCommands)
        {
            AddCommand(subCommand);
        }
    }

    Command IBaseCommand.Command => this;
}
