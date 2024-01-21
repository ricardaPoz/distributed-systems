#include "csv_reader.h"
#include "DataArray.h"
#include "benchmark/benchmark.h"
#include <filesystem>

#define PATH "..\\..\\..\\data\\SalaryFloat.csv"

static void BM_CorrelationMultithreading(benchmark::State& state)
{
    std::string* header;
    float** rows;
    size_t count_rows;
    size_t count_column;

    reader::read_csv(std::filesystem::absolute(PATH).string(),
        header, rows, count_rows,count_column);

    DataArray data{header, rows, count_rows, count_column};
    for (auto _: state)
    {
        float** correlation_matrix = data.CorrelationMultithreading();
    }
}

static void BM_Correlation(benchmark::State& state)
{
    std::string* header;
    float** rows;
    size_t count_rows;
    size_t count_column;
    reader::read_csv(std::filesystem::absolute(PATH).string(),
        header, rows, count_rows,count_column);

    DataArray data{header, rows, count_rows, count_column};
    for (auto _: state)
    {
        float** corr = data.Correlation();
    }
}


BENCHMARK(BM_CorrelationMultithreading)
    ->Unit(benchmark::kMillisecond)
    ->Iterations(15)
    ->Repetitions(2)
    ->MinTime(0)
    ->UseRealTime();


BENCHMARK(BM_Correlation)
    ->Unit(benchmark::kMillisecond)
    ->Iterations(15)
    ->Repetitions(2)
    ->MinTime(0)
    ->UseRealTime();


BENCHMARK_MAIN();
