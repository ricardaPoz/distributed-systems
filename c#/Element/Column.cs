using System.Text;
using System.Text.RegularExpressions;

namespace CSharp.Element;

public class Column : BaseElement
{
    public Column(string line)
    {
        Values = Regex.Matches(line, Pattern).Select((match, i) => (match.Value, i)).ToArray();
    }
    public (string name, int index)[] Values { get; }
    public int this[string nameColumn]
    {
        get
        {
            var index = Array.IndexOf(Values.Select(v => v.name).ToArray(), nameColumn);

            if (index == -1)
                throw new InvalidOperationException($"The '{nameColumn}' element is missing from the array.");
            return index;
        }
    }
}