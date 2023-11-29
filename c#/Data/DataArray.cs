using System.Collections.Immutable;
using System.Text;
using CSharp.Correlation;
using CSharp.Element;
using CSharp.Reader.Interfaces;

namespace CSharp.Data;

public class DataArray
{
    private readonly ICorrelation correlation;
    private Column column;
    private List<Row> rows;

    public string[] Header
    {
        get => column.Values.Select(c => c.name).ToArray();
    }

    public int CountRows { get => rows.Count; }

    public DataArray(Column column, List<Row> rows, ICorrelation correlation)
    {
        this.rows = rows;
        this.column = column;
        this.correlation = correlation;
    }
    public DataArray(Column column, Row row, ICorrelation correlation)
    {
        this.rows = new List<Row> { row };
        this.column = column;
        this.correlation = correlation;
    }

    public Row this[int index] => index >= 0 && index < rows.Count
                 ? rows[index]
                 : throw new ArgumentOutOfRangeException();

    public string this[string nameColumn, int indexRow]
    {
        get
        {
            var indexColumn = this.column[nameColumn];
            return rows[indexRow][indexColumn];
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

            return rows.Select(row =>
            {
                var values = indexs.Select(i => row[i.index]);
                return new Row(string.Join(",", values));
            }).ToList();
        }
    }

    public IEnumerable<T> Get<T>(string columnName)
    {
        var indexColumn = this.column[columnName];
        return rows.Select(row => row.Get<T>(indexColumn));
    }

    public void Replase(Dictionary<string, string> values)
    {
        Replase<string>(values);
    }

    public void Replase<T>(Dictionary<string, T> values) where T : notnull
    {
        foreach (var row in rows)
        {
            foreach (var value in values)
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
        sb.Append(string.Join(Environment.NewLine, rows.Select(r => r.ToString())));
        return sb.ToString();
    }

    public DataArray Head(int take)
    {
        return new DataArray(column, rows.Take(take).ToList(), correlation);
    }

    public List<(string name, DataArray)> Correlation()
    {
        List<(string name, DataArray)>? result = new();
        foreach (var column in this.column.Values)
        {
            result.Add((column.name, Correlation(column.name)));
        }
        return result;
    }

    public DataArray Correlation(string columnName)
    {
        List<(string, double)> result = new();
        foreach (var column in this.column.Values)
        {
            var x = Get<double>(column.name).ToArray();
            var y = Get<double>(columnName).ToArray();
            var corr = correlation.Correlation(ref x, ref y);
            result.Add((column.name, corr));
        }

        var c = string.Join(",", result.Select(c => c.Item1));
        var r = string.Join(",", result.Select(c => c.Item2));
        return new DataArray(new Column(c), new Row(r), correlation);
    }
    public async Task<List<(string name, DataArray)>> CorrelationAsync()
    {
        List<Task<(string name, DataArray)>> tasks = new();

        foreach (var col in column.Values)
        {
            var t = Task.Run(async () => {
                return (col.name, await CorrelationAsync(col.name));
            });
            tasks.Add(t);
        }
        var result = await Task.WhenAll(tasks.ToArray());
        return result.ToList();
    }
    public async Task<DataArray> CorrelationAsync(string columnName)
    {
        List<Task<(string, double)>> tasks = new();

        foreach (var column in this.column.Values)
        {
            Task<(string, double)> t = Task.Run(() =>
            {
                var x = Get<double>(column.name).ToArray();
                var y = Get<double>(columnName).ToArray();
                var corr = correlation.Correlation(ref x, ref y);
                return (column.name, corr);
            });
            tasks.Add(t);
        }
        var result = await Task.WhenAll(tasks.ToArray());
        var c = string.Join(",", result.Select(c => c.Item1));
        var r = string.Join(",", result.Select(c => c.Item2));
        return new DataArray(new Column(c), new Row(r), correlation);
    }
}

static class DataExtension
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
