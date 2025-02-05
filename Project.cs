﻿using System.Collections.Generic;
using System.IO;

namespace Monad.FLParser
{
    public class Project
    {
        public const int MaxInsertCount = 127;
        //public const int MaxTrackCount = 499; //No longer used

        public int MainVolume { get; set; } = 300;
        public int MainPitch { get; set; } = 0;
        public int Ppq { get; set; } = 0;
        public double Tempo { get; set; } = 140;
        public string ProjectTitle { get; set; } = string.Empty;
        public string Comment { get; set; } = string.Empty;
        public string Author { get; set; } = string.Empty;
        public string Genre { get; set; } = string.Empty;
        public string VersionString { get; set; } = string.Empty;
        public int Version { get; set; } = 0x100;
        public List<Channel> Channels { get; set; } = new List<Channel>();
        public Track[] Tracks { get; set; }
        public List<Pattern> Patterns = new List<Pattern>();
        public Insert[] Inserts { get; set; } = new Insert[MaxInsertCount];
        public bool PlayTruncatedNotes { get; set; } = false;

        public Project()
        {
            for (var i = 0; i < MaxInsertCount; i++)
            {
                Inserts[i] = new Insert { Id = i, Name = $"Insert {i}" };
            }

            Inserts[0].Name = "Master";
            InitTracks(199);
        }

        public static Project Load(string path, bool verbose)
        {
            using (var stream = File.OpenRead(path))
            {
                return Load(stream, verbose);
            }
        }

        public static Project Load(Stream stream, bool verbose)
        {
            using (var reader = new BinaryReader(stream))
            {
                return Load(reader, verbose);
            }
        }

        public static Project Load(BinaryReader reader, bool verbose)
        {
            var parser = new ProjectParser(verbose);
            return parser.Parse(reader);
        }

        internal void InitTracks(int count)
        {
          Tracks = new Track[count];

          for(var i = 0; i < Tracks.Length; i++)
          {
            Tracks[i] = new Track();
          }
        }
    }
}
