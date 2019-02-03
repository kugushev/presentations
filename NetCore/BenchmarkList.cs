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
    [RPlotExporter]
    public class BenchmarkList
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
        public object List_Select()
        {
            return BigList
                .Select(i => i.ToString())
                .ToList();
        }


        [Benchmark]
        public object List_ConvertAll()
        {
            return BigList.ConvertAll(i => i.ToString());
        }


        [Benchmark]
        public object ImmutableList_Select()
        {
            return BigImmutableList
                .Select(i => i.ToString())
                .ToImmutableList();
        }

        [Benchmark]
        public object ImmutableList_Convert()
        {
            return BigImmutableList.ConvertAll(i => i.ToString());
        }

        [Benchmark]
        public object List_Where()
        {
            return BigList
                .Where(i => i < 100)
                .ToList();
        }

        [Benchmark]
        public object List_FindAll()
        {
            return BigList.FindAll(i => i < 100);
        }

        [Benchmark]
        public object ImmutablsList_Where()
        {
            return BigImmutableList
                .Where(i => i < 100)
                .ToImmutableList();
        }

        [Benchmark]
        public object ImmutablsList_FindAll()
        {
            return BigImmutableList.FindAll(i => i < 100);
        }

        [Benchmark]
        public object List_TakeSkip()
        {            
            return BigList.Skip(Count / 2).Take(Count / 4).ToList();
        }

        [Benchmark]
        public object List_GetRange()
        {
            return BigList.GetRange(Count / 2, Count / 4);
        }

        [Benchmark]
        public object ImmutableList_TakeSkip()
        {
            return BigImmutableList.Skip(Count / 2).Take(Count / 4).ToImmutableList();
        }

        [Benchmark]
        public object ImmutableList_GetRange()
        {
            return BigImmutableList.GetRange(Count / 2, Count / 4);
        }

        [Benchmark]
        public object List_Sort()
        {
            BigList.Sort();
            return BigList;
        }


        [Benchmark]
        public object ImmutableList_Sort()
        {
            return BigImmutableList.Sort();
        }

        [Benchmark]
        public void List_Add()
        {
            var list = new List<int>();
            for (int i = 0; i < Count; i++)
                list.Add(i);
        }

        [Benchmark]
        public void ImmutableList_Add()
        {
            var list = ImmutableList.Create<int>();
            for (int i = 0; i < Count; i++)
                list.Add(i);
        }

        [Benchmark]
        public void List_GetEnumerator_Foreach()
        {
            foreach (var item in BigList)
                Spike(item);
        }

        [Benchmark]
        public void ImmutableList_GetEnumenator_Foreach()
        {
            foreach (var item in BigImmutableList)
                Spike(item);
        }

        [Benchmark]
        public void List_GetEnumerator_For()
        {
            for (int i = 0; i < Count; i++)
                Spike(BigList[i]);
        }

        [Benchmark]
        public void ImmutableList_GetEnumenator_For()
        {
            for (int i = 0; i < Count; i++)
                Spike(BigImmutableList[i]);
        }

        [MethodImpl(MethodImplOptions.NoInlining | MethodImplOptions.NoOptimization)]
        private void Spike(int i)
        {

        }

    }
}
