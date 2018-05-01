using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using BenchmarkDotNet.Attributes.Exporters;
using BenchmarkDotNet.Attributes.Jobs;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;

namespace NetCore
{
    [CoreJob]
    [RPlotExporter, RankColumn]
    public class BenchmarkReadOnlyList
    {
        private List<int> BigList;
        private ImmutableList<int> BigImmutableList;

        [Params(1000)]
        public int Count { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            var r = new Random();
            BigList = Enumerable.Range(0, Count).Select(i => r.Next()).ToList();
            BigImmutableList = ImmutableList.Create<int>(BigList.ToArray());
        }

        [Benchmark]
        public object List_Add()
        {
            return BigList.Append(42)
                .ToList()
                .AsReadOnly();
        }

        [Benchmark]
        public object ImmutableList_Add()
        {
            return BigImmutableList
                .Add(42);
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void Spike(int i)
        {

        }

    }
}
