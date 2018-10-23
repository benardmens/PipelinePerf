namespace PipeWriterSample
{
    public class RecordEntry
    {
        public RecordEntry(object value)
        {
            Value = value;
        }

        public RecordEntry()
        {
        }


        public object Value { get; set; }
    }
}
