using System;

namespace MitsubaArchivizer.Utils
{
    internal class SeedRandom
    {
        private const int Width = 256;
        private const int Chunks = 6;
        private const int Digits = 52;
        private static readonly double StartDenom = Math.Pow(Width, Chunks);
        private static readonly double Significance = Math.Pow(2, Digits);
        private static readonly double Overflow = Significance * 2;
        private const int Mask = Width - 1;

        private readonly Prng _prng;

        private class Arc4
        {
            private readonly byte[] _s = new byte[Width];

            private int _i;
            private int _j;

            public Arc4(byte[] key)
            {
                var keyLength = key.Length;
                var j = 0;
                
                if (keyLength == 0)
                {
                    key = new byte[] {0};
                }
                
                for (var i = 0; i < Width;)
                {
                    _s[i] = (byte) i++;
                }

                for (var i = 0; i < Width; i++)
                {
                    byte t;
                    _s[i] = _s[j = (Mask & j + key[i % keyLength] + (t = _s[i]))];
                    _s[j] = t;
                }

                Gen(Width);
            }

            public ulong Gen(int count)
            {
                var r = 0ul;
                var i = _i;
                var j = _j;
                var s = _s;

                while (count-- > 0)
                {
                    var t = s[i = Mask & (i + 1)];
                    r = r * Width + s[Mask & ((s[i] = s[j = Mask & (j + t)]) + (s[j] = t))];
                }

                _i = i;
                _j = j;

                return r;
            }

            public byte[] GetSeed()
            {
                return _s;
            }
        }

        private class Prng
        {
            private readonly Arc4 _arc4;

            public Prng(Arc4 arc4)
            {
                _arc4 = arc4;
            }

            public double Random()
            {
                var n = _arc4.Gen(Chunks);
                var d = StartDenom;
                var x = 0ul;

                while (n < Significance)
                {
                    n = (n + x) * Width;
                    d *= Width;
                    x = _arc4.Gen(1);
                }

                while (n >= Overflow)
                {
                    n /= 2;
                    d /= 2;
                    x >>= 1;
                }
                
                return (n + x) / d;
            }
        }

        public SeedRandom(string seed)
        {
            var key = new byte[seed.Length];
            var pool = new byte[Width];

            MixKey(seed, ref key);

            var arc4 = new Arc4(key);
            _prng = new Prng(arc4);
            
            MixKey(System.Text.Encoding.Default.GetString(arc4.GetSeed()), ref pool);
        }

        private static void MixKey(string seed, ref byte[] key)
        {
            var smear = 0;

            for (var i = 0; i < seed.Length;)
            {
                key[Mask & i] = (byte) (Mask & ((smear ^= key[Mask & i] * 19) + seed[i++]));
            }
        }

        public double Random()
        {
            return _prng.Random();
        }
    }
}