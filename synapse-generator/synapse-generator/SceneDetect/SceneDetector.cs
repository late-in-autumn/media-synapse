using System;
using System.IO;

namespace SynapseGenerator.SceneDetect
{
    class SceneDetector
    {
        private static readonly string EXE_NAME = @"scenedetect.exe";
        private static readonly string EXE_ARG_TEMPLATE = @"-i {0}/{1}{2} -s {0}/{1}/scenes.csv detect-content list-scenes -n save-images -p -c 9 -n 1 -o {0}/{1}/imgs split-video --copy -o {0}/{1}/scenes";

        private readonly string DirName;
        private readonly string BaseName;
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
            using (System.Diagnostics.Process proc = new System.Diagnostics.Process())
            {
                proc.StartInfo.FileName = EXE_NAME;
                proc.StartInfo.Arguments =
                    String.Format(EXE_ARG_TEMPLATE, DirName, BaseName, ExtName);
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
