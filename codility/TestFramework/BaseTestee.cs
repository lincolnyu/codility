using System;
using System.Reflection;

namespace codility.TestFramework
{
    public class BaseTestee : ITestee
    {
        public object Run(params object[] args)
        {
            var t = GetType();
            var bf = BindingFlags.Public | BindingFlags.IgnoreCase
                | BindingFlags.Instance | BindingFlags.Static;
            var sol1 = t.GetMethod("solution", bf);
            if (sol1 !=null)
            {
                return sol1.Invoke(this, args);
            }
            var sol2 = t.GetMethod("solve", bf);
            if (sol2 != null)
            {
                return sol2.Invoke(this, args);
            }
            throw new NotImplementedException("Solution function not defined in the class");
        }
    }
}
