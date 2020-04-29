using ImageMagick;
using System;
using System.Collections.Generic;
using System.IO;

namespace SynapseGenerator.StillsSynapse.BuildSynapse
{
    class StillsSynapseBuilder
    {
        // where the synapse image will be saved
        private readonly string OutputFolder;
        // stills that will be included in the synapse
        private readonly List<string> SynapseStills;

        public StillsSynapseBuilder(string outputFolder, List<string> synapseStills)
        {
            if (String.IsNullOrWhiteSpace(outputFolder))
            {
                throw new System.ArgumentException("message", nameof(outputFolder));
            }

            OutputFolder = outputFolder;
            SynapseStills =
                synapseStills ?? throw new System.ArgumentNullException(nameof(synapseStills));
        }

        public void Build()
        {
            // only stitch the source images for now
            using (var images = new MagickImageCollection())
            {
                foreach (var i in SynapseStills)
                    images.Add(new MagickImage(i));

                // stitch horizontally as demonstrated in the assignment example
                using (var result = images.AppendHorizontally())
                {
                    result.Write(Path.Join(OutputFolder, "synapse.png"));
                }
            }
        }
    }
}
