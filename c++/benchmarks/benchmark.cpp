#include "csv_reader.h"
#include "DataArray.h"
#include "benchmark/benchmark.h"


DataArray FillCsv()
{
    std::string* header;
    float** rows;
    size_t size;
    size_t countFilds;
    reader::read_csv("{Нужно вставить полный путь до SalaryFloat.csv}", header, rows, size,
                     countFilds);
    return DataArray{header, rows, size, countFilds};
}

DataArray FillRandom()
{
    constexpr size_t numRows = 10000;
    constexpr size_t numColumns = 100;

    std::string* header = new std::string[numColumns];
    for (size_t i = 0; i < numColumns; ++i)
    {
        header[i] = "Column_" + std::to_string(i);
    }

    float** rows = new float *[numRows];
    for (size_t i = 0; i < numRows; ++i)
    {
        rows[i] = new float[numColumns];
        for (size_t j = 0; j < numColumns; ++j)
        {
            rows[i][j] = static_cast<float>(rand()) / RAND_MAX;
        }
    }

    return DataArray{header, rows, numRows, numColumns};
}

static void BM_CorrelationCsv(benchmark::State& state)
{
    DataArray data = FillCsv();
    for (auto _: state)
    {
        auto x = data.Correlation(0);
    }
}

static void BM_CorrelationParallelCsv(benchmark::State& state)
{
    DataArray data = FillCsv();
    for (auto _: state)
    {
        auto x = data.CorrelationParallel(0);
    }
}

static void BM_CorrelationMatrixCsv(benchmark::State& state)
{
    DataArray data = FillCsv();
    for (auto _: state)
    {
        auto x = data.Correlation();
    }
}

static void BM_CorrelationMatrixParallelCsv(benchmark::State& state)
{
    DataArray data = FillCsv();
    for (auto _: state)
    {
        auto x = data.CorrelationParallel();
    }
}

static void BM_Correlation100Column10000Row(benchmark::State& state)
{
    DataArray data = FillRandom();
    for (auto _: state)
    {
        auto x = data.Correlation(0);
    }
}

static void BM_CorrelationParallel100Column10000Row(benchmark::State& state)
{
    DataArray data = FillRandom();
    for (auto _: state)
    {
        auto x = data.CorrelationParallel(0);
    }
}

static void BM_CorrelationMatrix100Column10000Row(benchmark::State& state)
{
    DataArray data = FillRandom();
    for (auto _: state)
    {
        auto x = data.Correlation();
    }
}

static void BM_CorrelationMatrixParallel100Column10000Row(benchmark::State& state)
{
    DataArray data = FillRandom();
    for (auto _: state)
    {
        auto x = data.CorrelationParallel();
    }
}

BENCHMARK(BM_CorrelationCsv)
    ->Unit(benchmark::kMillisecond)
    ->Iterations(15)
    ->Repetitions(2)
    ->MinTime(0)
    ->UseRealTime();


BENCHMARK(BM_CorrelationParallelCsv)
    ->Unit(benchmark::kMillisecond)
    ->Iterations(15)
    ->Repetitions(2)
    ->MinTime(0)
    ->UseRealTime();

BENCHMARK(BM_CorrelationMatrixCsv)
    ->Unit(benchmark::kMillisecond)
    ->Iterations(15)
    ->Repetitions(2)
    ->MinTime(0)
    ->UseRealTime();

BENCHMARK(BM_CorrelationMatrixParallelCsv)
    ->Unit(benchmark::kMillisecond)
    ->Iterations(15)
    ->Repetitions(2)
    ->MinTime(0)
    ->UseRealTime();

BENCHMARK(BM_Correlation100Column10000Row)
    ->Unit(benchmark::kMillisecond)
    ->Iterations(15)
    ->Repetitions(2)
    ->MinTime(0)
    ->UseRealTime();

BENCHMARK(BM_CorrelationParallel100Column10000Row)
    ->Unit(benchmark::kMillisecond)
    ->Iterations(15)
    ->Repetitions(2)
    ->MinTime(0)
    ->UseRealTime();


BENCHMARK(BM_CorrelationMatrix100Column10000Row)
    ->Unit(benchmark::kMillisecond)
    ->Iterations(15)
    ->Repetitions(2)
    ->MinTime(0)
    ->UseRealTime();

BENCHMARK(BM_CorrelationMatrixParallel100Column10000Row)
    ->Unit(benchmark::kMillisecond)
    ->Iterations(15)
    ->Repetitions(2)
    ->MinTime(0)
    ->UseRealTime();


BENCHMARK_MAIN();
