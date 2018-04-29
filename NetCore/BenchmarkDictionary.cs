using BenchmarkDotNet.Attributes;
using BenchmarkDotNet.Attributes.Columns;
using BenchmarkDotNet.Attributes.Exporters;
using BenchmarkDotNet.Attributes.Jobs;
using System;
using System.Collections.Generic;
using System.Collections.Immutable;
using System.Linq;
using System.Text;

namespace NetCore
{
    [CoreJob]
    [RPlotExporter]
    public class BenchmarkDictionary
    {
        private Dictionary<int, string> Big;
        private ImmutableDictionary<int, string> BigImmutable;

        [Params(1000)]
        public int Count { get; set; }

        [GlobalSetup]
        public void GlobalSetup()
        {
            var r = new Random();
            Big = Enumerable.Range(0, Count).ToDictionary(i => i, i => r.Next().ToString());
            BigImmutable = Big.ToImmutableDictionary();
        }

        [Benchmark]
        public object Dictianary_Get()
        {
            return Big[42];
        }

        [Benchmark]
        public object ImmutablDictionary_Get()
        {
            return BigImmutable[42];
        }

        [Benchmark]
        public void Dictianary_Set()
        {
            var newDict = new Dictionary<int, string>(Big);
            newDict[42] = "123";
            newDict.Add(Count + 1, "new");
        }

        [Benchmark]
        public void ImmutablDictionary_Set()
        {
            BigImmutable.SetItem(42, "123");
            BigImmutable.Add(Count + 1, "new");
        }
    }
}
