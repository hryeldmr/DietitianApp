using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.OleDb;
using System.Data;

namespace Diyetisyen
{
    public partial class frmAna : Form
    {        

        public frmAna()
        {
            InitializeComponent();
        }

        #region Normal İşlemler
        private void lblKapat_Click(object sender, EventArgs e)
        {
            DialogResult cvp = MessageBox.Show("Kapatmak İstermisiniz?", "Kapat?", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (cvp==DialogResult.Yes)//burada kayıt işlemi yaptır
            {
                Application.Exit();
            }
        }

        private void lblSimge_Click(object sender, EventArgs e)
        {
            this.WindowState = FormWindowState.Minimized;
        }

        private void lblAyar_Click(object sender, EventArgs e)
        {
            frmAyar frAyar = new frmAyar();
            frAyar.Show();
        }

        private void frmAna_Load(object sender, EventArgs e)
        {
            doldur();
            
        }
        #endregion


        void BesinDoldur()//besin tablosunu dolurur
        {
            try
            {   
                string cumle = "Select * from besin";
                baglanti bag = new baglanti();
                DataTable tb = new DataTable();
                tb = bag.tablogetir(cumle);
                dataGriedBesin.DataSource = tb;
                
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.Message);
            }
        }



        public void doldur()//kullanıcı tablosunu doldurur
        {
            DataTable tb = new DataTable();
            string cumle = "Select * from musteri order by id desc";
            baglanti bag = new baglanti();
            tb = bag.tablogetir(cumle);
            dataGridView1.DataSource = tb;
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

        #region Arama İşlemleri
        private void btnAra_Click(object sender, EventArgs e)
        {
            Ara();
        }


        public void Ara()
        {
            DataTable tb = new DataTable();
            string cumle = "Select * from musteri where adsoyad like '%"+txtAdAra.Text+"%' order by id desc";
            baglanti bag = new baglanti();
            tb = bag.tablogetir(cumle);
            dataGridView1.DataSource = tb;

        }
        private void btnSifirla_Click(object sender, EventArgs e)
        {
            doldur();
            txtAdAra.Text = "Ad Soyad";
            txtAdAra.ForeColor = Color.FromArgb(255, 192, 192);
        }



        #endregion

        #region Arama Bölümü Tasarım Kontrolleri
        private void txtAdAra_Enter(object sender, EventArgs e)
        {
            if (txtAdAra.Text== "AD SOYAD")
            {
                txtAdAra.Text = string.Empty;
                txtAdAra.ForeColor = Color.Coral;
            }
        }
        private void txtAdAra_Leave(object sender, EventArgs e)
        {
            if (txtAdAra.Text == string.Empty)
            {
                txtAdAra.Text = "AD SOYAD";
                txtAdAra.ForeColor = Color.FromArgb(255, 192, 192);
            }
        }
        #endregion

        #region Radio Butonlar
        public string cinsiyet = "";
        private void rdKadin_CheckedChanged(object sender, EventArgs e)
        {
            cinsiyet = "Kadın";
        }

        private void rdErkek_CheckedChanged(object sender, EventArgs e)
        {
            cinsiyet = "Erkek";
        }

        void SadeceSayi(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        #endregion

        #region İdeal Kilo
        string indexCinsiyet = "Kadın";
        private void btnindexHesapla_Click(object sender, EventArgs e)
        {
            idealKilo();
        }

        void idealKilo()
        {
            try
            {

                if (txtindexBoy.Text == string.Empty || txtindexKilo.Text == string.Empty || txtindexYas.Text == string.Empty)
                {
                    MessageBox.Show("Boş alanları doldurunuz", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                else
                {

                    lblindexSonuc.Text = string.Empty;
                    if (rdindexErkek.Checked)
                    {
                        indexCinsiyet = "Erkek";
                    }
                    else if (rdindexKadin.Checked)
                    {
                        indexCinsiyet = "Kadın";
                    }

                    int indexboy = Convert.ToInt16(txtindexBoy.Text);
                    int indexkilo = Convert.ToInt16(txtindexKilo.Text);
                    int indexyas = Convert.ToInt16(txtindexYas.Text);
                    float idealkilo = 0;
                    if (indexCinsiyet == "Kadın") { idealkilo = (indexboy - 100 + indexyas / 10) * 0.8f; }
                    if (indexCinsiyet == "Erkek") { idealkilo = (indexboy - 100 + indexyas / 10) * 0.9f; }


                    if (idealkilo == indexkilo || (idealkilo >= indexkilo - 3 && idealkilo <= indexkilo + 5))
                    {
                        lblindexSonuc.Text = "İdeal kilonuz: " + idealkilo.ToString() + "\nkilon gayet iyi";
                    }
                    else
                    {
                        if (idealkilo > indexkilo)
                        {
                            lblindexSonuc.Text = "İdeal kilonuz: " + idealkilo.ToString() + "\nbiraz kilo almalısın.";
                        }
                        else if (idealkilo < indexkilo)
                        {
                            lblindexSonuc.Text = "İdeal kilonuz: " + idealkilo.ToString() + "\nbiraz kilo vermelisin.";
                        }
                    }
                }
            }
            catch (Exception hata)
            {

                MessageBox.Show(hata.Message);
            }

        }
        private void btnidealTem_Click(object sender, EventArgs e)
        {
            txtindexBoy.Text = string.Empty;
            txtindexKilo.Text = string.Empty;
            txtindexYas.Text = string.Empty;
            lblindexSonuc.Text = string.Empty;
        }

        #endregion

        #region Menu Strip İşlemleri
        private void vucutİndexHesaplaToolStripMenuItem_Click(object sender, EventArgs e)
        {
            try
            {
                
               
                txtindexBoy.Text = dataGridView1.Rows[tasiyici.RowIndex].Cells[4].Value.ToString();
                txtindexKilo.Text = dataGridView1.Rows[tasiyici.RowIndex].Cells[5].Value.ToString();
                txtindexYas.Text = dataGridView1.Rows[tasiyici.RowIndex].Cells[2].Value.ToString();
                indexCinsiyet = dataGridView1.Rows[tasiyici.RowIndex].Cells[3].Value.ToString();
                if (indexCinsiyet == "Kadın")
                {
                    rdindexKadin.Checked = true;
                    rdindexErkek.Checked = false;
                }
                if (indexCinsiyet == "Erkek")
                {
                    rdindexKadin.Checked = false;
                    rdindexErkek.Checked = true;
                }
                idealKilo();
            }
            catch (Exception hata)
            {
                MessageBox.Show(hata.ToString());
            }

            
        }

        public DataGridViewCellEventArgs tasiyici;
        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
              tasiyici =e;
              
        }




        private void silToolStripMenuItem_Click(object sender, EventArgs e)
        {
            seciliId = Convert.ToInt16(dataGridView1.Rows[tasiyici.RowIndex].Cells[0].Value.ToString());
            ogeSil();
        }
        int seciliId;
        void ogeSil()
        {
            try
            {
                DialogResult cvp = MessageBox.Show("Seçili kayıt silinsinmi?", "Silme İşlemi", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
                if (cvp == DialogResult.Yes)
                {
                    string cumle = "delete from musteri where id=" + seciliId;
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



        double GunlukKal = 0;
        private void düzenleToolStripMenuItem_Click(object sender, EventArgs e)
        {
            grpBesinListesi.Visible = true;
            grpAlinanBesinler.Visible = true;
            BesinDoldur();
            string cinsiyet;
            txtKalAdSoyad.Text = dataGridView1.Rows[tasiyici.RowIndex].Cells[1].Value.ToString();
            txtKalBoy.Text = dataGridView1.Rows[tasiyici.RowIndex].Cells[4].Value.ToString();
            txtKalKilo.Text = dataGridView1.Rows[tasiyici.RowIndex].Cells[5].Value.ToString();
            txtKalYas.Text = dataGridView1.Rows[tasiyici.RowIndex].Cells[2].Value.ToString();
            txtGizli.Text = dataGridView1.Rows[tasiyici.RowIndex].Cells[0].Value.ToString();
            txtKalGunlukKalori.Text = dataGridView1.Rows[tasiyici.RowIndex].Cells[6].Value.ToString();


            GunlukKal = Convert.ToDouble(dataGridView1.Rows[tasiyici.RowIndex].Cells[6].Value);
            lblBesKulKalori.Text = GunlukKal.ToString();

            cinsiyet = dataGridView1.Rows[tasiyici.RowIndex].Cells[3].Value.ToString();
            if (cinsiyet == "Erkek")
            {
                rdKalErkek.Checked = true;
                rdKalKAdin.Checked = false;
            }
            else
            {
                rdKalKAdin.Checked = true;
                rdKalErkek.Checked = false;
            }
            btnKalDuzenle.Visible = true;
            btnKulKaydet.Enabled = false;
        }



        #endregion

        #region Kullanıcı Kaydetme İşlemleri
        private void btnKulKaydet_Click(object sender, EventArgs e)
        {
            if (txtKalAdSoyad.Text==string.Empty || txtKalBoy.Text==string.Empty || txtKalKilo.Text==string.Empty || txtKalYas.Text==string.Empty || txtKalGunlukKalori.Text==string.Empty)
            {
                MessageBox.Show("Kayıt için önce boş alanları doldurunuz", "Dikkat", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            else
            {
                KulKaydet();
            }
        }

        void KulKaydet()
        {
            string cinsiyet = "Kadın";
            if (rdKalErkek.Checked)
            {
                cinsiyet = "Erkek";
            }
            else if (rdKalKAdin.Checked)
            {
                cinsiyet = "Kadın";
            }
            string cumle = "INSERT INTO musteri (adsoyad, yas, cinsiyet, boy, kilo, kalori) VALUES('"+txtKalAdSoyad.Text+"', '"+txtKalYas.Text+"', '"+cinsiyet+"', '"+ txtKalBoy.Text + "', '"+ txtKalKilo.Text+ "', '"+txtKalGunlukKalori.Text + "')";
           
            baglanti bag = new baglanti();
            bag.idu(cumle);
            MessageBox.Show("Yeni kayıt eklendi", "Kayıt", MessageBoxButtons.OK, MessageBoxIcon.Information);
            doldur();
            txtKalAdSoyad.Text = string.Empty; txtKalBoy.Text =string.Empty; txtKalKilo.Text = string.Empty; txtKalYas.Text = string.Empty; txtKalGunlukKalori.Text = string.Empty;
        }

        private void btnOtoHesap_Click(object sender, EventArgs e)
        {
            float kalori = 0;
            //erkek kalori hesabı: 66 + ( 9,6 * kilonuz ) + ( 1,7 * boyunuz ) - ( 4,7 * yaşınız )
            //kadın kalori hesabı: 665 + ( 9,6 * kilonuz ) + ( 1,7 * boyunuz ) - ( 4,7 * yaşınız )
            if (rdKalErkek.Checked)
            {
                kalori = 66 + (9.6f * Convert.ToInt16(txtKalKilo.Text)) + (1.7f * Convert.ToInt16(txtKalBoy.Text)) - (4.7f * Convert.ToInt16(txtKalYas.Text));
            }
            else if (rdKalKAdin.Checked)
            {
                 kalori = 665 + (9.6f * Convert.ToInt16(txtKalKilo.Text)) + (1.7f * Convert.ToInt16(txtKalBoy.Text)) - (4.7f * Convert.ToInt16(txtKalYas.Text));
            }
            txtKalGunlukKalori.Text = kalori.ToString();
        }

        private void txtKalAdSoyad_TextChanged(object sender, EventArgs e)
        {
            txtKalAdSoyad.CharacterCasing = CharacterCasing.Upper;
            txtAdAra.CharacterCasing = CharacterCasing.Upper;
        }

        #endregion

        #region Kullanıcı Düzenleme İşlemleri
        private void btnKalDuzenle_Click(object sender, EventArgs e)
        {
            KulDuz();
            btnKalDuzenle.Visible = false;
            btnKulKaydet.Enabled = true;
            grpBesinListesi.Visible = false;
            grpAlinanBesinler.Visible = false;
        }
        
        void KulDuz()
        {
            int id = Convert.ToInt16(txtGizli.Text);
            string cinsiyet = "Kadın";
            if (rdKalErkek.Checked)
            {
                cinsiyet = "Erkek";
            }
            else if (rdKalKAdin.Checked)
            {
                cinsiyet = "Kadın";
            }
            
            string cumle = "update musteri set adsoyad='" + txtKalAdSoyad.Text + "', yas='"+ txtKalYas.Text + "', cinsiyet='"+cinsiyet+"', boy='" + txtKalBoy.Text + "', kilo='"+txtKalKilo.Text+"', kalori='"+txtKalGunlukKalori.Text+"' where id="+Convert.ToInt16(txtGizli.Text);

            baglanti bag = new baglanti();
            bag.idu(cumle);
            MessageBox.Show("Kayıt düzenlendi", "Düzenleme", MessageBoxButtons.OK, MessageBoxIcon.Information);
            doldur();
            txtKalAdSoyad.Text = string.Empty; txtKalBoy.Text = string.Empty; txtKalKilo.Text = string.Empty; txtKalYas.Text = string.Empty; txtKalGunlukKalori.Text = string.Empty;
        }
        #endregion

        #region Listeye besin ve adet ekleme ve günlük kaloriden düşme
        string besinAdi;
        private void dataGriedBesin_CellClick(object sender, DataGridViewCellEventArgs e)
        {
          
            txtBesinKalori.Text = dataGriedBesin.Rows[e.RowIndex].Cells[2].Value.ToString();               
            besinAdi = dataGriedBesin.Rows[e.RowIndex].Cells[1].Value.ToString();
            
        }

        private void btnDusur_Click(object sender, EventArgs e)
        {
            if (txtBesinMiktar.Text==string.Empty)
            {
                MessageBox.Show("Miktar Giriniz");
            }
            else
            {
                int adet = Convert.ToInt16(txtBesinMiktar.Text);
                double besinKalori = Convert.ToDouble(txtBesinKalori.Text);

                GunlukKal = GunlukKal - (adet * besinKalori);
                lblBesKulKalori.Text = GunlukKal.ToString();

                lstAlinanBesinler.Items.Add(txtBesinMiktar.Text + " adet " + besinAdi);
            }
        }
        #endregion





    }
}
