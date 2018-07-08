using codility.Lessons.Lesson1;
using codility.Lessons.Lesson10;
using codility.Lessons.Lesson11;
using codility.Lessons.Lesson12;
using codility.Lessons.Lesson13;
using codility.Lessons.Lesson14;
using codility.Lessons.Lesson15;
using codility.Lessons.Lesson16;
using codility.Lessons.Lesson17;
using codility.Lessons.Lesson2;
using codility.Lessons.Lesson3;
using codility.Lessons.Lesson4;
using codility.Lessons.Lesson5;
using codility.Lessons.Lesson6;
using codility.Lessons.Lesson7;
using codility.Lessons.Lesson8;
using codility.Lessons.Lesson9;
using codility.Lessons.Lesson90;
using codility.Lessons.Lesson91;
using codility.Lessons.Lesson92;
using codility.Lessons.Lesson99;
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
            //Test<FibFrog.Tester>();
            //Test<Ladder.Tester>();
            //Test<MinMaxDivision.Tester>();
            //Test<NailingPlanks.Tester>();
            //Test<CountDistinctSlices.Tester>();
            //Test<CountTriangles.Tester>();
            //Test<AbsDistinct.Tester>();
            //Test<MinAbsSumOfTwo.Tester>();
            //Test<TieRopes.Tester>();
            //Test<MaxNonoverlappingSegments.Tester>();
            //Test<NumberSolitaire.Tester>();
            //Test<MinAbsSum.Tester>();
            //Test<LongestPassword.Tester>();
            //Test<FloodDepth.Tester>();

            //Test<SlalomSkiing.Tester>();

            //Test<DwarfsRafting.Tester>();
            //Test<RectangleBuilderGreaterArea.Tester>();
            //Test<TennisTournament.Tester>();
            //Test<SocksLaundering.Tester>();
            //Test<HilbertMaze.Tester>();

            //Test<TreeProduct.Tester>();
            //Test<StrSymmetryPoint.Tester>();
            //Test<TreeHeight.Tester>();
            //Test<ArrayInversionCount.Tester>();
            //Test<PolygonConcavityIndex.Tester>();
            //Test<ArrayRecovery.Tester>();
            Test<DiamondsCount.Tester>();
        }
    }
}
