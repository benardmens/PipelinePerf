using Microsoft.VisualStudio.TestTools.UnitTesting;
using NodaTime;
using System;
using System.Diagnostics;
using System.Linq;

namespace PipeWriterSample.Test
{
    [TestClass]
    public class PocTest
    {
        [TestMethod]
        public void Poc()
        {
            var random = new Random();
            var writer = new AsyncToBinaryConverter(new[] { "", "as", "asdsa" }, new ObjectToJsonRecordSerializer());
            // var reader = new FromBinaryConverter();
            var count = 0;
            writer.OnBytes += x =>
            {

                var m = x;
            };
            //   reader.OnObject += x => count++;
            var record = new Record(SystemClock.Instance.GetCurrentInstant(), Enumerable.Range(1, 50).Select(x => new RecordEntry(random.NextDouble())));
            var records = Enumerable.Range(1, 10000).Select(x => record);
            var sw = Stopwatch.StartNew();
            writer.Write(records).Wait();
            sw.Stop();
            Console.WriteLine(sw.ElapsedMilliseconds);

            //Assert.AreEqual(10000, count);
        }

    }
}
