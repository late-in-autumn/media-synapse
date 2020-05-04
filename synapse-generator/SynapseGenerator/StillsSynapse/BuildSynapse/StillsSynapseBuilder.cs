using ImageMagick;
using System;
using System.Collections.Generic;

namespace MediaFileConverter.StillsSynapse.BuildSynapse
{
    class StillsSynapseBuilder
    {
        // where the synapse image will be saved
        private readonly string OutputBaseName;
        // stills that will be included in the synapse
        private readonly List<string> SynapseStills;

        public StillsSynapseBuilder(string outputBaseName, List<string> synapseStills)
        {
            if (String.IsNullOrWhiteSpace(outputBaseName))
            {
                throw new System.ArgumentException("message", nameof(outputBaseName));
            }

            OutputBaseName = outputBaseName;
            SynapseStills =
                synapseStills ?? throw new System.ArgumentNullException(nameof(synapseStills));
        }

        public void Build()
        {
            // only stitch the source images for now
            using var images = new MagickImageCollection();
            foreach (var i in SynapseStills)
            {
                var img = new MagickImage(i);
                img.Scale(Constants.Constants.SYNAPSE_WIDTH, Constants.Constants.SYNAPSE_HEIGHT);
                images.Add(img);
            }

            // stitch horizontally as demonstrated in the assignment example
            using var result = images.AppendHorizontally();
            result.Write($"{OutputBaseName}.png");
        }
    }
}
