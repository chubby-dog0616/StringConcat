using BenchmarkDotNet.Attributes;
using System.Runtime.CompilerServices;
using System.Text;

namespace StringConcat
{
    [MemoryDiagnoser]
    public class StringConcatTest
    {
        private const string DELIMITER = ",";
        private readonly int _row = 500;
        private readonly int _col = 10000;
        private readonly double[] _data;
        
        public StringConcatTest()
        {
            _data = GenerateRandomArray(_row * _col);
        }
       

        private static double[] GenerateRandomArray(int size)
        {
            Random random = new();
            return Enumerable.Range(0, size).Select(_ => random.NextDouble() * 2.0).ToArray();
        }

        private void Write(IEnumerable<string> lines)
        {
            foreach (var line in lines) Console.WriteLine(line);
        }

        [Benchmark]
        public List<string> SpanStringJoin()
        {
            List<string> result = [];
            Span<double> dataSpan = _data.AsSpan(); 
            for (int i = 0; i < _row; i++)
            {
                Span<double> rowSpan = dataSpan.Slice(i * _col, _col);
                string rowData = string.Join(DELIMITER, rowSpan.ToArray().Select(d => d.ToString("F2")));
                result.Add($"{i + 1}: {rowData}");
            }
            return result;
        }

        [Benchmark]
        public List<string> SpanStringBuilder()
        {
            List<string> result = [];
            Span<double> dataSpan = _data.AsSpan();
            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < _row; i++)
            {
                Span<double> rowSpan = dataSpan.Slice(i * _col, _col);
                sb.Append(i + 1).Append(": "); // 行番号を追加

                // 各要素を追加（最後の要素には区切り文字を付けない）
                for (int j = 0; j < _col; j++)
                {
                    if (j > 0) sb.Append(DELIMITER);
                    sb.Append(rowSpan[j].ToString("F2"));
                }

                result.Add(sb.ToString());
                sb.Clear();
            }
            return result;
        }

        [Benchmark]
        public List<string> SpanDefaultInterpolatedStringHandler()
        {
            List<string> result = [];
            Span<double> dataSpan = _data.AsSpan();
            var handler = new DefaultInterpolatedStringHandler(0, 0);

            for (int i = 0; i < _row; i++)
            {
                handler.AppendFormatted(i + 1);
                handler.AppendLiteral(": ");

                Span<double> rowSpan = dataSpan.Slice(i * _col, _col);
                for (int j = 0; j < _col; j++)
                {
                    if (j > 0) handler.AppendLiteral(DELIMITER);
                    handler.AppendFormatted(rowSpan[j], "F2");
                }

                result.Add(handler.ToStringAndClear());
            }
            return result;
        }

        [Benchmark]
        public List<string> ChunkStringJoin()
        {
            List<string> result = _data.Chunk(_col).Select((row, index) => (index + 1) + ": " + string.Join(DELIMITER, row.Select(d => d.ToString("F2")))).ToList();
            return result;
        }

        [Benchmark]
        public List<string> ChunkStringBuilder()
        {
            List<string> result = [];
            var sb = new StringBuilder();
            int rowIndex = 1;

            foreach (var row in _data.Chunk(_col))
            {
                sb.Append(rowIndex++).Append(": ");
                sb.Append(string.Join(DELIMITER, row.Select(d => d.ToString("F2"))));
                result.Add(sb.ToString());
                sb.Clear();
            }
            
            return result;
        }

        [Benchmark]
        public List<string> ChunkDefaultInterpolatedStringHandler()
        {
            List<string> result = [];
            var handler = new DefaultInterpolatedStringHandler(0, 0);
            int rowIndex = 1;

            foreach (var row in _data.Chunk(_col))
            {
                handler.AppendFormatted(rowIndex++);
                handler.AppendLiteral(": ");
                handler.AppendLiteral(string.Join(DELIMITER, row.Select(d => d.ToString("F2"))));
                result.Add(handler.ToStringAndClear());
            }
            return result;
        }
    }
}
