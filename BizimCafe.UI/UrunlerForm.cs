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
    public partial class UrunlerForm : Form
    {
        private KahveVeri _db;

        public UrunlerForm(KahveVeri db)
        {
            InitializeComponent();
            _db = db;
            UrunleriListele();
        }

        private void UrunleriListele()
        {
            dgvUrunler.DataSource = _db.Urunler.ToList();
        }

        private void btnEkle1_Click(object sender, EventArgs e)
        {
            string ad = txbUrunAdi.Text.Trim();

            if (string.IsNullOrEmpty(ad))
            {
                MessageBox.Show("Ürün adı veya Fiyatı girmediniz!!");
            }

            if (btnEkle1.Text == "EKLE")
            {
                _db.Urunler.Add(new Urun() { UrunAd = txbUrunAdi.Text, BirimFiyat = nudBirimFiyat.Value });
                UrunleriListele();
            }
            else
            {
                DataGridViewRow satir = dgvUrunler.SelectedRows[0];
                Urun urun = (Urun)satir.DataBoundItem;
                urun.BirimFiyat = nudBirimFiyat.Value;
                urun.UrunAd= txbUrunAdi.Text;
                UrunleriListele();
                btnEkle1.Text = "EKLE";
                btnİptal.Visible = false;
                txbUrunAdi.Clear();
                nudBirimFiyat.Value = 0;

            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            if (dgvUrunler.SelectedRows.Count==0)
            {
                MessageBox.Show("Önce ürün seçiniz. ");
                return;
            }

            DialogResult dr = MessageBox.Show("Seçili ürünü silmek istediğinize emin misiniz?","Silme Onayı",MessageBoxButtons.YesNo,MessageBoxIcon.Question,MessageBoxDefaultButton.Button2);
            if (dr == DialogResult.No)
                return;

            DataGridViewRow satir = dgvUrunler.SelectedRows[0];
            Urun urun = (Urun)satir.DataBoundItem;
            _db.Urunler.Remove(urun);
            UrunleriListele();
        }

        private void btnDuzenle_Click(object sender, EventArgs e)
        {
            btnEkle1.Text = "KAYDET";
            btnİptal.Visible = true;
            DataGridViewRow satir = dgvUrunler.SelectedRows[0];
            Urun urun = (Urun)satir.DataBoundItem;
            txbUrunAdi.Text = urun.UrunAd;
            nudBirimFiyat.Value = urun.BirimFiyat;

           
        }

        private void btnİptal_Click(object sender, EventArgs e)
        {
            btnEkle1.Text = "EKLE";
            btnİptal.Visible = false;
            txbUrunAdi.Text = "";
            nudBirimFiyat.Value = 0;
        }
    }
}
