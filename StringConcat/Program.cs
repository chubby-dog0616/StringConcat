using BenchmarkDotNet.Running;

namespace StringConcat
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var summary = BenchmarkRunner.Run<StringConcatTest>();

            // 簡易的な結合結果の検証
            var st = new StringConcatTest();

            var list1 = st.SpanStringJoin();
            var list2 = st.SpanStringBuilder();
            var list3 = st.SpanDefaultInterpolatedStringHandler();
            var list4 = st.ChunkStringJoin();
            var list5 = st.ChunkStringBuilder();
            var list6 = st.ChunkDefaultInterpolatedStringHandler();

            Console.WriteLine($"1 is 2: {list1.SequenceEqual(list2)}");
            Console.WriteLine($"1 is 3: {list1.SequenceEqual(list3)}");
            Console.WriteLine($"1 is 4: {list1.SequenceEqual(list4)}");
            Console.WriteLine($"1 is 5: {list1.SequenceEqual(list5)}");
            Console.WriteLine($"1 is 6: {list1.SequenceEqual(list6)}");
        }
    }
}
