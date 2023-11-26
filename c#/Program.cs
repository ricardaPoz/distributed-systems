using c_;
using c_.Reader.CSV;

var path = 
    Directory.GetParent(Directory.GetCurrentDirectory())
    ?.Parent?.Parent?.Parent?.ToString() + @"\data\Salary.csv";


var df = new CsvDataArray(new CsvReader(path));
df.Reaplase<int>(new Dictionary<string, int> {
    {"Male", 0},
    {"Female", 1}
});

var unique = df["Gender"].Unique<int>();

// BenchmarkRunner.Run<BenchmarkReader>();