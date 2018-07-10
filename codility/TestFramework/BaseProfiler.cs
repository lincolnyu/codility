using System.Collections.Generic;

namespace codility.TestFramework
{
    using System;
    using static BaseTester;

    public interface IProfiler<TTestee> where TTestee : ITestee
    {
        IEnumerable<TestSet> GetProfilingTestSets();

        IEnumerable<string> ProfileAndShowResults(TTestee testee);

    }

    public interface ISelfProfileAndShowResults
    {
        IEnumerable<string> ProfileAndShowResults();
    }

    public abstract class BaseProfiler<TTestee> : IProfiler<TTestee> where TTestee : ITestee
    {
        public abstract IEnumerable<TestSet> GetProfilingTestSets();

        public  IEnumerable<string> ProfileAndShowResults(TTestee testee)
        {
            var prs = Profile(testee);
            var i = 1;
            foreach (var pr in prs)
            {
                yield return $"Test {i++} took {pr.Elapse.TotalSeconds} second(s).";
            }
        }

        public virtual IEnumerable<TestResult> Profile(ITestee testee)
        {
            var testsets = GetProfilingTestSets();
            foreach (var testset in testsets)
            {
                var start = DateTime.UtcNow;
                var actual = testee.Run(testset.Input);
                var end = DateTime.UtcNow;
                var tr = new TestResult
                {
                    TestSet = testset,
                    Actual = actual,
                    Passed = false, // not cared
                    Elapse = end - start
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
