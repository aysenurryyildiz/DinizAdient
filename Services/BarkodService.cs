using StokBarkodSistemi.Models;

namespace StokBarkodSistemi.Services
{
    public class BarkodService
    {
        public void GenerateBarkod(string stokNo, decimal gelenMiktar)
        {
            var stokKartBilgi = StokKartService.StokKartBilgiler
                .FirstOrDefault(x => x.StokNo == stokNo);

            if (stokKartBilgi == null)
                throw new Exception("Stok kartı bilgisi bulunamadı.");

            var mevcutBarkods = StokKartService.Barkodlar
                .Where(b => b.StokNo == stokNo && b.KasaIciMiktar < stokKartBilgi.KasaIciMiktar)
                .OrderBy(b => b.BarkodNo)
                .ToList();

            decimal remainingMiktar = gelenMiktar;

            foreach (var barkod in mevcutBarkods)
            {
                if (remainingMiktar <= 0)
                    break;

                decimal tamamlananMiktar = Math.Min(stokKartBilgi.KasaIciMiktar - barkod.KasaIciMiktar, remainingMiktar);
                barkod.KasaIciMiktar += tamamlananMiktar;
                barkod.EksiltmeMiktar = barkod.KasaIciMiktar == stokKartBilgi.KasaIciMiktar
                    ? stokKartBilgi.EksiltmeMiktar
                    : barkod.KasaIciMiktar;
                remainingMiktar -= tamamlananMiktar;
            }

            if (remainingMiktar > 0)
            {
                int lastBarkodNo = GetLastBarkodNumber();

                while (remainingMiktar > 0)
                {
                    decimal currentMiktar = Math.Min(stokKartBilgi.KasaIciMiktar, remainingMiktar);
                    decimal eksiltmeMiktar = currentMiktar == stokKartBilgi.KasaIciMiktar ? stokKartBilgi.EksiltmeMiktar : currentMiktar;

                    var newBarkod = new Barkod
                    {
                        BarkodNo = GenerateUniqueBarkodNo(lastBarkodNo),
                        StokNo = stokNo,
                        KasaIciMiktar = currentMiktar,
                        EksiltmeMiktar = eksiltmeMiktar
                    };

                    StokKartService.Barkodlar.Add(newBarkod);
                    remainingMiktar -= currentMiktar;
                    lastBarkodNo++;
                }
            }
        }

        private int GetLastBarkodNumber()
        {
            var lastBarkod = StokKartService.Barkodlar
                .OrderByDescending(b => b.BarkodNo)
                .FirstOrDefault();

            return lastBarkod == null ? 0 : int.Parse(lastBarkod.BarkodNo.Substring(6, 4));
        }
        private string GenerateUniqueBarkodNo(int lastBarkodNo)
        {
            return DateTime.Now.ToString("yyMMdd") + (lastBarkodNo + 1).ToString("D4");
        }
    }


}
