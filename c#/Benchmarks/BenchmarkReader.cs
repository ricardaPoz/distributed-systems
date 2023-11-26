using BenchmarkDotNet.Attributes;
using c_.Reader.CSV;

namespace c_.Benchmarks;

[MemoryDiagnoser]
[RankColumn]
public class BenchmarkReader
{
    private static string path = @"C:\Users\santa\source\Repos\distributed-systems\data\Salary.csv";

    [Benchmark]
    public void ReadFileTest()
    {
        var result = new CsvReader(path).ReadFile();
    }
    [Benchmark]
    public async Task ReadFileTestAsync()
    {
        var x = new CsvReader(path);
        var result = await x.ReadFileAsync();
    }
}
