using AutoMapper.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.CommandLine;
using System.CommandLine.Binding;
using TSGSystemsToolkit.CmdLine.GraphQL;
using TSGSystemsToolkit.CmdLine.Handlers;
using TSGSystemsToolkit.CmdLine.Options;
using TSGSystemsToolkit.CmdLine.Services;

namespace TSGSystemsToolkit.CmdLine.Binders
{
    public class SendFileBinder : BinderBase<SendFileOptions>
    {
        private readonly Argument<string> _filePath;
        private readonly Option<string> _cluster;
        private readonly Option<string> _target;
        private readonly Option<string> _list;
        private readonly Option<string> _site;
        private readonly IHost _host;

        public SendFileBinder(Argument<string> filePath, Option<string> cluster, Option<string> list, Option<string> target, Option<string> site, IHost host)
        {
            _filePath = filePath;
            _cluster = cluster;
            _target = target;
            _list = list;
            _site = site;
            _host = host;
        }

        protected override SendFileOptions GetBoundValue(BindingContext bindingContext)
        {
            AddDependencies(bindingContext);

            return new()
            {
                FilePath = bindingContext.ParseResult.GetValueForArgument(_filePath),
                Cluster = bindingContext.ParseResult.GetValueForOption(_cluster),
                Target = bindingContext.ParseResult.GetValueForOption(_target),
                List = bindingContext.ParseResult.GetValueForOption(_list),
                Site = bindingContext.ParseResult.GetValueForOption(_site)
            };
        }

        private void AddDependencies(BindingContext bindingContext)
        {
            bindingContext.AddService<SysTkApiClient>(x =>
                _host.Services.GetService(typeof(SysTkApiClient)) as SysTkApiClient);

            bindingContext.AddService<ILogger<SendFileHandler>>(x =>
                _host.Services.GetService(typeof(ILogger<SendFileHandler>)) as ILogger<SendFileHandler>);

            bindingContext.AddService<IConfiguration>(x =>
                _host.Services.GetService(typeof(IConfiguration)) as IConfiguration);

            bindingContext.AddService<IAuthService>(x =>
                _host.Services.GetService(typeof(IAuthService)) as IAuthService);
        }
    }
}
