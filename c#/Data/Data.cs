using System.Numerics;
using CSharp.Correlation;
using CSharp.Element;

namespace CSharp.Data;

public abstract class Data
{
    protected abstract Column Column { get; }
    protected abstract ICorrelation Correlate { get; }
    protected abstract IEnumerable<float> Get(string columnName);
    
    public virtual List<(string name, List<(string columnName, float coef)>)> Correlation()
    {
        List<(string name, List<(string columnName, float coef)>)> correlationMatrix = new();
        foreach (var column in Column.Values)
        {
            correlationMatrix.Add((column.name, Correlation(column.name)));
        }
        return correlationMatrix;
    }
    public virtual async Task<List<(string name, List<(string columnName, float coef)>)>> CorrelationAsync()
    {
        List<Task<(string name, List<(string columnName, float coef)>)>> tasks = new();

        foreach (var col in Column.Values)
        {
            var t = Task.Run(async () => (col.name, await CorrelationAsync(col.name)));
            tasks.Add(t);
        }

        var correlationMatrix = await Task.WhenAll(tasks.ToArray());
        return correlationMatrix.ToList();
    }
    protected virtual async Task<List<(string columnName, float coef)>> CorrelationAsync(string columnName)
    {
        List<Task<(string, float)>> tasks = new();
        foreach (var columnValue in Column.Values)
        {
            Task<(string, float)> t = Task.Run(() =>
            {
                var x = Get(columnValue.name).ToArray();
                var y = Get(columnName).ToArray();
                var corr = Correlate.Correlation(ref x, ref y);
                return (columnValue.name, corr);
            });
            tasks.Add(t);
        }

        var correlationResult = await Task.WhenAll(tasks);
        return correlationResult.ToList();
    }
    protected virtual List<(string columnName, float coef)> Correlation(string columnName)
    {
        List<(string name, float correlation)> correlationResult = new();
        foreach (var columnValue in Column.Values)
        {
            var x = Get(columnValue.name).ToArray();
            var y = Get(columnName).ToArray();
            var corr = Correlate.Correlation(ref x, ref y);
            correlationResult.Add((columnValue.name, corr));
        }
        return correlationResult;
    }
}

public abstract class Data<T>
    where T : struct, IFloatingPoint<T>
{
    protected abstract Column Column { get; }
    protected abstract ICorrelation<T> Correlate { get; }
    protected abstract IEnumerable<T> Get(string columnName);
    
    public virtual List<(string name, List<(string columnName, T coef)>)> Correlation()
    {
        List<(string name, List<(string columnName, T coef)>)> correlationMatrix = new();
        foreach (var column in Column.Values)
        {
            correlationMatrix.Add((column.name, Correlation(column.name)));
        }
        return correlationMatrix;
    }
    public virtual async Task<List<(string name, List<(string columnName, T coef)>)>> CorrelationAsync()
    {
        List<Task<(string name, List<(string columnName, T coef)>)>> tasks = new();

        foreach (var col in Column.Values)
        {
            var t = Task.Run(async () => (col.name, await CorrelationAsync(col.name)));
            tasks.Add(t);
        }

        var correlationMatrix = await Task.WhenAll(tasks.ToArray());
        return correlationMatrix.ToList();
    }
    protected virtual async Task<List<(string columnName, T coef)>> CorrelationAsync(string columnName)
    {
        List<Task<(string, T)>> tasks = new();
        foreach (var columnValue in Column.Values)
        {
            Task<(string, T)> t = Task.Run(() =>
            {
                var x = Get(columnValue.name).ToArray();
                var y = Get(columnName).ToArray();
                var corr = Correlate.Correlation(ref x, ref y);
                return (columnValue.name, corr);
            });
            tasks.Add(t);
        }

        var correlationResult = await Task.WhenAll(tasks);
        return correlationResult.ToList();
    }
    protected virtual List<(string columnName, T coef)> Correlation(string columnName)
    {
        List<(string name, T correlation)> correlationResult = new();

        foreach (var columnValue in Column.Values)
        {
            var x = Get(columnValue.name).ToArray();
            var y = Get(columnName).ToArray();
            var corr = Correlate.Correlation(ref x, ref y);
            correlationResult.Add((columnValue.name, corr));
        }
        return correlationResult;
    }
}