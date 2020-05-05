using SynapseGenerator.Misc.AbstractClasses;
using SynapseGenerator.CombineSynapse;
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
            string combineFolderName = args[2];
            Console.WriteLine($"Input video file: {videoFileName}");
            Console.WriteLine($"Input stills folder: {stillsFolderName}");

            Console.WriteLine("==================================");

            // generate video synapse
            Console.WriteLine("Generating video synapse...");
            Driver video = new VideoSynapseDriver(videoFileName);
            video.Execute();

            Console.WriteLine("==================================");

            // generate stills synapse
            Console.WriteLine("Generating stills synapse...");
            Driver stills = new StillsSynapseDriver(stillsFolderName);
            stills.Execute();

            Console.WriteLine("==================================");

            // combining synapse
            Console.WriteLine("Combining stills synapse...");
            Driver combine = new CombineSynapseDriver(combineFolderName);
            combine.Execute();
        }
    }
}
