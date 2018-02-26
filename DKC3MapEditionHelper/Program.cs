using System;
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
                        MapEditionAssistant.ExportMap(numberArgument.Value);
                    else
                        MapEditionAssistant.ExportAllMaps();
                    break;
                case "import":
                    if (numberArgument.HasValue)
                        MapEditionAssistant.CompressAndImportMap(numberArgument.Value);
                    else
                        MapEditionAssistant.CompressAndImportAllMaps();
                    break;
                case "edit":
                    if (numberArgument.HasValue)
                        MapEditionAssistant.EditMap(numberArgument.Value);
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

        private static int? GetNumberArgument(string[] args)
        {
            if (args.Length <= 1)
                return null;

            return int.Parse(args[1]);
        }
    }
}