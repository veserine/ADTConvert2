using ADTConvert2.Files.Interfaces;
using System;
using System.IO;
using System.Collections.Generic;
using System.Text;

namespace ADTConvert2.Files.ADT
{
    /// <summary>
    /// Contains offsets in the file for specific chunks. WoW only takes this for parsing the ADT file.
    /// </summary>
    [System.Diagnostics.DebuggerDisplay("Flag: {Flag}")]
    class MHDR : IIFFChunk, IBinarySerializable
    {
        public const string Signature = "MHDR";

        public uint Flag { get; set; }
        public uint MCINOffset { get; set; }
        public uint MTEXOffset { get; set; }
        public uint MMDXOffset { get; set; }
        public uint MMIDOffset { get; set; }
        public uint MWMOOffset { get; set; }
        public uint MWIDOffset { get; set; }
        public uint MDDFOffset { get; set; }
        public uint MODFOffset { get; set; }
        public uint MFBOOffset { get; set; }
        public uint MH2OOffset { get; set; }
        public uint MTXFOffset { get; set; }
        public byte MAMPValue { get; set; }
        public byte Padding1 { get; set; }
        public byte Padding2 { get; set; }
        public byte Padding3 { get; set; }
        public uint Unused1 { get; set; }
        public uint Unused2 { get; set; }
        public uint Unused3 { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MHDR"/> class.
        /// </summary>
        /// <param name="inData">ExtendedData.</param>
        public MHDR(byte[] inData)
        {
            LoadBinaryData(inData);
        }

        public string GetSignature()
        {
            return Signature;
        }

        public uint GetSize()
        {
            return 64;
        }

        public void LoadBinaryData(byte[] inData)
        {
            using (var ms = new MemoryStream(inData))
            using (var br = new BinaryReader(ms))
            {
                Flag = br.ReadUInt32();
                MCINOffset = br.ReadUInt32();
                MTEXOffset = br.ReadUInt32();
                MMDXOffset = br.ReadUInt32();
                MMIDOffset = br.ReadUInt32();
                MWMOOffset = br.ReadUInt32();
                MWIDOffset = br.ReadUInt32();
                MDDFOffset = br.ReadUInt32();
                MODFOffset = br.ReadUInt32();
                MFBOOffset = br.ReadUInt32();
                MH2OOffset = br.ReadUInt32();
                MTXFOffset = br.ReadUInt32();
                MAMPValue = br.ReadByte();
                Padding1 = br.ReadByte();
                Padding2 = br.ReadByte();
                Padding3 = br.ReadByte();
                Unused1 = br.ReadUInt32();
                Unused2 = br.ReadUInt32();
                Unused3 = br.ReadUInt32();

            }
        }

        public byte[] Serialize()
        {
            using (var ms = new MemoryStream())
            using (var bw = new BinaryWriter(ms))
            {
                bw.Write(this.Flag);
                bw.Write(this.MCINOffset);
                bw.Write(this.MTEXOffset);
                bw.Write(this.MMDXOffset);
                bw.Write(this.MMIDOffset);
                bw.Write(this.MWMOOffset);
                bw.Write(this.MWIDOffset);
                bw.Write(this.MDDFOffset);
                bw.Write(this.MODFOffset);
                bw.Write(this.MFBOOffset);
                bw.Write(this.MH2OOffset);
                bw.Write(this.MTXFOffset);
                bw.Write(this.MAMPValue);
                bw.Write(this.Padding1);
                bw.Write(this.Padding2);
                bw.Write(this.Padding3);
                bw.Write(this.Unused1);
                bw.Write(this.Unused2);
                bw.Write(this.Unused3);

                return ms.ToArray();
            }
        }
    }
}
