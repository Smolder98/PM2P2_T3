using SQLite;
using System;
using System.Collections.Generic;
using System.Text;

namespace PM2P2_T3.Model
{
    public class Audio
    {
        [PrimaryKey, AutoIncrement]
        public int Id { get; set; }
        public string Descripcion { get; set; }
        public string Path { get; set; }
        public DateTime DateCreation { get; set; }
    }
}
