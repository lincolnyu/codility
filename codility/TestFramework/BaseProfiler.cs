using System.Collections.Generic;

namespace codility.TestFramework
{
    using System;
    using static BaseTester;

    public static class Constants
    {
        public const int DefaultRepetitionCount = 10;
    }

    public interface IProfiler<TTestee> where TTestee : ITestee
    {
        IEnumerable<TestSet> GetProfilingTestSets();

        IEnumerable<string> ProfileAndShowResults(TTestee testee, int count = Constants.DefaultRepetitionCount);

    }

    public interface ISelfProfileAndShowResults
    {
        IEnumerable<string> ProfileAndShowResults(int count = Constants.DefaultRepetitionCount);
    }

    public abstract class BaseProfiler<TTestee> : IProfiler<TTestee> where TTestee : ITestee
    {
        public abstract IEnumerable<TestSet> GetProfilingTestSets();

        public  IEnumerable<string> ProfileAndShowResults(TTestee testee, int count = Constants.DefaultRepetitionCount)
        {
            var prs = Profile(testee, count);
            var i = 1;
            foreach (var pr in prs)
            {
                yield return $"Test {i++} (returning {pr.Actual}) took {pr.Elapse.TotalSeconds} second(s).";
            }
        }

        public virtual IEnumerable<TestResult> Profile(ITestee testee, int count = Constants.DefaultRepetitionCount)
        {
            var testsets = GetProfilingTestSets();
            foreach (var testset in testsets)
            {
                object actual = null;
                TimeSpan total = TimeSpan.Zero;
                for (var i = 0; i < count; i++)
                {
                    var start = DateTime.UtcNow;
                    actual = testee.Run(testset.Input);
                    var end = DateTime.UtcNow;
                    var diff = end-start;
                    total += diff;
                }
                total /= count;
                var tr = new TestResult
                {
                    TestSet = testset,
                    Actual = actual,
                    Passed = false, // not cared
                    Elapse = total
                };
                yield return tr;
            }
        }
    }

    public abstract class BaseSelfProfiler<TTestee> : BaseProfiler<TTestee>, ISelfProfileAndShowResults where TTestee : ITestee, new()
    {
        TTestee _testee = new TTestee();
        
        public virtual IEnumerable<string> ProfileAndShowResults(int count = Constants.DefaultRepetitionCount)
            => ProfileAndShowResults(_testee, count);
    }
}
