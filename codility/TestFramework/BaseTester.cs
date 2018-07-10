using System;
using System.Collections.Generic;

namespace codility.TestFramework
{
    public abstract class BaseTester
    {
        public struct TestSet
        {
            public object[] Input { get; set; }
            public object ExpectedOutput { get; set; }
        }

        public struct TestResult
        {
            public TestSet TestSet { get; set; }
            public object Actual { get; set; }
            public bool Passed { get; set; }
            public TimeSpan Elapse { get; set; }
        }

        public abstract IEnumerable<TestSet> GetTestSets();

        public virtual bool ResultsEqual(object a, object b)
            => a.Equals(b);

        public virtual string ResultToString(object r)
            => r.ToString();

        public virtual IEnumerable<TestResult> Test(ITestee testee)
        {
            var testsets = GetTestSets();
            foreach (var testset in testsets)
            {
                var start = DateTime.UtcNow;
                var actual = testee.Run(testset.Input);
                var end = DateTime.UtcNow;
                var tr = new TestResult
                {
                    TestSet = testset,
                    Actual = actual,
                    Passed = ResultsEqual(testset.ExpectedOutput, actual),
                    Elapse = end - start
                };
                yield return tr;
            }
        }

        public virtual IEnumerable<string> TestAndShowResults(ITestee testee)
        {
            var trs = Test(testee);
            var i = 1;
            foreach (var tr in trs)
            {
                if (tr.Passed)
                {
                    yield return $"Test {i++} passed, taking {tr.Elapse.TotalSeconds} second(s).";
                }
                else
                {
                    yield return $"Test {i++} failed, taking {tr.Elapse.TotalSeconds} second(s), '{ResultToString(tr.TestSet.ExpectedOutput)}' expected but '{ResultToString(tr.Actual)}' returned.";
                    yield break;
                }
            }
        }

        protected TestSet CreateSingleInputSet<TInput, TOutput>(TInput input, TOutput expected)
        {
            return new TestSet
            {
                Input = new object[] { input },
                ExpectedOutput = expected
            };
        }

        protected TestSet Create2InputSet<TInput1, TInput2, TOutput>(TInput1 input1, TInput2 input2, TOutput expected)
        {
            return new TestSet
            {
                Input = new object[] { input1, input2 },
                ExpectedOutput = expected
            };
        }

        protected TestSet Create3InputSet<TInput1, TInput2, TInput3, TOutput>
            (TInput1 input1, TInput2 input2, TInput3 input3, TOutput expected)
        {
            return new TestSet
            {
                Input = new object[] { input1, input2, input3 },
                ExpectedOutput = expected
            };
        }

        protected TestSet Create4InputSet<TInput1, TInput2, TInput3, TInput4, TOutput>
            (TInput1 input1, TInput2 input2, TInput3 input3, TInput4 input4, TOutput expected)
        {
            return new TestSet
            {
                Input = new object[] { input1, input2, input3, input4 },
                ExpectedOutput = expected
            };
        }

        protected TestSet Create5InputSet<TInput1, TInput2, TInput3, TInput4, TInput5, TOutput>
            (TInput1 input1, TInput2 input2, TInput3 input3, TInput4 input4, TInput5 input5, TOutput expected)
        {
            return new TestSet
            {
                Input = new object[] { input1, input2, input3, input4, input5 },
                ExpectedOutput = expected
            };
        }


        public static TestSet CreateInputSet(object expected, params object[] inputs)
        {
            return new TestSet
            {
                Input = inputs,
                ExpectedOutput = expected
            };
        }
    }
}
