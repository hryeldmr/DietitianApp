using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Diyetisyen
{
    public partial class frmAyar : Form
    {
        public frmAyar()
        {
            InitializeComponent();
        }

        private void frmAyar_Load(object sender, EventArgs e)
        {
            doldur();
        }
        public void doldur()
        {
            DataTable tb = new DataTable();
            string cumle = "Select * from besin";
            baglanti bag = new baglanti();
            tb = bag.tablogetir(cumle);
            dataGridView1.DataSource = tb;

        }

        private void label1_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        #region Sürükleme İşlemleri
        bool dragging;
        Point offset;

        private void panel1_MouseDown(object sender, MouseEventArgs e)
        {
            dragging = true;
            offset = e.Location;
        }

        private void panel1_MouseUp(object sender, MouseEventArgs e)
        {
            dragging = false;
        }

        private void panel1_MouseMove(object sender, MouseEventArgs e)
        {
            if (dragging)
            {
                Point currentScreenPos = PointToScreen(e.Location);
                Location = new
                Point(currentScreenPos.X - offset.X,
                currentScreenPos.Y - offset.Y);
            }
        }
        #endregion

        #region Gölge işlemi
        protected override CreateParams CreateParams
        {
            get
            {
                const int CS_DROPSHADOW = 0x20000;
                CreateParams cp = base.CreateParams;
                cp.ClassStyle |= CS_DROPSHADOW;
                return cp;
            }
        }
        #endregion      

        #region Arama işlemi
        public void Ara()
        {
            DataTable tb = new DataTable();
            string cumle = "Select * from besin where besin_ad like '%" + txtAdAra.Text + "%' order by id desc";
            baglanti bag = new baglanti();
            tb = bag.tablogetir(cumle);
            dataGridView1.DataSource = tb;

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Ara();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            doldur();
            txtAdAra.Text = string.Empty;
        }
        #endregion

        #region Datagried tıklama işlemleri
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            btnDuzenle.Visible = true;
            btnSil.Visible = true;

            txtGizli.Text= dataGridView1.Rows[e.RowIndex].Cells[0].Value.ToString();
            txtBesinAd.Text = dataGridView1.Rows[e.RowIndex].Cells[1].Value.ToString();
            txtBesinAdet.Text = dataGridView1.Rows[e.RowIndex].Cells[3].Value.ToString();
            txtKalori.Text = dataGridView1.Rows[e.RowIndex].Cells[2].Value.ToString();
        }
        #endregion

        #region Besin ekleme işlenmleri
        private void btnEkle_Click(object sender, EventArgs e)
        {
            BesinEkle();
            btnDuzenle.Visible = false;
            btnSil.Visible = false;
        }

        void BesinEkle()
        {
            string cumle = "INSERT INTO besin (besin_ad, besin_adet ,besin_kalori) VALUES('" + txtBesinAd.Text + "','" + txtBesinAdet.Text + "','" + txtKalori.Text + "')";

            baglanti bag = new baglanti();
            bag.idu(cumle);
            MessageBox.Show("Yeni kayıt eklendi", "Kayıt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            doldur();
        }
        #endregion

        #region Besin silme işlemleri
        void ogeSil()
        {
            try
            {
                DialogResult cvp = MessageBox.Show("Seçili kayıt silinsinmi?", "Silme İşlemi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (cvp == DialogResult.Yes)
                {
                    string cumle = "delete from besin where id=" + Convert.ToInt16(txtGizli.Text);
                    baglanti bag = new baglanti();
                    bag.idu(cumle);
                    MessageBox.Show("Silme İşlemi Tamamlandı", "Silindi", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    doldur();
                }
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message);
            }
        }

        private void btnSil_Click(object sender, EventArgs e)
        {
            ogeSil();
            btnDuzenle.Visible = false;
            btnSil.Visible = false;
        }
        #endregion

        #region Besin düzenleme işlemleri
        private void btnDuzenle_Click(object sender, EventArgs e)
        {
            Duzenle();
            txtBesinAd.Text = string.Empty; txtBesinAdet.Text = string.Empty; txtKalori.Text = string.Empty;
            btnDuzenle.Visible = false;
            btnSil.Visible = false;
        }

        void Duzenle()
        {           

            string cumle = "update besin set besin_ad='" + txtBesinAd.Text + "', besin_adet='" + txtBesinAdet.Text + "', besin_kalori='" + txtKalori.Text + "' where id=" + Convert.ToInt16(txtGizli.Text);

            baglanti bag = new baglanti();
            bag.idu(cumle);
            MessageBox.Show("Kayıt düzenlendi", "Düzenleme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            doldur();
        }
        #endregion


        private void txtBesinAd_TextChanged(object sender, EventArgs e)
        {
            txtBesinAd.CharacterCasing = CharacterCasing.Upper;
        }


    }
}
