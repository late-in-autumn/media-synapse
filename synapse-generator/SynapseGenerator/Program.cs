using SynapseGenerator.StillsSynapse;
using SynapseGenerator.VideoSynapse;
using System;

namespace SynapseGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            // parse input
            string videoFileName = args[0];
            string stillsFolderName = args[1];
            string stillsOutputName = stillsFolderName + "_synapse";
            Console.WriteLine($"Input video file: {videoFileName}");
            Console.WriteLine($"Input stills folder: {stillsFolderName}");

            Console.WriteLine("==================================");

            // generate video synapse
            Console.WriteLine("Generating video synapse...");
            VideoSynapseDriver video = new VideoSynapseDriver(videoFileName);
            video.Execute();

            Console.WriteLine("==================================");

            // generate stills synapse
            Console.WriteLine("Generating stills synapse...");
            StillsSynapseDriver stills = new StillsSynapseDriver(stillsFolderName, stillsOutputName);
            stills.Execute();
        }
    }
}
