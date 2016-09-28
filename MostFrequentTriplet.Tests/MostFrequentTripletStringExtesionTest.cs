using System;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using MostFrequentTriplet.Tests.Properties;
using NUnit.Framework;

namespace MostFrequentTriplet.Tests {
    [TestFixture]
    public class MostFrequentTripletStringExtesionTest {
        [Test]
        [TestCase("asdqweasdqweasdzxc", 0, "asd", 3)]
        [TestCase("asdqweasdqweasdzxc", 1, "sdq", 2)]
        [TestCase("asdqweasdqweasdzxc", 2, "dqw", 2)]
        public void CollectTriplets_ReturnRightMap(string str, int startIndex, string mostFrequentTriplet, int count) {
            var map = MostFrequentTripletStringExtesion.CollectTriplets(startIndex, str, CancellationToken.None);
            var mostFrequentPair = map.OrderByDescending(_ => _.Value.Value).First();

            Assert.AreEqual(mostFrequentTriplet, mostFrequentPair.Key);
            Assert.AreEqual(count, mostFrequentPair.Value.Value);
        }

        [Test]
        public void MostFrequentTriplet_Benchmark() {
            var sw = Stopwatch.StartNew();
            var result = Resources.str.MostFrequentTriplet(CancellationToken.None);
            var elapsed = sw.Elapsed;
            sw.Stop();
            Console.WriteLine(elapsed);
            Console.WriteLine(result);
            Assert.True(elapsed < TimeSpan.FromMilliseconds(100));
        }

        [Test]
        public void MostFrequentTriplet_ReturnRightResult() {
            var result = "asdqweasdqweasdzxc".MostFrequentTriplet(CancellationToken.None);

            Assert.AreEqual("asd\t3", result);
        }

        [Test]
        public void MostFrequentTriplet_ThrowOperationCanceledException_OnCancel() {
            var cts = new CancellationTokenSource(100);
            cts.Cancel();
            Assert.Throws<OperationCanceledException>(() => Resources.str.MostFrequentTriplet(cts.Token));
        }

        [Test]
        public void MostFrequentTriplet2_ReturnEmptyString_OnCancel() {
            var cts = new CancellationTokenSource(100);
            var sw = Stopwatch.StartNew();
            cts.CancelAfter(10);
            var result = Resources.str.MostFrequentTriplet2(cts.Token);
            var elapsed = sw.Elapsed;
            sw.Stop();
            Assert.IsEmpty(result);
            Console.WriteLine(elapsed);
            Console.WriteLine(result);
            Assert.True(elapsed < TimeSpan.FromMilliseconds(20));
        }
    }
}