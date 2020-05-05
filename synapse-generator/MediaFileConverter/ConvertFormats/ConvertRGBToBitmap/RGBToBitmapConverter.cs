using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;

namespace MediaFileConverter.ConvertFormats.ConvertRGBToBitmap
{
    class RGBToBitmapConverter
    {
        private readonly string InputFileName;
        private readonly string OutputFileName;
        private readonly ImageFormat OutputFormat;

        private Bitmap Image;

        public RGBToBitmapConverter(string inputFileName,
            string outputFileName,
            ImageFormat outputFormat)
        {
            if (String.IsNullOrWhiteSpace(inputFileName))
            {
                throw new ArgumentException("message", nameof(inputFileName));
            }

            if (String.IsNullOrWhiteSpace(outputFileName))
            {
                throw new ArgumentException("message", nameof(outputFileName));
            }

            InputFileName = inputFileName;
            OutputFileName = outputFileName;
            OutputFormat = outputFormat ?? throw new ArgumentNullException(nameof(outputFormat));
            Image = new Bitmap(Misc.Constants.Constants.INPUT_WIDTH,
                Misc.Constants.Constants.INPUT_HEIGHT);
        }

        public void Convert()
        {
            ReadRGBIntoBitmap();
            WriteBitmapToFile();
        }

        private void ReadRGBIntoBitmap()
        {
            int width = Misc.Constants.Constants.INPUT_WIDTH;
            int height = Misc.Constants.Constants.INPUT_HEIGHT;
            int frameLength = width * height * 3;
            FileStream source = new FileStream(InputFileName, FileMode.Open, FileAccess.Read)
            {
                Position = 0
            };
            byte[] buffer = new byte[frameLength];
            int index = 0;

            source.Read(buffer);
            for (int y = 0; y < height; y ++)
                for (int x = 0; x < width; x ++)
                {
                    byte r = buffer[index];
                    byte g = buffer[index + height * width];
                    byte b = buffer[index + height * width * 2];
                    Image.SetPixel(x, y, Color.FromArgb(r, g, b));
                    source.Read(buffer);
                    index++;
                }
        }

        private void WriteBitmapToFile()
        {
            Image.Save(OutputFileName, OutputFormat);
        }
    }
}
