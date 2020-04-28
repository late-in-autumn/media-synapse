using SynapseGenerator.VideoSynapse.BuildSynapse;
using SynapseGenerator.VideoSynapse.DetectScene;
using SynapseGenerator.VideoSynapse.WriteMetaData;
using System;

namespace SynapseGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            // parse input
            string fileName = args[0];
            Console.WriteLine($"Input video file: {fileName}");

            // analyze the input video
            SceneDetector detector = new SceneDetector(fileName);
            Console.WriteLine("Analyzing input video file, this may take a long time...");
            detector.Detect();

            // build the synapse image
            SynapseBuilder builder = new SynapseBuilder(fileName);
            Console.WriteLine("Generating synapse image...");
            builder.Generate();

            // write the metadata
            MetaDataWriter writer = new MetaDataWriter(fileName);
            Console.WriteLine("Writing synapse metadata...");
            writer.Write();
        }
    }
}
