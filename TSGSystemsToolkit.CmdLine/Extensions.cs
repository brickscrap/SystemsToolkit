using System.CommandLine;
using System.CommandLine.Invocation;
using System.Reflection;

namespace TSGSystemsToolkit.CmdLine
{
    internal static class Extensions
    {
        internal static Command WithHandler<T>(this Command command, string name)
        {
            var flags = BindingFlags.NonPublic | BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance;
            var method = typeof(T).GetMethod(name, flags);

            var handler = CommandHandler.Create(method!);
            command.Handler = handler;

            return command;
        }
    }
}
