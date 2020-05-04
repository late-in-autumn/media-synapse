using ImageMagick;
using System;
using System.IO;

namespace MediaFileConverter.CombineSynapse.CombineSynapseImage
{
    class SynapseImageCombiner
    {
        private static readonly string VIDEO_SYNAPSE_FILENAME = "video_*_synapse.png";
        private static readonly string STILLS_SYNAPSE_FILENAME = "stills_synapse.png";

        private readonly string SynapseFolder;

        public SynapseImageCombiner(string synapseFolder)
        {
            if (String.IsNullOrWhiteSpace(synapseFolder))
            {
                throw new ArgumentException("message", nameof(synapseFolder));
            }

            SynapseFolder = synapseFolder;
        }

        public void Combine()
        {
            string[] videoImgs = Directory.GetFiles(SynapseFolder, VIDEO_SYNAPSE_FILENAME);
            string[] stillsImgs = Directory.GetFiles(SynapseFolder, STILLS_SYNAPSE_FILENAME);
            using var images = new MagickImageCollection();
            foreach (var v in videoImgs)
                images.Add(new MagickImage(v));
            foreach (var s in stillsImgs)
                images.Add(new MagickImage(s));

            using var result = images.AppendHorizontally();
            result.Write(Path.Join(SynapseFolder, "combined_synapse.png"));
        }
    }
}
