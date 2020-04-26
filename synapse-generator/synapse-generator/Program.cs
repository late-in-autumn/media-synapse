using System;

namespace synapse_generator
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = args[0];
            string result = SceneDetect.SceneDetect.DetectScenes(fileName);
            Console.WriteLine(result);
        }
    }
}
