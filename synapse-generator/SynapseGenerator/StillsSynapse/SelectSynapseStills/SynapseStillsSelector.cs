using Emgu.CV;
using Emgu.CV.Structure;
using System;
using System.Collections.Generic;
using System.IO;

namespace SynapseGenerator.StillsSynapse.SelectSynapseStills
{
    class SynapseStillsSelector
    {
        // only accept JPEGs for stills synapse for now
        private static readonly string INPUT_IMGS = "*.jpg";
        // threshold for histogram similarity
        private static readonly double HISTOGRAM_SIMILARITY = 0.24;

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
            GroupAndSelectImagesByHistogram();
            return SelectedImages;
        }

        private void GroupAndSelectImagesByHistogram()
        {
            // selection method: group by histogram, and select the first image of each group
            string[] input = Directory.GetFiles(InputFolder, INPUT_IMGS);
            List<(string fileName, bool visited)> images = new List<(string, bool)>();
            foreach (var i in input)
                images.Add((fileName: i, visited: false));
            List<List<string>> imageGroups = new List<List<string>>();

            for (int i = 0; i < images.Count; i++)
            {
                if (!images[i].visited)
                {
                    List<string> group = new List<string>
                    {
                        images[i].fileName
                    };
                    images[i] = (images[i].fileName, true);
                    for (int j = 0; j < images.Count; j++)
                    {
                        if (CompareHistogram(images[i].fileName, images[j].fileName))
                        {
                            group.Add(images[j].fileName);
                            images[j] = (images[j].fileName, true);
                        }
                    }
                    imageGroups.Add(group);
                }
            }

            foreach (var g in imageGroups)
                SelectedImages.Add(g[0]);
        }

        private bool CompareHistogram(string imgOne, string imgTwo)
        {
            // based on the outdated code from: http://www.emgu.com/forum/viewtopic.php?t=2956
            var imageOne = new Image<Rgb, byte>(imgOne);
            var imageTwo = new Image<Rgb, byte>(imgTwo);

            // separate image one by channel
            DenseHistogram imageOneBlueHist = new DenseHistogram(64, new RangeF(0, 64));
            DenseHistogram imageOneGreenHist = new DenseHistogram(64, new RangeF(0, 64));
            DenseHistogram imageOneRedHist = new DenseHistogram(64, new RangeF(0, 64));
            Image<Gray, Byte> imageOneBlue = imageOne[2];
            Image<Gray, Byte> imageOneGreen = imageOne[1];
            Image<Gray, Byte> imageOneRed = imageOne[0];

            // calculate histogram of image one
            imageOneBlueHist.Calculate(new Image<Gray, Byte>[] { imageOneBlue }, true, null);
            imageOneGreenHist.Calculate(new Image<Gray, Byte>[] { imageOneGreen }, true, null);
            imageOneRedHist.Calculate(new Image<Gray, Byte>[] { imageOneRed }, true, null);

            // separate image two by channels
            DenseHistogram imageTwoBlueHist = new DenseHistogram(64, new RangeF(0, 64));
            DenseHistogram imageTwoGreenHist = new DenseHistogram(64, new RangeF(0, 64));
            DenseHistogram imageTwoRedHist = new DenseHistogram(64, new RangeF(0, 64));
            Image<Gray, Byte> imageTwoBlue = imageTwo[2];
            Image<Gray, Byte> imageTwoGreen = imageTwo[1];
            Image<Gray, Byte> imageTwoRed = imageTwo[0];

            // calculate histogram of image two
            imageTwoBlueHist.Calculate(new Image<Gray, Byte>[] { imageTwoBlue }, true, null);
            imageTwoGreenHist.Calculate(new Image<Gray, Byte>[] { imageTwoGreen }, true, null);
            imageTwoRedHist.Calculate(new Image<Gray, Byte>[] { imageTwoRed }, true, null);

            // comparing histograms
            double blueResult
                = CvInvoke.CompareHist(
                    imageTwoBlueHist, imageOneBlueHist, Emgu.CV.CvEnum.HistogramCompMethod.Correl);
            double greenResult
                = CvInvoke.CompareHist(
                    imageTwoGreenHist, imageOneGreenHist, Emgu.CV.CvEnum.HistogramCompMethod.Correl);
            double redResult
                = CvInvoke.CompareHist(
                    imageTwoRedHist, imageOneRedHist, Emgu.CV.CvEnum.HistogramCompMethod.Correl);

            // judge the comparison result
            if ((blueResult * greenResult * redResult) >= HISTOGRAM_SIMILARITY)
                return true;
            else
                return false;
        }
    }
}
