#include "csv_reader.h"
#include "benchmark/benchmark.h"

static void BM_SimpleRecursion(benchmark::State& state){
    for(auto _: state)
    {
            std::string* csvData;
            size_t numStrings;

            std::string fileName = "C:/Users/santa/source/Repos/distributed-systems/data/Salary.csv";

            reader::read_csv(fileName, csvData, numStrings);
            delete[] csvData;
    }
}

BENCHMARK(BM_SimpleRecursion);

BENCHMARK_MAIN();