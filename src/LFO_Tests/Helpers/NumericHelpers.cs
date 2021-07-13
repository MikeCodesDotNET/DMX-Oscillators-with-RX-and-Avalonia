using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LFO_Tests.Helpers
{
    public static class NumericHelpers
    {
        /// <summary>
        /// Scales a value to a required range.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="outputLow"></param>
        /// <param name="outputHigh"></param>
        /// <returns></returns>
        public static double Scale(double input, double outputLow, double outputHigh, double inputLow = 0, double inputHigh = 1)
        {
            return (double)(((input - inputLow) / (inputHigh - inputLow)) * (outputHigh - outputLow) + outputLow);
        }

        /// <summary>
        /// Scales a value as a percent without any decimal numbers. Range is 0 through 100.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="inputLow"></param>
        /// <param name="inputHigh"></param>
        /// <returns></returns>
        public static double ScaleToPercentage(double input, double inputLow = 0, double inputHigh = 1)
        {
            return Scale(input, 0, 100, inputLow, inputHigh);
        }


        /// <summary>
        /// Scales a value as an 8-bit DMX value. The range is from 0 through 255.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="inputLow"></param>
        /// <param name="inputHigh"></param>
        /// <returns></returns>
        public static byte ScaleTo8Bit(double input, double inputLow = 0, double inputHigh = 1)
        {
            return (byte)Scale(input, 0, byte.MaxValue, inputLow, inputHigh);
        }

        /// <summary>
        /// Scales a value as a 8-bit Hexadecimal number. The range is from 0 through FF.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="inputLow"></param>
        /// <param name="inputHigh"></param>
        /// <returns></returns>
        public static string ScaleTo8BitHex(double input, double inputLow = 0, double inputHigh = 1)
        {
            var scaledInput = ScaleTo8Bit(input, inputLow, inputHigh);
            return scaledInput.ToString("X");
        }

        /// <summary>
        /// Scales a value as an 16-bit DMX value. The range is from 0 through 65 535.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="inputLow"></param>
        /// <param name="inputHigh"></param>
        /// <returns></returns>
        public static short ScaleTo16Bit(double input, double inputLow = 0, double inputHigh = 1)
        {
            return (short)Scale(input, 0, short.MaxValue, inputLow, inputHigh);
        }

        /// <summary>
        /// Scales a value as a 16-bit Hexadecimal number. The range is from 0 through FFFF.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="inputLow"></param>
        /// <param name="inputHigh"></param>
        /// <returns></returns>
        public static string ScaleTo16BitHex(double input, double inputLow = 0, double inputHigh = 1)
        {
            var scaledInput = ScaleTo16Bit(input, inputLow, inputHigh);
            return scaledInput.ToString("X");
        }





        public static int Clamp(this int input, int min, int max)
        {
            var result = input;
            if (input < min)
                result = min;
            if (input > max)
                result = max;

            return result;
        }

        public static short Clamp(this short input, short min, short max)
        {
            var result = input;
            if (input < min)
                result = min;
            if (input > max)
                result = max;

            return result;
        }

        public static byte Clamp(this byte input, byte min, byte max)
        {
            var result = input;
            if (input < min)
                result = min;
            if (input > max)
                result = max;

            return result;
        }

        public static float Clamp(this float input, float min, float max)
        {
            var result = input;
            if (input < min)
                result = min;
            if (input > max)
                result = max;

            return result;
        }

        public static double Clamp(this double input, double min, double max)
        {
            var result = input;
            if (input < min)
                result = min;
            if (input > max)
                result = max;

            return result;
        }


        public static bool InRange(this double input, double min, double max)
        {
            if(input < min || input > max)
                return false;
            return true;
        }


        public static bool LessThan(this double value1, double value2) => value1 < value2 && !AboutEqual(value1, value2);

        public static bool LessThanOrClose(this double value1, double value2) => value1 < value2 || AboutEqual(value1, value2);

        public static bool GreaterThan(this double value1, double value2) => value1 > value2 && !AboutEqual(value1, value2);

        public static bool GreaterThanOrClose(this double value1, double value2) => value1 > value2 || AboutEqual(value1, value2);


        const double D_EPSILON = 1E-03;

        /// <summary>
        /// Compare two double taking in account the double precision potential error.
        /// Take care: truncation errors accumulate on calculation. More you do, more you should increase the epsilon.
        public static bool AboutEqual(this double lhs, double rhs)
        {
            double epsilon = Math.Max(Math.Abs(lhs), Math.Abs(rhs)) * D_EPSILON;
            return Math.Abs(lhs - rhs) <= epsilon;
        }

        /// <summary>
        /// Compare two double taking in account the double precision potential error.
        /// Take care: truncation errors accumulate on calculation. More you do, more you should increase the epsilon.
        /// You get really better performance when you can determine the contextual epsilon first.
        /// </summary>
        /// <param name="value1"></param>
        /// <param name="value2"></param>
        /// <param name="precalculatedContextualEpsilon"></param>
        /// <returns></returns>
        public static bool AboutEqual(this double lhs, double rhs, double contextualEpsilon)
        {
            return Math.Abs(lhs - rhs) <= contextualEpsilon;
        }

        public static bool IsZero(double value) => Math.Abs(value) < D_EPSILON;



        public static double GetContextualEpsilon(this double biggestPossibleContextualValue)
        {
            return biggestPossibleContextualValue * D_EPSILON;
        }
    }
}
