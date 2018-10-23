using NodaTime;
using System.Collections.Generic;
using System.Collections.Immutable;

namespace PipeWriterSample
{
    public class Record 
    {
        public Record(Instant recordTime, IEnumerable<RecordEntry> entries)
        {
            Entries = entries.ToImmutableList();
            RecordTime = recordTime;
        }

        public Record()
        {

        }

        public ImmutableList<RecordEntry> Entries { get; set; }

        public Instant RecordTime { get; set; }

    }
}
