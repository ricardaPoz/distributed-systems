#include "csv_reader.h"
#include <fstream>
#include <sstream>

void reader::read_csv(const std::string& filename, std::string*& array, size_t& count_rows)
{
    std::ifstream file(filename);

    count_rows = 0;
    std::string line;

    while (std::getline(file, line))
        count_rows++;

    file.clear();
    file.seekg(0, std::ios::beg);

    array = new std::string[count_rows];

    for (size_t i = 0; i < count_rows; ++i)
    {
        std::getline(file, line);
        array[i] = line;
    }
    file.close();
}

void reader::read_csv(const std::string& filename, std::string*& header, float**& rows, size_t& count_rows, size_t& count_column)
{
    std::string* data;
    count_column = 0;

    read_csv(filename, data, count_rows);

    std::istringstream headerStream(data[0]);
    std::string field;
    while (std::getline(headerStream, field, ','))
    {
        count_column++;
    }

    header = new std::string[count_column];
    headerStream.clear();
    headerStream.seekg(0);
    for (size_t i = 0; i < count_column; ++i)
    {
        std::getline(headerStream, header[i], ',');
    }

    rows = new float *[count_rows - 1];
    for (size_t i = 1; i < count_rows; i++)
    {
        rows[i - 1] = new float[count_column];
        std::istringstream rowStream(data[i]);
        for (size_t j = 0; j < count_column; j++)
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
    count_rows--;
    delete[] data;
}

