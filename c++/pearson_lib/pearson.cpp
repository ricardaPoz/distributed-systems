#include "pearson.h"

float pearson::Correlation(const float* x, const float* y , const size_t size)
{
    const float meanX = Average(x, size);
    const float meanY = Average(y, size);

    float numerator = 0.0f;
    float denominatorX = 0.0f;
    float denominatorY = 0.0f;

    for (size_t i = 0; i < size; i++)
    {
        numerator += (x[i] - meanX) * (y[i] - meanY);
        denominatorX += powf(x[i] - meanX, 2.0f);
        denominatorY += powf(y[i] - meanY, 2.0f);
    }

    return numerator / sqrtf(denominatorX * denominatorY);;
}

float pearson::Average(const float* arr, const size_t size)
{
    float sum = 0.0f;

    for (size_t i = 0; i < size; i++)
    {
        sum += arr[i];
    }

    return sum / static_cast<float>(size);
}

