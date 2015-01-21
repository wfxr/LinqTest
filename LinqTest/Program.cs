using System;
using System.Collections.Generic;
using System.Xml.Schema;

namespace LinqTest {
    internal class Program {
        private static void Main(string[] args) {
            const int minSampleSize = 5120000;
            const int maxSampleSize = 128000000;
            const int repeatTimes = 3;
            const int step = maxSampleSize/4;
            const int growthFactor = 2;
            for (var sampleSize = step; sampleSize <= maxSampleSize;
                sampleSize += step) {
                Console.WriteLine("\n\n\tSample size: {0:N0}", sampleSize);

                Console.WriteLine("\nTest using List<string>:");
                TestUsingStringList(sampleSize, repeatTimes);

                Console.WriteLine("\nTest using List<int>:");
                TestUsingIntList(sampleSize, repeatTimes);

                Console.WriteLine("\nTest using List<UserType>:");
                TestUsingUserTypeList(sampleSize, repeatTimes);
                Console.WriteLine("========================================");
            }
        }

        private static void TestUsingStringList(int sampleSize, int repeatTimes) {
            var strSamples = PrepareStringSamples(sampleSize);
            DoTest(strSamples, repeatTimes);
        }
        private static void TestUsingIntList(int sampleSize, int repeatTimes) {
            var intSamples = PrepareIntSamples(sampleSize);
            DoTest(intSamples, repeatTimes);
        }

        private static void TestUsingUserTypeList(int sampleSize, int repeatTimes) {
            var intSamples = PrepareUserTypeSamples(sampleSize);
            DoTest(intSamples, repeatTimes);
        }
        private static void DoTest(List<object> samples, int repeatTimes)
        {
            var rand = new Random();
            var sampleSize = samples.Count;
            var sought = samples[rand.Next(0, sampleSize)];
            var lt = new LinqTest(samples, sought, repeatTimes);

            Console.WriteLine("operation\ttime");
            Console.WriteLine("------------------------");

            var msForeach = lt.DoForeachTimingTest();
            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = ConsoleColor.Yellow;
            ShowElapsedTime(msForeach, "foreach");
            var msLinq = lt.DoLinqTimingTest();
            Console.ForegroundColor = ConsoleColor.Green;
            ShowElapsedTime(msLinq, "linq");
            Console.ForegroundColor = defaultColor;

            Console.WriteLine("------------------------");
            Console.WriteLine("linq's relative efficiency: {0:P}",
                (double)msForeach / msLinq);

        }
        private static void ShowElapsedTime(Int64 time, string name) {
            Console.WriteLine("{0}\t\t{1}ms", name, time);
        }

        private static List<object> PrepareSamples(int sampleSize, List<object> baseSample) {
            var samples = new List<object>();
            for (var i = 0; i < sampleSize; ++i)
                samples.Add(baseSample[i%baseSample.Count]);
            return samples;
        }

        private static List<object> PrepareStringSamples(int sampleSize) {
            var baseSample = new List<object>()
            {"Tom", "Jack", "John", "Smith", "Richard"};
            return PrepareSamples(sampleSize, baseSample);
        }

        private static List<object> PrepareIntSamples(int sampleSize) {
            var baseSample = new List<object>() {1, 2, 3, 4, 5, 6, 7, 8, 9, 0};
            return PrepareSamples(sampleSize, baseSample);
        }

        private static List<object> PrepareUserTypeSamples(int sampleSize) {
            var baseSample = new List<object>() {
                new Student("Tom", "1043062001", 100), 
                new Student("Jack", "1043062002", 87),
                new Student("John", "1043062003", 96),
                new Student("Smith", "1043062004", 52),
                new Student("Richard", "1043062005", 78)
            };
            return PrepareSamples(sampleSize, baseSample);
        }
    }

    class Student {
        public Student(string name, string id, int score) {
            Name = name;
            Id = id;
            Score = score;
        }
        public string Name { get; set; }
        public string Id { get; set; }
        public int Score { get; set; }
    }
}
