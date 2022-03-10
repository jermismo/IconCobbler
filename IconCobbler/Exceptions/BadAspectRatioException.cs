using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace IconCobbler.Exceptions
{
    public class BadAspectRatioException : Exception
    {
        public string FilePath { get; set; }

        public BadAspectRatioException(string filePath)
            : base($"'{filePath}' did not have a square aspect ratio.")
        {
            FilePath = filePath;
        }

        public BadAspectRatioException(string filePath, string message)
            : base(message)
        {
            FilePath = filePath;
        }
    }
}
