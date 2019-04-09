using ADTConvert2.Files.ADT.Entry;
using System;

namespace ADTConvert2.Files.ADT.Flags
{
    /// <summary>
    /// Flags for the <see cref="MDDFEntry"/>.
    /// </summary>
    [Flags]
    public enum MDDFFlags : ushort
    {
        /// <summary>
        /// Biodome. Perhaps a skybox?
        /// </summary>
        Biodome = 0x1,

        /// <summary>
        /// Possibly used for vegetation and grass.
        /// </summary>
        Shrubbery = 0x2,

        /// <summary>
        /// Unknown Flag (Legion)
        /// </summary>
        Unknown_4 = 0x4,

        /// <summary>
        /// Unknown Flag (Legion)
        /// </summary>
        Unknown_8 = 0x8,

        /// <summary>
        /// Liquied_Known 
        /// </summary>
        Liquid_Known = 0x20,

        /// <summary>
        /// Flag to skip MMID and MMDX and point directly to CASC Filedata Ids for more performance (Legion)
        /// </summary>
        Name_Id_Is_Filedata_Id = 0x40,

        /// <summary>
        /// Unknown Flag (Legion)
        /// </summary>
        Unknown_100 = 0x100
    }
}
