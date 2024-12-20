using StokBarkodSistemi.Models;

namespace StokBarkodSistemi.Services
{
    public class StokKartService
    {
        public static List<StokKartBilgi> StokKartBilgiler { get; set; } = new List<StokKartBilgi>
        {
            new StokKartBilgi { StokNo = "A", KasaIciMiktar = 50, EksiltmeMiktar = 10 },
            new StokKartBilgi { StokNo = "B", KasaIciMiktar = 20, EksiltmeMiktar = 5 },
            new StokKartBilgi { StokNo = "C", KasaIciMiktar = 1200, EksiltmeMiktar = 300 }
        };
        public static List<Barkod> Barkodlar { get; set; } = new List<Barkod>();
    }
}
