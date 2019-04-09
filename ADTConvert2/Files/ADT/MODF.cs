using ADTConvert2.Files.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ADTConvert2.Files.ADT
{
    class MODF : IIFFChunk, IBinarySerializable
    {
        public const string Signature = "MODF";

        /// <summary>
        /// Gets or sets <see cref="MapObjDef"/>s.
        /// </summary>
        public List<MapObjDef> MapObjDefs { get; set; }

        public MODF()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MODF"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public MODF(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        /// <inheritdoc/>
        public string GetSignature()
        {
            return Signature;
        }

        /// <inheritdoc/>
        public uint GetSize()
        {
            return (uint)Serialize().Length;
        }

        /// <inheritdoc/>
        public void LoadBinaryData(byte[] inData)
        {
            using (var ms = new MemoryStream(inData))
            using (var br = new BinaryReader(ms))
            {
                var objCount = br.BaseStream.Length / MapObjDef.GetSize();

                for (var i = 0; i < objCount; ++i)
                {
                    MapObjDefs.Add(new MapObjDef(br.ReadBytes(MapObjDef.GetSize())));
                }
            }
        }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                foreach (MapObjDef obj in MapObjDefs)
                {
                    ms.Write(obj.Serialize());
                }

                return ms.ToArray();
            }
        }
    }
}
