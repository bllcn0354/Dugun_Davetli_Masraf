using DevExpress.XtraGrid.Views.Grid;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dugun_Davetli_Masraf
{
    public partial class frm_anasayfa : Form
    {
        public frm_anasayfa()
        {
            InitializeComponent();
        }

        islemler i_ = new islemler();


       
        private void Form1_Load(object sender, EventArgs e)
        {
            string sq = "select * from tbl_davetli";
            DataTable denemedt = new DataTable();
            if (i_.SQL_Cek(sq,denemedt,false)==false)
            {
                i_.Create_LocalDB(false);
                i_.Create_DataBase(false);
                i_.lokasyon();
            }
            

            
            //tbl_davetli////////////////////////////////////////////////////////////
            sq = "CREATE TABLE [dbo].[tbl_davetli]( " +
            "[d_id][int] IDENTITY(1,1) NOT NULL CONSTRAINT PK_tbl_davetli PRIMARY KEY (d_id) " +
            ", [d_adi][nvarchar](100)  null, [d_soyad][nvarchar](100)  null , [d_sayi][int] null , " +
            "[d_yakinlik][nvarchar](100) null , [d_nereden][nvarchar](150) null) ; ";
           
            i_.SQL_Create_Table(sq, false);

            //tbl_masraf////////////////////////////////////////////////////////////
            sq = "CREATE TABLE [dbo].[tbl_masraf]( " +
            "[m_id][int] IDENTITY(1,1) NOT NULL CONSTRAINT PK_tbl_masraf PRIMARY KEY (m_id) " +
            ", [m_turu][nvarchar](100)  null, [m_bilgi][nvarchar](100)  null , [m_ucret][float] null , " +
            ") ; ";

            //tbl_bakiye////////////////////////////////////////////////////////////
            sq = "CREATE TABLE [dbo].[tbl_bakiye]( " +
            "[b_id][int] IDENTITY(1,1) NOT NULL CONSTRAINT PK_tbl_bakiye PRIMARY KEY (b_id) " +
            ", [b_ucret][float] not null ) ; " ;

            i_.SQL_Create_Table(sq, false);

            for (int i = 0; i < 50; i++)
            {
                cmb_sayi.Properties.Items.Add(i.ToString());
            }
            cmb_sayi.SelectedIndex = 0;

            DataTable dt = new DataTable();
            sq = "select SehirAdi from Sehirler";
            i_.SQL_Cek(sq, dt, false);

            for (int i = 0; i < dt.Rows.Count-1; i++)
            {
                cmb_nerden.Properties.Items.Add(i_.dtcek_String(dt,i, "SehirAdi"));
            }

            cmb_mtur.Properties.Items.Clear();
            cmb_mtur.Properties.Items.Add("Ulaşım");
            cmb_mtur.Properties.Items.Add("Yemek");
            cmb_mtur.Properties.Items.Add("Barınma");
            cmb_mtur.Properties.Items.Add("Kuaför");
            cmb_mtur.Properties.Items.Add("Diğer");
            cmb_mtur.SelectedIndex = 4;




            davetligetir();

            Font F = new Font("Tahoma", 8, FontStyle.Regular);
           
            i_.sutisdeg(gridView2, "ID", "d_id", 0, true, true, 0, F, 10);
            i_.sutisdeg(gridView2, "Adı", "d_adi", 1, true, true, 0, F, 10);
            i_.sutisdeg(gridView2, "Soyadı", "d_soyad", 2, false, true, 0, F, 10);
            i_.sutisdeg(gridView2, "Yanında Gelecek Sayısı", "d_sayi", 3, true, true, 0, F, 10);
            i_.sutisdeg(gridView2, "Yakınlık", "d_yakinlik", 4, false, true, 0, F, 10);
            i_.sutisdeg(gridView2, "Nereden", "d_nereden", 5, true, true, 0, F, 10);

            bakiyehesapla();

            Font F1 = new Font("Tahoma", 10, FontStyle.Bold);

            i_.sutisdeg(gridView4, "ID", "b_id", 0, false, true, 0, F1, 25);
            i_.sutisdeg(gridView4, "Ücret", "b_ucret", 1, true, true, 0, F1, 25);            

            i_.sutisdeg(gridView1, "ID", "m_id", 0, false, true, 0, F1, 25);
            i_.sutisdeg(gridView1, "Tür", "m_turu", 1, true, true, 0, F1, 25);
            i_.sutisdeg(gridView1, "Bilgi", "m_bilgi", 2, true, true, 0, F1, 25);
            i_.sutisdeg(gridView1, "Ücret", "m_ucret", 3, true, true, 0, F1, 25);


            btn_yenile.PerformClick();

           
        }

        private void simpleButton2_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            string sq= "select d_adi,d_soyad,d_nereden from tbl_davetli where d_adi='"+ txt_ad.Text + "' and d_soyad='" + txt_soyad.Text + "' and" +
                " d_nereden='" + cmb_nerden.Text + "' ";
            i_.SQL_Cek(sq, dt, false);
            if (dt.Rows.Count<=0)
            {
                sq = "insert into tbl_davetli (d_adi,d_soyad,d_sayi,d_yakinlik,d_nereden)" +
               "values ('" + txt_ad.Text + "','" + txt_soyad.Text + "'," + cmb_sayi.SelectedIndex + ",'" + txt_yakinlik.Text + "','" + cmb_nerden.Text + "')";
                i_.SQL_Yurut(sq, false);
            }
            else
            {
                MessageBox.Show("Bu kayıt mevcut","Uyarı",MessageBoxButtons.OK,MessageBoxIcon.Warning);
            }

            davetligetir();
            DataTable dt1 = new DataTable();
            dt1 = (DataTable)gridControl2.DataSource;
            gridView2.FocusedColumn = gridView2.Columns[1];
            Thread.Sleep(200);
            gridView2.FocusedRowHandle = dt1.Rows.Count-1;
            gridView2.FocusRectStyle = DrawFocusRectStyle.RowFocus;
            gridView2.ShowEditor();
            gridView2.SelectRow(dt1.Rows.Count - 1);
        }

        private void davetligetir()
        {
            DataTable dt = new DataTable();
            string sq;
            dt = new DataTable();
            sq = "select * from tbl_davetli";
            i_.SQL_Cek(sq, dt, false);

            gridControl2.DataSource = dt;

            
           


            int sayi_ = 0;
            lbl_davetiye.Text = dt.Rows.Count.ToString();
            try
            {
                for (int i = 0; i < dt.Rows.Count; i++)
                {
                    sayi_ += i_.dtcek_Int(dt, i, "d_sayi") + 1;
                }
                lbl_davetli_sayisi.Text = " Davetli Sayısı: " + sayi_.ToString();
            }
            catch (Exception)
            {


            }


            lbl_davetiye.Text = " Davetiye Sayısı: " + (dt.Rows.Count ).ToString();

        }


        private double masrafgetir()
        {
            DataTable dt = new DataTable();
            string sq;            
            sq = "select * from tbl_masraf";
            i_.SQL_Cek(sq, dt, false);
            gridControl1.DataSource = dt;
            double masraf = 0;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                masraf += i_.dtcek_double(dt,i,"m_ucret");
            }
            lbl_masraf_toplam.Text = " Toplam Masraf: " + masraf.ToString() + " TL";
            return masraf;
        }

        private double bakiyehesapla()
        {
            DataTable dt = new DataTable();
            string sq;
            sq = "select * from tbl_bakiye";
            i_.SQL_Cek(sq, dt, false);
            gridControl4.DataSource = dt;
            double bakiye = 0;
            double net;
            for (int i = 0; i < dt.Rows.Count; i++)
            {
                bakiye += i_.dtcek_double(dt, i, "b_ucret");
            }

            net = (bakiye - masrafgetir());

            lbl_bakiye.Text = " Toplam Bakiye: " + bakiye.ToString() + " TL";
            lbl_net.Text = " Net: " + net.ToString() + " TL";



            if (net <= -1)
            {
                lbl_net.Appearance.BackColor = Color.DarkRed;
                lbl_net.Appearance.ForeColor = Color.White;
            }
            else
            {
                lbl_net.Appearance.BackColor = Color.Green;
                lbl_net.Appearance.ForeColor = Color.White;
            }
            if (net == 0)
            {
                lbl_net.Appearance.BackColor = Color.LightGray;
                lbl_net.Appearance.ForeColor = Color.FromArgb(64,64,64);
            }

            return bakiye;




           

        }

        private void gridControl2_DoubleClick(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = (DataTable)gridControl2.DataSource;

            txt_ad.Text = i_.dtcek_String(dt, gridView2.FocusedRowHandle, "d_adi");
            txt_soyad.Text = i_.dtcek_String(dt, gridView2.FocusedRowHandle, "d_soyad");
            cmb_sayi.Text = i_.dtcek_String(dt, gridView2.FocusedRowHandle, "d_sayi");
            txt_yakinlik.Text = i_.dtcek_String(dt, gridView2.FocusedRowHandle, "d_yakinlik");
            cmb_nerden.Text = i_.dtcek_String(dt, gridView2.FocusedRowHandle, "d_nereden");
        }

        private void simpleButton3_Click(object sender, EventArgs e)
        {
            int secilisatir = gridView2.FocusedRowHandle -1;
            DataTable dt = new DataTable();
            dt = (DataTable)gridControl2.DataSource;
            string sq = "delete from tbl_davetli where d_id=" + i_.dtcek_Int(dt, gridView2.FocusedRowHandle, "d_id") + "";
            DialogResult = MessageBox.Show(i_.dtcek_String(dt, gridView2.FocusedRowHandle, "d_adi") +" isimli davetli silinsin mi?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (DialogResult  == DialogResult.Yes)
            {
                if (i_.SQL_Yurut(sq, false))
                {
                    //MessageBox.Show("Silme Başarılı", "Bildirim", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Silme Başarısız", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                davetligetir();
                gridView2.FocusedColumn = gridView2.Columns[1];
                Thread.Sleep(200);
                gridView2.FocusedRowHandle = secilisatir;
                gridView2.FocusRectStyle = DrawFocusRectStyle.RowFocus;
                gridView2.ShowEditor();
                gridView2.SelectRow(secilisatir);
            }

            
        }

        private void gridControl2_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.Delete)
            {
                btn_sil.PerformClick();
            }
        }

        private void simpleButton1_Click(object sender, EventArgs e)
        {
            davetligetir();
            bakiyehesapla();
            //simpleButton5_Click(sender,e);
            txt_ad.Text = "";
            txt_soyad.Text = "";
            txt_yakinlik.Text = "";
            cmb_nerden.Text = "";
            cmb_sayi.Text = "0";
            cmb_mtur.Text = "";
            txt_mbilgi.Text = "";
            txt_mucret.Text = "0";
            txt_bakiye.Text = "0";
        }

        private void txt_ad_EditValueChanged(object sender, EventArgs e)
        {
            try
            {


                string aranan = txt_ad.Text;
                DataTable dt = new DataTable();
                DataTable dtResult = new DataTable();
                dt = (DataTable)gridControl2.DataSource;
                dtResult = dt.Select("d_adi LIKE '%" + aranan + "%'").CopyToDataTable();
                
                gridControl3.DataSource = dtResult;



                Font F = new Font("Tahoma", 10, FontStyle.Bold);
                i_.sutisdeg(gridView3, "ID", "d_id", 0, false, true, 0, F, 10);
                i_.sutisdeg(gridView3, "Adı", "d_adi", 1, true, true, 0, F, 10);
                i_.sutisdeg(gridView3, "Soyadı", "d_soyad", 2, false, true, 0, F, 10);
                i_.sutisdeg(gridView3, "Yanında Gelecek Sayısı", "d_sayi", 3, false, true, 0, F, 10);
                i_.sutisdeg(gridView3, "Yakınlık", "d_yakinlik", 4, false, true, 0, F, 10);
                i_.sutisdeg(gridView3, "Nereden", "d_nereden", 5, false, true, 0, F, 10);



                if (aranan.Length>0)
                {
                    panelControl1.Visible = true;
                }
                else
                {
                    panelControl1.Visible = false;

                }

            }
            catch (Exception)
            {
                                
            }

           

        }

        private void simpleButton4_Click(object sender, EventArgs e)
        {

            int secilisatir = gridView2.FocusedRowHandle ;
            string sq;
            DataTable dt = new DataTable();
            dt = (DataTable)gridControl2.DataSource;
            sq = "update tbl_davetli set d_adi='"+txt_ad.Text+"' , d_soyad='"+txt_soyad.Text+"', d_sayi="+cmb_sayi.SelectedIndex+", " +
                "d_yakinlik='"+txt_yakinlik.Text+"', d_nereden='"+cmb_nerden.Text+ "' where d_id='" + i_.dtcek_Int(dt,gridView2.FocusedRowHandle,"d_id")+"'";

            DialogResult = MessageBox.Show(i_.dtcek_String(dt, gridView2.FocusedRowHandle, "d_adi") +" isimli davetli güncellensin mi?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (DialogResult  == DialogResult.Yes)
            {
                if (i_.SQL_Yurut(sq, false))
                {
                    //MessageBox.Show("Güncelleme Başarılı", "Bildirim", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Güncelleme Başarısız", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                davetligetir();
                gridView2.FocusedColumn = gridView2.Columns[1];
                Thread.Sleep(200);
                gridView2.FocusedRowHandle = secilisatir;
                gridView2.FocusRectStyle = DrawFocusRectStyle.RowFocus;
                gridView2.ShowEditor();
                gridView2.SelectRow(secilisatir);
            }
            
        }

        private void gridControl2_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = (DataTable)gridControl2.DataSource;

            txt_ad.Text = i_.dtcek_String(dt, gridView2.FocusedRowHandle, "d_adi");
            txt_soyad.Text = i_.dtcek_String(dt, gridView2.FocusedRowHandle, "d_soyad");
            cmb_sayi.Text = i_.dtcek_String(dt, gridView2.FocusedRowHandle, "d_sayi");
            txt_yakinlik.Text = i_.dtcek_String(dt, gridView2.FocusedRowHandle, "d_yakinlik");
            cmb_nerden.Text = i_.dtcek_String(dt, gridView2.FocusedRowHandle, "d_nereden");
        }

        private void simpleButton5_Click(object sender, EventArgs e)
        {
            txt_ad.Text = "";
            txt_soyad.Text = "";
            txt_yakinlik.Text = "";
            cmb_nerden.Text = "";
            cmb_sayi.Text = "0";
            cmb_mtur.Text = "";
            txt_mbilgi.Text = "";
            txt_mucret.Text = "0";
            txt_bakiye.Text = "0";
        }

        private void txt_ad_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode==Keys.F5)
            {
                btn_kaydet.PerformClick();
            }
            if (e.KeyCode == Keys.F2)
            {
                btn_temizle.PerformClick();
            }
            if (e.KeyCode == Keys.Enter)
            {
                cmb_sayi.Focus();
            }
            if (e.KeyCode == Keys.F1)
            {
                btn_guncelle.PerformClick();
            }
        }

        private void txt_soyad_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                btn_kaydet.PerformClick();
            }
            if (e.KeyCode == Keys.F2)
            {
                btn_temizle.PerformClick();
            }
            if (e.KeyCode == Keys.F1)
            {
                btn_guncelle.PerformClick();
            }
        }

        private void cmb_sayi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                btn_kaydet.PerformClick();
            }
            if (e.KeyCode == Keys.F2)
            {
                btn_temizle.PerformClick();
            }
            if (e.KeyCode == Keys.Enter)
            {
                cmb_nerden.Focus();
            }
            if (e.KeyCode == Keys.F1)
            {
                btn_guncelle.PerformClick();
            }
        }

        private void txt_yakinlik_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                btn_kaydet.PerformClick();
            }
            if (e.KeyCode == Keys.F2)
            {
                btn_temizle.PerformClick();
            }
            if (e.KeyCode == Keys.F1)
            {
                btn_guncelle.PerformClick();
            }
        }

        private void cmb_nerden_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                btn_kaydet.PerformClick();
            }
            if (e.KeyCode == Keys.F2)
            {
                btn_temizle.PerformClick();
            }
            if (e.KeyCode == Keys.Enter)
            {
                btn_kaydet.PerformClick();
            }
            if (e.KeyCode == Keys.F1)
            {
                btn_guncelle.PerformClick();
            }
        }

       

        private void btn_mkaydet_Click(object sender, EventArgs e)
        {
            if (i_.txt_ara_int(txt_mucret.Text, ".") > 1)
            {
                txt_mucret.Text = "0";
                Thread.Sleep(1000);
                MessageBox.Show("Sadece para birimi yazabilirsiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            txt_mucret.Text= txt_mucret.Text.Replace(",", ".");



            string sq;
                sq = "insert into tbl_masraf (m_turu,m_bilgi,m_ucret)" +
               "values ('" + cmb_mtur.Text + "','" + txt_mbilgi.Text + "'," + txt_mucret.Text + ")";
                i_.SQL_Yurut(sq, false);


            bakiyehesapla();
            DataTable dt1 = new DataTable();
            dt1 = (DataTable)gridControl1.DataSource;
            gridView1.FocusedColumn = gridView1.Columns[1];
            Thread.Sleep(200);
            gridView1.FocusedRowHandle = dt1.Rows.Count - 1;
            gridView1.FocusRectStyle = DrawFocusRectStyle.RowFocus;
            gridView1.ShowEditor();
            gridView1.SelectRow(dt1.Rows.Count - 1);
        }

        private void txt_mucret_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.';
        }

        private void xtraTabControl1_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabControl1.SelectedTabPage == xtraTabPage1)
            {
                xtraTabControl2.SelectedTabPage = xtraTabPage3;
                
            }
            if (xtraTabControl1.SelectedTabPage == xtraTabPage2)
            {
                xtraTabControl2.SelectedTabPage = xtraTabPage4;
            }
            if (xtraTabControl1.SelectedTabPage == xtraTabPage6)
            {
                xtraTabControl2.SelectedTabPage = xtraTabPage5;
            }
            if (izin_ == false)
            {
                xtraTabPage7.PageVisible = false;
            }
        }

        private void xtraTabControl2_SelectedPageChanged(object sender, DevExpress.XtraTab.TabPageChangedEventArgs e)
        {
            if (xtraTabControl2.SelectedTabPage == xtraTabPage3)
            {
                xtraTabControl1.SelectedTabPage = xtraTabPage1;
            }
            if (xtraTabControl2.SelectedTabPage == xtraTabPage4)
            {
                xtraTabControl1.SelectedTabPage = xtraTabPage2;
            }
            if (xtraTabControl2.SelectedTabPage == xtraTabPage5)
            {
                xtraTabControl1.SelectedTabPage = xtraTabPage6;
            }
            if (izin_==false)
            {
                xtraTabPage7.PageVisible = false;
            }
            
        }

        private void txt_mucret_EditValueChanged(object sender, EventArgs e)
        {
           
        }

        private void gridControl1_Click(object sender, EventArgs e)
        {
            DataTable dt = new DataTable();
            dt = (DataTable)gridControl1.DataSource;

            cmb_mtur.Text = i_.dtcek_String(dt, gridView1.FocusedRowHandle, "m_turu");
            txt_mbilgi.Text = i_.dtcek_String(dt, gridView1.FocusedRowHandle, "m_bilgi");
            txt_mucret.Text = i_.dtcek_String(dt, gridView1.FocusedRowHandle, "m_ucret");
            
        }

        private void cmb_mtur_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                btn_mkaydet.PerformClick();
            }
            if (e.KeyCode == Keys.F2)
            {
                btn_yenile.PerformClick();
            }
            if (e.KeyCode == Keys.Enter)
            {
                txt_mbilgi.Focus();
            }
            if (e.KeyCode == Keys.F1)
            {
                btn_mguncelle.PerformClick();
            }
        }

        private void txt_mbilgi_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                btn_mkaydet.PerformClick();
            }
            if (e.KeyCode == Keys.F2)
            {
                btn_yenile.PerformClick();
            }
            if (e.KeyCode == Keys.Enter)
            {
                txt_mucret.Focus();
            }
            if (e.KeyCode == Keys.F1)
            {
                btn_mguncelle.PerformClick();
            }
        }

        private void txt_mucret_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                btn_mkaydet.PerformClick();
            }
            if (e.KeyCode == Keys.F2)
            {
                btn_yenile.PerformClick();
            }
            if (e.KeyCode == Keys.Enter)
            {
                btn_mkaydet.PerformClick();
            }
            if (e.KeyCode == Keys.F1)
            {
                btn_mguncelle.PerformClick();
            }
        }

        private void btn_mguncelle_Click(object sender, EventArgs e)
        {

            if (i_.txt_ara_int(txt_mucret.Text, ".") > 1)
            {
                txt_mucret.Text = "0";
                Thread.Sleep(1000);
                MessageBox.Show("Sadece para birimi yazabilirsiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            txt_mucret.Text = txt_mucret.Text.Replace(",", ".");


            int secilisatir = gridView1.FocusedRowHandle;
            string sq;
            DataTable dt = new DataTable();
            dt = (DataTable)gridControl1.DataSource;
            sq = "update tbl_masraf set m_turu='" + cmb_mtur.Text + "' , m_bilgi='" + txt_mbilgi.Text + "', m_ucret=" + txt_mucret.Text + "" +
                " where m_id='" + i_.dtcek_Int(dt, gridView1.FocusedRowHandle, "m_id") + "'";

            DialogResult = MessageBox.Show(i_.dtcek_String(dt, gridView1.FocusedRowHandle, "m_bilgi") + " bilgili masraf güncellensin mi?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (DialogResult == DialogResult.Yes)
            {
                if (i_.SQL_Yurut(sq, false))
                {
                    //MessageBox.Show("Güncelleme Başarılı", "Bildirim", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Güncelleme Başarısız", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                bakiyehesapla();
                gridView1.FocusedColumn = gridView1.Columns[1];
                Thread.Sleep(200);
                gridView1.FocusedRowHandle = secilisatir;
                gridView1.FocusRectStyle = DrawFocusRectStyle.RowFocus;
                gridView1.ShowEditor();
                gridView1.SelectRow(secilisatir);
            }

        }

        private void btn_msil_Click(object sender, EventArgs e)
        {
            int secilisatir = gridView1.FocusedRowHandle - 1;
            DataTable dt = new DataTable();
            dt = (DataTable)gridControl1.DataSource;
            string sq = "delete from tbl_masraf where m_id=" + i_.dtcek_Int(dt, gridView1.FocusedRowHandle, "m_id") + "";
            DialogResult = MessageBox.Show(i_.dtcek_String(dt, gridView1.FocusedRowHandle, "m_bilgi") + " bilgili masraf silinsin mi?", "Uyarı", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (DialogResult == DialogResult.Yes)
            {
                if (i_.SQL_Yurut(sq, false))
                {
                    //MessageBox.Show("Silme Başarılı", "Bildirim", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Silme Başarısız", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                bakiyehesapla();
                gridView1.FocusedColumn = gridView1.Columns[1];
                Thread.Sleep(200);
                gridView1.FocusedRowHandle = secilisatir;
                gridView1.FocusRectStyle = DrawFocusRectStyle.RowFocus;
                gridView1.ShowEditor();
                gridView1.SelectRow(secilisatir);
            }

            
        }

        private void btn_bsil_Click(object sender, EventArgs e)
        {
            int secilisatir = gridView4.FocusedRowHandle - 1;
            DataTable dt = new DataTable();
            dt = (DataTable)gridControl4.DataSource;
            string sq = "delete from tbl_bakiye where b_id=" + i_.dtcek_Int(dt, gridView4.FocusedRowHandle, "b_id") + "";
           
                if (i_.SQL_Yurut(sq, false))
                {
                    //MessageBox.Show("Silme Başarılı", "Bildirim", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    MessageBox.Show("Silme Başarısız", "Hata", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            bakiyehesapla();
            gridView4.FocusedColumn = gridView4.Columns[1];
            Thread.Sleep(200);
            gridView4.FocusedRowHandle = secilisatir;
            gridView4.FocusRectStyle = DrawFocusRectStyle.RowFocus;
            gridView4.ShowEditor();
            gridView4.SelectRow(secilisatir);
            }

        private void btn_bkaydet_Click(object sender, EventArgs e)
        {
            if (i_.txt_ara_int(txt_bakiye.Text, ".") > 1)
            {
                txt_bakiye.Text = "0";
                Thread.Sleep(1000);
                MessageBox.Show("Sadece para birimi yazabilirsiniz!", "Uyarı", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            txt_bakiye.Text = txt_bakiye.Text.Replace(",", ".");



            string sq;
            sq = "insert into tbl_bakiye (b_ucret)" +
           "values (" + txt_bakiye.Text + ")";
            i_.SQL_Yurut(sq, false);


            bakiyehesapla();
            DataTable dt1 = new DataTable();
            dt1 = (DataTable)gridControl4.DataSource;
            gridView4.FocusedColumn = gridView4.Columns[1];
            Thread.Sleep(200);
            gridView4.FocusedRowHandle = dt1.Rows.Count - 1;
            gridView4.FocusRectStyle = DrawFocusRectStyle.RowFocus;
            gridView4.ShowEditor();
            gridView4.SelectRow(dt1.Rows.Count - 1);
        }

        private void txt_bakiye_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar) && e.KeyChar != '.' && e.KeyChar != '-' && e.KeyChar != '+';
        }

        private void txt_bakiye_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F5)
            {
                btn_bkaydet.PerformClick();
            }
            if (e.KeyCode == Keys.F2)
            {
                btn_yenile.PerformClick();
            }
            if (e.KeyCode == Keys.Enter)
            {
                btn_bkaydet.PerformClick();
            }           
        }

        private void cmb_sayi_KeyPress(object sender, KeyPressEventArgs e)
        {
            e.Handled = !char.IsDigit(e.KeyChar) && !char.IsControl(e.KeyChar);
        }

        private void gridControl1_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                btn_msil.PerformClick();
            }
        }

        private void gridControl4_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Delete)
            {
                btn_bsil.PerformClick();
            }
        }
        bool izin_ = false;
        private void btn_yardım_Click(object sender, EventArgs e)
        {
            xtraTabPage7.PageVisible = true;
            izin_ = true;
            xtraTabControl2.SelectedTabPage = xtraTabPage7;
            izin_ = false;
        }

        private void xtraTabPage7_Click(object sender, EventArgs e)
        {
            //Process.Start("https://www.youtube.com/channel/UCIRx9CWQCMgaM20M11OWvew?sub_confirmation=1");
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            Process.Start("https://www.youtube.com/channel/UCIRx9CWQCMgaM20M11OWvew?sub_confirmation=1");
        }

        //ayselin kayınbabası
    }
}
