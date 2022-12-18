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
        DataTable tabela;

        public Odeljenje()
        {
            InitializeComponent();
        }

        private void Odeljenje_Load(object sender, EventArgs e)
        {
            tabela = new DataTable();
            SqlConnection veza = konekcija.connect();
            SqlDataAdapter adapter = new SqlDataAdapter("SELECT * FROM Odeljenje", veza);
            adapter.Fill(tabela);
            Populate();
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
                index = Convert.ToInt32(tabela.Rows[broj_sloga][4]) - 1;
                comboBoxRazredni.SelectedIndex = index;
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
