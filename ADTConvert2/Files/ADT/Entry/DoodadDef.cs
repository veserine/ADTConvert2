using ADTConvert2.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SharpDX;

namespace ADTConvert2.Files.ADT
{
    /// <summary>
    /// Placement information for doodads (M2 models).
    /// <para>Use in <see cref="MDDF"/> Chunk</para>
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("UniqueID: {UniqueID}")]
    class DoodadDef
    {
        /// <summary>
        /// References an entry in the <see cref="Warcraft.ADT.Chunks.MMID"/> chunk, specifying the model to use.
        /// </summary>
        uint NameID { get; set; }
        uint UniqueID { get; set; }
        Vector3 Position { get; set; }
        Vector3 Rotation { get; set; }

        /// <summary>
        /// 1024 is the default size equaling 1.0f.
        /// </summary>
        uint Scale { get; set; }

        /// <summary>
        /// Values from enum <see cref="Warcraft.ADT.Chunks.MDDF"/>Flags.
        /// </summary>
        uint Flags { get; set; }

        public DoodadDef(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            using (var br = new BinaryReader(ms))
            {
                NameID = br.ReadUInt32();
                UniqueID = br.ReadUInt32();
                Position = br.ReadVector3();
                Rotation = br.ReadVector3();
                Scale = br.ReadUInt32();
                Flags = br.ReadUInt32();
            }
        }

        public static int GetSize()
        {
            return sizeof(uint) * 10; // 4*uint + 2*Vector3. Vector3 = 3*uint(x,y,z)
        }

        public byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                bw.Write(NameID);
                bw.Write(UniqueID);
                bw.WriteVector3(Position);
                bw.WriteVector3(Rotation);
                bw.Write(Scale);
                bw.Write(Flags);

                return ms.ToArray();
            }
        }
    }
}
