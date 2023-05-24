using System;
using static NetsisDbCreatePostgreSQL.Enums;

namespace NetsisDbCreatePostgreSQL
{
    class Program
    {

        static void Main(string[] args)
        {


            var mng = new DbUpdateManager();
            mng.OnLog += X_OnLog;
            mng.ResetDatabase();
            ObjectType[] ObjectArg = {
                                        ObjectType.Table 
                                        //,ObjectType.Index 
                                        //,ObjectType.View 
                                        //,ObjectType.SPSF 
                                        //,ObjectType.Synonym 
                                        //,ObjectType.Sequence
                                        //,ObjectType.Trigger 
                                        //,ObjectType.RequiredRecord 
                                        //,ObjectType.InstallRecord   
                                    };

            mng.CreateNetsisDbOjects(ObjectArg);
         //   x.CreateSIRKETDBDbOjects(ObjectArg);
            Console.ReadLine();
        }

        private static void X_OnLog(object sender, LogEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
