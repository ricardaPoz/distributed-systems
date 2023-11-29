namespace CSharp.Element;

public abstract class BaseElement
{
    protected const string PATTERN = "(?<=^|,)([^,]*)(?=,|$)";
}
