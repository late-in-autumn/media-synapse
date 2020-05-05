using System.Collections.Generic;

namespace SynapseGenerator.Misc.DataStructures
{
    class IndividualSynapseStruct
    {
        // whether the source is video or stills
        public string SourceType { get; set; }

        // the filename of the source
        public string SourceFileName { get; set; }

        // width of the synapse image
        public long ImageWidth { get; set; }

        // number of scenes included
        public long NumberOfScenes { get; set; }

        // number of stills (shots) included in the synapse image
        public long NumberOfShots { get; set; }

        // the starting frame of each scene
        public List<long> SceneStartFrameNumbers { get; set; }

        // the respective filenames of the included stills (shots)
        public List<string> ShotFileNames { get; set; }
    }
}
