using MediaFileConverter.AbstractClasses;
using MediaFileConverter.CombineSynapse.CombineSynapseImage;
using MediaFileConverter.CombineSynapse.CombineSynapseMetaData;
using System;

namespace MediaFileConverter.CombineSynapse
{
    class CombineSynapseDriver : Driver
    {
        private readonly string SynapseFolder;

        public CombineSynapseDriver(string synapseFolder)
        {
            if (String.IsNullOrWhiteSpace(synapseFolder))
            {
                throw new ArgumentException("message", nameof(synapseFolder));
            }

            SynapseFolder = synapseFolder;
        }

        public override void Execute()
        {
            // combine synapse image
            Console.WriteLine("Combining synapse image...");
            SynapseImageCombiner imageCombiner = new SynapseImageCombiner(SynapseFolder);
            imageCombiner.Combine();

            // combine synapse metadata
            Console.WriteLine("Combining synapse metadata...");
            SynapseMetaDataCombiner metaDataCombiner = new SynapseMetaDataCombiner(SynapseFolder);
            metaDataCombiner.Combine();
        }
    }
}
