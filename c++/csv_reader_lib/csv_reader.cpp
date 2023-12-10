#include "csv_reader.h"

void reader::read_csv(const std::string& filename, std::string*& array, size_t& size)
{
    std::ifstream file(filename);

    size_t linesCount = 0;
    std::string line;

    while (std::getline(file, line))
        linesCount++;

    file.clear();
    file.seekg(0, std::ios::beg);

    array = new std::string[linesCount];

    size = linesCount;
    for (size_t i = 0; i < linesCount; ++i)
    {
        std::getline(file, line);
        array[i] = line;
    }
    file.close();
}
