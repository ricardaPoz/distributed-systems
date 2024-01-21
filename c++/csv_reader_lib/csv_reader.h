#ifndef CSV_READER_H
#define CSV_READER_H
#include <string>

namespace reader
{
    void read_csv(const std::string& filename, std::string*& array, size_t& size);

    void read_csv(const std::string& filename, std::string*& header, float**& rows, size_t& count_rows, size_t& count_column);
}


#endif //CSV_READER_H
