using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Reflection;
using Microsoft.Extensions.FileProviders;

namespace MitsubaArchivizer.Utils
{
    internal static class UidHighlighter
    {
        private static readonly List<string> Names = new List<string>();

        static UidHighlighter()
        {
            var embeddedProvider = new EmbeddedFileProvider(Assembly.GetExecutingAssembly());
            using (var stream = embeddedProvider.GetFileInfo("Resources.names.txt").CreateReadStream())
            using (var sr = new StreamReader(stream))
            {
                string line;
                while ((line = sr.ReadLine()) != null)
                {
                    Names.Add(line);
                }
            }
        }

        public static Tuple<string, Color> GetHighlightForPost(uint threadId, string posterId)
        {
            var rnd = new SeedRandom($"{posterId}_t{threadId}");

            var h = (Math.Floor(rnd.Random() * 37) * 10) / 360.0;
            const double s = 0.80;
            const double l = 0.61;

            // dummy call to keep deterministic consistency with karakao ork. no kurwa moje najszczersze gratulacje djmati xD
            rnd.Random();

            var idx = (int) Math.Floor(rnd.Random() * Names.Count);
            return new Tuple<string, Color>(Names[idx], FromHSL(h, s, l));
        }
        
        private static Color FromHSL(double h, double sl, double l)
        {
            var r = l;
            var g = l;
            var b = l;

            var v = (l <= 0.5) ? (l * (1.0 + sl)) : (l + sl - l * sl);

            if (v > 0)
            {
                var m = l + l - v;
                var sv = (v - m) / v;
                h *= 6.0;
                var sextant = (int) h;
                var fract = h - sextant;
                var vsf = v * sv * fract;
                var mid1 = m + vsf;
                var mid2 = v - vsf;

                switch (sextant)
                {
                    case 0:
                        r = v;
                        g = mid1;
                        b = m;
                        break;
                    case 1:
                        r = mid2;
                        g = v;
                        b = m;
                        break;
                    case 2:
                        r = m;
                        g = v;
                        b = mid1;
                        break;
                    case 3:
                        r = m;
                        g = mid2;
                        b = v;
                        break;
                    case 4:
                        r = mid1;
                        g = m;
                        b = v;
                        break;
                    case 5:
                        r = v;
                        g = m;
                        b = mid2;
                        break;
                }
            }

            return Color.FromArgb((int) (r * 255.0), (int) (g * 255.0), (int) (b * 255.0));
        }
    }
}