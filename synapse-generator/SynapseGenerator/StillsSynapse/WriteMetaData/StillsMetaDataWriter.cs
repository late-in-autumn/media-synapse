using Newtonsoft.Json;
using SynapseGenerator.Misc.Constants;
using SynapseGenerator.Misc.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SynapseGenerator.StillsSynapse.WriteMetaData
{
    class StillsMetaDataWriter
    {
        // the name of the input folder
        private readonly string InputBaseName;
        // where the synapse metadata will be saved
        private readonly string OutputBaseName;
        // the stills selected to be included in the synapse
        private readonly List<string> SynapseStills;

        private string SerializedMeta;

        public StillsMetaDataWriter(string inputBaseName,
            string outputBaseName,
            List<string> synapseStills)
        {
            if (String.IsNullOrWhiteSpace(inputBaseName))
            {
                throw new ArgumentException("message", nameof(inputBaseName));
            }

            if (String.IsNullOrWhiteSpace(outputBaseName))
            {
                throw new ArgumentException("message", nameof(outputBaseName));
            }

            InputBaseName = inputBaseName;
            OutputBaseName = outputBaseName;
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
            // extract only the basename of source files
            List<string> SynapseStillsBaseName = new List<string>();
            foreach (var s in SynapseStills)
                SynapseStillsBaseName.Add(Path.GetFileNameWithoutExtension(s));

            // build our data structure from input
            IndividualSynapseStruct meta = new IndividualSynapseStruct()
            {
                SourceType = "stills",
                SourceFileName = InputBaseName,
                ImageWidth = Constants.SYNAPSE_WIDTH * SynapseStills.LongCount(),
                NumberOfShots = SynapseStillsBaseName.LongCount(),
                ShotFileNames = SynapseStillsBaseName
            };

            // serialize our data structure
            SerializedMeta = JsonConvert.SerializeObject(meta);
        }

        private void WriteSerializedMetaData()
        {
            if (String.IsNullOrWhiteSpace(SerializedMeta))
                // empty JSON for empty synapse 
                File.WriteAllText($"{OutputBaseName}.json", String.Empty);
            else
                //serialize our data structure into JSON
                File.WriteAllText($"{OutputBaseName}.json", SerializedMeta);
        }
    }
}
