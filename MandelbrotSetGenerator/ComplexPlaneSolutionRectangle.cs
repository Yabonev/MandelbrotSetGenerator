using System;
using System.Collections.Generic;
using System.Numerics;
using System.Text;

namespace Mandelbrot
{
    public class ComplexPlaneSolutionRectangle
    {
        private double imageWidth;
        private double imageHeight;

        public double RealLineStart { get; set; }
        public double RealLineEnd { get; set; }
        public double ImaginaryLineStart { get; set; }
        public double ImaginaryLineEnd { get; set; }

        // Good default values for a centralized image.
        public ComplexPlaneSolutionRectangle(int width = 640,
                                             int height = 480,
                                             double realLineStart = -2.0,
                                             double realLineEnd = 2.0,
                                             double imaginaryLineStart = -1.0,
                                             double imaginaryLineEnd = 1.0)
        {
            imageWidth = (double)width;
            imageHeight = (double)height;
            RealLineStart = realLineStart;
            RealLineEnd = realLineEnd;
            ImaginaryLineStart = imaginaryLineStart;
            ImaginaryLineEnd = imaginaryLineEnd;
        }
        public void SetNewDimensions(double[] newDimensions)
        {
            if (newDimensions.Length != 4)
            {
                throw new InvalidOperationException("Please enter 4 values. Two for each dimension's start and end.");
            }

            RealLineStart = newDimensions[0];
            RealLineEnd = newDimensions[1];
            ImaginaryLineStart = newDimensions[2];
            ImaginaryLineEnd = newDimensions[3];
        }
        private double GetRealLineMargin()
        {
            return RealLineEnd - RealLineStart;
        }
        private double GetImaginaryLineMargin()
        {
            return ImaginaryLineEnd - ImaginaryLineStart;
        }
        public Complex TranslatePixelToComplexNumber(int pixelX, int pixelY)
        {
            double realPart = RealLineStart + (((double)pixelX / imageWidth) * GetRealLineMargin());
            double imaginaryPart = ImaginaryLineStart + (((double)pixelY / imageHeight) * GetImaginaryLineMargin());

            return new Complex(realPart, imaginaryPart);
        }
    }
}
