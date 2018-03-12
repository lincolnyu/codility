using codility.Lessons.Lesson1;
using codility.Lessons.Lesson4;
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
            Test<BinaryGap.Tester>();
        }
    }
}
