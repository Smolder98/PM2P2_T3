using PM2P2_T3.Model;
using SQLite;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace PM2P2_T3.Service
{
    public class DataBase
    {

        readonly SQLiteAsyncConnection dbase;

        public DataBase(string dbpath)
        {
            dbase = new SQLiteAsyncConnection(dbpath);

            //Creacion de las tablas de la base de datos

            dbase.CreateTableAsync<Audio>(); //Creando la tabla Audio

        }

        #region OperacionesAudio
        //Metodos CRUD - CREATE
        public Task<int> insertUpdateAudio(Audio Audio)
        {
            if (Audio.Id != 0)
            {
                return dbase.UpdateAsync(Audio);
            }
            else
            {
                return dbase.InsertAsync(Audio);
            }
        }

        //Metodos CRUD - READ
        public Task<List<Audio>> getListAudio()
        {
            return dbase.Table<Audio>().ToListAsync();
        }

        public Task<Audio> getAudio(int id)
        {
            return dbase.Table<Audio>()
                .Where(i => i.Id == id)
                .FirstOrDefaultAsync();
        }

        //Metodos CRUD - DELETE
        public Task<int> deleteAudio(Audio Audio)
        {
            return dbase.DeleteAsync(Audio);
        }

        #endregion OperacionesAudio

    }
}
