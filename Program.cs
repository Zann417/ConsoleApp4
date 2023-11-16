using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

class Program
{
    static void Main()
    {
        const int arraySize = 10000000;
        const int minValue = -10000;
        const int maxValue = 10000;
        int[] numbers = GenerateRandomArray(arraySize, minValue, maxValue);

        int threadsCount = Environment.ProcessorCount;
        int[] sums = new int[threadsCount];
        Stopwatch stopwatch = new Stopwatch();

        Console.WriteLine($"Вычисление суммы {arraySize} случайных чисел в {threadsCount} потоках.");

        for (int i = 1; i <= threadsCount; i++)
        {
            int threadIndex = i;
            stopwatch.Restart();

            Parallel.For(0, i, j =>
            {
                int elementsPerThread = arraySize / i;
                int startIndex = threadIndex * elementsPerThread - elementsPerThread;
                int endIndex = (threadIndex == i) ? arraySize : threadIndex * elementsPerThread;

                for (int k = startIndex; k < endIndex; k++)
                {
                    Interlocked.Add(ref sums[j], numbers[k]);
                }
            });

            stopwatch.Stop();
            Console.WriteLine($"Сумма при {i} потоках: {sums.Sum()} (Время: {stopwatch.ElapsedMilliseconds} мс)");
        }
    }

    static int[] GenerateRandomArray(int size, int minValue, int maxValue)
    {
        Random random = new Random();
        int[] array = new int[size];
        for (int i = 0; i < size; i++)
        {
            array[i] = random.Next(minValue, maxValue + 1);
        }
        return array;
    }
}