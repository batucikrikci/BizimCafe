using BizimCafe.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BizimCafe.UI
{
    public partial class SiparisForm : Form
    {
        private readonly KahveVeri _db;
        private readonly Siparis _siparis;

        public SiparisForm(KahveVeri db, Siparis siparis)
        {
            _db = db;
            _siparis = siparis;
            InitializeComponent();
            BilgileriGuncelle();
            UrunleriListele();
        }

        private void UrunleriListele()
        {
            cboUrun.DataSource = _db.Urunler;
        }

        private void BilgileriGuncelle()
        {
            Text = $"Masa {_siparis.MasaNo}";
            lblMasaNo.Text = _siparis.MasaNo.ToString("00");
            lblOdemeTutari.Text = _siparis.ToplamTutarTL;
            dgvSiparisDetaylar.DataSource = _siparis.SiparisDetaylar.ToList();
        }

        private void btnEkle_Click(object sender, EventArgs e)
        {
            Urun urun = (Urun)cboUrun.SelectedItem;

            if (urun == null)
                return;

            SiparisDetay sd = _siparis.SiparisDetaylar
                .FirstOrDefault(x => x.UrunAd == urun.UrunAd);

            if (sd != null)
            {
                sd.Adet += (int)nudAdet.Value;
            }
            else
            {
                sd = new SiparisDetay()
                {
                    UrunAd = urun.UrunAd,
                    BirimFiyat = urun.BirimFiyat,
                    Adet = (int)nudAdet.Value,
                };

                _siparis.SiparisDetaylar.Add(sd);
            }

            BilgileriGuncelle();
        }


        private void SiparisiKapat(decimal odenentTutar, SiparisDurum yeniDurum)
        {
            _siparis.KapanisZamani = DateTime.Now;
            _siparis.OdenenTutar = odenentTutar;
            _siparis.Durum = yeniDurum;
            _db.AktifSiparisler.Remove(_siparis);
            _db.GecmisSiparisler.Add(_siparis);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void btnAnasayfa_Click(object sender, EventArgs e)
        {
            Close();
        }

        private void btnSiparisIptal_Click(object sender, EventArgs e)
        {
            SiparisiKapat(0, SiparisDurum.Iptal);
        }

        private void btnOdemeAl_Click(object sender, EventArgs e)
        {
            SiparisiKapat(_siparis.ToplamTutar(), SiparisDurum.Odendi);
        }
    }
}
