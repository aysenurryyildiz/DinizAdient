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

            decimal remainingMiktar = gelenMiktar;
            int lastBarkodNo = GetLastBarkodNumber(); 

            while (remainingMiktar > 0)
            {
                decimal currentMiktar = Math.Min(stokKartBilgi.KasaIciMiktar, remainingMiktar);
                decimal eksiltmeMiktar = currentMiktar == stokKartBilgi.KasaIciMiktar ? stokKartBilgi.EksiltmeMiktar : remainingMiktar;

                var barkod = new Barkod
                {
                    BarkodNo = GenerateUniqueBarkodNo(lastBarkodNo),
                    StokNo = stokNo,
                    KasaIciMiktar = currentMiktar,
                    EksiltmeMiktar = eksiltmeMiktar
                };

                StokKartService.Barkodlar.Add(barkod);
                remainingMiktar -= currentMiktar;
                lastBarkodNo++; 
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
