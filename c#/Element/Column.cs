using System.Text;
using System.Text.RegularExpressions;

namespace CSharp.Element;

public class Column : BaseElement
{
    public Column(string line)
    {
        Values = Regex.Matches(line, Pattern).Select((match, i) => (match.Value, i)).ToArray();
    }

    /// <summary>
    ///     Свойство, описывающие столбец
    ///     <returns>
    ///         name - наименование столбца;
    ///         index - индекс столбца
    ///     </returns>
    /// </summary>
    public (string name, int index)[] Values { get; }

    /// <summary>
    ///     Индексатор, который возращает наименование столбца по его индексу
    /// </summary>
    /// <param name="index">Индекс столбца</param>
    /// <returns>Наименование столбца</returns>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public string this[int index] => index >= 0 && index < Values.Length
        ? Values[index].name
        : throw new ArgumentOutOfRangeException();


    /// <summary>
    ///     Индексатор, который индекс столбца по его наименованию
    /// </summary>
    /// <param name="nameColumn">Наименование столбца</param>
    /// <returns>Индекс столбца</returns>
    /// <exception cref="InvalidOperationException"></exception>
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

    public override string ToString()
    {
        var sb = new StringBuilder();
        sb.AppendJoin(',', Values);
        return sb.ToString();
    }
}