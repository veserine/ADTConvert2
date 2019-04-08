using ADTConvert2.Files.Interfaces;
using System;
using System.IO;
using SharpDX;
using System.Text;
using System.Collections.Generic;
using ADTConvert2.Files.Structures;

namespace ADTConvert2.Extensions
{
    public static class ExtendedIO
    {
        #region Reader
        /// <summary>
        /// Reads a standard null-terminated string from the data stream.
        /// </summary>
        /// <returns>The null terminated string.</returns>
        /// <param name="binaryReader">The reader.</param>
        public static string ReadNullTerminatedString(this BinaryReader binaryReader)
        {
            var sb = new StringBuilder();

            char c;
            while ((c = binaryReader.ReadChar()) != 0)
            {
                sb.Append(c);
            }

            return sb.ToString();
        }

        /// <summary>
        /// Reads a 12-byte <see cref="Vector3"/> structure from the data stream and advances the position of the stream by
        /// 12 bytes.
        /// </summary>
        /// <returns>The vector3f.</returns>
        /// <param name="binaryReader">The reader.</param>
        /// <param name="convertTo">Which axis configuration the read vector should be converted to.</param>
        public static Vector3 ReadVector3(this BinaryReader binaryReader, AxisConfiguration convertTo = AxisConfiguration.YUp)
        {
            switch (convertTo)
            {
                case AxisConfiguration.Native:
                    {
                        return new Vector3(binaryReader.ReadSingle(), binaryReader.ReadSingle(), binaryReader.ReadSingle());
                    }
                case AxisConfiguration.YUp:
                    {
                        var x1 = binaryReader.ReadSingle();
                        var y1 = binaryReader.ReadSingle();
                        var z1 = binaryReader.ReadSingle();

                        var x = x1;
                        var y = z1;
                        var z = -y1;

                        return new Vector3(x, y, z);
                    }
                case AxisConfiguration.ZUp:
                    {
                        var x1 = binaryReader.ReadSingle();
                        var y1 = binaryReader.ReadSingle();
                        var z1 = binaryReader.ReadSingle();

                        var x = x1;
                        var y = -z1;
                        var z = y1;

                        return new Vector3(x, y, z);
                    }
                default:
                    {
                        throw new ArgumentOutOfRangeException(nameof(convertTo), convertTo, null);
                    }
            }
        }

        /// <summary>
        /// Reads a 24-byte <see cref="BoundingBox"/> structure from the data stream and advances the position of the stream by
        /// 24 bytes.
        /// </summary>
        /// <returns>The box.</returns>
        /// <param name="binaryReader">The reader.</param>
        public static BoundingBox ReadBoundingBox(this BinaryReader binaryReader)
        {
            return new BoundingBox(binaryReader.ReadVector3(), binaryReader.ReadVector3());
        }

        /// <summary>
        /// Reads a 4-byte RIFF chunk signature from the data stream.
        /// </summary>
        /// <param name="binaryReader">The reader.</param>
        /// <returns>The signature as a string.</returns>
        public static string ReadBinarySignature(this BinaryReader binaryReader)
        {
            // The signatures are stored in reverse in the file, so we'll need to read them backwards into
            // the buffer.
            var signatureBuffer = new char[4];
            for (var i = 0; i < 4; ++i)
            {
                signatureBuffer[3 - i] = binaryReader.ReadChar();
            }

            var signature = new string(signatureBuffer);
            return signature;
        }

        /// <summary>
        /// Reads an IFF-style chunk from the stream. The chunk must have the <see cref="IIFFChunk"/>
        /// interface, and implement a parameterless constructor.
        /// </summary>
        /// <param name="reader">The current <see cref="BinaryReader"/>.</param>
        /// <typeparam name="T">The chunk type.</typeparam>
        /// <returns>The chunk.</returns>
        public static T ReadIFFChunk<T>(this BinaryReader reader) where T : IIFFChunk, new()
        {
            string chunkSignature = reader.ReadBinarySignature();
            var chunkSize = reader.ReadUInt32();
            var chunkData = reader.ReadBytes((int)chunkSize);

            var chunk = new T();
            if (chunk.GetSignature() != chunkSignature)
            {
                throw new Exception($"An unknown chunk with the signature \"{chunkSignature}\" was read.");
            }

            chunk.LoadBinaryData(chunkData);

            return chunk;
        }
        #endregion

        #region Write
        /// <summary>
        /// Writes the provided string to the data stream as a C-style null-terminated string.
        /// </summary>
        /// <param name="binaryWriter">The current <see cref="BinaryWriter"/> object.</param>
        /// <param name="inputString">Input string.</param>
        public static void WriteNullTerminatedString(this BinaryWriter binaryWriter, string inputString)
        {
            foreach (var c in inputString)
            {
                binaryWriter.Write(c);
            }

            binaryWriter.Write((char)0);
        }
        /// <summary>
        /// Writes the provided string to the data stream as a C-style null-terminated string.
        /// </summary>
        /// <param name="binaryWriter">The current <see cref="BinaryWriter"/> object.</param>
        /// <param name="inputStrings">Input string array.</param>
        public static void WriteNullTerminatedString(this BinaryWriter binaryWriter, string[] inputStrings)
        {
            foreach (var s in inputStrings)
            {
                binaryWriter.WriteNullTerminatedString(s);
            }
        }

        /// <summary>
        /// Writes an RIFF-style chunk signature to the data stream.
        /// </summary>
        /// <param name="binaryWriter">The current <see cref="BinaryWriter"/> object.</param>
        /// <param name="signature">Signature.</param>
        public static void WriteChunkSignature(this BinaryWriter binaryWriter, string signature)
        {
            if (signature.Length != 4)
            {
                throw new InvalidDataException("The signature must be an ASCII string of exactly four characters.");
            }

            for (var i = 3; i >= 0; --i)
            {
                binaryWriter.Write(signature[i]);
            }
        }

        /// <summary>
        /// Writes an RIFF-style chunk to the data stream.
        /// </summary>
        /// <typeparam name="T">The chunk type.</typeparam>
        /// <param name="binaryWriter">The writer.</param>
        /// <param name="chunk">The chunk.</param>
        public static void WriteIFFChunk<T>(this BinaryWriter binaryWriter, T chunk) where T : IIFFChunk, IBinarySerializable
        {
            var serializedChunk = chunk.Serialize();

            binaryWriter.WriteChunkSignature(chunk.GetSignature());
            binaryWriter.Write((uint)serializedChunk.Length);
            binaryWriter.Write(serializedChunk);
        }

        /// <summary>
        /// Writes a 12-byte <see cref="Vector3"/> value to the data stream. This function
        /// expects a Y-up vector. By default, this function will store the vector in a Z-up axis configuration, which
        /// is what World of Warcraft expects. This can be overridden. Passing <see cref="AxisConfiguration.Native"/> is
        /// considered Y-up, as it is the way vectors are handled internally in the library.
        /// </summary>
        /// <param name="binaryWriter">The current <see cref="BinaryWriter"/> object.</param>
        /// <param name="vector">The Vector to write.</param>
        /// <param name="storeAs">Which axis configuration the read vector should be stored as.</param>
        public static void WriteVector3(this BinaryWriter binaryWriter, Vector3 vector, AxisConfiguration storeAs = AxisConfiguration.ZUp)
        {
            switch (storeAs)
            {
                case AxisConfiguration.Native:
                case AxisConfiguration.YUp:
                    {
                        binaryWriter.Write(vector.X);
                        binaryWriter.Write(vector.Y);
                        binaryWriter.Write(vector.Z);
                        break;
                    }

                case AxisConfiguration.ZUp:
                    {
                        binaryWriter.Write(vector.X);
                        binaryWriter.Write(vector.Z * -1.0f);
                        binaryWriter.Write(vector.Y);
                        break;
                    }

                default:
                    throw new ArgumentOutOfRangeException(nameof(storeAs), storeAs, null);
            }
        }

        /// <summary>
        /// Writes a 24-byte <see cref="BoundingBox"/> to the data stream.
        /// </summary>
        /// <param name="binaryWriter">The current <see cref="BinaryWriter"/> object.</param>
        /// <param name="box">In box.</param>
        public static void WriteBoundingBox(this BinaryWriter binaryWriter, BoundingBox box)
        {
            binaryWriter.WriteVector3(box.Minimum);
            binaryWriter.WriteVector3(box.Maximum);
        }
        #endregion
    }
}
