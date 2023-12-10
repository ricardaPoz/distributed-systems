#include "csv_reader.h"
#include <iostream>
#include <thread>


int main()
{
    std::string* csvData;
    size_t numStrings;

    std::string fileName = "C:/Users/santa/source/Repos/distributed-systems/data/Salary.csv";

    reader::read_csv(fileName, csvData, numStrings);

    for (size_t i = 0; i < numStrings; ++i) {
        std::cout << csvData[i] << std::endl;
    }

    delete[] csvData;
    return 0;
}
