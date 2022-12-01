using BizimCafe.Data;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace BizimCafe.UI
{
    public partial class AnaForm : Form
    {
        KahveVeri Db = new KahveVeri();

        public AnaForm()
        {
            InitializeComponent();
            VerileriYukle();
            MasalariOlustur();
            
        }

        private void VerileriYukle()
        {
            try
            {
                string json = File.ReadAllText("veri.json");
                Db = JsonSerializer.Deserialize<KahveVeri>(json);
            }
            catch (Exception)
            {
                OrnekVeriYukle();

            }
        }

        private void OrnekVeriYukle()
        {
            Db.Urunler.Add(new Urun() { UrunAd = "Kola", BirimFiyat = 7.00m });
            Db.Urunler.Add(new Urun() { UrunAd = "Ayran", BirimFiyat = 6.50m });
        }

        private void MasalariOlustur()
        {
            for (int i = 1; i <= Db.MasaAdet; i++)
            {
                ListViewItem item = new ListViewItem($"Masa {i}");
                item.ImageKey = "bos";
                item.Tag = i;
                lvwMasalar.Items.Add(item);
            }
        }
        private void lvwMasalar_DoubleClick(object sender, EventArgs e)
        {
            ListViewItem lvi = lvwMasalar.SelectedItems[0];
            int masaNo = (int)lvi.Tag;

            Siparis siparis = Db.AktifSiparisler.FirstOrDefault(x => x.MasaNo == masaNo);

            if (siparis == null)
            {
                siparis = new Siparis() { MasaNo = masaNo };
                Db.AktifSiparisler.Add(siparis);
                lvi.ImageKey = "dolu";

            }

            var frmSiparis = new SiparisForm(Db, siparis);
            DialogResult sonuc = frmSiparis.ShowDialog();

            if (sonuc == DialogResult.OK)
                lvi.ImageKey = "bos";
        }

        private void tsmiGecmisSiparisler_Click(object sender, EventArgs e)
        {
            new GecmisSiparisForm(Db).ShowDialog();
        }

        private void tsmiUrunler_Click(object sender, EventArgs e)
        {
            new UrunlerForm(Db).ShowDialog();
        }

        private void AnaForm_FormClosing_1(object sender, FormClosingEventArgs e)
        {
           
        }

        private void AnaForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            string json = JsonSerializer.Serialize(Db);
            File.WriteAllText("veri,json", json);
        }
    }
}
