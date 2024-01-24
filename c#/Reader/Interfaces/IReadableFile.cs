using CSharp.Element;

namespace CSharp.Reader.Interfaces;

public interface IReadableFile
{
    IEnumerable<string> ReadFile();
    Column GetColumn();
    IEnumerable<Row> GetRows();
    
}