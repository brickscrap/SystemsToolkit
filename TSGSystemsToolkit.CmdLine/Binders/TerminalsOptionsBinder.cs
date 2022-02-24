using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.CommandLine;
using System.CommandLine.Binding;
using TSGSystemsToolkit.CmdLine.Handlers;
using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Binders
{
    public class TerminalsOptionsBinder : BinderBase<TerminalsOptions>
    {
        private readonly Argument<string> _filePathArg;
        private readonly Option<bool> _emisFileOpt;
        private readonly Option<string> _outputOpt;
        private readonly IHost _host;

        public TerminalsOptionsBinder(Argument<string> filePathArg, Option<bool> emisFileOpt, Option<string> outputOpt, IHost host)
        {
            _filePathArg = filePathArg;
            _emisFileOpt = emisFileOpt;
            _outputOpt = outputOpt;
            _host = host;
        }

        protected override TerminalsOptions GetBoundValue(BindingContext bindingContext)
        {
            AddDependencies(bindingContext);

            return new()
            {
                CreateEmisFile = bindingContext.ParseResult.GetValueForOption(_emisFileOpt),
                FilePath = bindingContext.ParseResult.GetValueForArgument(_filePathArg),
                OutputPath = bindingContext.ParseResult.GetValueForOption(_outputOpt)
            };
        }

        private void AddDependencies(BindingContext bindingContext)
        {
            bindingContext.AddService<ILogger<TerminalsHandler>>(x =>
                _host.Services.GetService(typeof(ILogger<TerminalsHandler>)) as ILogger<TerminalsHandler>);
        }
    }
}
