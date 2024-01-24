#include "csv_reader.h"
#include <iostream>
#include <filesystem>
#include "DataArray.h"

int main()
{
    std::string* header;
    float** rows;
    size_t count_rows;
    size_t count_column;

    const std::filesystem::path currentPath = "..\\..\\data\\SalaryFloat.csv";

    std::cout << absolute(currentPath).string();
    reader::read_csv(absolute(currentPath).string(), header, rows, count_rows, count_column);

    DataArray data{header, rows, count_rows, count_column};
    float** correlation_matrix = data.CorrelationMultithreading();

    for (size_t i = 0; i < count_column; i++)
    {
        for (size_t j = 0; j < count_column; j++)
        {
            std::cout << "[" << correlation_matrix[i][j] << "]";
        }
        std::cout <<  std::endl;
    }
    return 0;
}
