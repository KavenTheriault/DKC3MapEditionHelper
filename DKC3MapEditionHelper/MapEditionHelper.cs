using System;
using System.IO;
using System.Linq;
using DKC3MapEditionHelper.configurations;

namespace DKC3MapEditionHelper
{
    public class MapEditionHelper
    {
        private readonly ConfigurationAssistant _configurationAssistant;
        private readonly MapToolAssistant _mapToolAssistant;

        public MapEditionHelper()
        {
            _configurationAssistant = new ConfigurationAssistant();
            _mapToolAssistant = new MapToolAssistant();
        }

        public void ExportAllMaps()
        {
            foreach (var map in _configurationAssistant.Maps)
            {
                ExportMap(map);
            }
        }

        public void ExportMap(int number)
        {
            var map = _configurationAssistant.Maps.FirstOrDefault(x => x.Number == number);

            if (map == null)
            {
                Console.WriteLine($"Map with number ({number}) not found.");
                return;
            }

            ExportMap(map);
        }

        public void CompressAndImportAllMaps()
        {
            BackupRomFile();

            foreach (var map in _configurationAssistant.Maps)
            {
                CompressAndImportMap(map);
            }
        }

        public void CompressAndImportMap(int number)
        {
            var map = _configurationAssistant.Maps.FirstOrDefault(x => x.Number == number);

            if (map == null)
            {
                Console.WriteLine($"Map with number ({number}) not found.");
                return;
            }

            BackupRomFile();
            CompressAndImportMap(map);
        }

        private void ExportMap(Map map)
        {
            Console.WriteLine($"Exporting FMF and HDR files for ({map.Name}).");

            var mapWorkingDirectory = GetMapWorkingDirectory(map);

            if (!Directory.Exists(mapWorkingDirectory))
            {
                Directory.CreateDirectory(mapWorkingDirectory);
                Console.WriteLine($"Folder ({mapWorkingDirectory}) created.");
            }

            _mapToolAssistant.MapExport(
                _configurationAssistant.AppSettings.RomPath,
                GetFmfFilePath(map),
                GetHdrFilePath(map),
                map.Number);

            CopyChipFileInWorkingDirectory(map);
            CreateProjectFile(map);

            Console.WriteLine($"Exported FMF and HDR files of map ({map.Number}) in ({mapWorkingDirectory}).");
        }

        private void CopyChipFileInWorkingDirectory(Map map)
        {
            var sourceChipPath = Path.Combine(_configurationAssistant.AppSettings.ChipsFileDirectory, map.ChipName);
            var destinationChipPath = Path.Combine(GetMapWorkingDirectory(map), map.ChipName);

            if (File.Exists(destinationChipPath))
                return;

            File.Copy(sourceChipPath, destinationChipPath);
            Console.WriteLine($"Added file at ({destinationChipPath}).");
        }

        private void CompressAndImportMap(Map map)
        {
            Console.WriteLine($"Compressing and importing file in rom for ({map.Name}).");

            var sdkFilePath = GetSdkFilePath(map);

            _mapToolAssistant.MapCompress(
                GetFmfFilePath(map),
                GetHdrFilePath(map),
                sdkFilePath,
                map.Number);

            Console.WriteLine($"Compressed file for map ({map.Number}).");

            _mapToolAssistant.MapImport(
                _configurationAssistant.AppSettings.RomPath,
                sdkFilePath,
                map.Number);

            Console.WriteLine($"Imported file in rom for map ({map.Number}).");
        }

        private void BackupRomFile()
        {
            var backupDirectory = Path.Combine(_configurationAssistant.AppSettings.WorkingDirectory, "backups");

            if (!Directory.Exists(backupDirectory))
            {
                Directory.CreateDirectory(backupDirectory);
                Console.WriteLine($"Folder ({backupDirectory}) created.");
            }

            var backupName = DateTime.Now.ToString("yyyyMMdd_HHmmss");
            var backupPath = Path.Combine(backupDirectory, $"{backupName}.smc");

            File.Copy(_configurationAssistant.AppSettings.RomPath, backupPath);
            Console.WriteLine($"Added file at ({backupPath}).");
        }

        private void CreateProjectFile(Map map)
        {
            var projectFileText = FmfToPlatiniumProject.GetPlatiniumProjectFile(
                GetFmfFilePath(map),
                Path.Combine(GetMapWorkingDirectory(map), map.ChipName));
            var filePath = Path.Combine(GetMapWorkingDirectory(map), $"platinium_{map.Key}.xml");
            
            File.WriteAllText(filePath, projectFileText);
            Console.WriteLine($"Added/Updated project file at ({filePath}).");
        }

        private string GetMapWorkingDirectory(Map map)
        {
            return Path.Combine(_configurationAssistant.AppSettings.WorkingDirectory, map.Key);
        }

        private string GetFmfFilePath(Map map)
        {
            var fmfFileName = $"{map.Key}.fmf";
            return Path.Combine(GetMapWorkingDirectory(map), fmfFileName);
        }

        private string GetHdrFilePath(Map map)
        {
            var fmfFileName = $"{map.Key}.hdr";
            return Path.Combine(GetMapWorkingDirectory(map), fmfFileName);
        }

        private string GetSdkFilePath(Map map)
        {
            var fmfFileName = $"{map.Key}.sdk";
            return Path.Combine(GetMapWorkingDirectory(map), fmfFileName);
        }
    }
}