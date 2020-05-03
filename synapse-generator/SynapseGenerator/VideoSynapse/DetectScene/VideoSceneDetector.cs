using System;
using System.Diagnostics;
using System.IO;

namespace SynapseGenerator.VideoSynapse.DetectScene
{
    class VideoSceneDetector
    {
        // executable name
        private static readonly string SCENEDETECT_BIN = @"scenedetect";
        // executable arguments template
        private static readonly string SCENEDENECT_ARG_TEMPLATE = @"-i {0}/{1}{2} -s {0}/{1}/stats.csv detect-content -t {3} -m {4} list-scenes -f {0}/{1}/scenes.csv save-images -p -c 9 -n 2 -o {0}/{1}/imgs split-video --copy -o {0}/{1}/scenes";
        // scene detection threshold
        private static readonly string SCENE_DETECTION_THRESHOLD = "16";
        // min scene length
        private static readonly string MIN_SCENE_LENGTH = "00:00:06";

        // the directory where the video file is located
        private readonly string FolderName;
        // the basename of the video file
        private readonly string BaseName;
        // the extension of the video file
        private readonly string ExtName;

        public VideoSceneDetector(string inputFileName)
        {
            if (String.IsNullOrWhiteSpace(inputFileName))
            {
                throw new ArgumentException("message", nameof(inputFileName));
            }

            FolderName =
                String.IsNullOrWhiteSpace(Path.GetDirectoryName(inputFileName)) ?
                String.Empty : Path.GetDirectoryName(inputFileName);
            BaseName =
                Path.GetFileNameWithoutExtension(inputFileName);
            ExtName =
                Path.GetExtension(inputFileName);
        }

        public void Detect()
        {
            // invode PySceneDetect to analyze the video file
            using var proc = new Process();
            proc.StartInfo.FileName = SCENEDETECT_BIN;
            proc.StartInfo.Arguments =
                String.Format(SCENEDENECT_ARG_TEMPLATE,
                FolderName, BaseName, ExtName,
                SCENE_DETECTION_THRESHOLD, MIN_SCENE_LENGTH);
            proc.StartInfo.UseShellExecute = false;
            proc.StartInfo.RedirectStandardOutput = true;
            proc.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            proc.StartInfo.CreateNoWindow = true;
            proc.Start();
            Console.WriteLine(proc.StandardOutput.ReadToEnd());
            proc.WaitForExit();
        }
    }
}
