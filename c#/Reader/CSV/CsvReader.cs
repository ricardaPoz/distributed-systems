using CSharp.Element;
using CSharp.Reader.Interfaces;

namespace CSharp.Reader.CSV;

public class CsvReader : IReadableFile
{
    private readonly string _path;

    public CsvReader(string path)
    {
        this._path = path;
    }

    public IEnumerable<string> ReadFile()
    {
        return File.ReadAllLines(_path).Where(s => !string.IsNullOrEmpty(s));
    }

    public async Task<IEnumerable<string>> ReadFileAsync()
    {
        var lines = await File.ReadAllLinesAsync(_path);
        return lines.Where(s => !string.IsNullOrEmpty(s));
    }

    public IEnumerable<Row> GetRows()
    {
        return ReadFile().Skip(1).Select(line => new Row(line));
    }

    public async Task<IEnumerable<Row>> GetRowsAsync()
    {
        var lines = await ReadFileAsync();
        return lines.Skip(1).Select(line => new Row(line));
    }

    public Column GetColumn()
    {
        return new Column(ReadFile().First());
    }
}