using Newtonsoft.Json;
using Npgsql;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using static NetsisDbCreatePostgreSQL.Enums;

namespace NetsisDbCreatePostgreSQL
{
    class Program
    {
        //test
        public static NpgsqlConnection PostgreConn { get; set; }
        public static List<DBNewItem> DbUpdateNewList { get; set; }
        static void Main(string[] args)
        {

            var jsonData = File.ReadAllText("DbUpdateNew.json");
            DbUpdateNewList = JsonConvert.DeserializeObject<List<DBNewItem>>(jsonData);
            PostgreConn = GetConncetion();
            var SQLList= GetFilteredData();

            foreach (var item in SQLList.Where(x=>x.InckeyNo!=-1).ToList())
            {
                item.PostgreSQL = SQLToPostgreSQLConverter.ConvertToPostgreSQL(item.Cumle);
                try
                {
                    ExecuteCommand(item.PostgreSQL);
                    item.PostgreConverted = true;

                }

                
                catch (NpgsqlException e)
                {
                    
                    if (! e.Message.Contains("already exist"))
                    {
                        Console.WriteLine($" {item.InckeyNo}   {item.ObjeAcik}  {e.Message}  ");
                        Console.WriteLine($"  { item.PostgreSQL}  ");
                    }




                }

                
            }
            
            File.WriteAllText("DbUpdateNew.json", JsonConvert.SerializeObject(DbUpdateNewList));
            Console.WriteLine($"  {SQLList.Count(x => x.PostgreConverted)}  / {SQLList.Count()} ");



            Console.ReadLine();
        }

        private static NpgsqlConnection GetConncetion()
        {
            var connectionString = "Host =localhost;Username=postgres;Password=postgres;Database=SIRKETDB";
            var dataSourceBuilder = new NpgsqlDataSourceBuilder(connectionString);
            var dataSource = dataSourceBuilder.Build();
            return dataSource.OpenConnection();
        }

        private static void ExecuteCommand(string NetCommandText)
        {
            if (NetCommandText != string.Empty)
            {
                var command = new NpgsqlCommand(NetCommandText, PostgreConn);
                command.ExecuteNonQuery();
            }
        }

        private static List<DBNewItem> GetFilteredData()
        {

           var FilterDbType = DataBaseType.MSSql;
           var FilterDb = WhichDB.Entity;
           var FilterUrun = 0;

            var Tables = DbUpdateNewList.Where(x => x.Urun == FilterUrun && x.Nerde == FilterDb && x.Veritabani == FilterDbType && x.ObjeTipi == ObjectType.Table).OrderByDescending(x => x.IlkSira).ThenBy(x => x.SiraNo).ThenBy(x=>x.InckeyNo);
            var Indexes = DbUpdateNewList.Where(x => x.Urun == FilterUrun && x.Nerde == FilterDb && x.Veritabani == FilterDbType && x.ObjeTipi == ObjectType.Index).OrderByDescending(x => x.IlkSira).ThenBy(x => x.SiraNo).ThenBy(x => x.InckeyNo);
            var Views = DbUpdateNewList.Where(x => x.Urun == FilterUrun && x.Nerde == FilterDb && x.Veritabani == FilterDbType && x.ObjeTipi == ObjectType.View).OrderByDescending(x => x.IlkSira).ThenBy(x => x.SiraNo).ThenBy(x => x.InckeyNo);
            var Trigers = DbUpdateNewList.Where(x => x.Urun == FilterUrun && x.Nerde == FilterDb && x.Veritabani == FilterDbType && x.ObjeTipi == ObjectType.Trigger).OrderByDescending(x => x.IlkSira).ThenBy(x => x.SiraNo).ThenBy(x => x.InckeyNo);
            var Sequences = DbUpdateNewList.Where(x => x.Urun == FilterUrun && x.Nerde == FilterDb && x.Veritabani == FilterDbType && x.ObjeTipi == ObjectType.Sequence).OrderByDescending(x => x.IlkSira).ThenBy(x => x.SiraNo).ThenBy(x => x.InckeyNo);
            var Synonyms = DbUpdateNewList.Where(x => x.Urun == FilterUrun && x.Nerde == FilterDb && x.Veritabani == FilterDbType && x.ObjeTipi == ObjectType.Synonym).OrderByDescending(x => x.IlkSira).ThenBy(x => x.SiraNo).ThenBy(x => x.InckeyNo);
            var SPNetsis = DbUpdateNewList.Where(x => x.Urun == FilterUrun && x.Nerde == FilterDb && x.Veritabani == FilterDbType && x.ObjeTipi == ObjectType.SPSF).OrderByDescending(x => x.IlkSira).ThenBy(x => x.SiraNo).ThenBy(x => x.InckeyNo);
            var InstallNetsis = DbUpdateNewList.Where(x => x.Urun == FilterUrun && x.Nerde == FilterDb && x.Veritabani == FilterDbType && x.ObjeTipi == ObjectType.InstallRecord).OrderByDescending(x => x.IlkSira).ThenBy(x => x.SiraNo).ThenBy(x => x.InckeyNo);
            var RequirementNetsis = DbUpdateNewList.Where(x => x.Urun == FilterUrun && x.Nerde == FilterDb && x.Veritabani == FilterDbType && x.ObjeTipi == ObjectType.RequiredRecord).OrderByDescending(x => x.IlkSira).ThenBy(x => x.SiraNo).ThenBy(x => x.InckeyNo);
            return Tables.ToList();


        }
    }
}
