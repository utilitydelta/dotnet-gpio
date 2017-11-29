using System.IO;
using UtilityDelta.Gpio.Interfaces;

namespace UtilityDelta.Gpio.Implementation
{
    public class FileIo : IFileIo
    {
        public void WriteAllText(string path, string contents)
        {
            File.WriteAllText(path, contents);
        }

        public string ReadAllText(string path) => File.ReadAllText(path);
    }
}