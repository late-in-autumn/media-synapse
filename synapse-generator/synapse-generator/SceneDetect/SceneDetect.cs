using System;
using System.IO;

namespace SynapseGenerator.SceneDetect
{
    class SceneDetect
    {
        private static readonly string ExeName = @"scenedetect.exe";
        private static readonly string ExeArgTemplate = @"-i {0}/{1}{2} -s {0}/{1}/scenes.csv detect-content list-scenes -n save-images -p -c 9 -n 1 -o {0}/{1}/imgs split-video --copy -o {0}/{1}/scenes";

        public static string DetectScenes(string fileName)
        {
            string dirName =
                String.IsNullOrWhiteSpace(Path.GetDirectoryName(fileName)) ?
                String.Empty : Path.GetDirectoryName(fileName);
            string baseName =
                Path.GetFileNameWithoutExtension(fileName);
            string extName =
                Path.GetExtension(fileName);
            string output = String.Empty;
            using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
            {
                proc.StartInfo.FileName = ExeName;
                proc.StartInfo.Arguments =
                    String.Format(ExeArgTemplate, dirName, baseName, extName);
                proc.StartInfo.UseShellExecute = false;
                proc.StartInfo.RedirectStandardOutput = true;
                proc.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                proc.StartInfo.CreateNoWindow = true;
                proc.Start();
                output = proc.StandardOutput.ReadToEnd();
                proc.WaitForExit();
            }
            return output;
        }
    }
}
