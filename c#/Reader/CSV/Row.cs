using System.ComponentModel;
using System.Text.RegularExpressions;

namespace c_.Reader.CSV;

public class Row
{
    private const string PATTERN = "(?<=^|,)([^,]*)(?=,|$)";
    public string[] Values {get; init;}

    public Row(string line)
    {
        Values = Regex.Matches(line, PATTERN).Select(match => match.Value).ToArray();
    }

    public string this[int index]
    {
        get
        {
            if (index < 0 && index > Values.Length)
                throw new ArgumentOutOfRangeException();

            else return Values[index];
        }
    }

    public T Get<T>(int index)
    {
        if (index < 0 && index > Values.Length)
            throw new ArgumentOutOfRangeException();

        TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));

        if (!converter.IsValid(Values[index]))
            throw new InvalidCastException();

        return (T)Convert.ChangeType(Values[index], typeof(T));
    }
}


