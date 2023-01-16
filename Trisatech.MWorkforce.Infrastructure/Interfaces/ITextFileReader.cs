namespace Trisatech.MWorkforce.Infrastructure.Interface
{
    public interface ITextFileReader
    {
        IEnumerable<Dictionary<string, object>> Read(string path, string delimiter, int totalColumn);
    }
}
