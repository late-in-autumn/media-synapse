using Newtonsoft.Json;
using SynapseGenerator.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SynapseGenerator.CombineSynapse.CombineSynapseMetaData
{
    class SynapseMetaDataCombiner
    {
        private static readonly string VIDEO_SYNAPSE_FILENAME = "video_*_synapse.json";
        private static readonly string STILLS_SYNAPSE_FILENAME = "stills_synapse.json";

        private readonly string SynapseFolder;

        private CombinedSynapseStruct combined;

        public SynapseMetaDataCombiner(string synapseFolder)
        {
            if (String.IsNullOrWhiteSpace(synapseFolder))
            {
                throw new ArgumentException("message", nameof(synapseFolder));
            }

            SynapseFolder = synapseFolder;
        }

        public void Combine()
        {
            BuildCombinedSynapseMetaData();
            WriteCombinedSynapseMetaData();
        }

        private void BuildCombinedSynapseMetaData()
        {
            long totalWidth = 0;
            List<long> synapseBoundries = new List<long>();
            List<IndividualSynapseStruct> individualSources = new List<IndividualSynapseStruct>();
            string[] videoSynapseFiles = Directory.GetFiles(SynapseFolder, VIDEO_SYNAPSE_FILENAME);
            string[] stillsSynapseFiles = Directory.GetFiles(SynapseFolder, STILLS_SYNAPSE_FILENAME);

            foreach (var v in videoSynapseFiles)
            {
                var syn
                    = JsonConvert.DeserializeObject<IndividualSynapseStruct>(File.ReadAllText(v));
                individualSources.Add(syn);
                synapseBoundries.Add(totalWidth);
                totalWidth += syn.ImageWidth;
            }

            foreach (var s in stillsSynapseFiles)
            {
                var syn
                    = JsonConvert.DeserializeObject<IndividualSynapseStruct>(File.ReadAllText(s));
                individualSources.Add(syn);
                synapseBoundries.Add(totalWidth);
                totalWidth += syn.ImageWidth;
            }

            combined = new CombinedSynapseStruct()
            {
                NumberOfSources = individualSources.LongCount(),
                TotalWidth = totalWidth,
                SynapseBoundries = synapseBoundries,
                IndividualSources = individualSources
            };
        }

        private void WriteCombinedSynapseMetaData()
        {
            string serialized
                = combined is null ? String.Empty : JsonConvert.SerializeObject(combined);
            File.WriteAllText(Path.Join(SynapseFolder, "combined_synapse.json"), serialized);
        }
    }
}
