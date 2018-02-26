using System;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using DKC3MapEditionHelper.objects;

namespace DKC3MapEditionHelper.assistants
{
    public static class FmfToPlatiniumAssistant
    {
        private const int FmfWidthIndex = 8;
        private const int FmfHeightIndex = 12;
        private const int FmfDataIndex = 20;

        private static FmfData ReadFmfFile(string fmfFilePath)
        {
            var fmfData = new FmfData();
            var bytes = File.ReadAllBytes(fmfFilePath);

            var widthByte1 = bytes[FmfWidthIndex];
            var widthByte2 = bytes[FmfWidthIndex + 1];

            var heightByte1 = bytes[FmfHeightIndex];
            var heightByte2 = bytes[FmfHeightIndex + 1];

            fmfData.Width = BitConverter.ToInt16(new[] {widthByte1, widthByte2}, 0);
            fmfData.Height = BitConverter.ToInt16(new[] {heightByte1, heightByte2}, 0);

            var index = FmfDataIndex;
            fmfData.Datas = new short[fmfData.Height, fmfData.Width];

            for (var i = 0; i < fmfData.Height; i++)
            {
                for (var j = 0; j < fmfData.Width; j++)
                {
                    var byte1 = bytes[index];
                    var byte2 = bytes[index + 1];

                    fmfData.Datas[i, j] = BitConverter.ToInt16(new[] {byte1, byte2}, 0);
                    index = index + 2;
                }
            }

            return fmfData;
        }

        private static Size GetChipImageSize(string chipFilePath)
        {
            var chipImage = Image.FromFile(chipFilePath);
            return chipImage.Size;
        }

        public static string GeneratePlatiniumProjectFile(string fmfFilePath, string chipFilePath)
        {
            var fmfData = ReadFmfFile(fmfFilePath);
            var chipImageSize = GetChipImageSize(chipFilePath);

            var platiniumFile = File.ReadAllText("platinium_template.xml");

            platiniumFile = platiniumFile.Replace("{width}", fmfData.Width.ToString());
            platiniumFile = platiniumFile.Replace("{height}", fmfData.Height.ToString());

            platiniumFile = platiniumFile.Replace("{chip_file_name}", chipFilePath);
            platiniumFile = platiniumFile.Replace("{chip_width}", chipImageSize.Width.ToString());
            platiniumFile = platiniumFile.Replace("{chip_height}", chipImageSize.Height.ToString());

            var dataString = new StringBuilder();

            for (var i = 0; i < fmfData.Height; i++)
            {
                for (var j = 0; j < fmfData.Width; j++)
                {
                    dataString.Append(fmfData.Datas[i, j]);

                    if (j != fmfData.Width - 1)
                        dataString.Append(",");
                }

                if (i != fmfData.Height - 1)
                    dataString.Append(Environment.NewLine);
            }

            platiniumFile = platiniumFile.Replace("{datas}", dataString.ToString());

            return platiniumFile;
        }

        public static byte[] GenerateFmfWithPlaniniumProjectFile(string fmfFilePath, string projectFilePath)
        {
            var allBytes = File.ReadAllBytes(fmfFilePath);
            var resultBytes = allBytes.Take(FmfDataIndex).ToList();

            var projectFileText = File.ReadAllText(projectFilePath);

            var start = projectFileText.IndexOf("<data>", StringComparison.Ordinal) + 6;
            var end = projectFileText.IndexOf("</data>", StringComparison.Ordinal);

            var data = projectFileText.Substring(start, end - start).Replace("\r", "").Trim().Replace("\n", ",");
            var values = data.Split(",");

            foreach (var value in values)
            {
                var number = short.Parse(value);
                resultBytes.AddRange(BitConverter.GetBytes(number));
            }

            return resultBytes.ToArray();
        }
    }
}