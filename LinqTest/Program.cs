using System;
using System.Collections.Generic;
using System.Xml.Schema;

namespace LinqTest {
    internal class Program {
        private static void Main(string[] args) {
            const int maxSampleSize = 60000000;
            const int repeatTimes = 3;
            const int step = maxSampleSize / 6;
            for (var sampleSize = step; sampleSize <= maxSampleSize; sampleSize += step) {
                List<object> samples;
                Console.WriteLine("\n\n\tSample size: {0:N0}", sampleSize);

                // 测试List<int>
                Console.WriteLine("\nTest using List<int>:");
                samples = IntSamples(sampleSize); 
                DoTest(samples, repeatTimes);

                // 测试List<string>
                Console.WriteLine("\nTest using List<string>:");
                samples = StringSamples(sampleSize);
                DoTest(samples, repeatTimes);

                // 测试List<UserType>
                Console.WriteLine("\nTest using List<UserType>:");
                samples = UserTypeSamples(sampleSize);
                DoTest(samples, repeatTimes);

                Console.WriteLine("========================================");
            }
        }

        private static void DoTest(List<object> samples, int repeatTimes)
        {
            var rand = new Random();
            var sampleSize = samples.Count;
            var sought = samples[rand.Next(0, sampleSize)];
            var lt = new LinqTest(samples, sought, repeatTimes);

            const ConsoleColor colorForeach = ConsoleColor.Yellow;
            const ConsoleColor colorLinq = ConsoleColor.Cyan;
            const ConsoleColor colorPlinq = ConsoleColor.Green;

            Console.WriteLine("{0,-10}{1,5}", "mode", "ms elapsed");
            Console.WriteLine("------------------------");

            var msForeach = lt.ForeachTimingTest();
            ShowElapsedTime(msForeach, "foreach", colorForeach);

            var msLinq = lt.LinqTimingTest();
            ShowElapsedTime(msLinq, "linq", colorLinq);

            var msPLinq = lt.PLinqTimingTest();
            ShowElapsedTime(msPLinq, "plinq", colorPlinq);

            Console.WriteLine("------------------------");
            Console.WriteLine("{0,-10}{1,10}", "mode", "relative eff");
            Console.WriteLine("------------------------");
            ShowRelativeEff(msForeach, "foreach", msForeach, colorForeach);
            ShowRelativeEff(msLinq, "linq", msForeach, colorLinq);
            ShowRelativeEff(msPLinq, "plinq", msForeach, colorPlinq);
            Console.WriteLine("------------------------");
        }

        private static void ShowElapsedTime(long msElapsed, string name, ConsoleColor color) {
            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine("{0,-10}{1,5}ms", name, msElapsed);
            Console.ForegroundColor = defaultColor;
        }

        private static void ShowRelativeEff(long msTarget, string name, long msRefer,
            ConsoleColor color) {
            var defaultColor = Console.ForegroundColor;
            Console.ForegroundColor = color;
            Console.WriteLine("{0,-10}{1,10:P}", name, 
                (double)msRefer / msTarget);
            Console.ForegroundColor = defaultColor;
        }

        private static List<object> ProliferateSamples(int sampleSize, List<object> baseSample) {
            var samples = new List<object>();
            for (var i = 0; i < sampleSize; ++i)
                samples.Add(baseSample[i%baseSample.Count]);
            return samples;
        }

        private static List<object> StringSamples(int sampleSize) {
            var baseSample = new List<object>() {"Tom", "Jack", "John", "Smith", "Richard"};
            return ProliferateSamples(sampleSize, baseSample);
        }

        private static List<object> IntSamples(int sampleSize) {
            var baseSample = new List<object>() {1, 2, 3, 4, 5, 6, 7, 8, 9, 0};
            return ProliferateSamples(sampleSize, baseSample);
        }

        private static List<object> UserTypeSamples(int sampleSize) {
            var baseSample = new List<object>() {
                new Student("Tom", "1043062001", 100), 
                new Student("Jack", "1043062002", 52),
                new Student("John", "1043062003", 96),
                new Student("Smith", "1043062004", 52),
                new Student("Richard", "1043062005", 78)
            };
            return ProliferateSamples(sampleSize, baseSample);
        }
    }
}