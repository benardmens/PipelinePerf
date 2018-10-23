using Newtonsoft.Json;
using System;
using System.Buffers;
using System.IO;
using System.IO.Compression;
using System.IO.Pipelines;
using System.Text;
using System.Threading.Tasks;

namespace PipeWriterSample
{
    public class ObjectToJsonRecordSerializer : IRecordDataSerializer
    {
        public async Task<bool> Serialize(object record, PipeWriter writer)
        {
            try
            {
                var bytes = Encoding.ASCII.GetBytes(Serializer.Serialize(record, Formatting.None));
                var sizeBytes = BitConverter.GetBytes(bytes.Length);
                await writer.WriteAndAdvance(sizeBytes);

                await writer.WriteAndAdvance(bytes);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        private readonly ArrayPool<byte> _arrayPool = ArrayPool<byte>.Shared;
        private byte[] _rented;
        public async Task<bool> SerializeCompressed(object record, PipeWriter writer)
        {
            try
            {
                var data = Encoding.ASCII.GetBytes(Serializer.Serialize(record, Formatting.None));
                _rented = _arrayPool.Rent(data.Length * 2);

                using (var compressedStream = new MemoryStream(_rented))
                using (var zipStream = new GZipStream(compressedStream, CompressionMode.Compress))
                {
                    zipStream.Write(data, 0, data.Length);


                    var sizeBytes = BitConverter.GetBytes((int)compressedStream.Position);
                    await writer.WriteAndAdvance(sizeBytes);

                    //get the slice of the stream actually written to and persist
                    Memory<byte> memSlice = _rented;
                    await writer.WriteAndAdvance(memSlice.Slice(0, (int)compressedStream.Position));
                    _arrayPool.Return(_rented);
                    zipStream.Close();
                }

            }
            catch (Exception)
            {
                _arrayPool.Return(_rented);
                return false;
            }
            return true;
        }
    }
}
