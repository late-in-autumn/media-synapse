using SynapseGenerator.StillsSynapse.BuildSynapse;
using SynapseGenerator.StillsSynapse.SelectSynapseStills;
using SynapseGenerator.StillsSynapse.WriteMetaData;
using System;
using System.Collections.Generic;
using System.IO;

namespace SynapseGenerator.StillsSynapse
{
    class StillsSynapseDriver
    {
        private readonly string InputFolder;
        private readonly string OutputFolder;

        public StillsSynapseDriver(string inputFolder, string outputFolder)
        {
            if (String.IsNullOrWhiteSpace(inputFolder))
            {
                throw new ArgumentException("message", nameof(inputFolder));
            }

            if (String.IsNullOrWhiteSpace(outputFolder))
            {
                throw new ArgumentException("message", nameof(outputFolder));
            }

            InputFolder = inputFolder;
            OutputFolder = outputFolder;
            Directory.CreateDirectory(outputFolder);
        }

        public void Execute()
        {
            // select synapse stills
            Console.WriteLine("Selecting synapse stills, this may take a long time...");
            SynapseStillsSelector selector =
                new SynapseStillsSelector(InputFolder);
            List<string> selected = selector.Select();

            // generate synapse image
            Console.WriteLine("Generating stills synapse image...");
            StillsSynapseBuilder builder =
                new StillsSynapseBuilder(OutputFolder, selected);
            builder.Build();

            // write the metadata
            Console.WriteLine("Writing stills synapse metadata...");
            StillsMetaDataWriter writer =
                new StillsMetaDataWriter(OutputFolder, selected);
            writer.Write();

        }
    }
}
