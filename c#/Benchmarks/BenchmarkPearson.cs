using BenchmarkDotNet.Attributes;
using CSharp.Correlation;
using CSharp.Data;
using CSharp.Reader.CSV;

namespace CSharp.Benchmarks;


[MemoryDiagnoser]
[RankColumn]
public class BenchmarkPearson
{
    private static string path = @"C:\Users\santa\source\Repos\distributed-systems\data\Salary.csv";

    [Benchmark]
    public void RandomDoubleValue()
    {
        Random randNum = new Random();
        var x = Enumerable.Repeat(0, 100000)
            .Select(i => randNum.NextDouble())
            .ToArray();

        var y = Enumerable.Repeat(0, 100000)
            .Select(i => randNum.NextDouble())
            .ToArray();

        var pearson = new Pearson().Correlation(ref x, ref y);
    }

    [Benchmark]
    public void RandomFloatValue()
    {
        Random randNum = new Random();
        var x = Enumerable.Repeat(0, 100000)
            .Select(i => randNum.NextSingle())
            .ToArray();

        var y = Enumerable.Repeat(0, 100000)
            .Select(i => randNum.NextSingle())
            .ToArray();

        var pearson = new Pearson().Correlation(ref x, ref y);
    }
    [Benchmark]
    public void RandomDoubleValueGeneric()
    {
        Random randNum = new Random();
        var x = Enumerable.Repeat(0, 100000)
            .Select(i => randNum.NextDouble())
            .ToArray();

        var y = Enumerable.Repeat(0, 100000)
            .Select(i => randNum.NextDouble())
            .ToArray();

        var pearson = new Pearson<double>().Correlation(ref x, ref y);
    }

    [Benchmark]
    public void RandomFloatValueGeneric()
    {
        Random randNum = new Random();
        var x = Enumerable.Repeat(0, 100000)
            .Select(i => randNum.NextSingle())
            .ToArray();

        var y = Enumerable.Repeat(0, 100000)
            .Select(i => randNum.NextSingle())
            .ToArray();

        var pearson = new Pearson<float>().Correlation(ref x, ref y);
    }

    [Benchmark]
    public void DataArrayPearsonSalary()
    {
        var reader = new CsvReader(path);
        var df = new DataArray(reader.GetColumn(), reader.GetRows().ToList(), new Pearson());


        var nameColumns = new string[] { "Gender", "Race", "Country", "Job Title" };
        foreach (var nameColumn in nameColumns)
        {
            var unique = df[nameColumn]
                .Unique()
                .Select((value, index) => (value, index))
                .ToDictionary(v => v.value, v => v.index.ToString());
            df.Replase(unique);
        }

        var corr = df.Correlation("Salary");
    }

    [Benchmark]
    public async Task DataArrayPearsonSalaryAsync()
    {
        var reader = new CsvReader(path);
        var df = new DataArray(reader.GetColumn(), reader.GetRows().ToList(), new Pearson());


        var nameColumns = new string[] { "Gender", "Race", "Country", "Job Title" };
        foreach (var nameColumn in nameColumns)
        {
            var unique = df[nameColumn]
                .Unique()
                .Select((value, index) => (value, index))
                .ToDictionary(v => v.value, v => v.index.ToString());
            df.Replase(unique);
        }

        var corr = await df.CorrelationAsync("Salary");
    }

    [Benchmark]
    public void DataArrayPearson()
    {
        var reader = new CsvReader(path);
        var df = new DataArray(reader.GetColumn(), reader.GetRows().ToList(), new Pearson());


        var nameColumns = new string[] { "Gender", "Race", "Country", "Job Title" };
        foreach (var nameColumn in nameColumns)
        {
            var unique = df[nameColumn]
                .Unique()
                .Select((value, index) => (value, index))
                .ToDictionary(v => v.value, v => v.index.ToString());
            df.Replase(unique);
        }

        var corr = df.Correlation();
    }

    [Benchmark]
    public async Task DataArrayPearsonAsync()
    {
        var reader = new CsvReader(path);
        var df = new DataArray(reader.GetColumn(), reader.GetRows().ToList(), new Pearson());


        var nameColumns = new string[] { "Gender", "Race", "Country", "Job Title" };
        foreach (var nameColumn in nameColumns)
        {
            var unique = df[nameColumn]
                .Unique()
                .Select((value, index) => (value, index))
                .ToDictionary(v => v.value, v => v.index.ToString());
            df.Replase(unique);
        }

        var corr = await df.CorrelationAsync();
    }
}
