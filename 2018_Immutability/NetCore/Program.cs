using BenchmarkDotNet.Running;
using System;

namespace NetCore
{
    class Program
    {
        static void Main(string[] args)
        {
            RunBench<BenchmarkReadOnlyList>();
            RunBench<BenchmarkList>();
            RunBench<BenchmarkDictionary>();
            Console.ReadLine();
        }

        private static void RunBench<T>()
        {
            var summary = BenchmarkRunner.Run<T>();

            Console.WriteLine(summary);
        }
    }
}
