#include "DataArray.h"
#include "pearson.h"

DataArray::DataArray(std::string* header, float** rows, const size_t& countRows,
                     const size_t& countColumns): header(header),
                                                  rows(rows), countRows(countRows),
                                                  countColumns(countColumns)
{
}

DataArray::~DataArray()
{
    delete[] header;
    for (size_t i = 0; i < countRows; i++)
    {
        delete[] rows[i];
    }
    delete[] rows;
}

float** DataArray::CorrelationParallel()
{
    float** result = new float *[countColumns];
    std::thread* threads = new std::thread[countColumns];

    for (size_t i = 0; i < countColumns; i++)
    {
        threads[i] = std::thread([this, i, &result]()
        {
            float* corr = Correlation(i);
            mtx.lock();
            result[i] = corr;
            delete[] corr;
            mtx.unlock();
        });

    }

    for (size_t i = 0; i < countColumns; i++)
    {
        threads[i].join();
    }

    delete[] threads;
    return result;
}

float* DataArray::CorrelationParallel(size_t index)
{
    float* res = new float[countColumns];
    std::thread* threads = new std::thread[countColumns];

    float* y = GetColumnValue(index);
    for (size_t i = 0; i < countColumns; i++)
    {
        threads[i] = std::thread([this, i, y, &res]()
        {
            float* x = GetColumnValue(i);
            float corr = pearson::Correlation(x, y, countRows);
            delete[] x;
            mtx.lock();
            res[i] = corr;
            mtx.unlock();
        });
    }

    for (size_t i = 0; i < countColumns; i++)
    {
        threads[i].join();
    }

    delete[] y;
    delete[] threads;
    return res;
}

float* DataArray::Correlation(size_t index) const
{
    float* row = new float[countColumns];
    float* y = GetColumnValue(index);

    for (size_t i = 0; i < countColumns; i++)
    {
        float* x = GetColumnValue(i);
        float corr = pearson::Correlation(x, y, countRows);
        row[i] = corr;
        delete[] x;
    }
    delete[] y;
    return row;
}

float** DataArray::Correlation() const
{
    float** result = new float *[countColumns];
    for (size_t i = 0; i < countColumns; i++)
    {
        float* corr = Correlation(i);
        result[i] = corr;
        delete[] corr;
    }
    return result;
}

float* DataArray::GetColumnValue(size_t index) const
{
    float* res = new float[countRows];
    for (size_t i = 0; i < countRows; i++)
    {
        res[i] = rows[i][index];
    }
    return res;
}