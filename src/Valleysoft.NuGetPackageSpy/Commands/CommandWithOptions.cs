using System.CommandLine;
using System.CommandLine.Invocation;

namespace NuGetSpy.Commands;

internal abstract class CommandWithOptions<TOptions> : Command
    where TOptions : OptionsBase, new()
{
    public new TOptions Options { get; set; } = new();

    protected CommandWithOptions(string name, string description)
        : base(name, description)
    {
        Options.SetCommandOptions(this);
        this.SetHandler(ExecuteAsyncCore);
    }

    private async Task<int> ExecuteAsyncCore(InvocationContext context)
    {
        try
        {
            Options.SetParseResult(context.BindingContext.ParseResult);
            await ExecuteAsync();
            return 0;
        }
        catch (Exception ex)
        {
            Console.Error.WriteLine(ex.Message);
            return 1;
        }
    }

    protected abstract Task ExecuteAsync();
}