using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IconCobbler.Exceptions
{
    public class ImageTooSmallException : Exception
    {
        public string FilePath { get; set; }

        public ImageTooSmallException(string filePath)
            : base($"'{filePath}' was smaller than 16x16 pixels.")
        {
            FilePath = filePath;
        }

        public ImageTooSmallException(string filePath, string message)
            : base(message)
        {
            FilePath = filePath;
        }
    }
}
