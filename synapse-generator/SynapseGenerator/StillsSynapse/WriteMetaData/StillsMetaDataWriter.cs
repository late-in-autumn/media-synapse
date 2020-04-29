using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SynapseGenerator.StillsSynapse.WriteMetaData
{
    class StillsMetaDataWriter
    {
        // a data structure that helps with JSON serialization
        private class StillsSynapseMetaData
        {
            // will always be "stills" for this module
            public string SourceType { get; set; }

            // width of the synapse image
            public long ImageWidth { get; set; }

            // number of stills (shots) included in the synapse image
            public long NumberOfShots { get; set; }

            // the respective filenames of the included stills (shots)
            public List<string> ShotFileNames { get; set; }
        }

        // where the synapse metadata will be saved
        private readonly string OutputFolder;
        // the stills selected to be included in the synapse
        private readonly List<string> SynapseStills;

        private string SerializedMeta;

        public StillsMetaDataWriter(
            string outputFolder,
            List<string> synapseStills)
        {
            if (String.IsNullOrWhiteSpace(outputFolder))
            {
                throw new ArgumentException("message", nameof(outputFolder));
            }

            OutputFolder = outputFolder;
            SynapseStills =
                synapseStills ?? throw new ArgumentNullException(nameof(synapseStills));
        }

        public void Write()
        {
            SerializeSynapseMetaData();
            WriteSerializedMetaData();
        }

        private void SerializeSynapseMetaData()
        {
            // build our data structure from input
            StillsSynapseMetaData meta = new StillsSynapseMetaData()
            {
                SourceType = "stills",
                ImageWidth = 352 * SynapseStills.LongCount(),
                NumberOfShots = SynapseStills.LongCount(),
                ShotFileNames = SynapseStills
            };
            SerializedMeta = JsonConvert.SerializeObject(meta);
        }

        private void WriteSerializedMetaData()
        {
            if (String.IsNullOrWhiteSpace(SerializedMeta))
                // empty JSON for empty synapse 
                File.WriteAllText(Path.Join(OutputFolder, "synapse.json"), String.Empty);
            else
                //serialize our data structure into JSON
                File.WriteAllText(Path.Join(OutputFolder, "synapse.json"), SerializedMeta);
        }
    }
}
