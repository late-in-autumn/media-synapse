using System;
using System.IO;

namespace SynapseGenerator.SceneDetect
{
    class SceneDetect
    {
        private static readonly string EXE_NAME = @"scenedetect.exe";
        private static readonly string EXE_ARG_TEMPLATE = @"-i {0}/{1}{2} -s {0}/{1}/scenes.csv detect-content list-scenes -n save-images -p -c 9 -n 1 -o {0}/{1}/imgs split-video --copy -o {0}/{1}/scenes";

        public static void DetectScenes(string fileName)
        {
            string dirName =
                String.IsNullOrWhiteSpace(Path.GetDirectoryName(fileName)) ?
                String.Empty : Path.GetDirectoryName(fileName);
            string baseName =
                Path.GetFileNameWithoutExtension(fileName);
            string extName =
                Path.GetExtension(fileName);
            using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
            {
                proc.StartInfo.FileName = EXE_NAME;
                proc.StartInfo.Arguments =
                    String.Format(EXE_ARG_TEMPLATE, dirName, baseName, extName);
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                string output = proc.StandardOutput.ReadToEnd();
                proc.WaitForExit();
            }
        }
    }
}
