using System;
using System.Collections.Generic;
using System.Drawing;
using System.Text;

namespace Mandelbrot
{
    public class ImageSaver
    {
        public int ImageWidth { get; }
        public int ImageHeight { get; }

        public ImageSaver(int width, int height)
        {
            ImageWidth = width;
            ImageHeight = height;
        }
        public void SaveMandelbrotSet(Color[,] pixelsColors, string outputFilename = "fractal.png")
        {
            using (var picture = new Bitmap(ImageWidth, ImageHeight))
            {
                for (int row = 0; row < ImageWidth; row++)
                {
                    for (int col = 0; col < ImageHeight; col++)
                    {
                        picture.SetPixel(row, col, pixelsColors[row, col]);
                    }
                }

                picture.Save(outputFilename);
            }
        }
    }
}
