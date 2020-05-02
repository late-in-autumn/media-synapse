using Newtonsoft.Json;
using SynapseGenerator.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SynapseGenerator.StillsSynapse.WriteMetaData
{
    class StillsMetaDataWriter
    {
        // where the synapse metadata will be saved
        private readonly string OutputBaseName;
        // the stills selected to be included in the synapse
        private readonly List<string> SynapseStills;

        private string SerializedMeta;

        public StillsMetaDataWriter(
            string outputBaseName,
            List<string> synapseStills)
        {
            if (String.IsNullOrWhiteSpace(outputBaseName))
            {
                throw new ArgumentException("message", nameof(outputBaseName));
            }

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
            // build our data structure from input
            IndividualSynapseStruct meta = new IndividualSynapseStruct()
            {
                SourceType = "stills",
                ImageWidth = Constants.Constants.SYNAPSE_WIDTH * SynapseStills.LongCount(),
                NumberOfShots = SynapseStills.LongCount(),
                ShotFileNames = SynapseStills
            };
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
