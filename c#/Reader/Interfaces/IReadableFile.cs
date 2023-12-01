using CSharp.Element;

namespace CSharp.Reader.Interfaces;

public interface IReadableFile 
{
    IEnumerable<string> ReadFile();
    Task<IEnumerable<string>> ReadFileAsync();
    Column GetColumn();
    IEnumerable<Row> GetRows();
    Task<IEnumerable<Row>> GetRowsAsync();

}
