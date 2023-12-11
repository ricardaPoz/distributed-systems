#ifndef DATAARRAY_H
#define DATAARRAY_H
#include <string>
#include <ostream>
#include <thread>
#include <mutex>

class DataArray {
public:
    DataArray(std::string* header, float** rows, const size_t& countRows, const size_t& countColumns);
    ~DataArray();
    [[nodiscard]] float* CorrelationParallel(size_t index);
    [[nodiscard]] float* Correlation(size_t index) const;
    [[nodiscard]] float** CorrelationParallel();
    [[nodiscard]] float** Correlation() const;

private:
    std::string* header;
    float** rows;
    size_t countRows;
    size_t countColumns;
    [[nodiscard]] float* GetColumnValue(size_t index) const;
    std::mutex mtx;
};



#endif //DATAARRAY_H
