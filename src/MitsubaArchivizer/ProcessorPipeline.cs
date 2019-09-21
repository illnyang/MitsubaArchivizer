using System.Collections.Generic;
using System.Threading.Tasks;
using MitsubaArchivizer.Models;
using MitsubaArchivizer.Processors;

namespace MitsubaArchivizer
{
    public class ProcessorPipeline
    {
        private readonly List<IProcessor> _processors;
        private readonly MediaResolver _mediaResolver;

        public delegate void ProcessorInvoked(string name);
        public event ProcessorInvoked OnProcessorInvoked;

        public ProcessorPipeline(List<IProcessor> processors, MediaResolver mediaResolver)
        {
            _processors = processors;
            _mediaResolver = mediaResolver;
        }

        public async Task Process(Thread thread)
        {
            await _mediaResolver.ResolveMediaForThread(thread);
            
            foreach (var processor in _processors)
            {
                OnProcessorInvoked?.Invoke(processor.GetName());
                await processor.ProcessThread(thread);
            }
        }
    }
}