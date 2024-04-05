using System.Collections.Generic;
using System.IO;
using System.Linq;
using TF.Core.Files;
using TF.Core.TranslationEntities;
using TF.IO;

namespace YakuzaGame.Files.ShopEat
{
    public class File : BinaryTextFile
    {
        public File(string gameName, string path, string changesFolder, System.Text.Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {
        }

        protected override IList<Subtitle> GetSubtitles()
        {
            var result = new List<Subtitle>();

            using (var fs = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fs, FileEncoding, Endianness.BigEndian))
            {
                input.Skip(0x8);
                var stringsOffset = input.ReadInt32();

                input.Seek(stringsOffset, SeekOrigin.Begin);

                Subtitle subtitle;

                while (input.Position < input.Length)
                {
                    subtitle = ReadSubtitle(input);
                    subtitle.PropertyChanged += SubtitlePropertyChanged;
                    result.Add(subtitle);
                }
            }

            LoadChanges(result);

            return result;
        }

        public override void Rebuild(string outputFolder)
        {
            var outputPath = System.IO.Path.Combine(outputFolder, RelativePath);
            Directory.CreateDirectory(System.IO.Path.GetDirectoryName(outputPath));

            var subtitles = GetSubtitles();

            using (var fsInput = new FileStream(Path, FileMode.Open))
            using (var input = new ExtendedBinaryReader(fsInput, FileEncoding, Endianness.BigEndian))
            using (var fsOutput = new FileStream(outputPath, FileMode.Create))
            using (var output = new ExtendedBinaryWriter(fsOutput, FileEncoding, Endianness.BigEndian))
            {
                input.Skip(0x08);
                var stringsOffset = input.ReadInt32();

                input.Seek(0, SeekOrigin.Begin);

                output.Write(input.ReadBytes(stringsOffset));

                Subtitle subtitle;
                var dict = new Dictionary<long, long>(subtitles.Count);

                while (input.Position < input.Length)
                {
                    subtitle = ReadSubtitle(input);

                    var newSubtitle = subtitles.FirstOrDefault(x => x.Offset == subtitle.Offset);
                    if (newSubtitle != null)
                    {
                        dict.Add(subtitle.Offset, output.Position);
                        output.WriteString(newSubtitle.Translation);
                    }
                }

                input.Seek(0x08, SeekOrigin.Begin);
                output.Seek(0x08, SeekOrigin.Begin);

                for (var i = 0; i < subtitles.Count; i++)
                {
                    var stringOffset = input.ReadInt32();
                    output.Write((int)dict[stringOffset]);

                    input.Skip(8);
                    output.Skip(8);
                }
            }
        }
    }
}
