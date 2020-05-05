using Emgu.CV;
using Emgu.CV.Structure;
using SynapseGenerator.Misc.ExtensionMethods;
using System;
using System.Linq;

namespace SynapseGenerator.VideoSynapse.SelectFrame
{
    class VideoFrameSelector
    {
        private readonly VideoCapture InputVideoFile;
        private readonly string OutputFrameFileName;

        public VideoFrameSelector(string inputVideoFileName, string outputFrameFileName)
        {
            if (String.IsNullOrWhiteSpace(inputVideoFileName))
            {
                throw new ArgumentException("message", nameof(inputVideoFileName));
            }

            if (String.IsNullOrWhiteSpace(outputFrameFileName))
            {
                throw new ArgumentException("message", nameof(outputFrameFileName));
            }

            InputVideoFile = new VideoCapture(inputVideoFileName);
            OutputFrameFileName = outputFrameFileName;
        }

        public void Select()
        {
            WriteSelectedFrame(SelectMostColorfulFrame());
        }

        private void WriteSelectedFrame(Mat frameToWrite)
        {
            if (frameToWrite is null)
            {
                throw new ArgumentNullException(nameof(frameToWrite));
            }

            frameToWrite.ToImage<Bgr, byte>().Save(OutputFrameFileName);
        }

        private Mat SelectMostColorfulFrame()
        {
            double maxColorfulness = Double.MinValue;
            long frameCounter = 0;
            Mat mostColorfulFrame = null;
            Mat buffer;

            while ((buffer = InputVideoFile.QueryFrame()) != null)
            {
                double colorfulness = ComputeFrameColorfulness(buffer);
                if (colorfulness >= maxColorfulness)
                {
                    maxColorfulness = colorfulness;
                    mostColorfulFrame = buffer;
                }
                frameCounter++;
            }

            return mostColorfulFrame;
        }

        private double ComputeFrameColorfulness(Mat inputImage)
        {
            if (inputImage is null)
            {
                throw new ArgumentNullException(nameof(inputImage));
            }

            // based on the algorithm from https://www.pyimagesearch.com/2017/06/05/computing-image-colorfulness-with-opencv-and-python/
            byte[] blue = inputImage.ToImage<Bgr, byte>()[0].Bytes;
            byte[] green = inputImage.ToImage<Bgr, byte>()[1].Bytes;
            byte[] red = inputImage.ToImage<Bgr, byte>()[2].Bytes;

            double[] rg = new double[red.Length];
            double[] yb = new double[red.Length];

            for (int i = 0; i < rg.Length; i++)
            {
                rg[i] = Math.Abs(red[i] - green[i]);
                yb[i] = Math.Abs(0.5 * (red[i] + green[i]) - blue[i]);
            }

            double meanRoot = Math.Sqrt(Math.Pow(rg.Average(), 2) + Math.Pow(yb.Average(), 2));
            double stdRoot = Math.Sqrt(Math.Pow(rg.StdDev(), 2) + Math.Pow(yb.StdDev(), 2));

            return stdRoot + (0.3 * meanRoot);
        }
    }
}
