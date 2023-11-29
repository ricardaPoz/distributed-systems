using System.Text.RegularExpressions;

namespace CSharp.Element;

public class Column : BaseElement
{
    public (string name, int index)[] Values { get; init; }

    public Column(string line)
    {
        Values = Regex.Matches(line, PATTERN).Select((match, i) => (match.Value, i)).ToArray();
    }

    public string this[int index]
    {
        get
        {
            if (index < 0 && index > Values.Length)
                throw new ArgumentOutOfRangeException();

            else return Values[index].name;
        }
    }

    public int this[string name]
    {
        get
        {
            int index = Array.IndexOf(Values.Select(v => v.name).ToArray(), name);

            if (index == -1)
                throw new InvalidOperationException($"The '{name}' element is missing from the array.");


            else return index;
        }
    }
    public override string ToString()
    {
        return string.Join(",", Values);
    }
}
