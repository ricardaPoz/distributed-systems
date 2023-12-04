using System.Globalization;
using System.Text;
using CSharp.Correlation;
using CSharp.Element;

namespace CSharp.Data;

public class DataArray
{
    private readonly Column _column;
    private readonly ICorrelation _correlation;
    private readonly List<Row> _rows;
    public string[] Header => _column.Values.Select(c => c.name).ToArray();
    public int CountRows => _rows.Count;


    public DataArray(Column column, List<Row> rows, ICorrelation correlation)
    {
        _rows = rows;
        _column = column;
        _correlation = correlation;
    }

    public DataArray(Column column, Row row, ICorrelation correlation)
    {
        _rows = new List<Row> { row };
        _column = column;
        _correlation = correlation;
    }

    public List<Row> this[params string[] names]
    {
        get
        {
            var indexes = _column.Values
                .Where(c => names.Contains(c.name));

            if (indexes.Count() < names.Length)
            {
                var except = string.Join(",", names.Except(indexes.Select(c => c.name)));
                throw new ArgumentOutOfRangeException($"The '{except}' elements is missing from the array.");
            }

            return _rows.Select(row =>
            {
                var values = indexes.Select(i => row[i.index]);
                return new Row(string.Join(",", values));
            }).ToList();
        }
    }

    public IEnumerable<T> Get<T>(string columnName)
    {
        var indexColumn = _column[columnName];
        return _rows.Select(row => row.Get<T>(indexColumn));
    }

    public void Replace(Dictionary<string, string> values)
    {
        foreach (var row in _rows)
        foreach (var value in values)
        {
            var index = Array.FindIndex(row.Values, r => r.Contains(value.Key));
            if (index != -1) row[index] = value.Value;
        }
    }

    public List<(string name, DataArray)> Correlation()
    {
        List<(string name, DataArray)> result = new();
        foreach (var column in _column.Values)
        {
            result.Add((column.name, Correlation(column.name)));
        }
        return result;
    }
    
    public async Task<List<(string name, DataArray)>> CorrelationAsync()
    {
        List<Task<(string name, DataArray)>> tasks = new();

        foreach (var col in _column.Values)
        {
            var t = Task.Run(async () => (col.name, await CorrelationAsync(col.name)));
            tasks.Add(t);
        }

        var result = await Task.WhenAll(tasks.ToArray());
        return result.ToList();
    }
    
    /// <summary>
    /// Метод, расчитывающий корреляцию для конкретного столбца.
    /// Создется поток для каждого столбца.
    /// </summary>
    /// <param name="columnName">Наименование столбца</param>
    /// <returns>Task&lt;DataArray&gt;</returns>
    public async Task<DataArray> CorrelationAsync(string columnName)
    {
        List<Task<(string, float)>> tasks = new();
        var sb = new StringBuilder();

        foreach (var columnValue in _column.Values)
        {
            Task<(string, float)> t = Task.Run(() =>
            {
                var x = Get<float>(columnValue.name).ToArray();
                var y = Get<float>(columnName).ToArray();
                var corr = _correlation.Correlation(ref x, ref y);
                return (columnValue.name, corr);
            });
            tasks.Add(t);
        }

        var result = await Task.WhenAll(tasks.ToArray());
        sb.AppendJoin(',', result.Select(column => column.Item1));
        var column = new Column(sb.ToString());
        sb.Clear();
        sb.AppendJoin(',', result.Select(row => row.Item2.ToString(CultureInfo.InvariantCulture)));
        var row = new Row(sb.ToString());
        return new DataArray(column, row, _correlation);
    }

    /// <summary>
    /// Метод, расчитывающий корреляцию для конкретного столбца
    /// </summary>
    /// <param name="columnName">Наименование столбца</param>
    /// <returns>DataArray</returns>
    public DataArray Correlation(string columnName)
    {
        List<(string name, float correlation)> result = new();
        var sb = new StringBuilder();

        foreach (var columnValue in _column.Values)
        {
            var x = Get<float>(columnValue.name).ToArray();
            var y = Get<float>(columnName).ToArray();
            var corr = _correlation.Correlation(ref x, ref y);
            result.Add((columnValue.name, corr));
        }

        sb.AppendJoin(',', result.Select(column => column.name));
        var column = new Column(sb.ToString());
        sb.Clear();
        sb.AppendJoin(',', result.Select(row => row.correlation.ToString(CultureInfo.InvariantCulture)));
        var row = new Row(sb.ToString());
        return new DataArray(column, row, _correlation);
    }
}

internal static class DataExtension
{
    public static List<string> Unique(this List<Row> rows)
    {
        if (rows.First().Values.Length > 1) throw new ArgumentOutOfRangeException();
        return rows.Select(r => r[0]).Distinct().ToList();
    }
}