using System;
using System.Windows.Forms;
using OpenCvSharp;
using System.Drawing;
using System.Diagnostics;

namespace CSharpGTA
{
    internal class Program
    {
        private static string cvWindow = "GTA V Screen";

        private static void Main(string[] args)
        {
            //OpenCvSharp.Cuda.DeviceInfo info = new OpenCvSharp.Cuda.DeviceInfo();
            //Console.WriteLine(info.Name);
            ScreenCapture.Start(760,640);

            //ImageViewer viewer = new ImageViewer();
            //viewer.Show();

            Console.WriteLine("Press ESC to stop");

            Stopwatch timer = new Stopwatch();
            timer.Start();
            int totalUpdates = 0;
            do
            {
                while (!Console.KeyAvailable)
                {
                    if (ScreenCapture.mat != null)// && sw.ElapsedMilliseconds > ms)
                    {
                        
                        Cv2.ImShow(cvWindow, ScreenCapture.mat);
                        Cv2.WaitKey(1);
                        totalUpdates++;
                    }
                    //ImageViewer.Show(mat, cvWindow);
                    ////viewer.Image = mat;
                    ////viewer.Update();
                    ////ImageViewer.ActiveForm.Update();
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