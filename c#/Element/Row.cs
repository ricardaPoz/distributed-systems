using System.ComponentModel;
using System.Text.RegularExpressions;

namespace CSharp.Element;

public class Row: BaseElement
{
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
        set
        {
            if (index < 0 && index > Values.Length)
                throw new ArgumentOutOfRangeException();
            else Values[index] = value;
        }
    }

    public T Get<T>(int index)
    {
        if (index < 0 && index > Values.Length)
            throw new ArgumentOutOfRangeException();

        TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));

        if (!converter.IsValid(Values[index]))
            throw new InvalidCastException();

        return (T)Convert.ChangeType(Values[index].Replace('.', ','), typeof(T));
    }

    public override string ToString()
    {
        Func<string, int, string> trancute = (string input, int length) => {
            return input.Length > length ? input[..length] : input;
        };
        var result = Values.Select(r => trancute(r,5));
        return string.Join(",", result);
    }
}


