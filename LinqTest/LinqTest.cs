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

        private Int64 TimingTest(TestDelegate TestMethod)
        {
            var sw = new Stopwatch();
            sw.Start();
            for (var i = 0; i < RepeatTimes; ++i)
                TestMethod();
            sw.Stop();
            return sw.ElapsedMilliseconds / RepeatTimes;
        }

        public Int64 DoLinqTimingTest() { return TimingTest(DoLinq); }
        public Int64 DoForeachTimingTest() { return TimingTest(DoForeach); }

        private void DoLinq()
        {
            var result = Samples.Where(item => Equals(item, Sought)).ToList();
        }

        private void DoForeach()
        {
            var results = new List<object>();
            foreach (var item in Samples)
                if (Equals(item, Sought))
                    results.Add(item);
        }

        public int RepeatTimes
        {
            get { return _repeatTimes; }
            set {
                _repeatTimes = value < 1 ? 1 : value;
            }
        }

        private List<object> Samples { get; set; }
        private object Sought { get; set; }

        private int _repeatTimes;
    }
}