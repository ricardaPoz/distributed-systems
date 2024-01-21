using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Columns;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Reports;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;
using CSharp.Correlation;
using CSharp.Data;
using CSharp.Reader.CSV;
using Perfolizer.Horology;

namespace CSharp.Benchmarks;

[Config(typeof(AntiVirusFriendlyConfig))]
[MemoryDiagnoser]
[RankColumn]
public class BenchmarkPearson
{
    private class AntiVirusFriendlyConfig : ManualConfig
    {
        public AntiVirusFriendlyConfig()
        {
            SummaryStyle = SummaryStyle.Default
                .WithRatioStyle(RatioStyle.Percentage)
                .WithTimeUnit(TimeUnit.Second);
            
            AddJob(Job.MediumRun
                .WithToolchain(InProcessNoEmitToolchain.Instance));
        }
    }
    private readonly Data.Data _dataArray;
    private readonly Data.Data _dataDictionary;
    private readonly Data.Data<float> _dataArrayGeneric;
    private readonly Data.Data<float> _dataDictionaryGeneric;
    public BenchmarkPearson()
    {
        #region Создание DataArray и DataDictionary на основе CSV файла

        var path = Directory.GetParent(Directory.GetCurrentDirectory())
            ?.Parent?.Parent?.Parent + @"\data\SalaryFloat.csv";

        var reader = new CsvReader(path);
        _dataArray = new DataArray(reader.GetColumn(), reader.GetRows().ToList(), new Pearson());
        _dataArrayGeneric = new DataArray<float>(reader.GetColumn(), reader.GetRows().ToList(), new Pearson<float>());
        _dataDictionaryGeneric = new DataDictionary<float>(reader.GetColumn(), reader.GetRows().ToList(), new Pearson<float>());
        _dataDictionary = new DataDictionary(reader.GetColumn(), reader.GetRows().ToList(), new Pearson());
        #endregion
    }

    [Benchmark(Description = "Async correlation matrix CSV file. Array. Rows = 6685, Columns = 10")]
    public async Task<List<(string name, List<(string columnName, float coef)>)>> ArrayCorrelationAsync()
    {
        return await _dataArray.CorrelationAsync();
    }

    [Benchmark(Description = "Synchronous correlation matrix CSV file. Array. Rows = 6685, Columns = 10")]
    public List<(string name, List<(string columnName, float coef)>)> ArrayCorrelation()
    {
        return _dataArray.Correlation();
    }
    
    [Benchmark(Description = "Async correlation matrix CSV file. Dictionary. Rows = 6685, Columns = 10")]
    public async Task<List<(string name, List<(string columnName, float coef)>)>> DictionaryCorrelationAsync()
    {
        return await _dataDictionary.CorrelationAsync();
    }

    [Benchmark(Description = "Synchronous correlation matrix CSV file. Dictionary. Rows = 6685, Columns = 10")]
    public List<(string name, List<(string columnName, float coef)>)> DictionaryCorrelation()
    {
        return _dataDictionary.Correlation();
    }
    
    [Benchmark(Description = "Generic async correlation matrix CSV file. Array. Rows = 6685, Columns = 10")]
    public async Task<List<(string name, List<(string columnName, float coef)>)>> GArrayCorrelationAsync()
    {
        return await _dataArrayGeneric.CorrelationAsync();
    }

    [Benchmark(Description = "Generic synchronous correlation matrix CSV file. Array. Rows = 6685, Columns = 10")]
    public List<(string name, List<(string columnName, float coef)>)> GArrayCorrelation()
    {
        return _dataArrayGeneric.Correlation();
    }
    
    [Benchmark(Description = "Generic async correlation matrix CSV file. Dictionary. Rows = 6685, Columns = 10")]
    public async Task<List<(string name, List<(string columnName, float coef)>)>> GDictionaryCorrelationAsync()
    {
        return await _dataDictionaryGeneric.CorrelationAsync();
    }

    [Benchmark(Description = "Generic synchronous correlation matrix CSV file. Dictionary. Rows = 6685, Columns = 10")]
    public List<(string name, List<(string columnName, float coef)>)> GDictionaryCorrelation()
    {
        return _dataDictionaryGeneric.Correlation();
    }
}