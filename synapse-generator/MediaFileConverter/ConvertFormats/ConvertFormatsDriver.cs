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
        private readonly string VideoInputSequenceFolder;
        private readonly string VideoOutputFileName;

        public ConvertFormatsDriver(string stillsInputFolder,
            string stillsOutputFolder,
            string videoInputSequenceFolder,
            string videoOutputFileName)
        {
            if (String.IsNullOrWhiteSpace(stillsInputFolder))
            {
                throw new ArgumentException("message", nameof(stillsInputFolder));
            }

            if (String.IsNullOrWhiteSpace(stillsOutputFolder))
            {
                throw new ArgumentException("message", nameof(stillsOutputFolder));
            }

            if (String.IsNullOrWhiteSpace(videoInputSequenceFolder))
            {
                throw new ArgumentException("message", nameof(videoInputSequenceFolder));
            }

            if (String.IsNullOrWhiteSpace(videoOutputFileName))
            {
                throw new ArgumentException("message", nameof(videoOutputFileName));
            }

            StillsInputFolder = stillsInputFolder;
            StillsOutputFolder = stillsOutputFolder;
            VideoInputSequenceFolder = videoInputSequenceFolder;
            VideoOutputFileName = videoOutputFileName;
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
                RGBToBitmapConverter converter
                    = new RGBToBitmapConverter(i,
                    Path.Join(
                        StillsOutputFolder, Path.GetFileNameWithoutExtension(i) + ".jpg"),
                    ImageFormat.Jpeg);
                converter.Convert();
            });
        }

        private void ConvertVideo()
        {
            SequenceToVideoConverter converter
                = new SequenceToVideoConverter(VideoInputSequenceFolder, VideoOutputFileName);
            converter.Convert();
        }
    }
}
