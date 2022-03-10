using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IconCobbler.Exceptions
{
    public class ImageTooLargeException : Exception
    {
        public string FilePath { get; set; }

        public ImageTooLargeException(string filePath)
            : base($"'{filePath}' was larger than 256x256 pixels.")
        {
            FilePath = filePath;
        }

        public ImageTooLargeException(string filePath, string message)
            : base(message)
        {
            FilePath = filePath;
        }
    }
}
