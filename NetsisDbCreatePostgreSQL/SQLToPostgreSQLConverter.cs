﻿using System.Text.RegularExpressions;

namespace NetsisDbCreatePostgreSQL
{
    public class SQLToPostgreSQLConverter
    {
        public static string ConvertToPostgreSQL(string sqlScript)
        {

            sqlScript = sqlScript.ToUpperInvariant();


            sqlScript = Removebrackets(sqlScript);
            sqlScript = RemoveWithNoLock(sqlScript);
            sqlScript = RemoveClasstered(sqlScript);
            sqlScript = RemoveReplication(sqlScript);

            // Convert data types
            sqlScript = ConvertDataTypes(sqlScript);

            // Convert constraints
            sqlScript = ConvertConstraints(sqlScript);

            // Convert other syntax if needed
            sqlScript = ConvertIdentityColumns(sqlScript);
            sqlScript = ConvertDefaultValues(sqlScript);
            sqlScript = RemoveWithNolock(sqlScript);
            sqlScript = RemoveReplication(sqlScript);


            

            return sqlScript;
        }

        private static string RemoveWithNoLock(string sqlScript)
        {
            sqlScript = Regex.Replace(sqlScript, "\\bWITH\\s?\\(NOLOCK\\)\\b", "", RegexOptions.IgnoreCase);
            return sqlScript;
        }

        private static string ConvertConstraints(string sqlScript)
        {
          //  // Convert primary key constraint
          ////  sqlScript = Regex.Replace(sqlScript, "PRIMARY KEY \\(.*?\\)", "PRIMARY KEY", RegexOptions.IgnoreCase);

          //  // Convert foreign key constraint
          //  sqlScript = Regex.Replace(sqlScript, "FOREIGN KEY .*? REFERENCES .*?(?:,|\\))", "", RegexOptions.IgnoreCase);

          //  // Convert unique nonclustered index
          //  sqlScript = Regex.Replace(sqlScript, "UNIQUE NONCLUSTERED INDEX \\[.*?\\]", "UNIQUE", RegexOptions.IgnoreCase);

          //  // Remove FOR REPLICATION keyword
          //  sqlScript = Regex.Replace(sqlScript, "\\bFOR\\s?REPLICATION\\b", "", RegexOptions.IgnoreCase);

            return sqlScript;
        }

        private static string ConvertIdentityColumns(string sqlScript)
        {

            // Define the regular expression pattern to match the column type
       //     string pattern = @"INT|INTEGER\s+IDENTITY\s*\(\s*(\d+)\s*,\s*(\d+)\s*\)(\s+NOT\s+NULL)?";
            string pattern = @"INT\s+IDENTITY\s*\(\s*(\d+)\s*,\s*(\d+)\s*\)(\s+NOT\s+NULL)?";

            // Define the replacement pattern for the SERIAL equivalent
            string replacement = @"SERIAL";

            // Use Regex.Replace to convert the column type
            string output = Regex.Replace(sqlScript, pattern, replacement);
            return    ConvertIdentityColumns2(output);


        }

        
        private static string ConvertIdentityColumns2(string sqlScript)
        {

            // Define the regular expression pattern to match the column type
            //     string pattern = @"INTEGER\s+IDENTITY\s*\(\s*(\d+)\s*,\s*(\d+)\s*\)(\s+NOT\s+NULL)?";
            string pattern = @"INTEGER\s+IDENTITY\s*\(\s*(\d+)\s*,\s*(\d+)\s*\)(\s+NOT\s+NULL)?";

            // Define the replacement pattern for the SERIAL equivalent
            string replacement = @"SERIAL";

            // Use Regex.Replace to convert the column type
            string output = Regex.Replace(sqlScript, pattern, replacement);
            return output;

        }

        private static string ConvertDataTypes(string sqlScript)
        {
            

            // Netsis  Data type conversions as needed
            sqlScript = Regex.Replace(sqlScript, "TDBMUHKOD", "VARCHAR(35)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBADAYKOD", "VARCHAR(15)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBENTKEY", "VARCHAR(50)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBSTOKKOD", "VARCHAR(35)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBTELFAX", "VARCHAR(20)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBCARIKOD", "VARCHAR(15)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBPROJEKOD", "VARCHAR(15)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBREFKOD", "VARCHAR(15)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBBELGENO", "VARCHAR(15)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBNETSISKUL", "VARCHAR(12)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBPLASIYER", "VARCHAR(8)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBGRUPKOD", "VARCHAR(8)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBSIRKETKODU", "VARCHAR(20)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBISYERI", "VARCHAR(8)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBDETAYKODU", "VARCHAR(15)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBMASRAFKODU", "VARCHAR(15)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBDEMIRKODU", "VARCHAR(30)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBSICILNO", "VARCHAR(15)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBENCFLOAT", "VARCHAR(32)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBCRMKULKOD", "VARCHAR(35)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBAKTIVITEKOD", "VARCHAR(35)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBADAYKOD", "VARCHAR(35)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBSIKAYETKOD", "VARCHAR(35)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBSKYAKTIVITEKOD", "VARCHAR(35)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBANKETKODU", "VARCHAR(15)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBGRUPKODLONG", "VARCHAR(15)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBOZLUKKOD", "VARCHAR(35)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBURETGRUPKOD", "VARCHAR(15)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBILGIKODU", "VARCHAR(100)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBVARDIYAKOD", "VARCHAR(15)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBDATETIME", "TIMESTAMP", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBKDVORAN", "DECIMAL(5,2)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBFLOAT", "DECIMAL(28,8)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBINTEGER", "INTEGER", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBOZLUKID", "INTEGER", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBSMALLINT", "SMALLINT", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBMEMO", "BYTEA", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "TDBBYTE", "SMALLINT", RegexOptions.IgnoreCase);

            sqlScript = Regex.Replace(sqlScript, @"\bBOOLEAN\s+DEFAULT\s*\(\s*0\s*\)", "BOOLEAN DEFAULT FALSE");
            sqlScript = Regex.Replace(sqlScript, @"\bBOOLEAN\s+DEFAULT\s+0\b", "BOOLEAN DEFAULT FALSE");


            // sqlScript = Regex.Replace(sqlScript, "INT", "INTEGER", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, @"VARCHAR\(MAX\)", "VARCHAR(4000)", RegexOptions.IgnoreCase);

            sqlScript = Regex.Replace(sqlScript, "VARCHAR\\((\\d+)\\)", "VARCHAR($1)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "NVARCHAR\\((\\d+)\\)", "VARCHAR($1)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "CHAR\\((\\d+)\\)", "CHAR($1)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "NCHAR\\((\\d+)\\)", "CHAR($1)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "DECIMAL\\((\\d+),\\s?(\\d+)\\)", "DECIMAL($1, $2)", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, @"\bBIT\s+DEFAULT\s*\(\s*0\s*\)", "BOOLEAN DEFAULT FALSE");
            sqlScript = Regex.Replace(sqlScript, @"\bBIT\b", "BOOLEAN", RegexOptions.IgnoreCase);
            


            sqlScript = Regex.Replace(sqlScript, @"\bTINYINT\b", "SMALLINT", RegexOptions.IgnoreCase);

            sqlScript = Regex.Replace(sqlScript, @"\bSMALLDATETIME\b", "TIMESTAMP", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, @"\bDATETIME\b", "TIMESTAMP", RegexOptions.IgnoreCase);

            sqlScript = Regex.Replace(sqlScript, "VARBINARY\\(MAX\\)", "BYTEA", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, @"\bIMAGE\b", "BYTEA", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, @"\bTEXT\b", "TEXT", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, @"\bMEMO\b", "TEXT", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, @"\bUNIQUEIDENTIFIER\b", "UUID", RegexOptions.IgnoreCase);









            return sqlScript;
        }
        private static string ConvertDefaultValues(string sqlScript)
        {
            sqlScript = Regex.Replace(sqlScript, @"GETDATE\(\)", "CURRENT_TIMESTAMP");
            return sqlScript;
        }

        private static string RemoveWithNolock(string sqlScript)
        {
            // RemoveWithNolock
            sqlScript = Regex.Replace(sqlScript, @"(?i)WITH\s*\(\s*NOLOCK\s*\)", "");
            return sqlScript;

        }

        private static string Removebrackets(string sqlScript)
        {
            //Remove brackets
            sqlScript = Regex.Replace(sqlScript, @"[\[\]]", "", RegexOptions.IgnoreCase);
            return sqlScript;
        }
        private static string RemoveClasstered(string sqlScript)
        {
            //Remove CLUSTERED
            sqlScript = Regex.Replace(sqlScript, "NONCLUSTERED", " ", RegexOptions.IgnoreCase);
            sqlScript = Regex.Replace(sqlScript, "CLUSTERED", " ", RegexOptions.IgnoreCase);
            
            return sqlScript;
        }

        private static string RemoveReplication(string sqlScript)
        {


            string pattern = @"\s+(NOT\s+FOR\s+REPLICATION|NOT\s+NULL)";

           
             sqlScript = Regex.Replace(sqlScript, pattern, "");


         //   string pattern = @"\s+NOT\s+FOR\s+REPLICATION$";
            //sqlScript = Regex.Replace(sqlScript, pattern, "");
            return sqlScript;
        }

        

    }

}
