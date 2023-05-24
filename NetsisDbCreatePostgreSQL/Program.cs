using System;

namespace NetsisDbCreatePostgreSQL
{
    class Program
    {
       
        static void Main(string[] args)
        {


            var x = new DbUpdateManager();
            x.OnLog += X_OnLog;
            x.CreateNetsisDbOjects();
            x.CreateSIRKETDBDbOjects();
            Console.ReadLine();
        }

        private static void X_OnLog(object sender, LogEventArgs e)
        {
            Console.WriteLine(e.Message);
        }
    }
}
