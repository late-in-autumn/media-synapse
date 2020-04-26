using SynapseGenerator.BuildSynapse;
using SynapseGenerator.SceneDetect;
using System;

namespace SynapseGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = args[0];
            Console.WriteLine($"Input video file: {fileName}");
            SceneDetector detector = new SceneDetector(fileName);
            SynapseBuilder builder = new BuildSynapse.SynapseBuilder(fileName);
            Console.WriteLine("Analyzing input video file, this may take a long time...");
            detector.Detect();
            Console.WriteLine("Generating synapse image...");
            builder.Generate();
        }
    }
}
