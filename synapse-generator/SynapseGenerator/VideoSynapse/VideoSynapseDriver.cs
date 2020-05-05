using SynapseGenerator.Misc.AbstractClasses;
using SynapseGenerator.VideoSynapse.BuildSynapse;
using SynapseGenerator.VideoSynapse.DetectScene;
using SynapseGenerator.VideoSynapse.SelectFrames;
using SynapseGenerator.VideoSynapse.WriteMetaData;
using System;
using System.IO;
using System.Threading.Tasks;

namespace SynapseGenerator.VideoSynapse
{
    class VideoSynapseDriver : Driver
    {
        private static readonly string SCENE_CLIPS = "*.mkv";

        private readonly string InputFileName;
        private readonly string IntermediateFolder;

        public VideoSynapseDriver(string inputFileName)
        {
            InputFileName =
                inputFileName ?? throw new ArgumentNullException(nameof(inputFileName));
            IntermediateFolder = Path.Join(
                    String.IsNullOrWhiteSpace(Path.GetDirectoryName(inputFileName)) ?
                    String.Empty : Path.GetDirectoryName(inputFileName),
                    Path.GetFileNameWithoutExtension(inputFileName));
            Directory.CreateDirectory(Path.Join(IntermediateFolder, "frames"));
        }

        public override void Execute()
        {
            // analyze the input video
            Console.WriteLine("Analyzing input video file, this may take a long time...");
            VideoSceneDetector detector = new VideoSceneDetector(InputFileName);
            detector.Detect();

            // selecting synapse frame for each scene
            Console.WriteLine("Selecting frames to be included in the synapse image...");
            string[] sceneClips = Directory.GetFiles(
                Path.Join(IntermediateFolder, "scenes"), SCENE_CLIPS);
            Parallel.ForEach(sceneClips, c =>
            {
                VideoFrameSelector selector = new VideoFrameSelector(
                    c, Path.Join(
                        IntermediateFolder, "frames", $"{Path.GetFileNameWithoutExtension(c)}.png"));
                selector.Select();
            });

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
