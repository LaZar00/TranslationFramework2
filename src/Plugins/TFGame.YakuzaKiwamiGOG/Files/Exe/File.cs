using System;
using System.Collections.Generic;
using YakuzaGame.Files.Exe;

namespace TFGame.YakuzaKiwamiGOG.Files.Exe
{
    public class File : YakuzaGame.Files.Exe.File
    {
        // Buscar "data/font". La tabla empieza 0x1900 bytes antes
        protected override long FontTableOffset => 0xCB8A60;
        protected override string PointerSectionName => ".data\0\0\0";
        protected override string StringsSectionName => ".rdata\0\0";

        protected override int ChangesFileVersion => 2;
        protected override List<Tuple<long, long>> AllowedStringOffsets => new List<Tuple<long, long>>()
        {
            new Tuple<long, long>(0x00C4C628, 0x00C4C628),
            new Tuple<long, long>(0x00C51B58, 0x00C51B70),
            new Tuple<long, long>(0x00C5AE68, 0x00C5AE78),
            new Tuple<long, long>(0x00C5B0B8, 0x00C5B2F8),
            new Tuple<long, long>(0x00C5BEA0, 0x00C5BEE0),
            new Tuple<long, long>(0x00C5C868, 0x00C5D6A0),
            new Tuple<long, long>(0x00C5EB50, 0x00C5EB60),
            new Tuple<long, long>(0x00C5EBD8, 0x00C5EBF0),
            new Tuple<long, long>(0x00C5F6E0, 0x00C63030),
            new Tuple<long, long>(0x00D0EA20, 0x00D0EA20),
            new Tuple<long, long>(0x00D0F780, 0x00D0F7F0),
            new Tuple<long, long>(0x00D0FD50, 0x00D0FD50),
            new Tuple<long, long>(0x00D54A90, 0x00D54A90),
            new Tuple<long, long>(0x00D56A48, 0x00D56A68),
            new Tuple<long, long>(0x00D5ABD0, 0x00D611A0),
            new Tuple<long, long>(0x00DC7E20, 0x00DC7E84),
            new Tuple<long, long>(0x00DC8510, 0x00DE1D18),
        };

        protected override List<ExePatch> Patches => new List<ExePatch>()
        {
            new ExePatch
            {
                Name = "Usar codificación ISO-8895-1",
                Description = "Cambia la codificación de los textos a ISO-8895-1 (NO SE REPRESENTARÁN CARACTERES UTF-8)",
                Enabled = false,
                Patches = new List<Tuple<long, byte[]>>
                {
                    new Tuple<long, byte[]>(0x158E77, new byte[] {0xEB, 0x1E, 0x90}),
                },
            },

            // Buscar el primer "CoInitialize". La cadena está justo antes
            new ExePatch
            {
                Name = "Cambiar posición de ¥",
                Description = "Cambia la posición del símbolo ¥ a la derecha de la cifra (de ¥1000 a 1000¥)",
                Enabled = false,
                Patches = new List<Tuple<long, byte[]>>
                {
                    new Tuple<long, byte[]>(0xDF35AC, new byte[] {0x25, 0x73, 0x5c})
                },
            },
        };

        public File(string gameName, string path, string changesFolder, System.Text.Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {
        }
    }
}
