using System;
using System.Collections.Generic;
using System.Drawing;
using System.Dynamic;
using System.Linq;
using System.Reflection;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using static System.Windows.Forms.VisualStyles.VisualStyleElement.ProgressBar;

namespace RumerSpreading.Ver1
{
    public class ConvolutionFilter
    {
        private static int NewMinusCount = 0;
        private static int NewOneCount = 0;
        //private static double UsedValue = 0;

        
        private static object lockObject = new object();
        //public static T Clamp<T>(T value, T min, T max) where T : IComparable<T>
        //{
        //    if (value.CompareTo(min) < 0)
        //    {
        //        return min;
        //    }
        //    else if (value.CompareTo(max) > 0)
        //    {
        //        return max;
        //    }
        //    else
        //    {
        //        return value;
        //    }
        //}

        public static int GetIndex(int value, int min, int max, int length)
        {
            if (value >= min && value <= max)
            {
                return value;
            }
            else if (value < min)
            {
                return length + value;
            }
            else
            {
                return value - length;
            }
        }

        //public static void KernelAlgrithm(int kernelSize, int kernelOffset, int x, int y, int GridWidth, int GridHeight, int[,] gridData, int[,] kernel, int OneCount, int MinusCount)
        //{
        //    for (int ky = 0; ky < kernelSize; ky++)
        //    {
        //        for (int kx = 0; kx < kernelSize; kx++)
        //        {
        //            int XY_Value;

        //            int offsetX = x + kx - kernelOffset;
        //            int offsetY = y + ky - kernelOffset;

        //            offsetX = GetIndex(offsetX, 0, GridWidth - 1, GridWidth);
        //            offsetY = GetIndex(offsetY, 0, GridHeight - 1, GridHeight);

        //            XY_Value = gridData[offsetY, offsetX] * kernel[ky, kx];


        //            if (XY_Value == -1 || XY_Value == -2 || XY_Value == -3)
        //                MinusCount++;
        //            else if (XY_Value == 1)
        //                OneCount++;
        //        }
        //    }
        //}

        public static (int[,] filteredGridData, int MinusCount, int OneCount, double UsedValue)
            ApplyFilter(int[,] gridData, Dictionary<Point, double> _thresholdValue)
        {
            int[,] kernel_81 = new int[,]
            {
                {1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 0, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1, 1, 1},


            };
            int[,] kernel_49 = new int[,]
            {
                {1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 0, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1, 1, 1},

            };
            int[,] kernel_25 = new int[,]
            {
                {1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1},
                {1, 1, 0, 1, 1},
                {1, 1, 1, 1, 1},
                {1, 1, 1, 1, 1},
            };
            int[,] kernel_9 = new int[,]
            {
                {1, 1, 1 },
                {1, 0, 1 },
                {1, 1, 1 },
            };

            int GridWidth = gridData.GetLength(1);
            int GridHeight = gridData.GetLength(0);

            int[,] filteredFrid = new int[GridHeight, GridWidth];
            NewMinusCount = 0;
            NewOneCount = 0;
            double UsedValue = 0;

            ParallelOptions parallelOptions = new ParallelOptions
            {
                MaxDegreeOfParallelism = (int)Math.Ceiling(Environment.ProcessorCount * 0.9)
            };

            Parallel.For(0, GridHeight, (y) =>
            //for (int y = 0; y < GridWidth; y++)
            {
                for (int x = 0; x < GridWidth; x++)
                {

                    int[,] kernel = null;

                    //if (gridData[y, x] == -1)
                    //    kernel = kernel_25;
                    //else if (gridData[y, x] == -2)
                    //    kernel = kernel_25;
                    //else if (gridData[y, x] == -3)
                    //    kernel = kernel_25;

                    kernel = kernel_9;

                    int kernelSize = kernel.GetLength(1);
                    int kernelOffset = kernelSize / 2;

                    int OneCount = 0;
                    int MinusCount = 0;
                    double Believe_idx = 0;

                    if (gridData[y, x] == 0)
                    {
                        filteredFrid[y, x] = gridData[y, x];
                        continue;
                    }

                    else
                    {
                        for (int ky = 0; ky < kernelSize; ky++)
                        {
                            for (int kx = 0; kx < kernelSize; kx++)
                            {
                                int XY_Value;

                                int offsetX = x + kx - kernelOffset;
                                int offsetY = y + ky - kernelOffset;

                                offsetX = GetIndex(offsetX, 0, GridWidth - 1, GridWidth);
                                offsetY = GetIndex(offsetY, 0, GridHeight - 1, GridHeight);

                                XY_Value = gridData[offsetY, offsetX] * kernel[ky, kx];

                                if (XY_Value == -1 || XY_Value == -2 || XY_Value == -3)
                                    MinusCount++;
                                else if (XY_Value == 1)
                                    OneCount++;
                            }
                        }

                        if (gridData[y, x] == 1)
                        {
                            filteredFrid[y, x] = gridData[y, x];
                            if (MinusCount != 0 && MinusCount != 0)
                                lock (lockObject)
                                    UsedValue += (double)OneCount / (OneCount + MinusCount);
                        }

                        else if (gridData[y, x] == -1 || gridData[y, x] == -2 || gridData[y, x] == -3)
                        {
                            double Believe_min = 0;
                            Believe_idx = (double)OneCount / (OneCount + MinusCount);

                            Believe_min = _thresholdValue[new Point(y, x)];
                            if (Believe_idx >= Believe_min)
                            {
                                filteredFrid[y, x] = 1;
                                System.Threading.Interlocked.Increment(ref NewOneCount);

                            }

                            else if (Believe_idx < Believe_min)
                            {
                                filteredFrid[y, x] = gridData[y, x];
                                System.Threading.Interlocked.Increment(ref NewMinusCount);
                            }
                        }

                    }
                }
            });

            return (filteredFrid, NewMinusCount, NewOneCount, UsedValue);
        }
    }
}
