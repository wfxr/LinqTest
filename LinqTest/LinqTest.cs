using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace LinqTest
{
    internal class LinqTest
    {
        public LinqTest(List<object> samples, object sought, int repeatTimes = 1)
        {
            Samples = samples;
            Sought = sought;
            RepeatTimes = repeatTimes;
        }

        private delegate void TestDelegate();

        private long TimeCost(TestDelegate testMethod)
        {
            var sw = new Stopwatch();
            sw.Start();
            for (var i = 0; i < RepeatTimes; ++i)
                testMethod();
            sw.Stop();
            return sw.ElapsedMilliseconds / RepeatTimes;
        }

        public long LinqTimingTest() { return TimeCost(DoLinq); }
        public long ForeachTimingTest() { return TimeCost(DoForeach); }
        public long PLinqTimingTest() { return TimeCost(DoPLinq); }

        private void DoPLinq() {
            var count = Samples.AsParallel().Count(sample => Equals(sample, Sought));
        }
        private void DoLinq()
        {
            var count = Samples.Count(sample => Equals(sample, Sought));
        }

        private void DoForeach()
        {
            var count = 0;
            foreach (var sample in Samples) 
                if (Equals(sample, Sought)) ++count;
        }

        public int RepeatTimes
        {
            get { return _repeatTimes; }
            set { _repeatTimes = value < 1 ? 1 : value; }
        }

        private List<object> Samples { get; set; }
        private object Sought { get; set; }
        private int _repeatTimes;
    }
}