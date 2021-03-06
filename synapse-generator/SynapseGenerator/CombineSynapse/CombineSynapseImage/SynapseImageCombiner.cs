﻿using ImageMagick;
using SynapseGenerator.Misc.Constants;
using System;
using System.IO;

namespace SynapseGenerator.CombineSynapse.CombineSynapseImage
{
    class SynapseImageCombiner
    {
        private static readonly string FILE_EXT = ".png";

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
            string[] videoImgs = Directory.GetFiles(
                SynapseFolder, Constants.VIDEO_SYNAPSE_BASENAME + FILE_EXT);
            string[] stillsImgs = Directory.GetFiles(
                SynapseFolder, Constants.STILLS_SYNAPSE_BASENAME + FILE_EXT);
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
