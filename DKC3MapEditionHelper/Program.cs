using System;
using System.Collections.Generic;
using DKC3MapEditionHelper.assistants;
using DKC3MapEditionHelper.configurations;

namespace DKC3MapEditionHelper
{
    internal static class Program
    {
        private static ToolTask _toolTask;
        private static int? _mapNumber;
        private static void Main(string[] args)
        {
            if (ValidateAndExtractArguments(args))
            {
                if (!TryExecuteTask())
                    ShowArgumentsError();
            }
            else
                ShowArgumentsError();
        }

        private static bool TryExecuteTask()
        {
            if (_mapNumber.HasValue)
                return TryExecuteMapTask(_mapNumber.Value);
            else
                return TryExecuteGeneralTask();
        }

        private static void ShowArgumentsError()
        {
            Console.WriteLine("Invalid arguments. Arguments are: [task] [map_number]");
        }

        private static bool ValidateAndExtractArguments(string[] args)
        {
            if (args.Length == 0)
                return false;

            if (!Enum.TryParse(typeof(ToolTask), args[0], out var toolTask))
                return false;

            _toolTask = (ToolTask)toolTask;
            _mapNumber = ExtractMapNumber(args);

            return true;
        }

        private static int? ExtractMapNumber(string[] args)
        {
            if (args.Length <= 1)
                return null;

            return int.Parse(args[1]);
        }
        private static bool TryExecuteMapTask(int mapNumber)
        {
            switch (_toolTask)
            {
                case ToolTask.export:
                    MapOperations.ExportMap(mapNumber);
                    return true;
                case ToolTask.import:
                    MapOperations.CompressAndImportMap(mapNumber);
                    return true;
                case ToolTask.edit:
                    MapOperations.EditMap(mapNumber);
                    return true;
                default:
                    return false;
            }
        }

        private static bool TryExecuteGeneralTask()
        {
            switch (_toolTask)
            {
                case ToolTask.export:
                    MapOperations.ExportAllMaps();
                    return true;
                case ToolTask.import:
                    MapOperations.CompressAndImportAllMaps();
                    return true;
                case ToolTask.test:
                    ProcessAssistant.ExecuteProcess(AppConfiguration.AppSettings.EmulatorPath,
                        AppConfiguration.AppSettings.RomPath);
                    return true;
                default:
                    return false;
            }
        }
    }
}