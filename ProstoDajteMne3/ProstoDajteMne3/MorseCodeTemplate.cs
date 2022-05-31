using System;
using System.Collections.Generic;
using System.Text;

namespace ProstoDajteMne3
{
    [SQLite.Table("Messages")]
    internal class MorseCodeTemplate
    {
        [SQLite.PrimaryKey, SQLite.AutoIncrement]
        public int? ID { get; set; }
        public string Name { get; set; }
        public string Text { get; set; }
        public string Morse { get; set; }
    }
}
