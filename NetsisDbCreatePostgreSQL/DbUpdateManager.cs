using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using static NetsisDbCreatePostgreSQL.Enums;

namespace NetsisDbCreatePostgreSQL
{

    public class LogEventArgs : EventArgs
    {
        public string Message { get; set; }
    }

    public class DbUpdateManager
    {
        

        public event EventHandler<LogEventArgs> OnLog;
        private  NpgsqlConnection PostgreConn { get; set; }
        private  List<DBNewItem> DbUpdateNewList { get; set; }
        public DbUpdateManager()
        {
            var jsonData = File.ReadAllText("DbUpdateNew.json");
            DbUpdateNewList = JsonConvert.DeserializeObject<List<DBNewItem>>(jsonData);

        }

        public void ResetDatabase()
        {
            PostgreConn = GetConncetion("postgres");
            
            ExecuteCommand("DROP DATABASE IF EXISTS NETSIS WITH (FORCE)"); FireEvent($" DROP DATABASE NETSIS ");            
            
            ExecuteCommand("CREATE DATABASE NETSIS"); FireEvent($" CREATE DATABASE NETSIS ");
            
            ExecuteCommand("DROP DATABASE IF EXISTS SIRKETDB WITH (FORCE) "); FireEvent($" DROP DATABASE SIRKETDB ");

            ExecuteCommand("CREATE DATABASE SIRKETDB"); FireEvent($" CREATE DATABASE SIRKETDB ");


            foreach (var item in DbUpdateNewList)
            {
                item.PostgreConverted = false;
                item.PostgreError = "";
                item.PostgreSQL = "";
            }


        }



        private void FireEvent(string _msg)
        {
            if (OnLog != null)
            {
                OnLog(this, new LogEventArgs() { Message = _msg });
            }
        }

        public void CreateNetsisDbOjects(ObjectType[] arr)
        {

            PostgreConn = GetConncetion("netsis");
            foreach (var _objType in arr)
            {
                var SQLList = GetFilteredData(WhichDB.Netsis, _objType);


                FireEvent($" NETSIS Db üzerinde  {Enum.GetName(typeof(ObjectType), _objType) }  Tipinde ki Cümleler çalışıyor ");
                Thread.Sleep(200);
                GenerateAndExecutePostgreSQLQuery(SQLList);
                FireEvent($" Başarılı Çalıştıralan DbUpdate Cumle ({Enum.GetName(typeof(ObjectType), _objType) }) sayısı:{SQLList.Count(x => x.PostgreConverted)}/{SQLList.Count()} ");
            }

            SaveAll();

        }

        private void SaveAll()
        {
            File.WriteAllText("DbUpdateNew.json", JsonConvert.SerializeObject(DbUpdateNewList));
        }

        public void CreateSIRKETDBDbOjects(ObjectType[] arr)
        {

            PostgreConn = GetConncetion("sirketdb");
            foreach (var _objType in arr)
            {
                var SQLList = GetFilteredData(WhichDB.Entity, _objType);
                FireEvent($" SIRKETDB Db üzerinde  {Enum.GetName(typeof(ObjectType), _objType) }  Tipinde ki Cümleler çalışıyor ");
                Thread.Sleep(200);
                GenerateAndExecutePostgreSQLQuery(SQLList);
                FireEvent($" Başarılı Çalıştıralan DbUpdate Cumle ({Enum.GetName(typeof(ObjectType), _objType) }) sayısı:{SQLList.Count(x => x.PostgreConverted)}/{SQLList.Count()} ");
            }
            SaveAll();
        }

        private void GenerateAndExecutePostgreSQLQuery(List<DBNewItem> SQLList)
        {
            foreach (var item in SQLList.Where(x => x.InckeyNo != -1 && !x.PostgreConverted).ToList())
            {
                item.PostgreSQL = SQLToPostgreSQLConverter.ConvertToPostgreSQL(item.Cumle);
                try
                {
                    ExecuteCommand(item.PostgreSQL);
                    item.PostgreConverted = true;

                }
                catch (PostgresException e)
                {
                    if (e.Code != "42P07") //Already Exist Object
                    {
                        FireEvent($" {item.InckeyNo}   {item.ObjeAcik}  {e.Message}  ");
                        FireEvent($"  { item.PostgreSQL}  ");
                        item.PostgreConverted = false;
                        item.PostgreSQL = "";
                        item.PostgreConverted = false;
                    }
                }
            }
        }

        private NpgsqlConnection GetConncetion(string dbname = "netsis")
        {
            var connectionString = "Host =localhost;Username=postgres;Password=postgres;Database=" + dbname;
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
            var dataSource = dataSourceBuilder.Build();
            return dataSource.OpenConnection();
        }

        private void ExecuteCommand(string NetCommandText)
        {
            if (NetCommandText != string.Empty)
            {
                var command = new NpgsqlCommand(NetCommandText, PostgreConn);
                command.ExecuteNonQuery();

            }
        }



        private List<DBNewItem> GetFilteredData(WhichDB dbType, ObjectType _ObjectType)
        {

            var FilterDbType = DataBaseType.MSSql;
            var FilterDb = dbType;
            var FilterUrun = 0;
            var FilterObjectType = _ObjectType;
            return DbUpdateNewList.Where(x => x.Urun == FilterUrun && x.Nerde == FilterDb && x.Veritabani == FilterDbType && x.ObjeTipi == FilterObjectType).OrderByDescending(x => x.IlkSira).ThenBy(x => x.SiraNo).ThenBy(x => x.InckeyNo).ToList();


        }

    }

}
