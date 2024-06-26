﻿using System;
using System.Collections.Generic;
using YakuzaGame.Files.Exe;

namespace TFGame.Yakuza0GOG.Files.Exe
{
    public class File : YakuzaGame.Files.Exe.File

        // Estos offsets son para el Yakuza 0, edición GOG, concretamente la versión 1.015a.
        // Es posible que en una actualización cambien.
    {
        // Buscar "data/font". La tabla empieza 0x1900 bytes antes
        protected override long FontTableOffset => 0xD488F0;
        protected override string PointerSectionName => ".data\0\0\0";
        protected override string StringsSectionName => ".rdata\0\0";

        protected override List<Tuple<long, long>> AllowedStringOffsets => new List<Tuple<long, long>>()
        {
            new Tuple<long, long>(0xCE34B0, 0xCE34B0),
            new Tuple<long, long>(0xCE5380, 0xCE6250),
            new Tuple<long, long>(0xCE8740, 0xCE87E0),
            new Tuple<long, long>(0xD21C40, 0xD21C40),  // Guard (in Keyboard Customization of Settings)
            new Tuple<long, long>(0xD2202C, 0xD2202C),  // Custom
            new Tuple<long, long>(0xD81F78, 0xD81FB0),  // Windowed / Borderless / Fullscreen
            new Tuple<long, long>(0xD825B0, 0xD825E0),  // Enabled / Off / Disabled
            new Tuple<long, long>(0xD83110, 0xD864C8),
            new Tuple<long, long>(0xD892F8, 0xD8CFF0),
            new Tuple<long, long>(0xE406D8, 0xE5BB40),
        };

        protected override List<ExePatch> Patches => new List<ExePatch>()
        {
            /*
            Buscar 33 D2 83 F0 20 2D A1 00 00 00 83 F8 3C
            Avanzar 0x1B bytes
            Sustituir 770A -> EB1C

            Buscar 66 0F 1F 44 00 00 42 0F B6 14 3B 80 FA E0
            Sustituir 7242 -> EB47

            Buscar B8 02 00 00 00 C3 84 D2
            Sustituir 7806 -> 9090
            */
            new ExePatch
            {
                Name = "Usar codificación ISO-8895-1",
                Description = "Cambia la codificación de los textos a ISO-8895-1 (NO SE REPRESENTARÁN CARACTERES UTF-8)",
                Enabled = false,
                Patches = new List<Tuple<long, byte[]>>
                {
                    new Tuple<long, byte[]>(0x396E9D, new byte[] {0xEB, 0x1C}),
                    new Tuple<long, byte[]>(0x39B358, new byte[] {0xEB, 0x47}),
                    new Tuple<long, byte[]>(0x6FAEF6, new byte[] {0x90, 0x90}),
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
                    new Tuple<long, byte[]>(0xE73B70, new byte[] {0x25, 0x73, 0x5C})
                },
            },

            // Buscar el primer "GET"/"LOST"/"LV"/"Information".
            new ExePatch
            {
                Name = "Traducir palabras sencillas directas",
                Description = "Con esta opción se traducen palabras directas como GET/LOST (se usa en los combates de Miss Tatsu p.ej.)",
                Enabled = false,
                Patches = new List<Tuple<long, byte[]>>
                {
                    new Tuple<long, byte[]>(0xDD73C8, new byte[] {(byte)'C', (byte)'O', (byte)'G', (byte)'E', (byte)'S'}),   // GET (COGES) Fights
                    new Tuple<long, byte[]>(0xE69424, new byte[] {(byte)'P', (byte)'I', (byte)'E', (byte)'R', (byte)'D', (byte)'E', (byte)'S'}),    // LOST (PIERDES) Fights
                    new Tuple<long, byte[]>(0xDC5308, new byte[] {(byte)'N', (byte)'V'}),   // LV (NV) Catfight
                    new Tuple<long, byte[]>(0xDC5338, new byte[] {(byte)'I', (byte)'n', (byte)'f', (byte)'o', (byte)'r', (byte)'m', (byte)'a', (byte)'c', (byte)'i', (byte)'ó', (byte)'n' }),  // Information Catfight
                    new Tuple<long, byte[]>(0xE69C00, new byte[] {(byte)'N', (byte)'º', (byte)' ' }),    // No. (Nº ) Cabaret Club
                },
            },
        };

        public File(string gameName, string path, string changesFolder, System.Text.Encoding encoding) : base(gameName, path, changesFolder, encoding)
        {
        }
    }
}
