using System;
using System.IO;

namespace SynapseGenerator.DetectScene
{
    class SceneDetector
    {
        // executable name
        private static readonly string EXE_NAME = @"scenedetect.exe";
        // executable arguments template
        private static readonly string EXE_ARG_TEMPLATE = @"-i {0}/{1}{2} -s {0}/{1}/stats.csv detect-content -t {3} -m {4} list-scenes -f {0}/{1}/scenes.csv save-images -p -c 9 -n 2 -o {0}/{1}/imgs split-video --copy -o {0}/{1}/scenes";
        // scene detection threshold
        private static readonly string SCENE_DETECTION_THRESHOLD = "16";
        // min scene length
        private static readonly string MIN_SCENE_LENGTH = "00:00:06";

        // the directory where the video file is located
        private readonly string DirName;
        // the basename of the video file
        private readonly string BaseName;
        // the extension of the video file
        private readonly string ExtName;

        public SceneDetector(string fileName)
        {
            DirName =
                String.IsNullOrWhiteSpace(Path.GetDirectoryName(fileName)) ?
                String.Empty : Path.GetDirectoryName(fileName);
            BaseName =
                Path.GetFileNameWithoutExtension(fileName);
            ExtName =
                Path.GetExtension(fileName);
        }

        public void Detect()
        {
            // invode PySceneDetect to analyze the video file
            using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
            {
                proc.StartInfo.FileName = EXE_NAME;
                proc.StartInfo.Arguments =
                    String.Format(EXE_ARG_TEMPLATE, 
                    DirName, BaseName, ExtName, 
                    SCENE_DETECTION_THRESHOLD, MIN_SCENE_LENGTH);
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                string output = proc.StandardOutput.ReadToEnd();
                proc.WaitForExit();
                Console.WriteLine(output);
            }
        }
    }
}
