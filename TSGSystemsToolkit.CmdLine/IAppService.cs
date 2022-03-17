namespace TSGSystemsToolkit.CmdLine;

internal interface IAppService
{
    Task<int> Run(string[] args);
}
