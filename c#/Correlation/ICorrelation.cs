using System.Numerics;

namespace CSharp.Correlation;

public interface ICorrelation<T>
    where T : struct, IFloatingPoint<T>
{
    T Correlation(ref T[] x, ref T[] y);
}

public interface ICorrelation
{
    double Correlation(ref double[] x, ref double[] y);
    float Correlation(ref float[] x, ref float[] y);
}