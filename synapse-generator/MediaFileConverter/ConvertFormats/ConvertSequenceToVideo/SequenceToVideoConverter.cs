using MediaFileConverter.ConvertFormats.ConvertRGBToBitmap;
using System;
using System.Diagnostics;
using System.Drawing.Imaging;
using System.IO;
using System.Threading.Tasks;

namespace MediaFileConverter.ConvertFormats.ConvertSequenceToVideo
{
    class SequenceToVideoConverter
    {
        private static readonly string FFMPEG_BIN = @"ffmpeg";
        private static readonly string FFMPEG_ARG_TEMPLATE = "-r {0} -f image2 -s 352x288 -i {1}/image-%04d.jpg -c:v mjpeg -q:v 1 -pix_fmt + {2}";
        private static readonly string FRAME_RATE = "29.97";

        private readonly string InputSequenceFolder;
        private readonly string OutputFileName;

        public SequenceToVideoConverter(string inputSequenceFolder, string outputFileName)
        {
            if (String.IsNullOrWhiteSpace(inputSequenceFolder))
            {
                throw new ArgumentException("message", nameof(inputSequenceFolder));
            }

            if (String.IsNullOrWhiteSpace(outputFileName))
            {
                throw new ArgumentException("message", nameof(outputFileName));
            }

            InputSequenceFolder = inputSequenceFolder;
            OutputFileName = outputFileName;
        }

        public void Convert()
        {
            RGBSequenceToBitmap();
            BitmapSequenceToVideo();
        }

        private void RGBSequenceToBitmap()
        {
            string[] rgbSequences = Directory.GetFiles(InputSequenceFolder,
                Constants.Constants.RGB_FILE_PATTERN);
            Parallel.ForEach(rgbSequences, s =>
            {
                RGBToBitmapConverter converter
                    = new RGBToBitmapConverter(s,
                    Path.Join(
                        InputSequenceFolder, Path.GetFileNameWithoutExtension(s) + ".jpg"),
                    ImageFormat.Jpeg);
                converter.Convert();
            });
        }

        private void BitmapSequenceToVideo()
        {
            using var proc = new Process();
            proc.StartInfo.FileName = FFMPEG_BIN;
            proc.StartInfo.Arguments = String.Format(
                FFMPEG_ARG_TEMPLATE, FRAME_RATE, InputSequenceFolder, OutputFileName);
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.StartInfo.CreateNoWindow = true;
            proc.Start();
            Console.WriteLine(proc.StandardOutput.ReadToEnd());
            proc.WaitForExit();
        }
    }
}
