using SynapseGenerator.BuildSynapse;
using System;

namespace SynapseGenerator
{
    class Program
    {
        static void Main(string[] args)
        {
            string fileName = args[0];
            SceneDetect.SceneDetect.DetectScenes(fileName);
            BuildSynapse.BuildSynapse build = new BuildSynapse.BuildSynapse(fileName);
            build.Generate();
        }
    }
}
