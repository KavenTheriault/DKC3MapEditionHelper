using System;

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

            var mapEditionHelper = new MapEditionHelper();
            var numberArgument = GetNumberArgument(args);

            switch (args[0])
            {
                case "export":
                    if (numberArgument.HasValue)
                        mapEditionHelper.ExportMap(numberArgument.Value);
                    else
                        mapEditionHelper.ExportAllMaps();
                    break;
                case "import":
                    if (numberArgument.HasValue)
                        mapEditionHelper.CompressAndImportMap(numberArgument.Value);
                    else
                        mapEditionHelper.CompressAndImportAllMaps();
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