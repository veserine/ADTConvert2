﻿using System;
using System.IO;
using Warcraft.NET.Files.ADT.Terrain.Wotlk;

namespace ADTConvert2
{
    class Program
    {
        /// <summary>
        /// Start
        /// </summary>
        /// <param name="args"></param>
        static void Main(string[] args)
        {
            if (!File.Exists("Test.adt"))
            {
                throw new Exception(String.Format("Place a Test.adt in {0}", AppContext.BaseDirectory));
            }

            var data = File.ReadAllBytes("Test.adt");
            Terrain terrain = new Terrain(data);

            byte[] a = terrain.Serialize();
            File.WriteAllBytes("New.adt", a);
        }
    }
}
