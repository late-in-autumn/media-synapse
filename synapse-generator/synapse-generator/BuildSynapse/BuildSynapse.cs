﻿using ImageMagick;
using System;
using System.IO;

namespace SynapseGenerator.BuildSynapse
{
    class SynapseBuilder
    {
        private static readonly string INPUT_IMGS = "*.png";

        private readonly string InputFolder;
        private readonly string OutputFile;

        public SynapseBuilder(string inputFile)
        {
            string dirName =
                String.IsNullOrWhiteSpace(Path.GetDirectoryName(inputFile)) ?
                String.Empty : Path.GetDirectoryName(inputFile);
            string baseName =
                Path.GetFileNameWithoutExtension(inputFile);
            InputFolder = Path.Join(dirName, baseName, "imgs");
            OutputFile = Path.Join(dirName, baseName + ".synapse.png");
        }

        public void Generate()
        {
            string[] imgs = Directory.GetFiles(InputFolder, INPUT_IMGS);
            using (var images = new MagickImageCollection())
            {
                foreach (var i in imgs)
                    images.Add(new MagickImage(i));

                using (var result = images.AppendHorizontally())
                {
                    result.Write(OutputFile);
                }
            }
        }
    }
}