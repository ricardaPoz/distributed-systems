#ifndef CSV_READER_H
#define CSV_READER_H
#include <string>
#include <fstream>

namespace reader
{
    void read_csv(const std::string& filename, std::string*& array, size_t& size);

    void read_csv(const std::string& filename, std::string*& header, float**& rows, size_t& size, size_t& countFilds);
}


#endif //CSV_READER_H