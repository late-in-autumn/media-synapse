using MediaFileConverter.AbstractClasses;
using MediaFileConverter.VideoSynapse.BuildSynapse;
using MediaFileConverter.VideoSynapse.DetectScene;
using MediaFileConverter.VideoSynapse.WriteMetaData;
using System;
using System.IO;

namespace MediaFileConverter.VideoSynapse
{
    class VideoSynapseDriver : Driver
    {
        private readonly string InputFileName;

        public VideoSynapseDriver(string inputFileName)
        {
            InputFileName =
                inputFileName ?? throw new ArgumentNullException(nameof(inputFileName));
            Directory.CreateDirectory(
                Path.Join(
                    String.IsNullOrWhiteSpace(Path.GetDirectoryName(inputFileName)) ?
                    String.Empty : Path.GetDirectoryName(inputFileName),
                    Path.GetFileNameWithoutExtension(inputFileName)));
        }

        public override void Execute()
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
