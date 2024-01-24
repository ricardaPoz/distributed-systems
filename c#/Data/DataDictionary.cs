using System.Numerics;
using CSharp.Correlation;
using CSharp.Element;

namespace CSharp.Data;

public class DataDictionary: Data
{
    protected override Column Column { get; }
    protected override ICorrelation Correlate { get; }
    private readonly Dictionary<string, IEnumerable<float>> _data;
    public DataDictionary(Column column, List<Row> rows, ICorrelation correlation)
    {
        Column = column;
        Correlate = correlation;

        _data = new Dictionary<string, IEnumerable<float>>();
        foreach (var col in column.Values)
        {
            var currentRow = rows.Select(row => row.Get<float>(col.index));
            _data.Add(col.name, currentRow);
        }
    }
    protected override IEnumerable<float> Get(string columnName)
    {
        return _data[columnName];
    }
}

public class DataDictionary<T> : Data<T>
    where T : struct, IFloatingPoint<T>
{
    protected override Column Column { get; }
    protected override ICorrelation<T> Correlate { get; }
    private readonly Dictionary<string, IEnumerable<T>> _data;

    public DataDictionary(Column column, List<Row> rows, ICorrelation<T> correlation)
    {
        Column = column;
        Correlate = correlation;

        _data = new Dictionary<string, IEnumerable<T>>();
        foreach (var col in column.Values)
        {
            var currentRow = rows.Select(row => row.Get<T>(col.index));
            _data.Add(col.name, currentRow);
        }
    }
    protected override IEnumerable<T> Get(string columnName)
    {
        return _data[columnName];
    }
}