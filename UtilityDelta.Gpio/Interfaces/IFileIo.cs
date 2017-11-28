namespace UtilityDelta.Gpio.Interfaces
{
    public interface IFileIo
    {
        void WriteAllText(string path, string contents);
        string ReadAllText(string path);
    }
}