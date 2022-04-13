using System;
using System.Collections.Generic;


namespace SF.Core.Entities.Concrete
{
    [Serializable]
    public class YetkiKullanici
    {
        public int Id { get; set; }
        public string Ad { get; set; }

        public string Soyad { get; set; }
        public string TcNo { get; set; }
    

        public string Unvan { get; set; }
        public int UnvanId { get; set; }
        public bool Gelitirici { get; set; }

        /// <summary>
        /// Kullanicı adı/sicil 
        /// </summary>
        public string KAdi { get; set; }


        public int UygulamaId { get; set; }

        public string BirimAdi { get; set; }
        public int BirimId { get; set; }

        public int? VekilId { get; set; }


        /// <summary>
        /// Kullanıcı Id 
        /// </summary>
        public int KId { get; set; }
        /// <summary>
        /// Kullanıcı tipi (iç dış)
        /// </summary>
        public int KTip { get; set; }


        public long TokenId { get; set; }

        public List<string> YetkiList { get; set; }
    }


    [Serializable]
    public class YetkiKullaniciDetay    {

        public int Id { get; set; }
        public string OnEk { get; set; }
        public string Ad { get; set; }
        public string Soyad { get; set; }
        public long? TcNo { get; set; }
        public string EPosta { get; set; }
        public string TelNo { get; set; }
        public string TelNo2 { get; set; }
        public int UnvanId { get; set; }
        public string UnvanAdi { get; set; }
        public string UnvanKod { get; set; }
        public int BirimId { get; set; }
        public string BirimAdi { get; set; }
        public int? BirimKod { get; set; }
        public string BirimYol { get; set; }
        public int? BirimGrupId { get; set; }
        public DateTime EklemeTarihi { get; set; }
        public DateTime? DegistirmeTarihi { get; set; }
        public bool GelistiriciMi { get; set; }
        public int UygulamaId { get; set; }
        public bool VarsayilanMi { get; set; }
        public bool Durum { get; set; }
        public bool Silindi { get; set; }
        public int EkleyenId { get; set; }
        public int? DegistirenId { get; set; }
        public int RowVersion { get; set; }
        public int KullaniciId { get; set; }

        public string SicilNo { get; set; }
        public bool KullaniciDurum { get; set; }
        public int KullaniciTipId { get; set; }
        public string SehirAdi { get; set; }
        public string SehirKodu { get; set; }
        public int? SehirId { get; set; }
        public int? SiraNo { get; set; }
        public string KullaniciTipAdi { get; set; }
        public string UygulamaAdi { get; set; }
        public string UygulamaUrl { get; set; }
        public string UygulamaIkon { get; set; }
    }

    [Serializable]
    public class YetkiKullaniciProfilOzet
    {

        public int Id { get; set; }
        public int KullaniciId { get; set; }

        public int UnvanId { get; set; }
        public string UnvanAdi { get; set; }
        public string UnvanKod { get; set; }
        public int BirimId { get; set; }
        public string BirimAdi { get; set; }
        public int? BirimKod { get; set; }
        public string BirimYol { get; set; }
        public int? BirimGrupId { get; set; }

        public int UygulamaId { get; set; }
        public bool VarsayilanMi { get; set; }

        public string UygulamaAdi { get; set; }
        public string UygulamaUrl { get; set; }
        public string UygulamaIkon { get; set; }
    }

}
