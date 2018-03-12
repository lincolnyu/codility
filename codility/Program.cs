using codility.Lessons.Lesson4;
using System;

namespace codility
{
    class Program
    {
        static void Main(string[] args)
        {
            var tester = new MissingInteger.Tester();
            var rs = tester.TestAndShowResults();
            foreach (var r in rs)
            {
                Console.WriteLine(r);
            }
        }
    }
}
