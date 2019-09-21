using System.IO;
using System.Threading.Tasks;
using MitsubaArchivizer.Models;
using MitsubaArchivizer.Utils;
using Newtonsoft.Json;

namespace MitsubaArchivizer.Processors
{
    public class JsonSerializerProcessor : IProcessor
    {
        public string OutputDirectory { get; set; }
        public bool Formatted { get; set; }

        public JsonSerializerProcessor(string outputDirectory, bool formatted)
        {
            OutputDirectory = outputDirectory;
            Formatted = formatted;
        }

        public string GetName() => "JSON Serializer";

        public async Task ProcessThread(Thread thread)
        {
            var outDir = PathUtils.GetOutputPath(OutputDirectory, thread);

            Directory.CreateDirectory(outDir);
                
            var finalFilePath = Path.Combine(outDir, "thread.json");
            var serialized = JsonConvert.SerializeObject(thread, Formatted ? Formatting.Indented : Formatting.None);
            File.WriteAllText(finalFilePath, serialized);
        }
    }
}