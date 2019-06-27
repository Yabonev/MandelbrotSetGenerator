using MandelbrotSetGenerator;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;

namespace Mandelbrot
{
    public class MandelbrotSetGenerator
    {
        private const int DefaultImageWidth = 640;
        private const int DefaultImageHeight = 480;
        private const int DefaultMagnitudeLimit = 6;
        private const int DefaultDegreeOfParallelism = 1;
        private const int DefaultIterationsLimit = 256;
        private const int ChunkSegmentWidth = 4;

        public int MaxDegreeOfParallelism { get; set; }
        public int ImageWidth { get; }
        public int ImageHeight { get; }
        public double MagnitudeLimit { get; set; }
        public int IterationsLimit { get; set; }
        public ComplexPlaneSolutionRectangle SolutionRectangle { get; set; }
        public Color[,] ColorsMatrix { get; }

        public MandelbrotSetGenerator()
        {
            SolutionRectangle = new ComplexPlaneSolutionRectangle();
            MaxDegreeOfParallelism = DefaultDegreeOfParallelism;
            ImageWidth = DefaultImageWidth;
            ImageHeight = DefaultImageHeight;
            MagnitudeLimit = DefaultMagnitudeLimit;
            IterationsLimit = DefaultIterationsLimit;
            ColorsMatrix = new Color[ImageWidth, ImageHeight];
        }
        public void ComputeColorsMatrix()
        {
            var chunks = GenerateChunksFromPicture();
            ComputeColorsMatrixFromChunks(chunks);
        }
        private IEnumerable<Chunk> GenerateChunksFromPicture()
        {
            Chunk[] chunks = InitializeChunks().ToArray();

            double realSegmentCount = ImageWidth / ChunkSegmentWidth;
            var segmentsCount = Convert.ToInt32(Math.Ceiling(realSegmentCount));

            int chunkIndex;
            for (var segmentIndex = 0; segmentIndex < segmentsCount; segmentIndex++)
            {
                chunkIndex = segmentIndex % MaxDegreeOfParallelism;
                chunks[chunkIndex].Columns.AddRange(ConvertSegmentIndexToColumnIndices(segmentIndex));
            }

            return chunks;
        }
        private void ComputeColorsMatrixFromChunks(IEnumerable<Chunk> chunks)
        {
            Parallel.ForEach(chunks, (chunk) =>
            {
                ProcessChunk(chunk);
            });
        }
        private void ProcessChunk(Chunk chunk)
        {
            foreach (int column in chunk.Columns)
            {
                ProcessColumn(column);
            }
        }
        private void ProcessColumn(int columnIndexInPicture)
        {
            var pixelX = columnIndexInPicture;
            for (var pixelY = 0; pixelY < ImageHeight; pixelY++)
            {
                ProcessPixel(pixelX, pixelY);
            }
        }
        private void ProcessPixel(int pixelX, int pixelY)
        {
            Complex currentlyObservedPoint;
            currentlyObservedPoint = SolutionRectangle.TranslatePixelToComplexNumber(pixelX, pixelY);
            int iterationsDone = GetPointIterations(currentlyObservedPoint);
            ColorsMatrix[pixelX, pixelY] = ComputeColorFromIterationsDone(iterationsDone);
        }
        private IEnumerable<int> ConvertSegmentIndexToColumnIndices(int segmentIndex)
        {
            var indices = new List<int>();

            int processedColumnsCount = segmentIndex * ChunkSegmentWidth;
            int processedColumnsCountAfterConvert = processedColumnsCount + ChunkSegmentWidth;
            int maxColumns = processedColumnsCountAfterConvert > ImageWidth ? ImageWidth : processedColumnsCountAfterConvert;

            for (var i = processedColumnsCount; i < maxColumns; i++)
            {
                indices.Add(i);
            }

            return indices;
        }
        private IEnumerable<Chunk> InitializeChunks()
        {
            var chunks = new List<Chunk>();
            for (var i = 0; i < MaxDegreeOfParallelism; i++)
            {
                chunks.Add(new Chunk());
            }

            return chunks;
        }
        private Color ComputeColorFromIterationsDone(int iterationsDone)
        {
            double normalizedRatio = NormalizeRatio(iterationsDone);
            int rgbValue = RemoveUnevenColorDistributionUsingHyperbolicTangent(normalizedRatio);

            return iterationsDone < IterationsLimit ? Color.FromArgb(rgbValue, rgbValue, rgbValue) : Color.Black;
        }
        private int RemoveUnevenColorDistributionUsingHyperbolicTangent(double normalizedRatio)
        {
            const int multiplier = 2;

            int rgbValue = 255 - (int)(Math.Tanh(multiplier * normalizedRatio) / Math.Tanh(multiplier) * 255);

            return rgbValue;
        }
        private double NormalizeRatio(int iterationsDone)
        {
            double ratio = Math.Sqrt(iterationsDone / (double)(IterationsLimit));
            return (Math.Cos((ratio * Math.PI) + Math.PI) + 1) / 2;
        }
        private int GetPointIterations(Complex currentlyObservedPoint)
        {
            var iterations = 0;
            var center = new Complex(0.0, 0.0);

            do
            {
                iterations++;
                center = Complex.Exp(Complex.Cos(Complex.Multiply(currentlyObservedPoint, center)));

                if (center.Magnitude > MagnitudeLimit) break;
            }
            while (iterations < IterationsLimit);

            return iterations;
        }
    }
}