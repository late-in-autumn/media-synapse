using Newtonsoft.Json;
using SynapseGenerator.Misc.Constants;
using SynapseGenerator.Misc.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SynapseGenerator.CombineSynapse.CombineSynapseMetaData
{
    class SynapseMetaDataCombiner
    {
        private static readonly string FILE_EXT = ".json";

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
            string[] videoSynapseFiles = Directory.GetFiles(
                SynapseFolder, Constants.VIDEO_SYNAPSE_BASENAME + FILE_EXT);
            string[] stillsSynapseFiles = Directory.GetFiles
                (SynapseFolder, Constants.STILLS_SYNAPSE_BASENAME + FILE_EXT);

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
            File.WriteAllText(
                Path.Join(
                    SynapseFolder, Constants.COMBINED_SYNAPSE_BASENAME + FILE_EXT),
                serialized);
        }
    }
}
