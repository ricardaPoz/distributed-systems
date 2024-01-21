using System.Collections;
using CSharp.Element;
using CSharp.Reader.Interfaces;

namespace CSharp.Reader.CSV;

public class CsvReader : IReadableFile
{
    private readonly string _path;

    public CsvReader(string path)
    {
        _path = path;
    }

    public IEnumerable<string> ReadFile()
    {
        return File.ReadAllLines(_path).Where(s => !string.IsNullOrEmpty(s));
    }
    public IEnumerable<Row> GetRows()
    {
        return ReadFile().Skip(1).Select(line => new Row(line));
    }

    public Column GetColumn()
    {
        return new Column(ReadFile().First());
    }
}