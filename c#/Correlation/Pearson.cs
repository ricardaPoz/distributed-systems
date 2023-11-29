
using System.Numerics;

namespace CSharp.Correlation;

public class Pearson<T> : ICorrelation<T>
    where T : struct, IFloatingPoint<T>
{
    public T Correlation(ref T[] x, ref T[] y)
    {
        if (x.Length != y.Length)
            throw new ArgumentException();

        int n = x.Length;

        T meanX = x.Aggregate((acc, current) => acc + current);
        T meanY = x.Aggregate((acc, current) => acc + current);

        T numerator = default;
        T denominatorX = default;
        T denominatorY = default;

        for (int i = 0; i < n; i++)
        {
            numerator += (x[i] - meanX) * (y[i] - meanY);
            denominatorX += ConvertTo(Math.Pow(ConvertTo<double>(x[i] - meanX), 2));
            denominatorY += ConvertTo(Math.Pow(ConvertTo<double>(y[i] - meanY), 2));
        }

        T correlation = numerator / ConvertTo(Math.Sqrt(ConvertTo<double>(denominatorX * denominatorY)));

        return correlation;
    }

    private K ConvertTo<K>(T value) where K : struct, IFloatingPoint<K>
    {
        return (K)Convert.ChangeType(value, typeof(K));
    }

    private T ConvertTo<K>(K value) where K : IFloatingPoint<K>
    {
        return (T)Convert.ChangeType(value, typeof(T));
    }
}

public class Pearson : ICorrelation
{
    public double Correlation(ref double[] x, ref double[] y)
    {
        if (x.Length != y.Length)
            throw new ArgumentException();

        int n = x.Length;
        double meanX = x.Average();
        double meanY = y.Average();

        double numerator = 0;
        double denominatorX = 0;
        double denominatorY = 0;

        for (int i = 0; i < n; i++)
        {
            numerator += (x[i] - meanX) * (y[i] - meanY);
            denominatorX += Math.Pow(x[i] - meanX, 2);
            denominatorY += Math.Pow(y[i] - meanY, 2);
        }

        double correlation = numerator / Math.Sqrt(denominatorX * denominatorY);

        return correlation;
    }

    public float Correlation(ref float[] x, ref float[] y)
    {
        if (x.Length != y.Length)
            throw new ArgumentException();

        int n = x.Length;
        float meanX = x.Average();
        float meanY = y.Average();

        float numerator = 0;
        float denominatorX = 0;
        float denominatorY = 0;

        for (int i = 0; i < n; i++)
        {
            numerator += (x[i] - meanX) * (y[i] - meanY);
            denominatorX += MathF.Pow(x[i] - meanX, 2);
            denominatorY += MathF.Pow(y[i] - meanY, 2);
        }

        float correlation = numerator / MathF.Sqrt(denominatorX * denominatorY);

        return correlation;
    }
}