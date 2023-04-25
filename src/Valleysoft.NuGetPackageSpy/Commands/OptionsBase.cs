using System.CommandLine;
using System.CommandLine.Parsing;
using System.Diagnostics.CodeAnalysis;

internal abstract class OptionsBase
{
    private readonly List<Argument> _arguments = new();
    private readonly List<Option> _options = new();
    private ParseResult? _parseResult;

    protected ParseResult ParseResult
    {
        get
        {
            ValidateParseResult();
            return _parseResult;
        }
    }

    protected Argument<T> Add<T>(Argument<T> argument)
    {
        _arguments.Add(argument);
        return argument;
    }

    protected Option<T> Add<T>(Option<T> option)
    {
        _options.Add(option);
        return option;
    }

    protected T GetValue<T>(Argument<T> arg)
    {
        ValidateParseResult();
        return _parseResult.GetValueForArgument(arg);
    }

    protected T? GetValue<T>(Option<T> option)
    {
        ValidateParseResult();
        return _parseResult.GetValueForOption(option);
    }

    [MemberNotNull(nameof(_parseResult))]
    private void ValidateParseResult()
    {
        if (_parseResult is null)
        {
            throw new Exception($"'{nameof(SetParseResult)}' method must be called before get argument value");
        }
    }

    public void SetParseResult(ParseResult parseResult)
    {
        _parseResult = parseResult;
        GetValues();
    }

    protected abstract void GetValues();

    public void SetCommandOptions(Command cmd)
    {
        foreach (Argument arg in _arguments)
        {
            cmd.AddArgument(arg);
        }

        foreach (Option option in _options)
        {
            cmd.AddOption(option);
        }
    }
}