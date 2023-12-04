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



    public string this[int index]
    {
        get
        {
            if (index >= 0 && index < Values.Length) return Values[index];
            throw new ArgumentOutOfRangeException();
        }
        set
        {
            if (index >= 0 && index < Values.Length) Values[index] = value;
            else throw new ArgumentOutOfRangeException();
        }
    }

    public T Get<T>(int index)
    {
        if (index >= 0 && index < Values.Length) 
            return (T)Convert.ChangeType(Values[index], typeof(T), CultureInfo.InvariantCulture);
        throw new ArgumentOutOfRangeException();
    }
}