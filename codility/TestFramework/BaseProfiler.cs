using System.Collections.Generic;

namespace codility.TestFramework
{
    using System;
    using static BaseTester;

    public interface IProfiler<TTestee> where TTestee : ITestee
    {
        IEnumerable<TestSet> GetProfilingTestSets();

        IEnumerable<string> ProfileAndShowResults(TTestee testee, int count = 5);

    }

    public interface ISelfProfileAndShowResults
    {
        IEnumerable<string> ProfileAndShowResults();
    }

    public abstract class BaseProfiler<TTestee> : IProfiler<TTestee> where TTestee : ITestee
    {
        public abstract IEnumerable<TestSet> GetProfilingTestSets();

        public  IEnumerable<string> ProfileAndShowResults(TTestee testee, int count = 5)
        {
            var prs = Profile(testee, count);
            var i = 1;
            foreach (var pr in prs)
            {
                yield return $"Test {i++} (returning {pr.Actual}) took {pr.Elapse.TotalSeconds} second(s).";
            }
        }

        public virtual IEnumerable<TestResult> Profile(ITestee testee, int count = 5)
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
        
        public virtual IEnumerable<string> ProfileAndShowResults()
            => ProfileAndShowResults(_testee);
    }
}
