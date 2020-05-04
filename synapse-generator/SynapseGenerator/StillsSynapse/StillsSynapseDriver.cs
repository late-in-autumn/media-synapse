using MediaFileConverter.AbstractClasses;
using MediaFileConverter.StillsSynapse.BuildSynapse;
using MediaFileConverter.StillsSynapse.SelectSynapseStills;
using MediaFileConverter.StillsSynapse.WriteMetaData;
using System;
using System.Collections.Generic;

namespace MediaFileConverter.StillsSynapse
{
    class StillsSynapseDriver : Driver
    {
        private readonly string InputFolder;
        private readonly string OutputBaseName;

        public StillsSynapseDriver(string inputFolder)
        {
            if (String.IsNullOrWhiteSpace(inputFolder))
            {
                throw new ArgumentException("message", nameof(inputFolder));
            }

            InputFolder = inputFolder;
            OutputBaseName = $"{inputFolder}_synapse";
        }

        public override void Execute()
        {
            // select synapse stills
            Console.WriteLine("Selecting synapse stills, this may take a long time...");
            SynapseStillsSelector selector =
                new SynapseStillsSelector(InputFolder);
            List<string> selected = selector.Select();

            // generate synapse image
            Console.WriteLine("Generating stills synapse image...");
            StillsSynapseBuilder builder =
                new StillsSynapseBuilder(OutputBaseName, selected);
            builder.Build();

            // write the metadata
            Console.WriteLine("Writing stills synapse metadata...");
            StillsMetaDataWriter writer =
                new StillsMetaDataWriter(OutputBaseName, selected);
            writer.Write();

        }
    }
}
