﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SQL_Project
{
    public partial class frmServis : Form
    {
        private SqlConnection baglanti;
        public long perno;
        private long musNo;
        public frmServis(SqlConnection baglanti)
        {
            this.baglanti = baglanti;
            InitializeComponent();
        }

        private void btnTCNoDoldur_Click(object sender, EventArgs e)
        {
            String komut = "SELECT musNo, ad, soyad, telefon, eposta, adres FROM musteri WHERE tckNo ='" + tbMusteriTCNo.Text + "'";
            SqlDataAdapter sqlDA = new SqlDataAdapter(komut, baglanti);
            DataSet DS = new DataSet();
            sqlDA.Fill(DS);

            if (DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
            {
                musNo = Int64.Parse(DS.Tables[0].Rows[0][0].ToString());
                tbAd.Text = DS.Tables[0].Rows[0][1].ToString();
                tbSoyad.Text = DS.Tables[0].Rows[0][2].ToString();
                tbTelNo.Text = DS.Tables[0].Rows[0][3].ToString();
                tbEPosta.Text = DS.Tables[0].Rows[0][4].ToString();
                tbAdres.Text = DS.Tables[0].Rows[0][5].ToString();
            }
            else
            {
                musNo = 0;
                tbAd.Text = "";
                tbSoyad.Text = "";
                tbTelNo.Text = "";
                tbEPosta.Text = "";
                tbAdres.Text = "";
            }
        }

        private void btnSasiNoDoldur_Click(object sender, EventArgs e)
        {
            String komut = "SELECT motorNo, plaka, marka, model, renk FROM araba WHERE sasiNo ='" + tbSasiNo.Text + "'";
            SqlDataAdapter sqlDA = new SqlDataAdapter(komut, baglanti);
            DataSet DS = new DataSet();
            sqlDA.Fill(DS);

            if (DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
            {
                tbMotorNo.Text = DS.Tables[0].Rows[0][0].ToString();
                tbPlaka.Text = DS.Tables[0].Rows[0][1].ToString();
                tbMarka.Text = DS.Tables[0].Rows[0][2].ToString();
                tbModel.Text = DS.Tables[0].Rows[0][3].ToString();
                tbRenk.Text = DS.Tables[0].Rows[0][4].ToString();
            }
            else
            {
                tbMotorNo.Text = "";
                tbPlaka.Text = "";
                tbMarka.Text = "";
                tbModel.Text = "";
                tbRenk.Text = "";
            }
        }

        private void btnIsEmriDoldur_Click(object sender, EventArgs e)
        {
            if (tbIsEmriNo.Text.Count() > 0)
            {
                String komut = "SELECT A.sasiNo, M.tckNo, " +
                               "S.girisTarihi, S.girisTalimati, S.aracKm, S.yapilanIslemler, " +
                               "S.cikisTarihi, S.faturaTutari FROM araba A " +
                               "JOIN servis S on S.sasiNo = A.sasiNo " +
                               "JOIN musteri M on S.musNo = M.musNo " +
                               " WHERE isEmriNo =" + tbIsEmriNo.Text;
                SqlDataAdapter sqlDA = new SqlDataAdapter(komut, baglanti);
                DataSet DS = new DataSet();
                sqlDA.Fill(DS);

                if (DS.Tables.Count > 0 && DS.Tables[0].Rows.Count > 0)
                {
                    tbSasiNo.Text = DS.Tables[0].Rows[0][0].ToString();
                    tbMusteriTCNo.Text = DS.Tables[0].Rows[0][1].ToString();
                    dtGiris.Text = DS.Tables[0].Rows[0][2].ToString();
                    tbGirisTalimat.Text = DS.Tables[0].Rows[0][3].ToString();
                    tbArackm.Text = DS.Tables[0].Rows[0][4].ToString();
                    tbYapilanIslemler.Text = DS.Tables[0].Rows[0][5].ToString();
                    dtCikis.Text = DS.Tables[0].Rows[0][6].ToString();
                    tbFaturaTutari.Text = DS.Tables[0].Rows[0][7].ToString();
                    btnTCNoDoldur_Click(sender, e);
                    btnSasiNoDoldur_Click(sender, e);
                    fillParcalar();
                }
                else
                {
                    btnTemizle_Click(sender, e);
                }
            }
        }

        private void btnTemizle_Click(object sender, EventArgs e)
        {
            tbSasiNo.Text = "";
            tbMotorNo.Text = "";
            tbPlaka.Text = "";
            tbMarka.Text = "";
            tbModel.Text = "";
            tbRenk.Text = "";
            tbMusteriTCNo.Text = "";
            tbAd.Text = "";
            tbSoyad.Text = "";
            tbTelNo.Text = "";
            tbEPosta.Text = "";
            tbAdres.Text = "";
            tbIsEmriNo.Text = "";
            dtGiris.ResetText();
            tbGirisTalimat.Text = "";
            tbArackm.Text = "";
            tbYapilanIslemler.Text = "";
            dtCikis.ResetText();
            tbFaturaTutari.Text = "";
            musNo = 0;
        }

        private void btnCik_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void btnParcaCikar_Click(object sender, EventArgs e)
        {
            if (tbIsEmriNo.Text.Count() > 0 && dgParcalar.SelectedRows.Count > 0)
            {
                String parcaKodu = dgParcalar.SelectedRows[0].Cells[0].Value.ToString();
                String komut = "DELETE FROM degisim WHERE isEmriNo = " + tbIsEmriNo.Text + " AND parcaKodu = '" + parcaKodu + "'";
                SqlCommand sorgu = new SqlCommand(komut, baglanti);
                sorgu.ExecuteNonQuery();
                fillParcalar();
            }
            
            //lbParcalar.Items.Remove(lbParcalar.SelectedItems);
        }

        private void btnIsEmriAc_Click(object sender, EventArgs e)
        {
            if (musNo != 0)
            {
                String komut = "INSERT INTO servis " +
                    " (girisTarihi, sasiNo, musNo, perNo, girisTalimati, aracKm) VALUES " +
                    " ('" + dtGiris.Value.ToString("yyyy-MM-dd HH:mm:ss") + "', '" + tbSasiNo.Text + "', " +
                    musNo + ", " + perno + ", '" + tbGirisTalimat.Text + "', " +
                    tbArackm.Text + ") SELECT SCOPE_IDENTITY()";
                SqlCommand sorgu = new SqlCommand(komut, baglanti);
                tbIsEmriNo.Text = Convert.ToInt64(sorgu.ExecuteScalar()).ToString();
            }
        }

        private void btnGuncelle_Click(object sender, EventArgs e)
        {
            if (tbIsEmriNo.Text.Count() > 0)
            {
                String komut = "UPDATE servis SET " +
                    " girisTarihi='" + dtGiris.Value.ToString("yyyy-MM-dd HH:mm:ss") +
                    "', sasiNo = '" + tbSasiNo.Text + "', musNo = " + musNo + ", perNo = " + perno +
                    ", girisTalimati = '" + tbGirisTalimat.Text + "', aracKm = " + tbArackm.Text +
                    ", cikisTarihi = '" + dtGiris.Value.ToString("yyyy-MM-dd HH:mm:ss") + "', " +
                    "yapilanIslemler = '" + tbYapilanIslemler.Text + "', " +
                    "faturaTutari = " + tbFaturaTutari.Text;
                SqlCommand sorgu = new SqlCommand(komut, baglanti);
                sorgu.ExecuteNonQuery();
                //tbIsEmriNo.Text = Convert.ToInt64(sorgu.ExecuteScalar()).ToString();
            }
        }

        private void btnParcaEkle_Click(object sender, EventArgs e)
        {
            if (tbIsEmriNo.Text.Count() > 0)
            {
                frmParcalar parcalarForm = new frmParcalar(baglanti);
                parcalarForm.isEmriNo = Convert.ToInt64(tbIsEmriNo.Text);
                parcalarForm.setEnabledBtnEkle(true);
                parcalarForm.ShowDialog();
                fillParcalar();
            }
        }

        private void fillParcalar()
        {
            if (tbIsEmriNo.Text.Count() > 0)
            {
                String komut = "SELECT P.parcaKodu, P.parcaAdi, count(D.degisimNo) as adet, P.parcaIscilik, P.parcaTutari " +
                    "FROM degisim D JOIN parca P on D.parcaKodu = P.parcaKodu WHERE D.isEmriNo = " + tbIsEmriNo.Text +
                    "GROUP BY P.parcaKodu, P.parcaAdi, P.parcaIscilik, P.parcaTutari";
                SqlDataAdapter sqlDA = new SqlDataAdapter(komut, baglanti);
                DataSet DS = new DataSet();
                sqlDA.Fill(DS);
                dgParcalar.DataSource = DS.Tables[0];
            }
            btnFaturaHesapla_Click(this, new EventArgs());
        }

        private void btnFaturaHesapla_Click(object sender, EventArgs e)
        {
            double Tutar = 0;
            for (int i = 0; i < dgParcalar.RowCount; i++)
            {
                int adet = int.Parse(dgParcalar.Rows[i].Cells[2].Value.ToString());
                double iscilik = Convert.ToDouble(dgParcalar.Rows[i].Cells[3].Value);
                double parcaTutari = Convert.ToDouble(dgParcalar.Rows[i].Cells[4].Value);

                Tutar += adet * parcaTutari + iscilik * 100;
            }
            tbFaturaTutari.Text = Tutar.ToString();
        }
    }
}
