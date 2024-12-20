namespace StokBarkodSistemi.Models
{
    public class Barkod
    {
        public string BarkodNo { get; set; } = string.Empty;
        public string StokNo { get; set; } = string.Empty;
        public decimal KasaIciMiktar { get; set; }
        public decimal EksiltmeMiktar { get; set; }
    }
}
