using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Oracle.DataAccess.Client;

namespace M5_220180519
{
    public partial class Form2 : Form
    {
        OracleConnection conn;
        DataTable dtBahan;
        OracleDataAdapter da;
        OracleCommand cmd;
        int selectedIndex = -1;
        public Form2()
        {
            InitializeComponent();
            this.Text = "MasterRoti";
            conn = Form1.conn;
            loadData();
        }

        public void loadData()
        {
            dtBahan = new DataTable();
            da = new OracleDataAdapter();
            cmd = new OracleCommand();

            cmd.Connection = conn;
            cmd.CommandText = "select r.id, r.kode as \"Kode Roti\", r.nama as \"Nama Roti\", r.deskripsi as \"Deskripsi Roti\", 'Rp.' || r.harga as \"Harga Roti\", r.stok || ' pcs' as \"Stok Roti\", case when r.status = 0 then :id else :nama end as \"Status Roti\", j.nama_jenis as \"Jenis Roti\" from roti r, jenis_roti j where r.jenis_roti = j.id";
            cmd.Parameters.Add(":id", "Tidak Tersedia");
            cmd.Parameters.Add(":nama", "Tersedia");

            conn.Open();
            cmd.ExecuteReader();
            da.SelectCommand = cmd;
            da.Fill(dtBahan);
            dgvData.DataSource = dtBahan;
            dgvData.Columns[0].Visible = false;

            conn.Close();

            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void textBox4_TextChanged(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            cmd.CommandText = "select max(id) from roti";
            conn.Open();
            int jmlh = Int32.Parse(cmd.ExecuteScalar().ToString());
            conn.Close();
            int id = jmlh + 1;
            string nama = tbNama.Text;
            string des = tbDeskripsi.Text;
            int harga = Int32.Parse(tbHarga.Text);
            int stok = Int32.Parse(tbStok.Text);
            int status = 0;
            if (rbTersedia.Checked)
            {
                status = 1;
            }
            else if (rbTidak.Checked)
            {
                status = 0;
            }

            int jenis = 0;
            if (tbJenis.Text == "BREAD")
            {
                jenis = 1;
            }
            else if (tbJenis.Text == "TOAST")
            {
                jenis = 2;
            }
            else if (tbJenis.Text == "FLAT BREAD")
            {
                jenis = 3;
            }
            else if (tbJenis.Text == "DRY CAKE")
            {
                jenis = 4;
            }
            else if (tbJenis.Text == "COOKIES")
            {
                jenis = 5;
            }
            else if (tbJenis.Text == "WAFFLE")
            {
                jenis = 6;
            }
            else if (tbJenis.Text == "PANCAKE")
            {
                jenis = 7;
            }

            string[] name = new string[5];
            string b = tbNama.Text;
            name = b.Split(' ');
            string kode = " ";
            if (b != "" && des != "" && harga >= 1 && stok >= 1)
            {
                if (name.Length < 2)
                {
                    //1 kata
                    if (name[0].Length >= 5)
                    {
                        kode = name[0].Substring(0, 4).ToUpper() + "0000" + id;

                        cmd = new OracleCommand();
                        cmd.Connection = conn;
                        cmd.CommandText = "insert into roti values(:id, :kode,:nama, :des,:harga,:stok,:status,:jenis, :fk)";
                        cmd.Parameters.Add(":id", id);
                        cmd.Parameters.Add(":kode", kode);
                        cmd.Parameters.Add(":nama", nama);
                        cmd.Parameters.Add(":des", des);
                        cmd.Parameters.Add(":harga", harga);
                        cmd.Parameters.Add(":stok", stok);
                        cmd.Parameters.Add(":status", status);
                        cmd.Parameters.Add(":jenis", jenis);
                        cmd.Parameters.Add(":fk", 13);
                    }
                    else
                    {
                        MessageBox.Show("nama minimal 5 huruf");
                    }
                }
                else if (name.Length <= 2)
                {
                    //2 kata
                    if (name[0].Length >= 3 && name[1].Length >= 3)
                    {
                        kode = name[0].Substring(0, 2).ToUpper() + name[1].Substring(0, 2).ToUpper() + "0000" + id;

                        cmd = new OracleCommand();
                        cmd.Connection = conn;
                        cmd.CommandText = "insert into roti values(:id, :kode,:nama, :des,:harga,:stok,:status,:jenis, :fk)";
                        cmd.Parameters.Add(":id", id);
                        cmd.Parameters.Add(":kode", kode);
                        cmd.Parameters.Add(":nama", nama);
                        cmd.Parameters.Add(":des", des);
                        cmd.Parameters.Add(":harga", harga);
                        cmd.Parameters.Add(":stok", stok);
                        cmd.Parameters.Add(":status", status);
                        cmd.Parameters.Add(":jenis", jenis);
                        cmd.Parameters.Add(":fk", 13);
                    }
                    else
                    {
                        MessageBox.Show("nama minimal 3 huruf");
                    }
                }

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                loadData();
            }
            else
            {
                MessageBox.Show("Gagal Input Data");
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedIndex < 0)
            {
                MessageBox.Show("Belum ada data yang dipilih");
            }
            else
            {
                cmd = new OracleCommand();
                cmd.Connection = conn;
                cmd.CommandText = "delete from roti where id =:id";
                cmd.Parameters.Add(":id", dtBahan.Rows[selectedIndex][0].ToString());
                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                loadData();
                MessageBox.Show("Berhasil Delete");
            }
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbKode.Text = "";
            tbNama.Text = "";
            tbDeskripsi.Text = "";
            tbHarga.Text = "0";
            tbStok.Text = "0";
            tbJenis.Text = "";
            rbTersedia.Checked = false;
            rbTidak.Checked = false;
            button1.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedIndex < 0)
            {
                MessageBox.Show("Belum ada data yang dipilih");
            }
            else
            {
                string nama = tbNama.Text;
                string des = tbDeskripsi.Text;
                int harga = Int32.Parse(tbHarga.Text);
                int stok = Int32.Parse(tbStok.Text);
                int status = 0;
                if (rbTersedia.Checked)
                {
                    status = 1;
                }
                else if (rbTidak.Checked)
                {
                    status = 0;
                }

                int jenis = 0;
                if (tbJenis.Text == "BREAD")
                {
                    jenis = 1;
                }
                else if (tbJenis.Text == "TOAST")
                {
                    jenis = 2;
                }
                else if (tbJenis.Text == "FLAT BREAD")
                {
                    jenis = 3;
                }
                else if (tbJenis.Text == "DRY CAKE")
                {
                    jenis = 4;
                }
                else if (tbJenis.Text == "COOKIES")
                {
                    jenis = 5;
                }
                else if (tbJenis.Text == "WAFFLE")
                {
                    jenis = 6;
                }
                else if (tbJenis.Text == "PANCAKE")
                {
                    jenis = 7;
                }

                string id = dtBahan.Rows[selectedIndex][0].ToString();
                string[] name = new string[5];
                string b = tbNama.Text;
                name = b.Split(' ');
                string kode = " ";

                if (name.Length < 2)
                {
                    //1 kata
                    if (name[0].Length >= 5)
                    {
                        kode = name[0].Substring(0, 4).ToUpper() + "0000" + id;
                        MessageBox.Show(kode);

                        Console.WriteLine("ID : " + id + " , " + kode);
                        cmd = new OracleCommand();
                        cmd.Connection = conn;
                        cmd.CommandText = $"update roti set kode='{kode}', nama = '{nama}', deskripsi = '{des}', harga = '{harga}', stok = '{stok}', status = '{status}', jenis_roti = '{jenis}'  where id={Convert.ToInt32(id)}";
                    }
                    else
                    {
                        MessageBox.Show("nama minimal 5 huruf");
                    }
                }
                else if (name.Length <= 2)
                {
                    //2 kata
                    if (name[0].Length >= 3 && name[1].Length >= 3)
                    {
                        kode = name[0].Substring(0, 2).ToUpper() + name[1].Substring(0, 2).ToUpper() + "0000" + id;
                        MessageBox.Show(kode);

                        cmd = new OracleCommand();
                        cmd.Connection = conn;
                        cmd.CommandText = $"update roti set kode='{kode}', nama = '{nama}', deskripsi = '{des}', harga = '{harga}', stok = '{stok}', status = '{status}', jenis_roti = '{jenis}'  where id={Convert.ToInt32(id)}";
                    }
                    else
                    {
                        MessageBox.Show("nama minimal 3 huruf");
                    }
                } 

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();

                loadData();
                MessageBox.Show("Berhasil Update");
                selectedIndex = -1;
            }
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvData_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedIndex = dgvData.CurrentCell.RowIndex;

            tbKode.Text = dgvData.Rows[selectedIndex].Cells[1].Value.ToString();
            tbNama.Text = dgvData.Rows[selectedIndex].Cells[2].Value.ToString();
            tbDeskripsi.Text = dgvData.Rows[selectedIndex].Cells[3].Value.ToString();
            tbHarga.Text = dgvData.Rows[selectedIndex].Cells[4].Value.ToString().Remove(0, 3);
            tbStok.Text = dgvData.Rows[selectedIndex].Cells[5].Value.ToString().Trim(' ', 'p', 'c', 's');
            tbJenis.Text = dgvData.Rows[selectedIndex].Cells[7].Value.ToString();
            string cek = dgvData.Rows[selectedIndex].Cells[6].Value.ToString();
            if (cek == "Tersedia")
            {
                rbTersedia.Checked = true;
            }
            else if (cek == "Tidak Tersedia")
            {
                rbTidak.Checked = true;
            }

            btnUpdate.Enabled = true;
            btnDelete.Enabled = true;
            button1.Enabled = false;
        }
    }
}
