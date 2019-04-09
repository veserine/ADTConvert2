using ADTConvert2.Files.ADT.Entry;
using System;

namespace ADTConvert2.Files.ADT.Flags
{
    /// <summary>
    /// Flags for the <see cref="MDDFEntry"/>.
    /// </summary>
    [Flags]
    public enum MODFFlags : ushort
    {
        /// <summary>
        /// Set for destroyable buildings like the tower in DeathknightStart. This makes it a server-controllable game object.
        /// </summary>
        Destroyable = 0x1,

        /// <summary>
        /// Load _lod1.WMO for use dependent on distance (WoD)
        /// </summary>
        Use_Lod = 0x2,

        /// <summary>
        /// Use scale. Otherwise scale is 1.0 (Legion)
        /// </summary>
        Has_Scale = 0x4,

        /// <summary>
        /// Flag to skip MMID and MMDX and point directly to CASC Filedata Ids for more performance (Legion)
        /// </summary>
        Name_Id_Is_Filedata_Id = 0x8,
    }
}
