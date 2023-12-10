using System.Diagnostics.CodeAnalysis;
using System.Text;
using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Jobs;
using BenchmarkDotNet.Toolchains.InProcess.NoEmit;
using CSharp.Correlation;
using CSharp.Data;
using CSharp.Element;
using CSharp.Reader.CSV;

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
            AddJob(Job.MediumRun
                .WithToolchain(InProcessNoEmitToolchain.Instance));
        }
    }

    private readonly DataArray _dataArray;
    private readonly DataArray _randomDataArray;

    
    public BenchmarkPearson()
    {
        #region Создание DataArray на основе CSV файла

        var path = Directory.GetParent(Directory.GetCurrentDirectory())
            ?.Parent?.Parent?.Parent + @"\data\Salary.csv";

        var reader = new CsvReader(path);
        _dataArray = new DataArray(reader.GetColumn(), reader.GetRows().ToList(), new Pearson());
        var nameColumns = new[] { "Gender", "Race", "Country", "Job Title" };
        foreach (var nameColumn in nameColumns)
        {
            var unique = _dataArray[nameColumn]
                .Unique()
                .Select((value, index) => (value, index))
                .ToDictionary(v => v.value, v => v.index.ToString());
            _dataArray.Replace(unique);
        }

        #endregion

        #region Создание DataArray на основе заданных колличества строк и стообцов
        
        var sb = new StringBuilder();
        var random = new Random();
        const int countRows = 10000, countColumns = 100;

        var columns = new string [countColumns];
        var rows = new List<Row>();

        for (var i = 0; i < countColumns; i++) columns[i] = i.ToString();

        for (var i = 0; i < countRows; i++)
        {
            var row = Enumerable.Repeat(0, 100)
                .Select(inx => random.NextSingle());

            sb.AppendJoin(',', row);
            rows.Add(new Row(sb.ToString()));
            sb.Clear();
        }

        sb.AppendJoin(',', columns);
        _randomDataArray = new DataArray(new Column(sb.ToString()), rows, new Pearson());

        #endregion
    }

    [Benchmark(Description = "Multithreading correlation. Rows = 10000, Columns = 100")]
    public async Task<DataArray> RCorrelationAsync()
    {
        return await _randomDataArray.CorrelationAsync("0");
    }

    [Benchmark(Description = "Synchronous correlation. Rows = 10000, Columns = 100")]
    public DataArray RCorrelation()
    {
        return _randomDataArray.Correlation("0");
    }

    [Benchmark(Description = "Multithreading correlation CSV file. Rows = 6685, Columns = 10")]
    public async Task<DataArray> CorrelationAsync()
    {
        return await _dataArray.CorrelationAsync("Salary");
    }

    [Benchmark(Description = "Synchronous correlation CSV file. Rows = 6685, Columns = 10")]
    public DataArray Correlation()
    {
        return _dataArray.Correlation("Salary");
    }
    
    [Benchmark(Description = "Multithreading correlation matrix CSV file. Rows = 6685, Columns = 10")]
    public async Task<List<(string name, DataArray)>> MCorrelationAsync()
    {
        return await _dataArray.CorrelationAsync();
    }

    [Benchmark(Description = "Synchronous correlation matrix CSV file. Rows = 6685, Columns = 10")]
    public List<(string name, DataArray)> MCorrelation()
    {
        return _dataArray.Correlation();
    }
    
}