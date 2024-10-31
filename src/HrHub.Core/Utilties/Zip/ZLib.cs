using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;

namespace HrHub.Core.Utilties.Zip
{
    public static class ZLib
    {
        private const byte LENGTH_SIZE = 4;
        private const byte LENGTH_MODE = 2;
        private const byte LENGTH_CHECKSUM = 4;
        private const byte LENGTH_PREFIX = LENGTH_SIZE + LENGTH_MODE;
        private const byte LENGTH_SUFFIX = LENGTH_CHECKSUM;
        private static readonly byte[] COMPRESSION_MODE = new byte[] { 0x78, 0x9C };

        private const uint MOD_ADLER = 65521;

        private static uint adler32(byte[] data, int len)
        /* 
            where data is the location of the data in physical memory and 
            len is the length of the data in bytes 
        */
        {
            uint a = 1;
            uint b = 0;

            // Process each byte of the data in order
            for (int index = 0; index < len; ++index)
            {
                a = (a + data[index]) % MOD_ADLER;
                b = (b + a) % MOD_ADLER;
            }

            return b << 16 | a;
        }

        public static byte[] MicrosoftCompress(byte[] input)
        {
            MemoryStream uncompressed = new MemoryStream(input);
            MemoryStream compressed = new MemoryStream();
            DeflateStream deflateStream = new DeflateStream(compressed, CompressionMode.Compress);
            uncompressed.CopyTo(deflateStream);
            deflateStream.Close();
            byte[] data = compressed.ToArray();

            List<byte> ret = new List<byte>();
            byte[] length = Utils.BEU32((uint)input.Length);
            uint checksum = adler32(input, input.Length);

            ret.AddRange(length);
            ret.AddRange(COMPRESSION_MODE);
            ret.AddRange(data);
            ret.AddRange(Utils.BEU32(checksum));

            return ret.ToArray();
        }

        public static byte[] MicrosoftDecompress(byte[] input)
        {
            if (input.Length <= LENGTH_PREFIX + LENGTH_SUFFIX)
                return null;

            byte[] data = new byte[input.Length - (LENGTH_PREFIX + LENGTH_SUFFIX)];
            Array.Copy(input, LENGTH_PREFIX, data, 0, data.Length);

            MemoryStream compressed = new MemoryStream(data);
            MemoryStream decompressed = new MemoryStream();
            DeflateStream deflateStream = new DeflateStream(compressed, CompressionMode.Decompress);
            deflateStream.CopyTo(decompressed);

            return decompressed.ToArray();
        }
    }
}
