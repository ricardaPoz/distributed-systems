var path = 
    Directory.GetParent(Directory.GetCurrentDirectory())
    ?.Parent?.Parent?.Parent?.ToString() + @"\data\Salary.csv";

// BenchmarkRunner.Run<BenchmarkReader>();