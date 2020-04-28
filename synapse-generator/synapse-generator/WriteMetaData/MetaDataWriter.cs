using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SynapseGenerator.WriteMetaData
{
    class MetaDataWriter
    {
        private class SynapseMetaData
        {
            public string SourceType { get; set; }
            public long ImageWidth { get; set; }
            public long NumberOfScenes { get; set; }
            public List<long> SceneStartFrameNumbers { get; set; }
        }

        // the directory where the video file is located
        private readonly string DirName;
        // the basename of the video file
        private readonly string BaseName;

        private SynapseMetaData SynapseMeta;
        private string SerializedMeta;

        public MetaDataWriter(string fileName)
        {
            DirName =
                String.IsNullOrWhiteSpace(Path.GetDirectoryName(fileName)) ?
                String.Empty : Path.GetDirectoryName(fileName);
            BaseName =
                Path.GetFileNameWithoutExtension(fileName);
        }

        public void Write()
        {
            ReadSceneDetectorInput();
            SerializeSynapseMetaData();
            WriteSerializedMetaData();
        }

        private void ReadSceneDetectorInput()
        {
            List<long> frameNumbers = new List<long>();
            TextFieldParser parser = new TextFieldParser(
                Path.Join(DirName, BaseName, "scenes.csv"));
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");
            for (int i = 0; i < 2; i++) parser.ReadLine();
            while (!parser.EndOfData)
            {
                string[] currentRow = parser.ReadFields();
                frameNumbers.Add(long.Parse(currentRow[1]));
            }
            SynapseMeta = new SynapseMetaData()
            {
                SourceType = "video",
                ImageWidth = 352 * frameNumbers.LongCount(),
                NumberOfScenes = frameNumbers.LongCount(),
                SceneStartFrameNumbers = frameNumbers
            };
        }

        private void SerializeSynapseMetaData()
        {
            if (SynapseMeta is null) SerializedMeta = String.Empty;
            else SerializedMeta = JsonConvert.SerializeObject(SynapseMeta);
        }

        private void WriteSerializedMetaData()
        {
            File.WriteAllText(Path.Join(DirName, BaseName, "synapse.json"), SerializedMeta);
        }
    }

}
