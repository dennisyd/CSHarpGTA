using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using OpenCvSharp;

namespace CSharpGTA
{
    class ScreenCapture
    {
        public static Bitmap currentFrame;
        public static OpenCvSharp.Mat mat;
        public static Mat cvMat { get; private set; } 

        private static int width;
        private static int height;

        private static int totalScreenGrabs;
        private static Stopwatch captureImageTimer = new Stopwatch();

        private static volatile bool isQuitting;
        private static Thread thread;

        public static void Start()
        {
            width = Screen.PrimaryScreen.Bounds.Width;
            height = Screen.PrimaryScreen.Bounds.Height;

            StartThread();
        }

        public static void Start(int x, int y)
        {
            width = x;
            height = y;

            StartThread();
        }

        public static void Stop()
        {
            isQuitting = true;
            thread.Join();

            Console.WriteLine("Screen Capture Thread Reaults: ");
            Console.WriteLine("Total grabs: {0} Total time: {1}s", totalScreenGrabs, Math.Round(captureImageTimer.ElapsedMilliseconds / 1000f));
            Console.WriteLine("Grabs/second: {0}", totalScreenGrabs / (captureImageTimer.ElapsedMilliseconds / 1000f));

        }

        private static void StartThread()
        {
            isQuitting = false;
            totalScreenGrabs = 0;
            thread = new Thread(() =>
            {
                Console.WriteLine("Capturing screens of size: {0}x{1}", width, height);

                captureImageTimer = new Stopwatch();
                captureImageTimer.Start();
                ThreadingMethod();
                captureImageTimer.Stop();
            });
            thread.Start();
            thread.IsBackground = true;
            thread.Name = "Capture Image Thread";
        }

        public static Bitmap captureScreen(ref Bitmap bmp, bool debug)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            //bmp = new Bitmap(width, height, PixelFormat.Format24bppRgb);
            Graphics graphics = Graphics.FromImage(bmp as Image);
            graphics.CopyFromScreen(0, 40, 0, 0, bmp.Size);
            sw.Stop();

            if (debug)
            {
                Console.WriteLine("Screen capture took: {0} ms", sw.ElapsedMilliseconds);
            }
            //bmp.Save("C:\\Users\\danfl\\Desktop\\test.jpeg", ImageFormat.Jpeg);
            totalScreenGrabs++;
            return bmp;
        }

        private static void ThreadingMethod()
        {
            Bitmap bmp = new Bitmap(width, height, PixelFormat.Format32bppArgb);
            while (!isQuitting)
            {
                currentFrame = captureScreen(ref bmp, false);
                mat = OpenCvSharp.Extensions.BitmapConverter.ToMat(currentFrame);
                Thread.Sleep(1);
            }
        }

        public static void Dispose()
        {
            if (thread != null)
            {
                if (thread.IsAlive)
                {
                    thread.Join();
                }
            }
        }
    }
}
