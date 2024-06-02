// Based on NLZ-GBA's compression

using System;
using System.Collections.Generic;
using System.IO;

namespace AdvancedLib.Compression
{
    static class LZ77
    {
        #region Don't edit this
        private const int SlidingWindowSize = 4096;
        private const int ReadAheadBufferSize = 18;
        private const int BlockSize = 8;
        enum ScanDepth : byte
        {
            Byte = 1,
            HalfWord = 2,
            Word = 4
        };
        private const ScanDepth scanDepth = ScanDepth.Word;
        private const int sizeMultible = 32;
        private const int maxSize = 0x8000;
        #endregion

        /// <summary>
        /// Scans the stream for potential LZ77 compressions
        /// </summary>
        /// <param name="br">Stream to scan.</param>
        /// <returns>An array of offsets relative to the beginning of scan area</returns>
        static public int[] Scan(BinaryReader br, int offset, int size)
        {
            br.BaseStream.Position = offset;
            List<int> results = new List<int>();

            for (int i = 0; i < size; i += (int)scanDepth)
            {
                br.BaseStream.Position = offset + i;

                uint header = br.ReadUInt32();
                if ((header & 0xFF000000)>>24 == 0x10)
                {
                    header >>= 8;
                    if ((header % sizeMultible == 0)
                        && (header <= maxSize)
                        && (header > 0))
                        //&& (CanBeUnCompressed(pointer + i, amount - i)))
                    {
                        results.Add(i);
                    }
                }
            }
            return results.ToArray();
        }

        /// <summary>
        /// Checks weather the data can be uncompressed.
        /// </summary>
        /// <param name="offset">Offset of the compressed data in the stream.</param>
        /// <param name="br">Stream where the compressed data is.</param>
        /// <returns>Returns "true" if data can be uncompressed, false if can't.</returns>
        static public bool CanBeDecompressed(BinaryReader br, int offset)
        {
            br.BaseStream.Position = offset;
            uint size = br.ReadUInt32();
            if (!((size & 0xFF) == 0x10))
                return false;

            size >>= 8;
            int UncompressedDataSize = 0;

            while (UncompressedDataSize < size)
            {
                if (br.BaseStream.Position + 1 > br.BaseStream.Length)
                    return false;
                byte isCompressed = br.ReadByte();

                for (int i = 0; i < BlockSize; i++)
                {
                    if ((isCompressed & 0x80) != 0)
                    {
                        if (br.BaseStream.Position + 2 > br.BaseStream.Length)
                            return false;

                        byte first = br.ReadByte();
                        byte second = br.ReadByte();
                        int amountToCopy = 3 + ((first >> 4));
                        int copyFrom = 1 + ((first & 0xF) << 8) + second;

                        if (copyFrom > UncompressedDataSize)
                            return false;

                        UncompressedDataSize += amountToCopy;
                    }
                    else
                    {
                        if (br.BaseStream.Position + 1 > br.BaseStream.Length)
                            return false;

                        br.BaseStream.Position++;
                        UncompressedDataSize++;
                    }
                    isCompressed <<= 1;
                }
            }
            return true;
        }  //test

        /// <summary>
        /// Gets the length of the compressed data in a stream
        /// </summary>
        /// <param name="br">The stream with data</param>
        /// <param name="offset">The position of the data in stream</param>
        /// <returns>The lenght of the data</returns>
        static public bool GetCompressedDataLength(BinaryReader br, int offset, out int length)
        {
            length = 0; int UncompSize = 0;
            br.BaseStream.Position = offset;
            int size = br.ReadInt32();

            if ((size & 0xFF) != 0x10)
                return false;
            size >>= 8;

            while (UncompSize < size)
            {
                byte isCompressed = br.ReadByte();
                for (int i = 0; i < 8; i++)
                {
                    if ((isCompressed & 0x80) != 0)
                    {
                        if (!(br.BaseStream.Position < br.BaseStream.Length - 1))
                            return false;

                        byte first = br.ReadByte();
                        byte second = br.ReadByte();
                        ushort CopyPosition = (ushort)(((first) & 0xF << 8) + second + 1);
                        byte AmountToCopy = (byte)(3 + (first >> 4));

                        if (CopyPosition > UncompSize)
                            return false;

                        UncompSize += (AmountToCopy);

                    }
                    else
                    {
                        if (!(br.BaseStream.Position++ < br.BaseStream.Length))
                            return false;
                        UncompSize++;
                    }
                    if (!(UncompSize < size))
                        break;

                    isCompressed <<= 1;
                }
            }
            length = (int)(br.BaseStream.Position - offset);
            if ((length % 4) != 0)
                length += 4 - (length % 4);
            return true;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="br"></param>
        /// <param name="offset"></param>
        /// <param name="lenght"></param>
        /// <returns></returns>
        static unsafe public byte[] Compress(BinaryReader br, int offset, int lenght)
        {
            byte[] uncompressedData;
            br.BaseStream.Position = offset;
            if (br.BaseStream.Length < offset + lenght)
            {
                return new byte[0];
            }
            uncompressedData = br.ReadBytes(lenght);

            unsafe
            {
                fixed (byte* uncomp = &uncompressedData[0])
                {
                    return Compress(uncomp, lenght);
                }
            }
        }

        /// <summary>
        /// Compresses data with LZ77
        /// </summary>
        /// <param name="source">Pointer to beginning of the data</param>
        /// <param name="length">Lenght of the data to compress in bytes</param>
        /// <returns>Array of bytes</returns>
        static unsafe public byte[] Compress(byte* source, int length)
        {
            int position = 0;

            List<byte> CompressedData = new List<byte>();
            CompressedData.Add(0x10);

            {
                byte* pointer = (byte*)&length;
                for (int i = 0; i < 3; i++)
                {
                    CompressedData.Add(*(pointer++));
                }
            }

            while (position < length)
            {
                byte isCompressed = 0;
                List<byte> tempList = new List<byte>();

                for (int i = 0; i < BlockSize; i++)
                {
                    int[] searchResult = Search(source, position, length);

                    if (searchResult[0] > 2)
                    {
                        byte add = (byte)((((searchResult[0] - 3) & 0xF) << 4) + (((searchResult[1] - 1) >> 8) & 0xF));
                        tempList.Add(add);
                        add = (byte)((searchResult[1] - 1) & 0xFF);
                        tempList.Add(add);
                        position += searchResult[0];
                        isCompressed |= (byte)(1 << (8 - i - 1));
                    }
                    else if (searchResult[0] >= 0)
                        tempList.Add(*(source + position++));
                    else
                        break;
                }
                CompressedData.Add(isCompressed);
                CompressedData.AddRange(tempList);
            }
            while (CompressedData.Count%4!=0) CompressedData.Add(0);

            return CompressedData.ToArray();
        }

        static unsafe private int[] Search(byte* source, int position, int length)
        {
            List<int> results = new List<int>();

            if ((position < 3) || ((length - position) < 3))
                return new int[2] { 0, 0 };
            if (!(position < length))
                return new int[2] { -1, 0 };

            for (int i = 1; ((i < SlidingWindowSize) && (i < position)); i++)
            {
                if (*(source + position - i - 1) == *(source + position))
                {
                    results.Add(i + 1);
                }
            }
            if (results.Count == 0)
                return new int[2] { 0, 0 };

            int amountOfBytes = 0;

            while (amountOfBytes < ReadAheadBufferSize)
            {
                amountOfBytes++;
                bool Break = false;
                for (int i = 0; i < results.Count; i++)
                {
                    if (*(source + position + amountOfBytes) != *(source + position - results[i] + (amountOfBytes % (results[i]))))
                    {
                        if (results.Count > 1)
                        {
                            results.RemoveAt(i);
                            i--;
                        }
                        else
                            Break = true;
                    }
                }
                if (Break)
                    break;
            }
            return new int[2] { amountOfBytes, results[0] }; //lenght of data is first, then position
        }

        /// <summary>
        /// Uncompresser LZ77 data from a stream
        /// </summary>
        /// <param name="br">Stream where the compressed data is</param>
        /// <param name="offset">Position of the compressed data</param>
        /// <param name="destination">Pointer to where uncompressed data goes</param>
        /// <returns>True if successful, else false</returns>
        static unsafe public bool Decompress(BinaryReader br, int offset, byte* destination)
        {
            br.BaseStream.Position = offset;
            int size = br.ReadInt32();
            int uncompPosition = 0;

            if (!((size & 0xFF) == 0x10))
                return false;

            size >>= 8;

            while ((uncompPosition < size) && (br.BaseStream.Position < br.BaseStream.Length))
            {
                byte isCompressed = br.ReadByte();

                for (int i = 0; i < BlockSize; i++)
                {
                    if ((isCompressed & 0x80) != 0)
                    {
                        byte first = br.ReadByte();
                        byte second = br.ReadByte();
                        ushort Position = (ushort)((((first << 8) + second) & 0xFFF) + 1);
                        byte AmountToCopy = (byte)(3 + ((first >> 4) & 0xF));

                        if (Position > uncompPosition)
                            return false;

                        for (int u = 0; u < AmountToCopy; u++)
                            *(destination + uncompPosition + u) = *(destination + uncompPosition - Position + (u % Position));

                        uncompPosition += AmountToCopy;
                    }
                    else
                    {
                        *(destination + uncompPosition++) = br.ReadByte();
                    }
                    if (!(uncompPosition < size) && (br.BaseStream.Position < br.BaseStream.Length))
                        break;

                    isCompressed <<= 1;
                }
            }
            return !(uncompPosition < size);
        }
    }
}