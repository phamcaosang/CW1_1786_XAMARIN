using SQLite;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace app1786
{
    public class SQLiteDatabase
    {
        private SQLiteConnection dbConnection;
        public const string DB_FILENAME = "hike_db.db3";
        public const SQLiteOpenFlags FLAGS = SQLiteOpenFlags.ReadWrite | SQLiteOpenFlags.Create | SQLiteOpenFlags.SharedCache;
        public string dbPath = "";

        public SQLiteDatabase()
        {
            Init();
        }

        public void Init()
        {
            dbPath = System.IO.Path.Combine(FileSystem.AppDataDirectory, DB_FILENAME);
            dbConnection = new SQLiteConnection(dbPath);
            dbConnection.CreateTable<Hike>();
        }

        public int InsertHike(Hike hike)
        {
            return dbConnection.Insert(hike);
        }

        public int DeleteHike(int hikeId)
        {
            return dbConnection.Delete<Hike>(hikeId);
        }

        public Hike GetHike(int hikeId)
        {
            return dbConnection.Table<Hike>().FirstOrDefault(hike => hike.Id == hikeId);
        }

        public ObservableCollection<Hike> GetHikeList()
        {
            return new ObservableCollection<Hike>(dbConnection.Table<Hike>().ToList());
        }
    }
}
