using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RumerSpreading.Ver1
{
    public class ShellCommandExecutor
    {
        public static (CliWrap.CommandResult cmdResult, string output) Run(string cmd, string cmdArgs, Action<string> outputPipeCallback = null)
        {
            return RunAsync(cmd, cmdArgs, outputPipeCallback).GetAwaiter().GetResult();
        }

        //public static async Task<(CliWrap.CommandResult cmdResult, string output)> RunAsync(string cmd, string cmdArgs, Action<string> outputPipeCallback = null)
        //    => await RunAsync(cmd, cmdArgs, outputPipeCallback, CancellationToken.None);
        public static async Task<(CliWrap.CommandResult cmdResult, string output)> RunAsync(string cmd, string cmdArgs, Action<string> outputPipeCallback = null,  CancellationToken token=default)
        {
            var stdoutBuffer = new StringBuilder();
            var stderrBuffer = new StringBuilder();

            var workingDir = System.IO.Path.GetDirectoryName(System.IO.Path.GetFullPath(cmd));
            if (cmd.StartsWith("."))
            {
                var binPath = System.IO.Path.GetDirectoryName(System.Reflection.Assembly.GetExecutingAssembly().Location);
                workingDir = System.IO.Path.GetFullPath(System.IO.Path.Combine(binPath, cmd));
            }



            var task = CliWrap.Cli.Wrap(cmd)
                .WithWorkingDirectory(workingDir)
                .WithArguments(cmdArgs)
                .WithStandardInputPipe(CliWrap.PipeSource.Null)
                .WithStandardOutputPipe(CliWrap.PipeTarget.ToDelegate((s) => {
                    stdoutBuffer.Append(s);
                    outputPipeCallback?.Invoke(s);
                }))
                //.WithStandardErrorPipe(CliWrap.PipeTarget.ToStringBuilder(stderrBuffer))
                .WithStandardErrorPipe(CliWrap.PipeTarget.ToDelegate((s) => {
                    stderrBuffer.Append(s);
                    stdoutBuffer.Append(s);
                    outputPipeCallback?.Invoke(s);

                }))
                .WithValidation(CliWrap.CommandResultValidation.None)
                ;

            CliWrap.CommandResult cmdResult = null;
            try
            {
                cmdResult = await task.ExecuteAsync(token);
            }
            catch (Exception ex)
            {
                throw;
            }

            return (cmdResult, stdoutBuffer.ToString());
        }

    }
}
