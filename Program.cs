using System;
using System.Windows.Forms;
using OpenCvSharp;
using System.Drawing;
using System.Diagnostics;
using System.Threading;

namespace CSharpGTA
{
    internal class Program
    {
        private static string cvWindow = "GTA V Screen";
        private static int totalUpdates;

        public static Mat convertImage(Mat origional)
        {
            
            Mat newMat = new Mat();
            origional.CopyTo(newMat);
            Cv2.Canny(origional, newMat, 200, 300);
            return newMat;
        }


        private static void Main(string[] args)
        {
            totalUpdates = 0;
            //OpenCvSharp.Cuda.DeviceInfo info = new OpenCvSharp.Cuda.DeviceInfo();
            //Console.WriteLine(info.Name);
            ScreenCapture.Start(760,640);

            //ImageViewer viewer = new ImageViewer();
            //viewer.Show();

            Console.WriteLine("Press ESC to stop");

            Stopwatch timer = new Stopwatch();
            timer.Start();

            do
            {
                while (!Console.KeyAvailable)
                {
                    if (ScreenCapture.mat != null)// && sw.ElapsedMilliseconds > ms)
                    {
                        Mat processedMat = convertImage(ScreenCapture.mat);
                        Thread.Sleep(1);
                        Cv2.ImShow(cvWindow, processedMat);
                        Cv2.WaitKey(1);
                        totalUpdates++;
                    }
                }
            } while (Console.ReadKey(true).Key != ConsoleKey.Escape);

            timer.Stop();
            Console.WriteLine("CV Thread Results:");
            Console.WriteLine("Total updates: {0} Total time: {1}s", totalUpdates, Math.Round(timer.ElapsedMilliseconds / 1000f));
            Console.WriteLine("Updates/second: {0}", totalUpdates / (timer.ElapsedMilliseconds / 1000f));

            ScreenCapture.Stop();
            Cv2.DestroyAllWindows();
            Console.ReadKey();
            Environment.Exit(0);
        }
    }
}