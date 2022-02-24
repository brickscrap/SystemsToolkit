using FluentFTP;
using Spectre.Console;
using System;

namespace TSGSystemsToolkit.CmdLine
{
    public class ProgressHandler : IProgress<FtpProgress>
    {
        public ProgressTask ProgressTask;

        public ProgressHandler(ProgressTask progressTask)
        {
            ProgressTask = progressTask;
        }

        public void Report(double value)
        {
            ProgressTask.Increment(value);
        }

        public void Report(FtpProgress value)
        {
            ProgressTask.Increment(value.Progress / 100);
        }
    }
}
