using System.Collections.Generic;

namespace codility.TestFramework
{
    public interface ISelfTestAndShowResults
    {
        IEnumerable<string> TestAndShowResults();
    }

    public abstract class BaseSelfTester<T> : BaseTester, ISelfTestAndShowResults
        where T : ITestee, new()
    {
        private ITestee _testee = new T();

        public ITestee Testee => _testee;

        public IEnumerable<string> TestAndShowResults() 
            => TestAndShowResults(_testee);
    }
}
