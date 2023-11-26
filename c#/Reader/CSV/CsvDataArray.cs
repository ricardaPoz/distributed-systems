using c_.Reader.CSV;
using c_.Reader.Interfaces;

namespace c_;

public class CsvDataArray
{
    private readonly IReadableFile readableFile;
    private readonly Column column;
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
}
