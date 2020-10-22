using System;
using System.Threading;

namespace __Snippets__
{
    public class TextIdGenerator
    {
        private const int FixedStringLength = 13;
        private const int Mask = 0b_0001_1111;

        private long _lastId;

        public TextIdGenerator() : this(DateTime.UtcNow.Ticks)
        {
        }

        public TextIdGenerator(long seed)
        {
            _lastId = seed;
        }

        public string GetNextId() => GenerateId(Interlocked.Increment(ref _lastId));

        public static TextIdGenerator Instance = new TextIdGenerator();

        // Base 32 Encoding with Extended Hex Alphabet (RFC 4648 section 7)
        private static readonly char[] _base32hex = "0123456789ABCDEFGHIJKLMNOPQRSTUV".ToCharArray();

        private static string GenerateId(long id)
        {
            return string.Create(FixedStringLength, id, (buffer, value) =>
            {
                buffer[12] = _base32hex[value & Mask];
                buffer[11] = _base32hex[(value >> 5) & Mask];
                buffer[10] = _base32hex[(value >> 10) & Mask];
                buffer[9] = _base32hex[(value >> 15) & Mask];
                buffer[8] = _base32hex[(value >> 20) & Mask];
                buffer[7] = _base32hex[(value >> 25) & Mask];
                buffer[6] = _base32hex[(value >> 30) & Mask];
                buffer[5] = _base32hex[(value >> 35) & Mask];
                buffer[4] = _base32hex[(value >> 40) & Mask];
                buffer[3] = _base32hex[(value >> 45) & Mask];
                buffer[2] = _base32hex[(value >> 50) & Mask];
                buffer[1] = _base32hex[(value >> 55) & Mask];
                buffer[0] = _base32hex[(value >> 60) & Mask];
            });
        }
    }
}
