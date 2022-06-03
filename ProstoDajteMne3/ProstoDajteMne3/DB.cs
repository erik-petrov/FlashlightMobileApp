using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Xamarin.Forms;

namespace ProstoDajteMne3
{
    public class DB
    {
        SQLiteConnection database;
        public DB(string filename)
        {
            string databasePath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData), filename);
            database = new SQLiteConnection(databasePath);
            database.CreateTable<MorseCodeTemplate>();
        }
        public IEnumerable<MorseCodeTemplate> GetItems()
        {
            return (from i in database.Table<MorseCodeTemplate>() select i).ToList();

        }
        public MorseCodeTemplate GetItem(int id)
        {
            return database.Get<MorseCodeTemplate>(id);
        }
        public int DeleteItem(int id)
        {
            return database.Delete<MorseCodeTemplate>(id);
        }
        public int SaveItem(MorseCodeTemplate item)
        {
            if (item.ID != 0)
            {
                database.Update(item);
                return item.ID;
            }
            else
            {
                return database.Insert(item);
            }
        }
    }
}