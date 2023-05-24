using Newtonsoft.Json;
using static NetsisDbCreatePostgreSQL.Enums;

namespace NetsisDbCreatePostgreSQL
{
    public class DBNewItem
    {
        public int InckeyNo { get; set; }

        //public NetsisApplication NetsisAppType {get;}
        public int Urun { get; set; }

        public DataBaseType Veritabani { get; set; }
        public string Modul { get; set; }
        public string Baglanti { get; set; }
        public ObjectType ObjeTipi { get; set; }
        [JsonIgnore]
        public string ObjectTypeName { get; set; }
        public int IlkSira { get; set; }
        public int SiraNo { get; set; }
        public string ObjeAcik { get; set; }
        public string KayitTar { get; set; }
        public string Yazan { get; set; }
        public string Flag { get; set; }
        public WhichDB Nerde { get; set; }
        [JsonIgnore]
        public string NerdeName { get; set; }
        public string DuzeltTar { get; set; }
        public string Duzelten { get; set; }
        public bool Parametre { get; set; }
        public bool Guvenlik { get; set; }
        public bool LogTutulacak { get; set; }
        public string DBEnc { get; set; }
        public string Cumle { get; set; }

        public string PostgreSQL { get; set; }
        public bool PostgreConverted { get; set; }
        public string PostgreError { get; set; }
        public NetsisApplication getUrun()
        {
            switch (this.Urun)
            {
                case 0:
                    return NetsisApplication.Temelset;
                case 1:
                    return NetsisApplication.Demirbas;
                case 2:
                    return NetsisApplication.Personel;
                case 3:
                    return NetsisApplication.Isletme;
                case 4:
                    return NetsisApplication.YeniDemirbas;
                case 5:
                    return NetsisApplication.NDI;
                case 254:
                    return NetsisApplication.AfterOrtak;
                case 255:
                    return NetsisApplication.BeforeOrtak;
                default:
                    return NetsisApplication.Temelset;

            }

        }
    }
}
