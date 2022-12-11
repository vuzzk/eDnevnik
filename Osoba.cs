using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Data.SqlClient;

namespace eDnevnik
{
    public partial class Osoba : Form
    {
        int broj_sloga = 0;
        DataTable tabela;

        public Osoba()
        {
            InitializeComponent();
        }

        private void Osoba_Load(object sender, EventArgs e)
        {
            tabela = new DataTable();
            SqlConnection veza = konekcija.connect();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Osoba", veza);
            adapter.Fill(tabela);
            Populate();
        }

        //Prikaz podataka iz tabele
        private void Populate()
        {
            if (tabela.Rows.Count == 0)
            {
                textBoxID.Text = "";
                textBoxIme.Text = "";
                textBoxPrezime.Text = "";
                textBoxAdresa.Text = "";
                textBoxJMBG.Text = "";
                textBoxMejl.Text = "";
            }
            else
            {
                textBoxID.Text = tabela.Rows[broj_sloga][0].ToString();
                textBoxIme.Text = tabela.Rows[broj_sloga][1].ToString();
                textBoxPrezime.Text = tabela.Rows[broj_sloga][2].ToString();
                textBoxAdresa.Text = tabela.Rows[broj_sloga][3].ToString();
                textBoxJMBG.Text = tabela.Rows[broj_sloga][4].ToString(); 
                textBoxMejl.Text = tabela.Rows[broj_sloga][5].ToString(); 
                if (broj_sloga == tabela.Rows.Count - 1)
                {
                    buttonForward.Enabled = false;
                    buttonEnd.Enabled = false;
                }
                else
                {
                    buttonForward.Enabled = true;
                    buttonEnd.Enabled = true;
                }
                if (broj_sloga == 0)
                {
                    buttonBackward.Enabled = false;
                    buttonBegin.Enabled = false;
                }
                else
                {
                    buttonBackward.Enabled = true;
                    buttonBegin.Enabled = true;
                }

            }
        }

        //Dugmici za navigaciju po tabeli
        private void buttonBegin_Click(object sender, EventArgs e)
        {
            broj_sloga = 0;
            Populate();
        }

        private void buttonBackward_Click(object sender, EventArgs e)
        {
            broj_sloga--;
            Populate();
        }

        private void buttonForward_Click(object sender, EventArgs e)
        {
            broj_sloga++;
            Populate();
        }

        private void buttonEnd_Click(object sender, EventArgs e)
        {
            broj_sloga = tabela.Rows.Count - 1;
            Populate();
        }

        //Dugmici za manipulisanje podacima
        private void buttonInsert_Click(object sender, EventArgs e)
        {
            int uloga = comboBoxUloga.SelectedIndex++;
            string naredba = "INSERT INTO osoba VALUES('";
            naredba = naredba + textBoxIme.Text + "','";
            naredba = naredba + textBoxPrezime.Text + "','";
            naredba = naredba + textBoxAdresa.Text + "','";
            naredba = naredba + textBoxJMBG.Text + "','";
            naredba = naredba + textBoxMejl.Text + "','";
            naredba = naredba + textBoxPassword.Text + "',";
            naredba = naredba + comboBoxUloga.SelectedIndex + ")";
            textBoxCommand.Text = naredba;
            SqlConnection veza = konekcija.connect();
            SqlCommand komanda = new SqlCommand(naredba, veza);
            try
            {
                veza.Open();
                komanda.ExecuteNonQuery();
                veza.Close();
            }
            catch (Exception greska) { MessageBox.Show(greska.GetType().ToString(), "Pretraga", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM osoba", veza);
            tabela = new DataTable();
            adapter.Fill(tabela);
            Populate();
            labelUspesno.Visible = true;
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {

        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {

        }

        //Pretraga podataka
        int katPretrage = 0;
        string s;
        public void Search(string s, int kat)
        {
            for (int i = 0; i < tabela.Rows.Count; i++)
            {
                if(s == tabela.Rows[i][kat].ToString())
                {
                    broj_sloga = i;
                    Populate();
                    break;
                }
                else
                {
                    MessageBox.Show("Ova osoba ne postoji u bazi!", "Pretraga", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    break;
                }
            }
        }
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            s = textBoxSearch.Text;
            if (s == "")
            {
                MessageBox.Show("Morate uneti tekst u pretragu!", "Greška!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else
            {
                Search(s, katPretrage);
            }
        }
        private void comboBoxSearchBy_SelectedIndexChanged(object sender, EventArgs e)
        {
            katPretrage = comboBoxSearchBy.SelectedIndex;
        }

        //Pomoc
        private void buttonHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("-Pretraga se vrši klikom na lupu.", "Pomoć", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

    }
}
