using ImageMagick;
using System;
using System.IO;

namespace SynapseGenerator.VideoSynapse.BuildSynapse
{
    class VideoSynapseBuilder
    {
        // only accept PNGs for now
        private static readonly string INPUT_IMGS = "*-01.png";

        // the folder where the extracted source images reside
        private readonly string InputFolder;
        // the folder where the completed synapse image gets saved to
        private readonly string OutputFile;

        public VideoSynapseBuilder(string inputFileName)
        {
            if (String.IsNullOrWhiteSpace(inputFileName))
            {
                throw new ArgumentException("message", nameof(inputFileName));
            }

            string dirName =
                String.IsNullOrWhiteSpace(Path.GetDirectoryName(inputFileName)) ?
                String.Empty : Path.GetDirectoryName(inputFileName);
            string baseName =
                Path.GetFileNameWithoutExtension(inputFileName);
            InputFolder = Path.Join(dirName, baseName, "imgs");
            OutputFile = Path.Join(dirName, baseName, "synapse.png");
        }

        public void Build()
        {
            string[] imgs = Directory.GetFiles(InputFolder, INPUT_IMGS);

            // only stitch the source images for now
            using (var images = new MagickImageCollection())
            {
                foreach (var i in imgs)
                    images.Add(new MagickImage(i));

                // stitch horizontally as demonstrated in the assignment example
                using (var result = images.AppendHorizontally())
                {
                    result.Write(OutputFile);
                }
            }
        }
    }
}
