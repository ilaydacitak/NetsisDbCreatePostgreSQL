using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetsisDbCreatePostgreSQL
{
    public class Enums
    {
        public enum DataBaseType
        {
            MSSql = 1,
            Oracle = 0,
            Other = 2
        }
        public enum NetsisApplication
        {
            Temelset = 0,
            Demirbas = 1,
            Personel = 2,
            Isletme = 3,
            YeniDemirbas = 4,
            NDI = 5,
            AfterOrtak = 254,
            BeforeOrtak = 255
        }
        public enum ObjectType
        {
            Table = 0,
            Index = 1,
            View = 3,
            SPSF = 4,
            Synonym = 5,
            Sequence = 6,
            Trigger = 7,
            RequiredRecord = 8,
            InstallRecord = 9

        }
        public enum WhichDB
        {
            Entity,
            Netsis
        }
    }
}
