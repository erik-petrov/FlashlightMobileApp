using SQLite;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace ProstoDajteMne3
{
    internal class DB
    {
        static SQLiteAsyncConnection Database;

        public static readonly AsyncLazy<DB> Instance = new AsyncLazy<DB>(async () =>
        {
            var instance = new DB();
            CreateTableResult result = await Database.CreateTableAsync<MorseCodeTemplate>();
            return instance;
        });

        public DB()
        {
            Database = new SQLiteAsyncConnection(Constants.DatabasePath, Constants.Flags);
        }
        public Task<List<MorseCodeTemplate>> GetItemsAsync()
        {
            return Database.Table<MorseCodeTemplate>().ToListAsync();
        }

        public Task<MorseCodeTemplate> GetItemAsync(int id)
        {
            return Database.Table<MorseCodeTemplate>().Where(i => i.ID == id).FirstOrDefaultAsync();
        }

        public Task<int> SaveItemAsync(MorseCodeTemplate item)
        {
            if (item.ID != 0)
            {
                return Database.UpdateAsync(item);
            }
            else
            {
                return Database.InsertAsync(item);
            }
        }

        public Task<int> DeleteItemAsync(MorseCodeTemplate item)
        {
            return Database.DeleteAsync(item);
        }
    }
    public class AsyncLazy<T>
    {
        readonly Lazy<Task<T>> instance;

        public AsyncLazy(Func<T> factory)
        {
            instance = new Lazy<Task<T>>(() => Task.Run(factory));
        }

        public AsyncLazy(Func<Task<T>> factory)
        {
            instance = new Lazy<Task<T>>(() => Task.Run(factory));
        }

        public TaskAwaiter<T> GetAwaiter()
        {
            return instance.Value.GetAwaiter();
        }
    }
    public static class Constants
    {
        public const string DatabaseFilename = "SQLMorse.db3";

        public const SQLite.SQLiteOpenFlags Flags =
            SQLite.SQLiteOpenFlags.ReadWrite |
            SQLite.SQLiteOpenFlags.Create |
            SQLite.SQLiteOpenFlags.SharedCache;

        public static string DatabasePath
        {
            get
            {
                var basePath = Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData);
                return Path.Combine(basePath, DatabaseFilename);
            }
        }
    }
}
