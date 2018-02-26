using System;
using System.IO;
using System.Linq;
using DKC3MapEditionHelper.assistants;
using DKC3MapEditionHelper.configurations;

namespace DKC3MapEditionHelper
{
    public static class MapOperations
    {
        public static void ExportAllMaps()
        {
            foreach (var map in AppConfiguration.Maps)
            {
                ExportMap(map);
            }
        }

        public static void ExportMap(int number)
        {
            var map = AppConfiguration.Maps.FirstOrDefault(x => x.Number == number);

            if (map == null)
            {
                Console.WriteLine($"Map with number ({number}) not found.");
                return;
            }

            ExportMap(map);
        }

        public static void EditMap(int number)
        {
            var map = AppConfiguration.Maps.FirstOrDefault(x => x.Number == number);

            if (map == null)
            {
                Console.WriteLine($"Map with number ({number}) not found.");
                return;
            }

            ExportMap(map);
            MapToolAssistant.Platinium(GetProjectFilePath(map));

            OverrideFmfFile(map);
            BackupRomFile();
            CompressAndImportMap(map);
        }

        public static void CompressAndImportAllMaps()
        {
            BackupRomFile();

            foreach (var map in AppConfiguration.Maps)
            {
                CompressAndImportMap(map);
            }
        }

        public static void CompressAndImportMap(int number)
        {
            var map = AppConfiguration.Maps.FirstOrDefault(x => x.Number == number);

            if (map == null)
            {
                Console.WriteLine($"Map with number ({number}) not found.");
                return;
            }

            BackupRomFile();
            CompressAndImportMap(map);
        }

        private static void ExportMap(Map map)
        {
            Console.WriteLine($"Exporting FMF and HDR files for ({map.Name}).");

            var mapWorkingDirectory = GetMapWorkingDirectory(map);

            if (!Directory.Exists(mapWorkingDirectory))
            {
                Directory.CreateDirectory(mapWorkingDirectory);
                Console.WriteLine($"Folder ({mapWorkingDirectory}) created.");
            }

            MapToolAssistant.MapExport(
                AppConfiguration.AppSettings.RomPath,
                GetFmfFilePath(map),
                GetHdrFilePath(map),
                map.Number);

            CopyChipFileInWorkingDirectory(map);
            CreateProjectFile(map);

            Console.WriteLine($"Exported FMF and HDR files of map ({map.Number}) in ({mapWorkingDirectory}).");
        }

        private static void CopyChipFileInWorkingDirectory(Map map)
        {
            var sourceChipPath = Path.Combine(AppConfiguration.AppSettings.ChipsFileDirectory, map.ChipName);
            var destinationChipPath = Path.Combine(GetMapWorkingDirectory(map), map.ChipName);

            if (File.Exists(destinationChipPath))
                return;

            File.Copy(sourceChipPath, destinationChipPath);
            Console.WriteLine($"Added file at ({destinationChipPath}).");
        }

        private static void CompressAndImportMap(Map map)
        {
            Console.WriteLine($"Compressing and importing file in rom for ({map.Name}).");

            var sdkFilePath = GetSdkFilePath(map);

            MapToolAssistant.MapCompress(
                GetFmfFilePath(map),
                GetHdrFilePath(map),
                sdkFilePath,
                map.Number);

            Console.WriteLine($"Compressed file for map ({map.Number}).");

            MapToolAssistant.MapImport(
                AppConfiguration.AppSettings.RomPath,
                sdkFilePath,
                map.Number);

            Console.WriteLine($"Imported file in rom for map ({map.Number}).");
        }

        private static void BackupRomFile()
        {
            var backupDirectory = Path.Combine(AppConfiguration.AppSettings.WorkingDirectory, "backups");

            if (!Directory.Exists(backupDirectory))
            {
                Directory.CreateDirectory(backupDirectory);
                Console.WriteLine($"Folder ({backupDirectory}) created.");
            }

            var backupName = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var backupPath = Path.Combine(backupDirectory, $"{backupName}.smc");

            File.Copy(AppConfiguration.AppSettings.RomPath, backupPath);
            Console.WriteLine($"Added file at ({backupPath}).");
        }

        private static void CreateProjectFile(Map map)
        {
            var projectFileText = FmfToPlatiniumAssistant.GeneratePlatiniumProjectFile(
                GetFmfFilePath(map),
                Path.Combine(GetMapWorkingDirectory(map), map.ChipName));
            var filePath = GetProjectFilePath(map);

            File.WriteAllText(filePath, projectFileText);
            Console.WriteLine($"Added/Updated project file at ({filePath}).");
        }

        private static void OverrideFmfFile(Map map)
        {
            var fmfFilePath = GetFmfFilePath(map);
            var bytes = FmfToPlatiniumAssistant.GenerateFmfWithPlaniniumProjectFile(fmfFilePath, GetProjectFilePath(map));

            File.WriteAllBytes(fmfFilePath, bytes);
            Console.WriteLine($"Overrided FMF file ({fmfFilePath}).");
        }

        private static string GetMapWorkingDirectory(Map map)
        {
            return Path.Combine(AppConfiguration.AppSettings.WorkingDirectory, map.Key);
        }

        private static string GetFmfFilePath(Map map)
        {
            var fmfFileName = $"{map.Key}.fmf";
            return Path.Combine(GetMapWorkingDirectory(map), fmfFileName);
        }

        private static string GetHdrFilePath(Map map)
        {
            var fmfFileName = $"{map.Key}.hdr";
            return Path.Combine(GetMapWorkingDirectory(map), fmfFileName);
        }

        private static string GetSdkFilePath(Map map)
        {
            var fmfFileName = $"{map.Key}.sdk";
            return Path.Combine(GetMapWorkingDirectory(map), fmfFileName);
        }

        private static string GetProjectFilePath(Map map)
        {
            var fmfFileName = $"platinium_{map.Key}.xml";
            return Path.Combine(GetMapWorkingDirectory(map), fmfFileName);
        }
    }
}