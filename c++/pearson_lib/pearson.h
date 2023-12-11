#ifndef PEARSON_H
#define PEARSON_H
#include <cmath>

namespace pearson
{
    float Correlation(const float* x, const float* y ,size_t size);
    float Average(const float* arr, size_t size);
}


#endif // PEARSON_H
