using MediaFileConverter.AbstractClasses;
using MediaFileConverter.ConvertFormats;
using System;

namespace MediaFileConverter
{
    class Program
    {
        static void Main(string[] args)
        {
            // parse input
            string stillsRGBInput = args[0];
            string stillsRGBOutput = args[1];
            string videoSequenceInput = args[2];
            string videoFileOutput = args[3];
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
        }
    }
}
