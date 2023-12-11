#include "csv_reader.h"
#include <iostream>
#include <filesystem>
#include "DataArray.h"

int main()
{
    std::string* header;
    float** rows;
    size_t size;
    size_t countFilds;

    std::filesystem::path currentPath = std::filesystem::current_path()
            .parent_path().parent_path() / "data" / "SalaryFloat.csv";

    reader::read_csv(currentPath.string(), header, rows, size, countFilds);

    DataArray data{header, rows, size, countFilds};
    auto x = data.CorrelationParallel();

    for (size_t i = 0; i < countFilds; i++)
    {
        for (size_t j = 0; j < countFilds; j++)
        {
            std::cout << "[" << x[i][j] << "]";
        }
        std::cout <<  std::endl;
    }

    for (size_t i = 0; i < countFilds; i++)
    {
        delete[] x[i];
    }
    delete[] x;

    return 0;
}
