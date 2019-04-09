using ADTConvert2.Extensions;
using SharpDX;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace ADTConvert2.Files.ADT
{
    /// <summary>
    /// An axis aligned box described by the minimum and maximum point.
    /// <para>Use in <see cref="MapObjDef"/></para>
    /// </summary>
    class CAaBox
    {
        Vector3 Min { get; set; }
        Vector3 Max { get; set; }

        public CAaBox(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            using (var br = new BinaryReader(ms))
            {
                Min = br.ReadVector3();
                Max = br.ReadVector3();
            }
        }

        public static int GetSize()
        {
            return sizeof(uint) * 6; // 2*Vector3. Vector3 = 3*uint(x,y,z)
        }

        public byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                bw.WriteVector3(Min);
                bw.WriteVector3(Max);

                return ms.ToArray();
            }
        }
    }
}
