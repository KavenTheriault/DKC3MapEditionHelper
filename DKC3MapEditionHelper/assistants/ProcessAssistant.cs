using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace DKC3MapEditionHelper.assistants
{
    public static class ProcessAssistant
    {
        public static void ExecuteProcess(string processName, string argument)
        {
            ExecuteProcess(processName, new[] {argument});
        }

        public static void ExecuteProcess(string processName, IEnumerable<string> arguments)
        {
            var process = new Process
            {
                StartInfo = new ProcessStartInfo
                {
                    FileName = processName,
                    Arguments = arguments.Aggregate("", (current, argument) => current + $"{argument} "),
                    RedirectStandardOutput = true,
                    UseShellExecute = false,
                    CreateNoWindow = true,
                }
            };

            process.Start();

            Console.WriteLine(process.StandardOutput.ReadToEnd());
            process.WaitForExit();
        }
    }
}