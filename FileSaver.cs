using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace CSharpGTA
{
    static class FileSaver
    {
        /// <summary>
        /// Quickly save a byte[] to a file at a path with a given file name and extension
        /// </summary>
        /// <param name="data">byte[] holding data</param>
        /// <param name="path">folder path</param>
        /// <param name="fName">file name</param>
        /// <param name="fExtension">file extension</param>
        public static string quickSave(byte[] data, string path, string fName, string fExtension)
        {
            if (string.IsNullOrEmpty(path) || string.IsNullOrEmpty(fName))
            {
                Console.WriteLine("bad path or filename");
                return null;
            }
            checkDirectory(path);
            string fullPath = string.Format("{0}\\{1}{2}", path + fName + fExtension);

            saveBytesThread(fullPath, data);
            return fullPath;
        }

        /// <summary>
        /// checks to see if a directory exists on the computer
        /// </summary>
        /// <param name="dir">folder directory</param>
        private static void checkDirectory(string dir)
        {
            if (!Directory.Exists(dir))
            {
                Console.WriteLine(string.Format("Directory not found, creating one for path: {0}", dir));
                Directory.CreateDirectory(dir);
            }
        }

        /// <summary>
        /// Threading method to save file
        /// </summary>
        /// <param name="path">file path</param>
        /// <param name="data">file data</param>
        private static void saveBytesThread(string path, byte[] data)
        {
            new Thread(() =>
            {
                try
                {
                    File.WriteAllBytes(path, data);
                    //Debug.Log(string.Format("Created file at path {0}", path));
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }).Start();
        }
    }
}
