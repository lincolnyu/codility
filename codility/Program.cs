using codility.Lessons.Lesson1;
using codility.Lessons.Lesson10;
using codility.Lessons.Lesson11;
using codility.Lessons.Lesson2;
using codility.Lessons.Lesson3;
using codility.Lessons.Lesson4;
using codility.Lessons.Lesson5;
using codility.Lessons.Lesson6;
using codility.Lessons.Lesson7;
using codility.Lessons.Lesson8;
using codility.Lessons.Lesson9;
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
            //Test<MinAvgTwoSlice.Tester>();
            //Test<Triangle.Tester>();
            //Test<Distinct.Tester>();
            //Test<MaxProductOfThree.Tester>();
            //Test<NumberOfDiscIntersections.Tester>();
            //Test<StoneWall.Tester>();
            //Test<Brackets.Tester>();
            //Test<Nesting.Tester>();
            //Test<Nesting.Tester>();
            //Test<Dominator.Tester>();
            //Test<EquiLeader.Tester>();
            //Test<MaxDoubleSliceSum.Tester>();
            //Test<MaxProfit.Tester>();
            //Test<MaxSliceSum.Tester>();
            //Test<CountDiv.Tester>();
            //Test<MinPerimeterRectangle.Tester>();
            //Test<Flags.Tester>();
            //Test<Peaks.Tester>();
            //Test<CountSemiprimes.Tester>();
            Test<CountNonDivisible.Tester>();
        }
    }
}
