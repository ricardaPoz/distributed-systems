using System.Globalization;
using System.Text.RegularExpressions;

namespace CSharp.Element;

public class Row : BaseElement
{
    public string[] Values { get; }

    public Row(string line)
    {
        Values = Regex.Matches(line, Pattern)
            .Select(match => match.Value).ToArray();
    }
    public float Get(int index)
    {
        if (index >= 0 && index < Values.Length)
        {
            return float.TryParse(Values[index], CultureInfo.InvariantCulture, out var value)
                ? value
                : throw new InvalidCastException();
        }
        throw new ArgumentOutOfRangeException();
    }
    public T Get<T>(int index)
    {
        if (index >= 0 && index < Values.Length) 
            return (T)Convert.ChangeType(Values[index], typeof(T), CultureInfo.InvariantCulture);
        throw new ArgumentOutOfRangeException();
    }
}