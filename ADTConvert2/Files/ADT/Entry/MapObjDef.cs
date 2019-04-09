using ADTConvert2.Extensions;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using SharpDX;

namespace ADTConvert2.Files.ADT
{
    /// <summary>
    /// Placement information for world objects (WMO).
    /// <para>Use in <see cref="MODF"/> Chunk</para>
    /// </summary>
    class MapObjDef
    {
        uint NameID { get; set; }
        uint UniqueID { get; set; }
        Vector3 Position { get; set; }
        Vector3 Rotation { get; set; }
        CAaBox Extents { get; set; }
        uint Flags { get; set; }
        uint DoodadSet { get; set; }
        uint NameSet { get; set; }
        uint Scale { get; set; }

        public MapObjDef(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            using (var br = new BinaryReader(ms))
            {
                NameID = br.ReadUInt32();
                NameID = br.ReadUInt32();
                Position = br.ReadVector3();
                Rotation = br.ReadVector3();
                Extents = new CAaBox(br.ReadBytes(CAaBox.GetSize()));
                Flags = br.ReadUInt32();
                DoodadSet = br.ReadUInt32();
                NameSet = br.ReadUInt32();
                Scale = br.ReadUInt32();
            }
        }

        public static int GetSize()
        {
            return sizeof(uint) * 30; 
        }

        public byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                bw.Write(NameID);
                bw.Write(UniqueID);
                ms.Write(Extents.Serialize());
                bw.Write(Flags);
                bw.Write(DoodadSet);
                bw.Write(NameSet);
                bw.Write(Scale);

                return ms.ToArray();
            }
        }
    }
}
