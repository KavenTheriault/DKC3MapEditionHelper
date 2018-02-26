using System;
using System.Collections.Generic;
using DKC3MapEditionHelper.assistants;
using DKC3MapEditionHelper.configurations;

namespace DKC3MapEditionHelper
{
    internal static class Program
    {
        private static void Main(string[] args)
        {
            if (args.Length == 0)
            {
                Console.WriteLine("Please enter arguments.");
                return;
            }

            var numberArgument = GetNumberArgument(args);

            switch (args[0])
            {
                case "export":
                    if (numberArgument.HasValue)
                        MapOperations.ExportMap(numberArgument.Value);
                    else
                        MapOperations.ExportAllMaps();
                    break;
                case "import":
                    if (numberArgument.HasValue)
                        MapOperations.CompressAndImportMap(numberArgument.Value);
                    else
                        MapOperations.CompressAndImportAllMaps();
                    break;
                case "edit":
                    if (numberArgument.HasValue)
                        MapOperations.EditMap(numberArgument.Value);
                    else
                        Console.WriteLine("Please provide the map number to edit.");
                    break;
                case "test":
                    ProcessAssistant.ExecuteProcess(AppConfiguration.AppSettings.EmulatorPath,
                        AppConfiguration.AppSettings.RomPath);
                    break;
                default:
                    Console.WriteLine("Invalid arguments.");
                    break;
            }
        }

        private static int? GetNumberArgument(IReadOnlyList<string> args)
        {
            if (args.Count <= 1)
                return null;

            return int.Parse(args[1]);
        }
    }
}