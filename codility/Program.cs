using codility.Lessons.Lesson1;
using codility.Lessons.Lesson2;
using codility.Lessons.Lesson3;
using codility.Lessons.Lesson4;
using codility.Lessons.Lesson5;
using codility.TestFramework;
using System;

namespace codility
{
    class Program
    {
        static void Test<TTester>() where
            TTester : ISelfTestAndShowResults, new()
        {
            var tester = new TTester();
            var rs = tester.TestAndShowResults();
            foreach (var r in rs)
            {
                Console.WriteLine(r);
            }
        }

        static void Main(string[] args)
        {
            //Test<MissingInteger.Tester>();
            //Test<BinaryGap.Tester>();
            //Test<CyclicRotation.Tester>();
            //Test<OddOccurrencesInArray.Tester>();
            //Test<TapeEquilibrium.Tester>();
            //Test<PermMissingElem.Tester>();
            //Test<FrogRiverOne.Tester>();
            //Test<PermCheck.Tester>();
            //Test<MaxCounters.Tester>();
            //Test<CountDiv.Tester>();
            //Test<PassingCars.Tester>();
            //Test<GenomicRangeQuery.Tester>();
            Test<MinAvgTwoSlice.Tester>();
        }
    }
}
