using System.Numerics;
using CSharp.Correlation;
using CSharp.Element;

namespace CSharp.Data;

public class DataArray : Data
{
    protected override Column Column { get; }
    protected override ICorrelation Correlate { get; }
    private readonly List<Row> _rows;
    
    public DataArray(Column column, List<Row> rows, ICorrelation correlation)
    {
        _rows = rows;
        Column = column;
        Correlate = correlation;
    }
    protected override IEnumerable<float> Get(string columnName)
    {
        var indexColumn = Column[columnName];
        return _rows.Select(row => row.Get(indexColumn));
    }
}

public class DataArray<T> : Data<T>
    where T: struct, IFloatingPoint<T>
{
    protected override Column Column { get; }
    protected override ICorrelation<T> Correlate { get; }
    private readonly List<Row> _rows;

    public DataArray(Column column, List<Row> rows, ICorrelation<T> correlation)
    {
        _rows = rows;
        Column = column;
        Correlate = correlation;
    }
    
    protected override IEnumerable<T> Get(string columnName)
    {
        var indexColumn = Column[columnName];
        return _rows.Select(row => row.Get<T>(indexColumn));
    }
}


