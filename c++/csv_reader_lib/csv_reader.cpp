#include "csv_reader.h"

#include <sstream>

void reader::read_csv(const std::string& filename, std::string*& array, size_t& size)
{
    std::ifstream file(filename);

    size = 0;
    std::string line;

    while (std::getline(file, line))
        size++;

    file.clear();
    file.seekg(0, std::ios::beg);

    array = new std::string[size];

    for (size_t i = 0; i < size; ++i)
    {
        std::getline(file, line);
        array[i] = line;
    }
    file.close();
}

void reader::read_csv(const std::string& filename, std::string*& header, float**& rows, size_t& size, size_t& countFilds)
{
    std::string* data;
    countFilds = 0;

    read_csv(filename, data, size);

    std::istringstream headerStream(data[0]);
    std::string field;
    while (std::getline(headerStream, field, ','))
    {
        countFilds++;
    }

    header = new std::string[countFilds];
    headerStream.clear();
    headerStream.seekg(0);
    for (size_t i = 0; i < countFilds; ++i)
    {
        std::getline(headerStream, header[i], ',');
    }

    rows = new float *[size - 1];
    for (size_t i = 1; i < size; i++)
    {
        rows[i - 1] = new float[countFilds];
        std::istringstream rowStream(data[i]);
        for (size_t j = 0; j < countFilds; j++)
        {
            std::string value;
            std::getline(rowStream, value, ',');
            try
            {
                rows[i - 1][j] = std::stof(value);
            } catch (const std::invalid_argument& e)
            {
            }
        }
    }
    size--;
    delete[] data;
}

