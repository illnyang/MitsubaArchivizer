using System.IO;
using System.Reflection;
using Jint;
using Microsoft.Extensions.FileProviders;

namespace MitsubaArchivizer.Utils
{
    // too lazy to re-implement this properly in native C# 
    internal static class SeedRandom
    {
        private static readonly Engine Engine;

        static SeedRandom()
        {
            Engine = new Engine();

            var embeddedProvider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());
            using (var stream = embeddedProvider.GetFileInfo("Resources.seedrandom.min.js").CreateReadStream())
            using (var sr = new StreamReader(stream))
            {
                Engine.Execute(sr.ReadToEnd());
            }
        }

        public static double Random()
        {
            return Engine.Execute("Math.random()").GetCompletionValue().AsNumber();
        }

        public static void Seed(string seed)
        {
            Engine.Execute($@"Math.seedrandom('{seed}')");
        }
    }
}