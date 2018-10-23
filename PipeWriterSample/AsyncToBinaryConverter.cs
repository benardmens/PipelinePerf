using NodaTime.Serialization.JsonNet;
using System;
using System.Buffers;
using System.Collections.Generic;
using System.IO.Pipelines;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PipeWriterSample
{

    public static class PipeWriterExtensions
    {
        public static async Task WriteAndAdvance(this PipeWriter writer, byte[] data)
        {
            await writer.WriteAsync(data);
        }

        public static async Task WriteAndAdvance(this PipeWriter writer, Memory<byte> data)
        {
            await writer.WriteAsync(data);
        }
    }
    public class AsyncToBinaryConverter
    {
        private readonly string[] _headers;
        public event Action<PipeReader> OnBytes;
        private readonly Pipe _pipe = new Pipe();
        private bool _infoWritten;
        private readonly IRecordDataSerializer _serializer;
        private readonly object[] _array = new object[1];
        private PipeWriter _writer;

        public AsyncToBinaryConverter(IEnumerable<string> headers, IRecordDataSerializer serializer)
        {
            _serializer = serializer;
            _headers = headers.ToArray();
        }


        public Task<bool> WriteAsync(object subject)
        {
            _array[0] = subject;
            return Write(_array);
        }

        public async Task<bool> Write<TSubjectType>(IEnumerable<TSubjectType> subjects)
        {
            _pipe.Writer.Complete();
            _pipe.Reader.Complete();
            _pipe.Reset();
            //if any fails we cannot output
            _writer = _pipe.Writer;
            foreach (var subject in subjects)
            {
                var success = await Convert(subject);
                if (!success) return false;
            }
            await _writer.FlushAsync();

            //process and output the buffer
            OnBytes?.Invoke(_pipe.Reader);
            return true;
        }



        private async Task<bool> Convert(object subject)
        {

            try
            {
                //only write the type and headers once
                if (!_infoWritten)
                {
                    var result = await AddInfoBytes(subject.GetType(), _writer);
                    if (!result)
                    {
                        return false;
                    }
                    _infoWritten = true;
                }

                //serialize and compress the sample
                await _serializer.SerializeCompressed(subject, _writer);

                return true;
            }
            catch
            {
                return false;
            }
        }

        private async Task<bool> AddInfoBytes(Type subjectType, PipeWriter writer)
        {

            try
            {
                //get the string that's useable by Type.GetType

                var typeString = $"{subjectType.AssemblyQualifiedName}";
                var bytes = Encoding.ASCII.GetBytes(typeString);
                var contentLength = bytes.Length;
                var sizeBytes = BitConverter.GetBytes(contentLength);
                //write the size...
                await writer.WriteAndAdvance(sizeBytes);

                //and the contents
                await writer.WriteAndAdvance(bytes);

                //write the header bytes
                await _serializer.Serialize(_headers, writer);
            }
            catch (Exception ex)
            {
                return false;
            }
            return true;
        }
    }
}
