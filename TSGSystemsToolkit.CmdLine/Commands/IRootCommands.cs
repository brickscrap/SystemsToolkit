using System.CommandLine;

namespace TSGSystemsToolkit.CmdLine.Commands
{
    internal interface IRootCommands
    {
        RootCommand Create();
    }
}