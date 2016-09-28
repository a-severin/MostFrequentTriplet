using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace MostFrequentTriplet {
    public static class MostFrequentTripletStringExtesion {
        public static Dictionary<string, Count> CollectTriplets(int startIndex, string str, CancellationToken ct) {
            ct.ThrowIfCancellationRequested();
            var map = new Dictionary<string, Count>();
            for (var i = startIndex; i < str.Length - 2; i += 3) {
                var triplet = str.Substring(i, 3);
                Count count;
                if (map.TryGetValue(triplet, out count)) {
                    count.Value++;
                } else {
                    map.Add(triplet, new Count());
                }
                ct.ThrowIfCancellationRequested();
            }
            return map;
        }

        public static string MostFrequentTriplet(this string str, CancellationToken ct) {
            ct.ThrowIfCancellationRequested();

            var task0 = Task.Run(() => CollectTriplets(0, str, ct), ct);
            var task1 = Task.Run(() => CollectTriplets(1, str, ct), ct);
            var task2 = Task.Run(() => CollectTriplets(2, str, ct), ct);

            var result = Task.WhenAll(task0, task1, task2).Result;

            ct.ThrowIfCancellationRequested();

            var max = 0;
            var triplest = new List<string>();
            foreach (var pair in result.SelectMany(_ => _)) {
                if (max > pair.Value.Value) {
                    continue;
                }
                if (max == pair.Value.Value) {
                    triplest.Add(pair.Key);
                }
                if (max < pair.Value.Value) {
                    triplest.Clear();
                    triplest.Add(pair.Key);
                    max = pair.Value.Value;
                }

                ct.ThrowIfCancellationRequested();
            }

            return $"{string.Join(",", triplest.Distinct())}\t{max}";
        }

        public static string MostFrequentTriplet2(this string str, CancellationToken ct) {
            if (ct.IsCancellationRequested) {
                return string.Empty;
            }

            var task0 = Task.Run(() => CollectTriplets(0, str, ct), ct);
            var task1 = Task.Run(() => CollectTriplets(1, str, ct), ct);
            var task2 = Task.Run(() => CollectTriplets(2, str, ct), ct);

            Dictionary<string, Count>[] result;
            try {
                result = Task.WhenAll(task0, task1, task2).Result;
            } catch (AggregateException e) {
                if (e.InnerException is OperationCanceledException) {
                    return string.Empty;
                }
                throw;
            }

            var max = 0;
            var triplest = new List<string>();
            foreach (var pair in result.SelectMany(_ => _)) {
                if (max > pair.Value.Value) {
                    continue;
                }
                if (max == pair.Value.Value) {
                    triplest.Add(pair.Key);
                }
                if (max < pair.Value.Value) {
                    triplest.Clear();
                    triplest.Add(pair.Key);
                    max = pair.Value.Value;
                }

                if (ct.IsCancellationRequested) {
                    return string.Empty;
                }
            }

            return $"{string.Join(",", triplest.Distinct())}\t{max}";
        }

        public class Count {
            public int Value = 1;
        }
    }
}