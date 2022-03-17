using Microsoft.Extensions.Hosting;
using System.CommandLine.Binding;
using TSGSystemsToolkit.CmdLine.GraphQL;
using TSGSystemsToolkit.CmdLine.Options;

namespace TSGSystemsToolkit.CmdLine.Binders;

internal class FileZillaOptionsBinder : BinderBase<FileZillaOptions>
{
    private readonly Option<string> _siteManagerPath;
    private readonly IHost _host;

    public FileZillaOptionsBinder(Option<string> siteManagerPath, IHost host)
    {
        _siteManagerPath = siteManagerPath;
        _host = host;
    }

    protected override FileZillaOptions GetBoundValue(BindingContext bindingContext)
    {
        AddDependencies(bindingContext);

        return new()
        {
            SiteManagerPath = bindingContext.ParseResult.GetValueForOption(_siteManagerPath)
        };
    }

    private void AddDependencies(BindingContext bindingContext)
    {
        bindingContext.AddService<SysTkApiClient>(x =>
            (SysTkApiClient)_host.Services.GetService(typeof(SysTkApiClient)));
    }
}
