using System.Threading.Tasks;
using MitsubaArchivizer.Models;

namespace MitsubaArchivizer.Processors
{
    public interface IProcessor
    {
        string GetName();
        Task ProcessThread(Thread thread);
    }
}