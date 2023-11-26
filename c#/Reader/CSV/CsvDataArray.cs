using System.Security.Cryptography.X509Certificates;
using System.Text;
using c_.Reader.CSV;
using c_.Reader.Interfaces;

namespace c_;

public class CsvDataArray
{
    private readonly IReadableFile readableFile;
    private Column column;
    private List<Row> rows;

    public string[] Header
    {
        get => column.Values.Select(c => c.name).ToArray();
    }

    public int CountRows { get => rows.Count; }

    public CsvDataArray(IReadableFile readableFile)
    {
        this.readableFile = readableFile;
        this.column = this.readableFile.GetColumn();
        this.rows = this.readableFile.GetRows().ToList();
    }

    public Row this[int index] => index >= 0 && index < rows.Count
                 ? rows[index]
                 : throw new ArgumentOutOfRangeException();

    public string this[string column, int row]
    {
        get
        {
            var indexColumn = this.column[column];
            return rows[row].Values[indexColumn];
        }
    }

    public string this[int indexColumn, int indexRow]
    {
        get
        {
            return rows[indexRow].Values[indexColumn];
        }
    }

    public List<Row> this[Range range]
    {
        get
        {
            (int offset, int length) = range.GetOffsetAndLength(rows.Count);
            return rows.GetRange(offset, length);
        }
    }

    public Row this[Index index] { get => rows[index]; }

    public List<Row> this[params string[] names]
    {
        get
        {
            var indexs = column.Values
                .Where(c => names.Contains(c.name))
                .ToArray();

            if (indexs.Length < names.Length)
            {
                var except = string.Join(",", names.Except(indexs.Select(c => c.name)));
                throw new ArgumentOutOfRangeException($"The '{except}' elements is missing from the array.");
            }

            return rows.Select(row => {
                var values = indexs.Select(i => row[i.index]);
                return new Row(string.Join(",", values));
            }).ToList();
        }
    }

    public void Reaplase(Dictionary<string, string> values)
    {
        Reaplase<string>(values);
    }

    public void Reaplase<T>(Dictionary<string, T> values) where T: notnull
    {
        foreach(var row in rows)
        {
            foreach(var value in values)
            {
                var index = row.Values.ToList().FindIndex(r => r.Contains(value.Key));
                if (index != -1)
                {
                    row[index] = value.Value.ToString();
                }
            }
        }
    }


    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();

        sb.Append(string.Join(",", Header)).Append(Environment.NewLine);
        sb.Append(string.Join("\n", rows.Take(10).Select(r => string.Join(",", r.Values))));
        return sb.ToString();
    }
}

static class CsvDataExtension
{
    public static List<string> Unique(this List<Row> rows)
    {
        if (rows.First().Values.Length > 1)
        {
            throw new ArgumentOutOfRangeException();
        }
        return rows.Select(r => r[0]).Distinct().ToList();
    }

    public static List<T> Unique<T>(this List<Row> rows)
    {
        if (rows.First().Values.Length > 1)
        {
            throw new ArgumentOutOfRangeException();
        }
        return rows.Select(r => r.Get<T>(0)).Distinct().ToList();
    }
}
