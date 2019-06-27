using ImageMagick;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Numerics;
using System.Threading.Tasks;

namespace Mandelbrot
{

    class Program
    {
        static void Main(string[] args)
        {
            // Skips console and creates picture automatically.

            var mandelbrotSetGenerator = new MandelbrotSetGenerator() { MaxDegreeOfParallelism = 4, MagnitudeLimit = 640 };
            mandelbrotSetGenerator.ComputeColorsMatrix();

            var imageSaver = new ImageSaver(mandelbrotSetGenerator.ImageWidth, mandelbrotSetGenerator.ImageHeight);
            imageSaver.SaveMandelbrotSet(mandelbrotSetGenerator.ColorsMatrix, "fractal.png");
        }
    }
}

