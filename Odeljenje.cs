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
    public partial class Odeljenje : Form
    {
        int broj_sloga = 0;
        DataTable tabela, tabelaNastavnici;

        public Odeljenje()
        {
            InitializeComponent();
        }

        private void Odeljenje_Load(object sender, EventArgs e)
        {
            tabela = new DataTable();
            tabelaNastavnici = new DataTable();
            SqlConnection veza = konekcija.connect();
            SqlConnection vezaN = konekcija.connect();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Odeljenje", veza);
            adapter.Fill(tabela);
            SqlDataAdapter adapterN = new SqlDataAdapter("SELECT * FROM Osoba WHERE uloga = 2", vezaN);
            adapterN.Fill(tabelaNastavnici);
            Populate();

            //Combobox razredni, svaki put po pokretanju programa dodace se novi nastavnici
            comboBoxRazredni.Items.Clear();
            for (int x = 0; x < tabelaNastavnici.Rows.Count; x++)
            {
                comboBoxRazredni.Items.Add(tabelaNastavnici.Rows[x][1].ToString() + " " + tabelaNastavnici.Rows[x][2].ToString());
            }
        }

        //Prikaz podataka iz tabele
        private void Populate()
        {
            int index;
            if (tabela.Rows.Count == 0)
            {
                textBoxID.Text = "";
                textBoxRazred.Text = "";
                textBoxIndeks.Text = "";
                comboBoxSmer.SelectedIndex = -1;
                comboBoxGodina.SelectedIndex = -1;
                comboBoxRazredni.SelectedIndex = -1;
            }
            else
            {
                textBoxID.Text = tabela.Rows[broj_sloga][0].ToString();
                textBoxRazred.Text = tabela.Rows[broj_sloga][1].ToString();
                textBoxIndeks.Text = tabela.Rows[broj_sloga][2].ToString();
                index = Convert.ToInt32(tabela.Rows[broj_sloga][3]) - 1;
                comboBoxSmer.SelectedIndex = index;
                //Nije gotovo!!!
                //index = Convert.ToInt32(tabela.Rows[broj_sloga][4]) - 1;
                //index = Convert.ToInt32(tabelaNastavnici.Rows[)
                //comboBoxRazredni.SelectedIndex = index;
                index = Convert.ToInt32(tabela.Rows[broj_sloga][5]) - 1;
                comboBoxGodina.SelectedIndex = index;
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

        //Dugmici za manipulisanje podacima, nisu gotovi
        private void buttonInsert_Click(object sender, EventArgs e)
        {
            string naredba = "INSERT INTO odeljenje VALUES('";
            naredba = naredba + "razred = '" + textBoxRazred.Text + "',";
            naredba = naredba + "indeks = '" + textBoxIndeks.Text + "',";
            naredba = naredba + "smer_id = '" + comboBoxSmer.SelectedItem.ToString() + "',";
            naredba = naredba + "razredni_id = '" + comboBoxRazredni.SelectedItem.ToString() + "',";
            naredba = naredba + "godina_id = '" + comboBoxGodina.SelectedItem.ToString() + "')";
            textBoxCommand.Text = naredba;
            SqlConnection veza = konekcija.connect();
            SqlCommand komanda = new SqlCommand(naredba, veza);
            try
            {
                veza.Open();
                komanda.ExecuteNonQuery();
                veza.Close();
            }
            catch (Exception greska) { MessageBox.Show(greska.GetType().ToString(), "Greska!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM odeljenje", veza);
            tabela = new DataTable();
            adapter.Fill(tabela);
            Populate();
            labelUspesno.Visible = true;
        }

        private void buttonUpdate_Click(object sender, EventArgs e)
        {
            string naredba = "UPDATE odeljenje SET ";
            naredba = naredba + "razred = '" + textBoxRazred.Text + "',";
            naredba = naredba + "indeks = '" + textBoxIndeks.Text + "',";
            naredba = naredba + "smer_id = '" + comboBoxSmer.SelectedItem.ToString() + "',"; 
            naredba = naredba + "razredni_id = '" + comboBoxRazredni.SelectedItem.ToString() + "',";
            naredba = naredba + "godina_id = '" + comboBoxGodina.SelectedItem.ToString() + "' ";
            naredba = naredba + "WHERE id=" + textBoxID.Text;
            textBoxCommand.Text = naredba;
            SqlConnection veza = konekcija.connect();
            SqlCommand komanda = new SqlCommand(naredba, veza);
            try
            {
                veza.Open();
                komanda.ExecuteNonQuery();
                veza.Close();
            }
            catch (Exception greska) { MessageBox.Show(greska.GetType().ToString(), "Greska!", MessageBoxButtons.OK, MessageBoxIcon.Error); }
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM odeljenje", veza);
            tabela = new DataTable();
            adapter.Fill(tabela);
            Populate();
            labelUspesno.Visible = true;
        }

        private void buttonDelete_Click(object sender, EventArgs e)
        {
            string naredba = "DELETE FROM odeljenje WHERE id=" + textBoxID.Text;
            textBoxCommand.Text = naredba;
            SqlConnection veza = konekcija.connect();
            SqlCommand komanda = new SqlCommand(naredba, veza);
            if (broj_sloga == tabela.Rows.Count - 1) broj_sloga--;
            if (broj_sloga < 0) broj_sloga = 0;
            veza.Open();
            komanda.ExecuteNonQuery();
            veza.Close();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM odeljenje", veza);
            tabela = new DataTable();
            adapter.Fill(tabela);
            Populate();
            labelUspesno.Visible = true;
        }

        //Pretraga odeljenja
        string p,q,s;
        public void Search(string p, string q)
        {
            int n = 0;
            int i = 0;
            for (i = 0; i < tabela.Rows.Count; i++)
            {
                if (p == tabela.Rows[i][1].ToString() && q == tabela.Rows[i][2].ToString())
                {
                    broj_sloga = i;
                    Populate();
                }
                else
                {
                    n++;
                }
            }
            if (n == i) MessageBox.Show("Ovo odeljenje ne postoji u bazi!", "Pretraga", MessageBoxButtons.OK, MessageBoxIcon.Warning); ;
        } //pretraga odeljenja po 'nazivu'
        public void SearchID(string s) //pretraga odeljenja po ID-u
        {
            int n = 0;
            int i = 0;
            for (i = 0; i < tabela.Rows.Count; i++)
            {
                if (s == tabela.Rows[i][0].ToString())
                {
                    broj_sloga = i;
                    Populate();
                }
                else
                {
                    n++;
                }
            }
            if (n == i) MessageBox.Show("Ovo odeljenje ne postoji u bazi!", "Pretraga", MessageBoxButtons.OK, MessageBoxIcon.Warning); ;
        }
        private void buttonSearch_Click(object sender, EventArgs e)
        {
            p = textBoxSR.Text;
            q = textBoxSI.Text;
            s = textBoxSearchID.Text;
            if (p == "" && q == "" && textBoxSearchID.Text == "")
            {
                MessageBox.Show("Morate uneti tekst u pretragu!", "Greška!", MessageBoxButtons.OK, MessageBoxIcon.Warning);
            }
            else if (p != "" && q != "" && textBoxSearchID.Text == "")
            {
                Search(p, q);
            }
            else if (p == "" && q == "" && textBoxSearchID.Text != "")
            {
                SearchID(s);
            }
            else
            {
                MessageBox.Show("Neispravno ste uneli parametre pretrage!", "Greška!", MessageBoxButtons.OK, MessageBoxIcon.Error);

            }
        }

        //Pomoc
        private void buttonHelp_Click(object sender, EventArgs e)
        {
            MessageBox.Show("-Pretraga se vrši klikom na lupu.", "Pomoć", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}
