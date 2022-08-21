using DevExpress.XtraGrid;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Dugun_Davetli_Masraf
{
    class islemler
    {

        public static SqlConnection con_ilk = new SqlConnection("Data Source=(localdb)\\DGN;");
        public static SqlConnection con = new SqlConnection("Data Source=(localdb)\\DGN;Integrated Security=SSPI;Initial Catalog=dugun");
        public string lclhost = "DGN";
        public string db = "dugun";



        public void Create_LocalDB(bool catch_)
        {
            try
            {
                string strCmdText;
                strCmdText = "/C sqllocaldb create " + lclhost + "";
                Process.Start("CMD.exe", strCmdText);
                strCmdText = "/C sqllocaldb start " + lclhost + "";
                Process.Start("CMD.exe", strCmdText);


            }
            catch (Exception ex)
            {
                if (catch_)
                {
                    MessageBox.Show("Hata : " + ex.Message.ToString(), "SST", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

            }
        }

        /// <summary>
        /// Burada programın veri tabanı oluşturuluyor
        /// Türkçe: Turkish_100_CI_AS
        /// İngilizce: bir şey yazmayınca default o oluyor
        /// </summary>

        /// <summary>
        /// Burası İnsert İnto yapar
        /// </summary>       
        /// <param name="con_">SQL Connection</param>
        /// <param name="SQ">SQL String</param>
        public void Create_DataBase(bool catch_)
        {



            try
            {
                SqlConnection cn = new SqlConnection("Data Source=(localdb)\\" + lclhost + ";");
                string SQ = "CREATE DATABASE " + db + "  ";
                cn.Close();
                cn.Open();
                SqlCommand cmd = new SqlCommand(SQ);
                cmd.Connection = cn;
                cmd.ExecuteNonQuery();
                cn.Close();
            }
            catch (Exception ex)
            {
                if (catch_)
                {
                    MessageBox.Show("Hata : " + ex.Message.ToString(), "SST", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }

            }
        }

        public  void SQL_Create_Table(string sq, bool catch_)
        {

            // buraya oluşturulacak tabloalr yazılacak
            try
            {
                //SqlConnection cn = new SqlConnection("Data Source=(localdb)\\" + parametreler_.lcl_ + ";Integrated Security=SSPI;Initial Catalog=" + parametreler_.db_ + "");
                //string SQ = "CREATE TABLE [dbo].[tbl_deneme12345]( " +
                //"[adi][nvarchar](50) NULL, [soyadi] [nvarchar](50) NULL ) ";                
                con.Close();
                con.Open();
                SqlCommand cmd = new SqlCommand(sq);
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                if (catch_)
                {
                    MessageBox.Show("Hata : " + ex.Message.ToString(), "SSTR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
            }
        }


        public void lokasyon()
        {
            //lokasyonlar////////////////////////////////////////////////////////
            //
            try
            {
                string sq;
                StreamReader sr = new StreamReader("addresses");
                sq = sr.ReadToEnd();
                SQL_Create_Table(sq, false);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Hata : " + ex.Message.ToString(), "SSTR", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);

            }

        }


        public  bool SQL_Cek(string SQ, DataTable DT_, bool catch_)
        {


            try
            {

                //SqlConnection cn = new SqlConnection("Data Source=(localdb)\\" + parametreler_.lcl_ + ";Integrated Security=SSPI;Initial Catalog=" + parametreler_.db_ + "");
                con.Close();
                con.Open();
                SqlCommand cmd = new SqlCommand(SQ);
                cmd.Connection = con;
                SqlDataReader dr = cmd.ExecuteReader();
                DT_.Load(dr);
                return true;
            }
            catch (Exception ex)
            {
                if (catch_)
                {
                    MessageBox.Show("Hata : " + ex.Message.ToString(), "SST", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                return false;
            }

        }

        public  string dtcek_String(DataTable dt_, int str, string stn)
        {
            try
            {
                string dgr = dt_.Rows[str][stn].ToString();
                return (string)dgr;
            }
            catch (Exception)
            {
                return (string)"";
            }
        }


        public  bool SQL_Yurut(string SQ, bool catch_)
        {
            try
            {
                //SqlConnection cn = new SqlConnection("Data Source=(localdb)\\" + parametreler_.lcl_ + ";Integrated Security=SSPI;Initial Catalog=" + parametreler_.db_ + "");
                con.Close();
                con.Open();
                SqlCommand cmd = new SqlCommand(SQ);
                cmd.Connection = con;
                cmd.ExecuteNonQuery();
                con.Close();
                return true;
            }
            catch (Exception ex)
            {
                if (catch_)
                {
                    MessageBox.Show("Hata : " + ex.Message.ToString(), "SST", MessageBoxButtons.OK, MessageBoxIcon.Exclamation);
                }
                return false;
            }

        }


        public  void sutisdeg(DevExpress.XtraGrid.Views.Grid.GridView header, string yeni, string eski, int sira, bool goster, bool saltokunur, int sortmode, Font f, int rowheight_)
        {
            try
            {
                header.Columns[eski].SortMode = (ColumnSortMode)sortmode;
                header.RowHeight = rowheight_;
                //header.OptionsBehavior.ReadOnly = true;
                if (saltokunur)
                {
                    header.Columns[eski].OptionsColumn.AllowEdit = false;
                }
                header.Columns[eski].AppearanceCell.Font = f;

                //header.Columns[eski].GroupIndex = sira;     guruplandırıyor bakılacak
                header.Columns[eski].VisibleIndex = sira;
                if (goster == false)
                {
                    header.Columns[eski].Visible = false;
                }
                else
                {
                    header.Columns[eski].Visible = true;
                }

                header.Columns[eski].Caption = yeni;

            }
            catch (Exception)
            {

            }


        }

        public  int dtcek_Int(DataTable dt_, int str, string stn)
        {
            try
            {
                int dgr = (int)dt_.Rows[str][stn];
                return (int)dgr;
            }
            catch (Exception ex)
            {
                return (int)0;
            }
        }

        public double dtcek_double(DataTable dt_, int str, string stn)
        {
            try
            {
                double dgr = (double)dt_.Rows[str][stn];
                return (double)dgr;
            }
            catch (Exception ex)
            {
                return (double)0;
            }
        }


        public int txt_ara_int(string deger1, string aranan)
        {
            int sonuc = 0;
            try
            {
                foreach (var item in deger1.ToString())
                {
                    if (aranan == item.ToString())
                    {
                        sonuc++;
                    }
                }


                return sonuc;
            }
            catch (Exception)
            {

                return 0;
            }
        }


    }
}
