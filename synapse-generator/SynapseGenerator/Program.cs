using SynapseGenerator.AbstractClasses;
using SynapseGenerator.CombineSynapse;
using SynapseGenerator.ConvertFormats;
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
            string stillsRGBInput = args[3];
            string stillsRGBOutput = args[4];
            string videoSequenceInput = args[5];
            string videoFileOutput = args[6];
            Console.WriteLine($"Input video file: {videoFileName}");
            Console.WriteLine($"Input stills folder: {stillsFolderName}");
            Console.WriteLine($"RGB-format stills input: {stillsRGBInput}");
            Console.WriteLine($"Converted stills output: {stillsRGBOutput}");
            Console.WriteLine($"RGB-format video sequence input: {videoSequenceInput}");
            Console.WriteLine($"Converted video file output: {videoFileOutput}");

            Console.WriteLine("==================================");

            // convert RGB-format meda
            Console.WriteLine("Converting RGB-format inputs to standard formats");
            Driver convert = new ConvertFormatsDriver(stillsRGBInput, stillsRGBOutput,
                videoSequenceInput, videoFileOutput);
            convert.Execute();

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
