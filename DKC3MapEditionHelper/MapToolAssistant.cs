using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using DKC3MapEditionHelper.configurations;

namespace DKC3MapEditionHelper
{
    public class MapToolAssistant
    {
        private readonly ConfigurationAssistant _configurationAssistant;

        public MapToolAssistant()
        {
            _configurationAssistant = new ConfigurationAssistant();
        }

        public void MapExport(string romPath, string fmfFilePath, string hdrFilePath, int mapNumber)
        {
            ExecuteMapTool(_configurationAssistant.AppSettings.MapExportPath,
                new[]
                {
                    romPath,
                    fmfFilePath,
                    hdrFilePath,
                    mapNumber.ToString()
                });
        }

        public void MapCompress(string fmfFilePath, string hdrFilePath, string sdkFilePath, int mapNumber)
        {
            ExecuteMapTool(_configurationAssistant.AppSettings.MapCompPath,
                new[]
                {
                    fmfFilePath,
                    hdrFilePath,
                    sdkFilePath,
                    mapNumber.ToString()
                });
        }

        public void MapImport(string romPath, string sdkFilePath, int mapNumber)
        {
            ExecuteMapTool(_configurationAssistant.AppSettings.MapImportPath,
                new[]
                {
                    romPath,
                    sdkFilePath,
                    mapNumber.ToString()
                });
        }

        public void Platinium(string projectFilePath)
        {
            ExecuteMapTool(_configurationAssistant.AppSettings.PlatiniumPath,
                new[]
                {
                    projectFilePath
                });
        }

        private void ExecuteMapTool(string processName, IEnumerable<string> arguments)
        {
            if (!_configurationAssistant.AppSettings.WineMode)
            {
                ExecuteProcess(processName, arguments);
                return;
            }

            var newArguments = arguments.ToList();
            newArguments.Insert(0, processName);

            ExecuteProcess("wine", newArguments);
        }

        private static void ExecuteProcess(string processName, IEnumerable<string> arguments)
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