using SynapseGenerator.AbstractClasses;
using SynapseGenerator.ConvertFormats.ConvertRGBToBitmap;
using System;
using System.Drawing.Imaging;
using System.IO;

namespace SynapseGenerator.ConvertFormats
{
    class ConvertFormatsDriver : Driver
    {
        private static readonly string INPUT_IMAGE_PATTERN = "*.rgb";

        private readonly string StillsInputFolder;
        private readonly string StillsOutputFolder;

        public ConvertFormatsDriver(string stillsInputFolder, string stillsOutputFolder)
        {
            if (String.IsNullOrWhiteSpace(stillsInputFolder))
            {
                throw new ArgumentException("message", nameof(stillsInputFolder));
            }

            if (String.IsNullOrWhiteSpace(stillsOutputFolder))
            {
                throw new ArgumentException("message", nameof(stillsOutputFolder));
            }

            StillsInputFolder = stillsInputFolder;
            StillsOutputFolder = stillsOutputFolder;
            Directory.CreateDirectory(stillsOutputFolder);
        }

        private void ConvertStills()
        {
            string[] inputFiles = Directory.GetFiles(StillsInputFolder, INPUT_IMAGE_PATTERN);
            foreach (var i in inputFiles)
            {
                string outputFileName
                    = Path.Join(StillsOutputFolder, Path.GetFileNameWithoutExtension(i) + ".png");
                RGBToBitmapConverter converter
                    = new RGBToBitmapConverter(i, outputFileName, ImageFormat.Png);
                converter.Convert();
            }
        }

        public override void Execute()
        {
            Console.WriteLine("Converting stills...");
            ConvertStills();
        }
    }
}
