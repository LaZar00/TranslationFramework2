using System;
using System.Collections.Generic;

namespace TFGame.Yakuza0GOG.Files.Dll
{
    public class File : YakuzaGame.Files.Exe.PEFile
    {
        protected override string PointerSectionName => ".data\0\0\0";
        protected override string StringsSectionName => ".rdata\0\0";

        protected override List<Tuple<long, long>> AllowedStringOffsets => new List<Tuple<long, long>>
        {
            new Tuple<long, long>(0x15A3C8, 0x15C2E8),
            new Tuple<long, long>(0x165FE0, 0x16DB90),
            new Tuple<long, long>(0x175130, 0x1936C9),
        };

        public File(string gameName, string path, string changesFolder, System.Text.Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {
        }
    }
}
