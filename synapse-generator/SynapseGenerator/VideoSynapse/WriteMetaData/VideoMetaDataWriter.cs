using Microsoft.VisualBasic.FileIO;
using Newtonsoft.Json;
using SynapseGenerator.Misc.DataStructures;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace SynapseGenerator.VideoSynapse.WriteMetaData
{
    class VideoMetaDataWriter
    {
        // the directory where the video file is located
        private readonly string FolderName;
        // the basename of the video file
        private readonly string BaseName;

        private IndividualSynapseStruct SynapseMeta;
        private string SerializedMeta;

        public VideoMetaDataWriter(string inputFileName)
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
        }

        public void Write()
        {
            ReadSceneDetectorInput();
            SerializeSynapseMetaData();
            WriteSerializedMetaData();
        }

        private void ReadSceneDetectorInput()
        {
            // parse output from PySceneDetect
            List<long> frameNumbers = new List<long>();
            TextFieldParser parser = new TextFieldParser(
                Path.Join(FolderName, BaseName, "scenes.csv"));
            parser.TextFieldType = FieldType.Delimited;
            parser.SetDelimiters(",");
            for (int i = 0; i < 2; i++) parser.ReadLine();
            while (!parser.EndOfData)
            {
                string[] currentRow = parser.ReadFields();
                frameNumbers.Add(long.Parse(currentRow[1]));
            }

            // trun the parsed information into our data structure
            SynapseMeta = new IndividualSynapseStruct()
            {
                SourceType = "video",
                SourceFileName = BaseName,
                ImageWidth = Misc.Constants.Constants.SYNAPSE_WIDTH * frameNumbers.LongCount(),
                NumberOfScenes = frameNumbers.LongCount(),
                SceneStartFrameNumbers = frameNumbers
            };
        }

        private void SerializeSynapseMetaData()
        {
            // empty JSON for empty synapse
            if (SynapseMeta is null) SerializedMeta = String.Empty;
            // serialize our data structure into JSON
            else SerializedMeta = JsonConvert.SerializeObject(SynapseMeta);
        }

        private void WriteSerializedMetaData()
        {
            File.WriteAllText(Path.Join(FolderName, $"{BaseName}_synapse.json"), SerializedMeta);
        }
    }

}
