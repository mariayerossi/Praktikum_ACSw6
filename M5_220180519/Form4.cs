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
    public partial class Form4 : Form
    {
        OracleConnection conn;
        DataTable dtBahan;
        OracleDataAdapter da;
        OracleCommand cmd;
        int selectedIndex = -1;
        public Form4()
        {
            InitializeComponent();
            this.Text = "MasterKaryawan";
            conn = Form1.conn;
            loadData();
        }

        public void loadData()
        {
            dtBahan = new DataTable();
            da = new OracleDataAdapter();
            cmd = new OracleCommand();

            cmd.Connection = conn;
            cmd.CommandText = "select k.id, k.kode as \"Kode\", k.username as \"Username\", k.nama as \"Nama\", case k.jenis_kelamin when 'L' then :laki else :perem end as \"Jenis Kelamin\", k.alamat, k.email as \"Email\", k.no_telp as \"No Telp\", initcap(j.nama_jabatan) as \"Jabatan\", to_char(k.tanggal_lahir, 'DD-MM-YYYY') as \"Tanggal Lahir\" from karyawan k, jabatan j where k.fk_jabatan = j.id order by k.nama asc";
            cmd.Parameters.Add(":laki", "Laki-Laki");
            cmd.Parameters.Add(":perem", "Perempuan");

            conn.Open();
            cmd.ExecuteReader();
            da.SelectCommand = cmd;
            da.Fill(dtBahan);
            dgvData.DataSource = dtBahan;
            dgvData.Columns[0].Visible = false;
            dgvData.Columns[5].Visible = false;

            conn.Close();

            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            btnClear.Enabled = false;
        }

        private void btnInsert_Click(object sender, EventArgs e)
        {
            string nama = tbNama.Text;
            string user = tbUser.Text;
            string jenis = "";
            if (rbLaki.Checked == true)
            {
                jenis = "L";
            }
            else if (rbPerempuan.Checked == true)
            {
                jenis = "P";
            }
            string alamat = tbAlamat.Text;
            string email = tbEmail.Text;
            string no = tbNo.Text;
            string[] tgl = dtpTgl.Value.ToString().Split(' ');
            int jabatan = 0;
            if (cbJabatan.Text == "Karyawan")
            {
                jabatan = 1;
            }
            else if (cbJabatan.Text == "Manager")
            {
                jabatan = 2;
            }
            else if (cbJabatan.Text == "Chef")
            {
                jabatan = 3;
            }

            if (nama != "" && user != "" && jenis != "" && alamat != ""  && email != "" && no != "" && cbJabatan.Text != "")
            {
                cmd.CommandText = "select max(id) from karyawan";
                conn.Open();
                int jmlh = Int32.Parse(cmd.ExecuteScalar().ToString());
                conn.Close();
                int id = jmlh + 1;

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
                        tbKode.Text = kode;

                        cmd = new OracleCommand();
                        cmd.Connection = conn;
                        cmd.CommandText = $"insert into karyawan values('{id}', '{kode}', '{user}', '{user}', '{nama}', '{jenis}','{alamat}','{email}','{no}', to_date('{tgl[0]}', 'dd-mm-yyyy'), 1, '{jabatan}')";
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
                        tbKode.Text = kode;

                        cmd = new OracleCommand();
                        cmd.Connection = conn;
                        cmd.CommandText = $"insert into karyawan values('{id}', '{kode}', '{user}', '{user}', '{nama}', '{jenis}','{alamat}','{email}','{no}', to_date('{tgl[0]}', 'dd-mm-yyyy'), 1, '{jabatan}')";
                    }
                    else
                    {
                        MessageBox.Show("nama minimal 3 huruf");
                    }
                }

                conn.Open();
                cmd.ExecuteNonQuery();
                conn.Close();
                MessageBox.Show("Berhasil Insert!");
                loadData();
            }
            else
            {
                if (nama == "")
                {
                    tbKode.Text = "-";
                    MessageBox.Show("inputan tidak boleh kosong");  
                }
            }
        }

        ComboBox cb = new ComboBox();
        ComboBox cb1 = new ComboBox();

        private void rbJenis_Click(object sender, EventArgs e)
        {
            cb1.Visible = false;
            tbCari.Visible = false;
            cb.Visible = true;
            cb.Items.Clear();
            cb.Items.Add("Laki-Laki");
            cb.Items.Add("Perempuan");
            cb.Location = new Point(105, 34);
            cb.Size = new Size(225, 100);
            cb.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Controls.Add(cb);
        }

        private void rbJabatan_Click(object sender, EventArgs e)
        {
            cb.Visible = false;
            tbCari.Visible = false;
            cb1.Visible = true;
            cb1.Items.Clear();
            cb1.Items.Add("Karyawan");
            cb1.Items.Add("Manager");
            cb1.Items.Add("Chef");
            cb1.Location = new Point(105, 34);
            cb1.Size = new Size(225, 100);
            cb1.DropDownStyle = ComboBoxStyle.DropDownList;
            this.Controls.Add(cb1);
        }

        private void rbUser_Click(object sender, EventArgs e)
        {
            cb1.Visible = false;
            cb.Visible = false;
            tbCari.Visible = true;
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            tbKode.Text = "";
            tbNama.Text = "";
            tbUser.Text = "";
            rbLaki.Checked = false;
            rbPerempuan.Checked = false;
            tbAlamat.Text = "";
            tbEmail.Text = "";
            tbNo.Text = "";
            cbJabatan.Text = "";
            tbCari.Text = "";
            btnInsert.Enabled = true;
            btnUpdate.Enabled = false;
            btnDelete.Enabled = false;
            loadData();
        }

        private void btnBack_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        private void dgvData_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            selectedIndex = dgvData.CurrentCell.RowIndex;

            tbKode.Text = dgvData.Rows[selectedIndex].Cells[1].Value.ToString();
            tbNama.Text = dgvData.Rows[selectedIndex].Cells[2].Value.ToString();
            tbUser.Text = dgvData.Rows[selectedIndex].Cells[3].Value.ToString();
            string cek = dgvData.Rows[selectedIndex].Cells[4].Value.ToString();
            if (cek == "Laki-Laki")
            {
                rbLaki.Checked = true;
            }
            else if (cek == "Perempuan")
            {
                rbPerempuan.Checked = true;
            }
            tbAlamat.Text = dgvData.Rows[selectedIndex].Cells[5].Value.ToString();
            tbEmail.Text = dgvData.Rows[selectedIndex].Cells[6].Value.ToString();
            tbNo.Text = dgvData.Rows[selectedIndex].Cells[7].Value.ToString();
            dtpTgl.Value = Convert.ToDateTime(dgvData.Rows[selectedIndex].Cells[9].Value);
            cbJabatan.Text = dgvData.Rows[selectedIndex].Cells[8].Value.ToString();
            btnDelete.Enabled = true;
            btnUpdate.Enabled = true;
            btnInsert.Enabled = false;
            btnClear.Enabled = true;
        }

        private void btnCari_Click(object sender, EventArgs e)
        {
            if (rbUser.Checked == true)
            {
                string cari = tbCari.Text;

                dtBahan.Rows.Clear();
                cmd.Connection = conn;
                cmd.CommandText = $"select k.id, k.kode as \"Kode\", k.username as \"Username\", k.nama as \"Nama\", case k.jenis_kelamin when 'L' then 'Laki-Laki' else 'Perempuan' end as \"Jenis Kelamin\", k.alamat, k.email as \"Email\", k.no_telp as \"No Telp\", initcap(j.nama_jabatan) as \"Jabatan\", to_char(k.tanggal_lahir, 'DD-MM-YYYY') as \"Tanggal Lahir\" from karyawan k, jabatan j where k.fk_jabatan = j.id and k.username = '{cari}'";
                //cmd.Parameters.Add(":laki", "Laki-Laki");
                //cmd.Parameters.Add(":perem", "Perempuan");

                conn.Open();
                cmd.ExecuteReader();
                da.SelectCommand = cmd;
                da.Fill(dtBahan);
                dgvData.DataSource = dtBahan;
                dgvData.Columns[0].Visible = false;
                dgvData.Columns[5].Visible = false;

                conn.Close();
            }
            else if (rbJenis.Checked == true)
            {
                if (cb.Text == "Laki-Laki")
                {
                    dtBahan.Rows.Clear();
                    cmd.Connection = conn;
                    cmd.CommandText = "select k.id, k.kode as \"Kode\", k.username as \"Username\", k.nama as \"Nama\", case k.jenis_kelamin when 'L' then 'Laki-Laki' else 'Perempuan' end as \"Jenis Kelamin\", k.alamat, k.email as \"Email\", k.no_telp as \"No Telp\", initcap(j.nama_jabatan) as \"Jabatan\", to_char(k.tanggal_lahir, 'DD-MM-YYYY') as \"Tanggal Lahir\" from karyawan k, jabatan j where k.fk_jabatan = j.id and k.jenis_kelamin = 'L'";
                    //cmd.Parameters.Add(":laki", "Laki-Laki");
                    //cmd.Parameters.Add(":perem", "Perempuan");

                    conn.Open();
                    cmd.ExecuteReader();
                    da.SelectCommand = cmd;
                    da.Fill(dtBahan);
                    dgvData.DataSource = dtBahan;
                    dgvData.Columns[0].Visible = false;
                    dgvData.Columns[5].Visible = false;

                    conn.Close();
                }
                else if (cb.Text == "Perempuan")
                {
                    dtBahan.Rows.Clear();
                    cmd.Connection = conn;
                    cmd.CommandText = "select k.id, k.kode as \"Kode\", k.username as \"Username\", k.nama as \"Nama\", case k.jenis_kelamin when 'L' then 'Laki-Laki' else 'Perempuan' end as \"Jenis Kelamin\", k.alamat, k.email as \"Email\", k.no_telp as \"No Telp\", initcap(j.nama_jabatan) as \"Jabatan\", to_char(k.tanggal_lahir, 'DD-MM-YYYY') as \"Tanggal Lahir\" from karyawan k, jabatan j where k.fk_jabatan = j.id and k.jenis_kelamin = 'P'";
                    //cmd.Parameters.Add(":laki", "Laki-Laki");
                    //cmd.Parameters.Add(":perem", "Perempuan");

                    conn.Open();
                    cmd.ExecuteReader();
                    da.SelectCommand = cmd;
                    da.Fill(dtBahan);
                    dgvData.DataSource = dtBahan;
                    dgvData.Columns[0].Visible = false;
                    dgvData.Columns[5].Visible = false;

                    conn.Close();
                }
            }
            else if (rbJabatan.Checked == true)
            {
                if (cb1.Text == "Karyawan")
                {
                    dtBahan.Rows.Clear();
                    cmd.Connection = conn;
                    cmd.CommandText = "select k.id, k.kode as \"Kode\", k.username as \"Username\", k.nama as \"Nama\", case k.jenis_kelamin when 'L' then 'Laki-Laki' else 'Perempuan' end as \"Jenis Kelamin\", k.alamat, k.email as \"Email\", k.no_telp as \"No Telp\", initcap(j.nama_jabatan) as \"Jabatan\", to_char(k.tanggal_lahir, 'DD-MM-YYYY') as \"Tanggal Lahir\" from karyawan k, jabatan j where k.fk_jabatan = j.id and k.fk_jabatan = 1";
                    //cmd.Parameters.Add(":laki", "Laki-Laki");
                    //cmd.Parameters.Add(":perem", "Perempuan");

                    conn.Open();
                    cmd.ExecuteReader();
                    da.SelectCommand = cmd;
                    da.Fill(dtBahan);
                    dgvData.DataSource = dtBahan;
                    dgvData.Columns[0].Visible = false;
                    dgvData.Columns[5].Visible = false;

                    conn.Close();
                }
                else if (cb1.Text == "Manager")
                {
                    dtBahan.Rows.Clear();
                    cmd.Connection = conn;
                    cmd.CommandText = "select k.id, k.kode as \"Kode\", k.username as \"Username\", k.nama as \"Nama\", case k.jenis_kelamin when 'L' then 'Laki-Laki' else 'Perempuan' end as \"Jenis Kelamin\", k.alamat, k.email as \"Email\", k.no_telp as \"No Telp\", initcap(j.nama_jabatan) as \"Jabatan\", to_char(k.tanggal_lahir, 'DD-MM-YYYY') as \"Tanggal Lahir\" from karyawan k, jabatan j where k.fk_jabatan = j.id and k.fk_jabatan = 2";
                    //cmd.Parameters.Add(":laki", "Laki-Laki");
                    //cmd.Parameters.Add(":perem", "Perempuan");

                    conn.Open();
                    cmd.ExecuteReader();
                    da.SelectCommand = cmd;
                    da.Fill(dtBahan);
                    dgvData.DataSource = dtBahan;
                    dgvData.Columns[0].Visible = false;
                    dgvData.Columns[5].Visible = false;

                    conn.Close();
                }
                else if (cb1.Text == "Chef")
                {
                    dtBahan.Rows.Clear();
                    cmd.Connection = conn;
                    cmd.CommandText = "select k.id, k.kode as \"Kode\", k.username as \"Username\", k.nama as \"Nama\", case k.jenis_kelamin when 'L' then 'Laki-Laki' else 'Perempuan' end as \"Jenis Kelamin\", k.alamat, k.email as \"Email\", k.no_telp as \"No Telp\", initcap(j.nama_jabatan) as \"Jabatan\", to_char(k.tanggal_lahir, 'DD-MM-YYYY') as \"Tanggal Lahir\" from karyawan k, jabatan j where k.fk_jabatan = j.id and k.fk_jabatan = 3";
                    //cmd.Parameters.Add(":laki", "Laki-Laki");
                    //cmd.Parameters.Add(":perem", "Perempuan");

                    conn.Open();
                    cmd.ExecuteReader();
                    da.SelectCommand = cmd;
                    da.Fill(dtBahan);
                    dgvData.DataSource = dtBahan;
                    dgvData.Columns[0].Visible = false;
                    dgvData.Columns[5].Visible = false;

                    conn.Close();
                }
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DialogResult dialogResult = MessageBox.Show("Yakin menghapus karyawan?", "Konfirmasi delete karyawan", MessageBoxButtons.YesNo, MessageBoxIcon.Question);
            if (dialogResult == DialogResult.Yes)
            {
                if (selectedIndex < 0)
                {
                    MessageBox.Show("Belum ada data yang dipilih");
                }
                else
                {
                    cmd = new OracleCommand();
                    cmd.Connection = conn;
                    cmd.CommandText = "select count(*) from h_trans where fk_karyawan=:id";
                    cmd.Parameters.Add(":id", dtBahan.Rows[selectedIndex][0].ToString());
                    conn.Open();
                    int jumlah = Int32.Parse(cmd.ExecuteScalar().ToString());
                    conn.Close();

                    if (jumlah < 1)
                    {
                        cmd = new OracleCommand();
                        cmd.Connection = conn;
                        cmd.CommandText = "delete from karyawan where id =:id";
                        cmd.Parameters.Add(":id", dtBahan.Rows[selectedIndex][0].ToString());
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        loadData();
                        MessageBox.Show("Berhasil Delete");
                    }
                    else
                    {
                        OracleDataAdapter da = new OracleDataAdapter();
                        int id = Convert.ToInt32(dtBahan.Rows[selectedIndex][0].ToString());

                        DataTable dtNota = new DataTable();
                        cmd = new OracleCommand();
                        cmd.Connection = conn;
                        cmd.CommandText = $"select * from h_trans where fk_karyawan ={id}";
                        da.SelectCommand = cmd;
                        da.Fill(dtNota);

                        for (int i = 0; i < dtNota.Rows.Count; i++)
                        {
                            cmd = new OracleCommand();
                            cmd.Connection = conn;
                            cmd.CommandText = $"delete from d_trans where nomor_nota ='{dtNota.Rows[i]["nomor_nota"]}'";
                            //cmd.Parameters.Add(":no", dtBahan.Rows[selectedIndex][1].ToString());
                            conn.Open();
                            cmd.ExecuteNonQuery();
                            conn.Close();
                        }

                        cmd.CommandText = $"delete from h_trans where fk_karyawan ='{id}'";
                        //cmd.Parameters.Add(":no", dtBahan.Rows[selectedIndex][1].ToString());
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();

                        cmd.CommandText = $"delete from karyawan where id ='{id}'";
                        //cmd.Parameters.Add(":id", dtBahan.Rows[selectedIndex][0].ToString());
                        conn.Open();
                        cmd.ExecuteNonQuery();
                        conn.Close();
                        loadData();
                        MessageBox.Show("Test");
                    }
                }
            }
            
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
                string user = tbUser.Text;
                string jenis = "";
                if (rbLaki.Checked == true)
                {
                    jenis = "L";
                }
                else if (rbPerempuan.Checked == true)
                {
                    jenis = "P";
                }
                string alamat = tbAlamat.Text;
                string email = tbEmail.Text;
                string no = tbNo.Text;
                string[] tgl = dtpTgl.Value.ToString().Split(' ');
                int jabatan = 0;
                if (cbJabatan.Text == "Karyawan")
                {
                    jabatan = 1;
                }
                else if (cbJabatan.Text == "Manager")
                {
                    jabatan = 2;
                }
                else if (cbJabatan.Text == "Chef")
                {
                    jabatan = 3;
                }

                if (nama != "" && user != "" && jenis != "" && alamat != "" && email != "" && no != "" && cbJabatan.Text != "")
                {
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

                            Console.WriteLine("ID : " + id + " , " + kode);
                            cmd = new OracleCommand();
                            cmd.Connection = conn;
                            cmd.CommandText = $"update karyawan set kode = '{kode}', username = '{user}', password = '{user}', nama = '{nama}', jenis_kelamin = '{jenis}', alamat = '{alamat}', email = '{email}', no_telp = '{no}', tanggal_lahir = to_date('{tgl[0]}', 'dd-mm-yyyy'),fk_jabatan = '{jabatan}' where id = {Convert.ToInt32(id)})";
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
                            cmd.CommandText = $"update karyawan set kode = '{kode}', username = '{user}', password = '{user}', nama = '{nama}', jenis_kelamin = '{jenis}', alamat = '{alamat}', email = '{email}', no_telp = '{no}', tanggal_lahir = to_date('{tgl[0]}', 'dd-mm-yyyy'),fk_jabatan = '{jabatan}' where id = {Convert.ToInt32(id)})";
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
                    MessageBox.Show("inputan tidak boleh kosong");
                }
            }
        }
    }
}
