using System;
using System.Collections.Generic;

namespace TFGame.YakuzaKiwamiGOG.Files.Dll
{
    public class File : YakuzaGame.Files.Exe.PEFile
    {
        protected override string PointerSectionName => ".data\0\0\0";
        protected override string StringsSectionName => ".rdata\0\0";

        protected override List<Tuple<long, long>> AllowedStringOffsets => new List<Tuple<long, long>>
        {
            new Tuple<long, long>(0x16CC80, 0x16DDB0),
            new Tuple<long, long>(0x186F50, 0x1A5E50),
        };

        public File(string gameName, string path, string changesFolder, System.Text.Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {
        }
    }
}
