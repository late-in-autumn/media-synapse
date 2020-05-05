using System.Collections.Generic;

namespace SynapseGenerator.Misc.DataStructures
{
    class CombinedSynapseStruct
    {
        public long NumberOfSources { get; set; }

        public long TotalWidth { get; set; }

        public List<long> SynapseBoundries { get; set; }

        public List<IndividualSynapseStruct> IndividualSources { get; set; }
    }
}
