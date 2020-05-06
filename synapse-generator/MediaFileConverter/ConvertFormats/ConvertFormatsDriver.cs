using MediaFileConverter.ConvertFormats.ConvertRGBToBitmap;
using MediaFileConverter.ConvertFormats.ConvertSequenceToVideo;
using MediaFileConverter.Misc.AbstractClasses;
using System;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace MediaFileConverter.ConvertFormats
{
    class ConvertFormatsDriver : Driver
    {
        private readonly string StillsInputFolder;
        private readonly string StillsOutputFolder;
        private readonly string[] VideoInputSequenceFolders;
        private readonly string[] VideoOutputFileNames;

        public ConvertFormatsDriver(string stillsInputFolder,
            string stillsOutputFolder,
            string[] videoInputSequenceFolders,
            string[] videoOutputFileNames)
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
            VideoInputSequenceFolders
                = videoInputSequenceFolders ?? throw new ArgumentNullException(
                    nameof(videoInputSequenceFolders));
            VideoOutputFileNames
                = videoOutputFileNames ?? throw new ArgumentNullException(
                    nameof(videoOutputFileNames));
            Directory.CreateDirectory(stillsOutputFolder);
        }

        public override void Execute()
        {
            Console.WriteLine("Converting stills...");
            ConvertStills();
            Console.WriteLine("Converting video...");
            ConvertVideo();
        }

        private void ConvertStills()
        {
            string[] inputFiles = Directory.GetFiles(StillsInputFolder,
                Misc.Constants.Constants.RGB_FILE_PATTERN);
            Parallel.ForEach(inputFiles, i =>
            {
                var converter
                    = new RGBToBitmapConverter(i,
                    Path.Join(
                        StillsOutputFolder, Path.GetFileNameWithoutExtension(i) + ".jpg"),
                    ImageFormat.Jpeg);
                converter.Convert();
            });
        }

        private void ConvertVideo()
        {
            for (int i = 0; i < VideoInputSequenceFolders.Length; i++)
            {
                var converter
                    = new SequenceToVideoConverter(
                        VideoInputSequenceFolders[i], VideoOutputFileNames[i]);
                converter.Convert();
            }
        }
    }
}
