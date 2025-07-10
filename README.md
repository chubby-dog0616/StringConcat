## 概要

配列の文字列を先頭から Row * Col の Csv に変換する必要があり、文字列結合のパフォーマンスを検証する

## 検証結果

| Method                                | Mean     | Error    | StdDev   | Gen0       | Gen1      | Allocated |
|-------------------------------------- |---------:|---------:|---------:|-----------:|----------:|----------:|
| SpanStringJoin                        | 796.9 ms | 15.82 ms | 24.62 ms | 47000.0000 | 1000.0000 | 286.42 MB |
| SpanStringBuilder                     | 755.1 ms | 10.67 ms |  9.98 ms | 38000.0000 |         - | 200.51 MB |
| SpanDefaultInterpolatedStringHandler  | 683.1 ms |  7.60 ms |  6.74 ms |          - |         - |  47.96 MB |
| ChunkStringJoin                       | 822.1 ms | 16.12 ms | 24.13 ms | 47000.0000 | 1000.0000 | 286.55 MB |
| ChunkStringBuilder                    | 796.8 ms | 14.29 ms | 13.37 ms | 47000.0000 | 1000.0000 | 286.86 MB |
| ChunkDefaultInterpolatedStringHandler | 816.9 ms | 16.00 ms | 24.43 ms | 47000.0000 | 1000.0000 | 286.55 MB |

速度はともかく、メモリ効率が違いすぎるので可読性に目をつぶって DefaultInterpolatedStringHandler を使う
