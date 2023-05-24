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

        private void FireEvent(string _msg)
        {
            if (OnLog != null)
            {
                OnLog(this, new LogEventArgs() { Message = _msg });
            }
        }

        public void CreateNetsisDbOjects()
        {

            PostgreConn = GetConncetion("NETSIS");
            foreach (var _objType in (ObjectType[])Enum.GetValues(typeof(ObjectType)))
            {
                var SQLList = GetFilteredData(WhichDB.Netsis, _objType);

                
                FireEvent($" NETSIS Db üzerinde  {Enum.GetName(typeof(ObjectType), _objType) }  Tipinde ki Cümleler çalışıyor ");
                Thread.Sleep(200);
                GenerateAndExecutePostgreSQLQuery(SQLList);
                FireEvent($"  {SQLList.Count(x => x.PostgreConverted)}  / {SQLList.Count()} ");
            }

            File.WriteAllText("DbUpdateNew.json", JsonConvert.SerializeObject(DbUpdateNewList));

           
        }

        public void CreateSIRKETDBDbOjects()
        {

            PostgreConn = GetConncetion("SIRKETDB");
            foreach (var _objType in (ObjectType[])Enum.GetValues(typeof(ObjectType)))
            {
                var SQLList = GetFilteredData(WhichDB.Entity, _objType);
                FireEvent($" SIRKETDB Db üzerinde  {Enum.GetName(typeof(ObjectType), _objType) }  Tipinde ki Cümleler çalışıyor ");
                Thread.Sleep(200);
                GenerateAndExecutePostgreSQLQuery(SQLList);

                FireEvent($"  {SQLList.Count(x => x.PostgreConverted)}  / {SQLList.Count()} ");
            }
            File.WriteAllText("DbUpdateNew.json", JsonConvert.SerializeObject(DbUpdateNewList));
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

        private NpgsqlConnection GetConncetion(string dbname = "NETSIS")
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
