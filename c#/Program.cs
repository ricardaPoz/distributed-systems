using BenchmarkDotNet.Running;
using CSharp.Benchmarks;
using CSharp.Data;
using CSharp.Reader.CSV;

// var path =
//     Directory.GetParent(Directory.GetCurrentDirectory())
//     ?.Parent?.Parent?.Parent?.ToString() + @"\data\Salary.csv";


// var reader = new CsvReader(path);
// var df = new DataArray(reader.GetColumn(), reader.GetRows().ToList());


// var nameColumns = new string[] { "Gender", "Race", "Country", "Job Title" };
// foreach (var nameColumn in nameColumns)
// {
//     var unique = df[nameColumn]
//         .Unique()
//         .Select((value, index) => (value, index))
//         .ToDictionary(v => v.value, v => v.index.ToString());
//     df.Replase(unique);
// }

BenchmarkRunner.Run<BenchmarkPearson>();