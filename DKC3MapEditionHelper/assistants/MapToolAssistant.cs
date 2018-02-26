using System.Collections.Generic;
using System.Linq;
using DKC3MapEditionHelper.configurations;

namespace DKC3MapEditionHelper.assistants
{
    public static class MapToolAssistant
    {
        public static void MapExport(string romPath, string fmfFilePath, string hdrFilePath, int mapNumber)
        {
            ExecuteMapTool(AppConfiguration.AppSettings.MapExportPath,
                new[]
                {
                    romPath,
                    fmfFilePath,
                    hdrFilePath,
                    mapNumber.ToString()
                });
        }

        public static void MapCompress(string fmfFilePath, string hdrFilePath, string sdkFilePath, int mapNumber)
        {
            ExecuteMapTool(AppConfiguration.AppSettings.MapCompPath,
                new[]
                {
                    fmfFilePath,
                    hdrFilePath,
                    sdkFilePath,
                    mapNumber.ToString()
                });
        }

        public static void MapImport(string romPath, string sdkFilePath, int mapNumber)
        {
            ExecuteMapTool(AppConfiguration.AppSettings.MapImportPath,
                new[]
                {
                    romPath,
                    sdkFilePath,
                    mapNumber.ToString()
                });
        }

        public static void Platinium(string projectFilePath)
        {
            ExecuteMapTool(AppConfiguration.AppSettings.PlatiniumPath,
                new[]
                {
                    projectFilePath
                });
        }

        private static void ExecuteMapTool(string processName, IEnumerable<string> arguments)
        {
            if (!AppConfiguration.AppSettings.WineMode)
            {
                ProcessAssistant.ExecuteProcess(processName, arguments);
                return;
            }

            var newArguments = arguments.ToList();
            newArguments.Insert(0, processName);

            ProcessAssistant.ExecuteProcess("wine", newArguments);
        }
    }
}