using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;
using CSharp.Reader.CSV;

namespace CSharp.Benchmarks;

[Config(typeof(AntiVirusFriendlyConfig))]
[MemoryDiagnoser]
[RankColumn]
public class BenchmarkReader
{
    private class AntiVirusFriendlyConfig : ManualConfig
    {
        public AntiVirusFriendlyConfig()
        {
            AddJob(Job.MediumRun
                .WithToolchain(InProcessNoEmitToolchain.Instance));
        }
    }
    private readonly string _path;

    public BenchmarkReader()
    {
        _path = Directory.GetParent(Directory.GetCurrentDirectory())
            ?.Parent?.Parent?.Parent + @"\data\Salary.csv";
    }

    [Benchmark]
    public List<string> ReadFileTest()
    {
        return new CsvReader(_path).ReadFile().ToList();
    }

    [Benchmark]
    public async Task<List<string>> ReadFileTestAsync()
    {
        var reader = new CsvReader(_path);
        var result = await reader.ReadFileAsync();
        return result.ToList();
    }
}