using System.Text;

namespace codility.Helpers
{
    static class ResultsHelper
    {
        public static bool ResultsEqual(int[] aa, int[] ab)
        {
            if (aa.Length != ab.Length) return false;
            for (var i = 0; i < aa.Length; i++)
            {
                if (aa[i] != ab[i]) return false;
            }
            return true;
        }

        public static string ResultToString(int[] a)
        {
            var sb = new StringBuilder();
            sb.Append("{");
            foreach (var v in a)
            {
                sb.Append(v);
                sb.Append(',');
            }
            sb.Remove(sb.Length - 1, 1);
            sb.Append('}');
            return sb.ToString();
        }
    }
}
