using ADTConvert2.Files.Interfaces;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ADTConvert2.Files.ADT
{
    class MDDF : IIFFChunk, IBinarySerializable
    {
        public const string Signature = "MDDF";

        /// <summary>
        /// Gets or sets <see cref="DoodadDef"/>s.
        /// </summary>
        public List<DoodadDef> DoodadDefs { get; set; }

        public MDDF()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MDDF"/> class.
        /// </summary>
        /// <param name="inData">The binary data.</param>
        public MDDF(byte[] inData)
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
                var doodadCount = br.BaseStream.Length / DoodadDef.GetSize();

                for (var i = 0; i < doodadCount; ++i)
                {
                    DoodadDefs.Add(new DoodadDef(br.ReadBytes(DoodadDef.GetSize())));
                }
            }
        }

        /// <inheritdoc/>
        public byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                foreach (DoodadDef doodad in DoodadDefs)
                {
                    ms.Write(doodad.Serialize());
                }

                return ms.ToArray();
            }
        }
    }
}
