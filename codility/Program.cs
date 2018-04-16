using codility.Lessons.Lesson1;
using codility.Lessons.Lesson10;
using codility.Lessons.Lesson11;
using codility.Lessons.Lesson12;
using codility.Lessons.Lesson13;
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

        static void Debug<TTester, T>() where
            TTester : BaseSelfTester<T>, new()
            where T : ITestee, new()
        {
            var tester = new TTester();
            var rs = tester.Test(tester.Testee);
            var c = 0;
            foreach (var r in rs)
            {
                if (!r.Passed)
                {
                    var ar = (int[])r.TestSet.Input[0];
                    var l = ar.Length;
                    Console.WriteLine($"Test {++c} failed, length {l}, expected{r.TestSet.ExpectedOutput}, actual {r.Actual}");
                }
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
            //Test<CountNonDivisible.Tester>();
            //Test<ChocolatesByNumbers.Tester>();
            //Test<CommonPrimeDivisors.Tester>();
#if false
            //new FibFrog().TestFindFib();
            new FibFrog.Tester().TestFindFibs();

            var lvs = new[] { 1, 9, 15, 17, 22, 34, 40, 47, 65, 77, 91, 96 };
                var a = new int[98];
                foreach (var lv in lvs) a[lv] = 1;
                var ff = new FibFrog();
                var i = ff.Solve(a);
                Console.WriteLine($"i = {i}");
                var i1 = ff.Solve1(a);
                Console.WriteLine($"i1 = {i1}");
#else
        //    Test<FibFrog.Tester>();
            //Debug<FibFrog.Tester, FibFrog>();
#if true
            const int len = 100000;
            var a = new int[len];
            for (var i = 0; i < len; i++) a[i] = i%3==0? 1 : 0;
            var res = new FibFrog().Solve(a);
            Console.WriteLine($"res = {res}");
#endif
#endif
        }
    }
}
