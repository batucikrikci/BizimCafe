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
    public partial class GecmisSiparisForm : Form
    {
        private readonly KahveVeri _db;

        public GecmisSiparisForm(KahveVeri db)
        {
            _db = db;
            InitializeComponent();
            dgvSiparisler.DataSource = _db.GecmisSiparisler;
        }

        private void dgvSiparisler_SelectionChanged(object sender, EventArgs e)
        {
            if (dgvSiparisler.SelectedRows.Count==0)
            {
                dgvSiparisDetaylar.DataSource = null;
            }
            else
            {
                DataGridViewRow secilenSatir = dgvSiparisler.SelectedRows[0];
                Siparis secilenSiparis = (Siparis)secilenSatir.DataBoundItem;
                dgvSiparisDetaylar.DataSource = secilenSiparis.SiparisDetaylar;
            }
        }
    }
}

