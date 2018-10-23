using System.IO.Pipelines;
using System.Threading.Tasks;

namespace PipeWriterSample
{
    public interface IRecordDataSerializer
    {
        Task<bool> Serialize(object record, PipeWriter writer);
        Task<bool> SerializeCompressed(object record, PipeWriter writer);
    }
}
