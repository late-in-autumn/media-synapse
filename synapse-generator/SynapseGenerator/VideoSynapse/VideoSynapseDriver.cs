using SynapseGenerator.VideoSynapse.BuildSynapse;
using SynapseGenerator.VideoSynapse.DetectScene;
using SynapseGenerator.VideoSynapse.WriteMetaData;
using System;
using System.Collections.Generic;
using System.Text;

namespace SynapseGenerator.VideoSynapse
{
    class VideoSynapseDriver
    {
        private readonly string InputFileName;

        public VideoSynapseDriver(string inputFileName)
        {
            InputFileName =
                inputFileName ?? throw new ArgumentNullException(nameof(inputFileName));
        }

        public void Execute()
        {
            // analyze the input video
            Console.WriteLine("Analyzing input video file, this may take a long time...");
            VideoSceneDetector detector = new VideoSceneDetector(InputFileName);
            detector.Detect();

            // build the synapse image
            Console.WriteLine("Generating video synapse image...");
            VideoSynapseBuilder builder = new VideoSynapseBuilder(InputFileName);
            builder.Build();

            // write the metadata
            Console.WriteLine("Writing video synapse metadata...");
            VideoMetaDataWriter writer = new VideoMetaDataWriter(InputFileName);
            writer.Write();
        }
    }
}
