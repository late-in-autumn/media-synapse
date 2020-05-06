using SynapseGenerator.CombineSynapse;
using SynapseGenerator.Misc.Constants;
using SynapseGenerator.StillsSynapse;
using SynapseGenerator.VideoSynapse;
using System;
using System.IO;

namespace SynapseGenerator
{
    class Program
    {
        private static readonly string STILLS_FOLDER = "image";
        private static readonly string VIDEO_FILES = "*.mkv";

        static void Main(string[] args)
        {
            // parse input
            string inputFolder = args[0];
            string outputFolder = args[1];
            Console.WriteLine($"Media file input folder: {inputFolder}");
            Console.WriteLine($"Synapse output folder: {outputFolder}");

            // enumerating video files
            string[] videoFiles = Directory.GetFiles(inputFolder, VIDEO_FILES);

            Console.WriteLine("==================================");

            // generate video synapse
            Console.WriteLine("Generating video synapse...");
            foreach (var v in videoFiles)
            {
                var video = new VideoSynapseDriver(v);
                video.Execute();
            }

            Console.WriteLine("==================================");

            // generate stills synapse
            Console.WriteLine("Generating stills synapse...");
            var stills = new StillsSynapseDriver(Path.Join(inputFolder, STILLS_FOLDER));
            stills.Execute();

            Console.WriteLine("==================================");

            // combining synapse
            Console.WriteLine("Combining stills synapse...");
            var combine = new CombineSynapseDriver(inputFolder);
            combine.Execute();

            // copy generated file
            Console.WriteLine("Writing output files...");
            foreach (var ext in new string[] { ".json", ".png" })
                File.Copy(Path.Join(inputFolder, Constants.COMBINED_SYNAPSE_BASENAME + ext),
                    Path.Join(outputFolder, Constants.COMBINED_SYNAPSE_BASENAME + ext), true);
        }
    }
}
