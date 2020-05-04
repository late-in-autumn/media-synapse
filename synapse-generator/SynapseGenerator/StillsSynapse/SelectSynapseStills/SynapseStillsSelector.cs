using System;
using System.Collections.Generic;
using System.IO;

namespace MediaFileConverter.StillsSynapse.SelectSynapseStills
{
    class SynapseStillsSelector
    {
        // only accept JPEGs for stills synapse for now
        private static readonly string INPUT_IMGS = "*.jpg";

        // where the source stills are located
        private readonly string InputFolder;
        
        // list of selected stills for synapse
        private List<string> SelectedImages;

        public SynapseStillsSelector(string inputFolder)
        {
            if (String.IsNullOrWhiteSpace(inputFolder))
            {
                throw new ArgumentException("message", nameof(inputFolder));
            }

            InputFolder = inputFolder;
            SelectedImages = new List<string>();
        }

        public List<string> Select()
        {
            RandomlySelectSynapseStills();
            return SelectedImages;
        }

        private void RandomlySelectSynapseStills()
        {
            // randomly select 9 synapse images for now
            string[] images = Directory.GetFiles(InputFolder, INPUT_IMGS);
            Random random = new Random();
            for (int i = 0; i < 9; i++)
                SelectedImages.Add(images[random.Next(images.Length - 1)]);
        }
    }
}
