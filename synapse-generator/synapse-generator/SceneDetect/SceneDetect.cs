using System;
using System.IO;

namespace synapse_generator.SceneDetect
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
            using (System.Diagnostics.Process pProcess = new System.Diagnostics.Process())
            {
                pProcess.StartInfo.FileName = ExeName;
                pProcess.StartInfo.Arguments =
                    String.Format(ExeArgTemplate, dirName, baseName, extName);
                pProcess.StartInfo.UseShellExecute = false;
                pProcess.StartInfo.RedirectStandardOutput = true;
                pProcess.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                pProcess.StartInfo.CreateNoWindow = true;
                pProcess.Start();
                output = pProcess.StandardOutput.ReadToEnd();
                pProcess.WaitForExit();
            }
            return output;
        }
    }
}
