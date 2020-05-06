using MediaFileConverter.ConvertFormats;
using System;
using System.Collections.Generic;
using System.IO;

namespace MediaFileConverter
{
    class Program
    {
        private static readonly string STILLS_FOLDER_NAME = "image";
        private static readonly string VIDEO_FOLDER_NAME = "video*";
        private static readonly string VIDEO_TEMP_FILE_NAME = "*.jpg";

        static void Main(string[] args)
        {
            // parse input
            string inputFolder = args[0];
            string outputFolder = args[1];
            Console.WriteLine($"Input folder: {inputFolder}");
            Console.WriteLine($"Output folder: {outputFolder}");

            // enumerating video sequences
            string[] videoSequences = Directory.GetDirectories(inputFolder, VIDEO_FOLDER_NAME);
            List<string> videoFileOutputs = new List<string>();
            foreach (var v in videoSequences)
                videoFileOutputs.Add(Path.Join(
                    outputFolder, new DirectoryInfo(v).Name + ".mkv"));

            // prepare output directories
            Directory.CreateDirectory(Path.Join(outputFolder, STILLS_FOLDER_NAME));

            Console.WriteLine("==================================");

            // convert RGB-format meda
            Console.WriteLine("Converting RGB-format inputs to standard formats...");
            var convert = new ConvertFormatsDriver(
                Path.Join(inputFolder, STILLS_FOLDER_NAME),
                Path.Join(outputFolder, STILLS_FOLDER_NAME),
                videoSequences, videoFileOutputs.ToArray());
            convert.Execute();

            Console.WriteLine("==================================");

            // clean up temp files from input
            Console.WriteLine("Cleaning up...");
            foreach (var v in videoSequences)
                foreach (var f in Directory.GetFiles(v, VIDEO_TEMP_FILE_NAME))
                    File.Delete(f);
        }
    }
}
