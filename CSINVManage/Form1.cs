using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using System.Drawing.Imaging;
using System.Data.OleDb;

namespace CSINVManage
{
    public partial class Form1 : Form
    {
        static string filepath = Application.StartupPath;
        static string conStr = "Provider=Microsoft.ACE.OLEDB.12.0;Data Source=" + filepath + "/Phone_Inventory/Inventory.accdb;";
        OleDbConnection con = new OleDbConnection(conStr);

        public Form1()
        {
            InitializeComponent();
        }
        public PictureBox PictureBox
        {
            set
            {
                pictureBox1 = value;
            }
            get
            {
                return pictureBox1;
            }
        }
        private void Form1_Load(object sender, EventArgs e)
        {
            LoadFromDB(phoneGridView);
            UpdateComboBox();
            UpdateProblemBoxAndDepositbox(dataGridView2);
            ClearDGVText(sender, e);
        }
        private void UpdateProblemBoxAndDepositbox(DataGridView dgv)
        {
            dgv.Rows.Clear();
            dgv.Columns.Clear();
            ProblemList.Items.Clear();
            DepositBox.Items.Clear();
            DataTable dt = new DataTable();
            dt.Columns.Add("Problem");
            dt.Columns.Add("Deposit");
            string[] lines = File.ReadAllLines( filepath + "/Phone_Inventory/Problem.txt");
            int i = 0;
            foreach (string item in lines)
            {
                dt.Rows.Add();
                dt.Rows[i]["Problem"] = item;
                ProblemList.Items.Add(item);
                i++;
            }
            string[] lines2 = File.ReadAllLines(filepath + "/Phone_Inventory/Deposit.txt");
            int j = 0;
            foreach (string item in lines2)
            {
                dt.Rows.Add();
                dt.Rows[j]["Deposit"] = item;
                DepositBox.Items.Add(item);
                j++;
            }
            if (dt.Rows.Count > 0)
            {
                dt = dt.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is System.DBNull || string.Compare((field as string).Trim(), string.Empty) == 0)).CopyToDataTable();
            }
            DT2DGB(dgv, dt);
        }
        private void UpdateComboBox()
        {
            phoneBrandComboBox.Items.Clear();
            DirectoryInfo d = new DirectoryInfo(filepath + "/Phone_Inventory/Phones");
            FileInfo[] Files = d.GetFiles("*.txt");
            foreach (FileInfo file in Files)
            {
                string filename = Path.GetFileNameWithoutExtension( filepath + "/Phone_Inventory/Phones/" + file.Name + "");
                phoneBrandComboBox.Items.Add(filename);
            }
        }
        private void phoneBrandComboBox_SelectedIndexChanged(object sender, EventArgs e)
        {
            PhoneModelComboBox.Items.Clear();
            string filename = phoneBrandComboBox.Text;
            string[] lines = File.ReadAllLines(filepath + "/Phone_Inventory/Phones/" + filename + ".txt");
            foreach (string line in lines)
            {
                PhoneModelComboBox.Items.Add(line);
            }
        }
        private void addBtn_Click(object sender, EventArgs e)
        {
            bool selectedrow = false;
            if (dataGridView1.SelectedRows.Count == 1)
            {
                selectedrow = true;
            }
            if (!selectedrow)
            {
                string id = IDBox.Text;
                string cname = cNameBox.Text;
                string date = dateBox.Text;
                string cpn1 = CPN1.Text;
                string cpn2 = CPN2.Text;
                string cpn3 = CPN3.Text;
                string phonebrand = phoneBrandComboBox.Text;
                string phonemodel = PhoneModelComboBox.Text;
                string imei = imeiBox.Text;
                string problem = "";
                foreach (string text in ProblemList.CheckedItems)
                {
                    problem += text + ",";
                }
                if (problem.Length > 0)
                {
                    problem = problem.Remove(problem.Length - 1);
                }
                int deposit;
                Int32.TryParse(DepositBox.Text, out deposit);
                string password = passwordBox.Text;
                byte[] photo_aray = new byte[0];
                if (pictureBox1.Image != null)
                {
                    ImageConverter imgCon = new ImageConverter();
                    photo_aray = (byte[])imgCon.ConvertTo(pictureBox1.Image, typeof(byte[]));
                }
                string particular = "";
                foreach (string item in particularsBox.Items)
                {
                    particular += item + ",";
                }
                if (particular.Length > 0)
                {
                    particular = particular.Remove(particular.Length - 1);
                }
                string checkbox = "";
                if (warrantyT.CheckState.ToString() == "Checked")
                {
                    checkbox += "Nopay";
                }
                if (nopayT.CheckState.ToString() == "Checked")
                {
                    checkbox += "Nopay";
                }
                if (fullpayT.CheckState.ToString() == "Checked")
                {
                    checkbox += "Fullpay";
                }
                if (depositT.CheckState.ToString() == "Checked")
                {
                    checkbox += "Deposit";
                }
                string other = others.Rtf;
                string warranty = warrantyBox.Text;
                AddToDB(id, cname, date, cpn1, cpn2, cpn3, phonebrand, phonemodel, imei, problem, password, photo_aray, particular, deposit, checkbox, other, warranty);
                dbload();
                ClearDGVText(sender, e);
                MessageBox.Show("Data Added!");
            }
        }
        private void saveBtn_Click(object sender, EventArgs e)
        {
            bool selectedrow = false;
            if (dataGridView1.SelectedRows.Count == 1)
            {
                selectedrow = true;
            }
            if (selectedrow)
            {
                string id = IDBox.Text;
                string cname = cNameBox.Text;
                string date = dateBox.Text;
                string cpn1 = CPN1.Text;
                string cpn2 = CPN2.Text;
                string cpn3 = CPN3.Text;
                string phonebrand = phoneBrandComboBox.Text;
                string phonemodel = PhoneModelComboBox.Text;
                string imei = imeiBox.Text;
                string problem = "";
                foreach (string text in ProblemList.CheckedItems)
                {
                    problem += text + ",";
                }
                if (problem.Length > 0)
                {
                    problem = problem.Remove(problem.Length - 1);
                }
                int deposit;
                Int32.TryParse(DepositBox.Text, out deposit);
                string password = passwordBox.Text;
                byte[] photo_aray = new byte[0];
                if (pictureBox1.Image != null)
                {
                    ImageConverter imgCon = new ImageConverter();
                    photo_aray = (byte[])imgCon.ConvertTo(pictureBox1.Image, typeof(byte[]));
                }
                string particular = "";
                foreach (string item in particularsBox.Items)
                {
                    particular += item + ",";
                }
                if (particular.Length > 0)
                {
                    particular = particular.Remove(particular.Length - 1);
                }
                string checkbox = "";
                if (warrantyT.CheckState.ToString() == "Checked")
                {
                    checkbox += "Warranty" + ",";
                }
                if (nopayT.CheckState.ToString() == "Checked")
                {
                    checkbox += "Nopay" + ",";
                }
                if (fullpayT.CheckState.ToString() == "Checked")
                {
                    checkbox += "Fullpay" + ",";
                }
                if (depositT.CheckState.ToString() == "Checked")
                {
                    checkbox += "Deposit" + ",";
                }
                if (checkbox.Length > 0)
                {
                    checkbox = checkbox.Remove(checkbox.Length - 1);
                }
                string other = others.Rtf;
                string warranty = warrantyBox.Text;
                UpdateToDB(id, cname, date, cpn1, cpn2, cpn3, phonebrand, phonemodel, imei, problem, password, photo_aray, particular, deposit, checkbox, other, warranty);
                ClearDGVText(sender, e);
                dbload();
                MessageBox.Show("Data Updated!");
            }
        }
        private void btnform2_Click(object sender, EventArgs e)
        {
            Form2 scrnlock = new Form2(this);
            scrnlock.Show();
        }
        private void addPhoneBrand_Click(object sender, EventArgs e)
        {
            string phonebrand = phoneBrandText.Text;
            string phonemodel = phoneModelText.Text;
            if (phoneBrandText.Text != "")
            {
                phoneModelText.Clear();
                if (phoneGridView.Columns.Contains(phonebrand))
                {
                    Boolean found = false;
                    foreach (DataGridViewRow row in phoneGridView.Rows)
                    {
                        if (row.Cells[phonebrand].Value == DBNull.Value)
                        {
                            continue;
                        }
                        if ((String)row.Cells[phonebrand].Value == phonemodel)
                        {
                            found = true;
                            MessageBox.Show(phonemodel + "已经存在！");
                            break;
                        }
                        else
                        {
                            continue;
                        }
                    }
                    if (!found)
                    {
                        String fileName = filepath + "/Phone_Inventory/Phones/" + phonebrand + ".txt";
                        using (FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write))
                        using (StreamWriter sw = new StreamWriter(fs))
                        {
                            sw.WriteLine(phonemodel);
                        }
                    }
                }
                else
                {
                    String filename = filepath + "/Phone_Inventory/Phones/" + phonebrand + ".txt";
                    File.Create(filename).Close();
                }
                LoadFromDB(phoneGridView);
            }
        }
        private void DT2DGB(DataGridView dgv, DataTable dt)
        {
            foreach (DataColumn dc in dt.Columns)
            {
                dgv.Columns.Add(dc.ColumnName, dc.ColumnName);
            }
            int i = 0;
            foreach (DataRow dr in dt.Rows)
            {
                if (dgv.Rows[i].IsNewRow.Equals(true))
                {
                    dgv.Rows.Add(dr.ItemArray);
                }
                else
                {
                    dgv.Rows[i].SetValues(dr.ItemArray);
                }
                i++;
            }
        }
        private void LoadFromDB(DataGridView dgv)
        {
            dgv.Rows.Clear();
            dgv.Columns.Clear();
            DataTable dt = new DataTable();
            DirectoryInfo d = new DirectoryInfo( filepath + "/Phone_Inventory/Phones");
            FileInfo[] Files = d.GetFiles("*.txt");
            foreach (FileInfo file in Files)
            {
                string filename = Path.GetFileNameWithoutExtension( filepath + "/Phone_Inventory/Phones/" + file.Name + "");
                dt.Columns.Add(filename);
                string[] lines = File.ReadAllLines( filepath + "/Phone_Inventory/Phones/" + file.Name + "");
                int i = 0;
                foreach (string line in lines)
                {
                    dt.Rows.Add();
                    dt.Rows[i][filename] = line;
                    i++;
                }
            }
            // From http://stackoverflow.com/questions/7023140/how-to-remove-empty-rows-from-datatable
            if (dt.Rows.Count > 0)
            {
                dt = dt.Rows.Cast<DataRow>().Where(row => !row.ItemArray.All(field => field is System.DBNull || string.Compare((field as string).Trim(), string.Empty) == 0)).CopyToDataTable();
            }
            DT2DGB(dgv, dt);
            UpdateComboBox();
        }
        private void dbload()
        {
            filterbox.Items.Clear();
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            DataTable dt = new DataTable();
            string SQL = "SELECT * FROM InventoryTable";
            OleDbCommand cmd = new OleDbCommand(SQL, con);
            try
            {
                con.Open();
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                adapter.Fill(dt);
                foreach (DataColumn dc in dt.Columns)
                {
                    filterbox.Items.Add(dc.ColumnName);
                    dataGridView1.Columns.Add(dc.ColumnName, dc.ColumnName);
                }
                foreach (DataRow dr in dt.Rows)
                {
                    dataGridView1.Rows.Add(dr.ItemArray);
                }
                dt.Rows.Clear();
                dt.Columns.Clear();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void DelFromDB(string id)
        {
            bool selectedrow = false;
            if (dataGridView1.SelectedRows.Count == 1)
            {
                selectedrow = true;
            }
            if (dataGridView1.CurrentRow.Index == dataGridView1.Rows.Count - 1)
            {
                selectedrow = false;
            }
            if (selectedrow)
            {
                string SQL = "DELETE FROM [InventoryTable] WHERE ID = @ID";
                OleDbCommand cmd = new OleDbCommand(SQL, con);
                OleDbDataAdapter adapter = new OleDbDataAdapter();
                cmd.Parameters.AddWithValue("@ID", id);
                try
                {
                    if (MessageBox.Show("确定要删除此存档？", "DELETE ID - " + id, MessageBoxButtons.OKCancel, MessageBoxIcon.Warning) == DialogResult.OK)
                    {
                        con.Open();
                        adapter.DeleteCommand = con.CreateCommand();
                        adapter.DeleteCommand.CommandText = SQL;
                        cmd.ExecuteNonQuery();
                        con.Close();
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
            }
        }
        private void UpdateToDB(string id, string cname, string date, string cpn1, string cpn2, string cpn3, string phonebrand, string phonemodel, string imei, string problem, string password, byte[] picture, string particular, int deposit, string checkbox, string others, string warranty)
        {
            string SQL = "UPDATE InventoryTable SET [ID] = @ID, [CustomerName] = @CustomerName,[Date] = @Date ,[CPN1] = @CPN1, [CPN2] = @CPN2, [CPN3] = @CPN3, [Phone Brand] = @Phonebrand, [Phone Models] = @PhoneModel, [Imei] = @Imei, [Problem] = @Problem, [Password] = @Password, [Picture] = @Picture, [Particulars] = @Particulars,[Deposit] = @Deposit, [CheckBox] = @CheckBox, [Others] = @Others, [Warranty] = @Warranty WHERE ID = @ID";
            OleDbCommand cmd = new OleDbCommand(SQL, con);
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@CustomerName", cname);
            cmd.Parameters.AddWithValue("@Date", date);
            cmd.Parameters.AddWithValue("@CPN1", cpn1);
            cmd.Parameters.AddWithValue("@CPN2", cpn2);
            cmd.Parameters.AddWithValue("@CPN3", cpn3);
            cmd.Parameters.AddWithValue("@Phonebrand", phonebrand);
            cmd.Parameters.AddWithValue("@PhoneModel", phonemodel);
            cmd.Parameters.AddWithValue("@Imei", imei);
            cmd.Parameters.AddWithValue("@Problem", problem);
            cmd.Parameters.AddWithValue("@Password", password);
            cmd.Parameters.AddWithValue("@Picture", picture);
            cmd.Parameters.AddWithValue("@Particulars", particular);
            cmd.Parameters.AddWithValue("@Deposit", deposit);
            cmd.Parameters.AddWithValue("@CheckBox", checkbox);
            cmd.Parameters.AddWithValue("@Others", others);
            cmd.Parameters.AddWithValue("@Warranty", warranty);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void AddToDB(string id, string cname, string date, string cpn1, string cpn2, string cpn3, string phonebrand, string phonemodel, string imei, string problem, string password, byte[] picture, string particular, int deposit, string checkbox, string others, string warranty)
        {
            string SQL = "INSERT INTO [InventoryTable] VALUES (@ID,@CustomerName,@Date,@CPN1,@CPN2,@CPN3,@Phonebrand,@PhoneModel,@Imei,@Problem,@Password,@Picture,@Particulars,@Deposit,@CheckBox,@Others,@Warranty)";
            OleDbCommand cmd = new OleDbCommand(SQL, con);
            cmd.Parameters.AddWithValue("@ID", id);
            cmd.Parameters.AddWithValue("@CustomerName", cname);
            cmd.Parameters.AddWithValue("@Date", date);
            cmd.Parameters.AddWithValue("@CPN1", cpn1);
            cmd.Parameters.AddWithValue("@CPN2", cpn2);
            cmd.Parameters.AddWithValue("@CPN3", cpn3);
            cmd.Parameters.AddWithValue("@Phonebrand", phonebrand);
            cmd.Parameters.AddWithValue("@PhoneModel", phonemodel);
            cmd.Parameters.AddWithValue("@Imei", imei);
            cmd.Parameters.AddWithValue("@Problem", problem);
            cmd.Parameters.AddWithValue("@Password", password);
            cmd.Parameters.AddWithValue("@Picture", picture);
            cmd.Parameters.AddWithValue("@Particulars", particular);
            cmd.Parameters.AddWithValue("@Deposit", deposit);
            cmd.Parameters.AddWithValue("@CheckBox", checkbox);
            cmd.Parameters.AddWithValue("@Others", others);
            cmd.Parameters.AddWithValue("@Warranty", warranty);
            try
            {
                con.Open();
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void ClearDGVText(object sender, EventArgs e)
        {
            dateBox.Clear();
            cNameBox.Clear();
            IDBox.Clear();
            CPN1.Clear();
            CPN2.Clear();
            CPN3.Clear();
            phoneBrandComboBox.Text = "";
            PhoneModelComboBox.Text = "";
            imeiBox.Clear();
            for (int i = 0; i < ProblemList.Items.Count; i++)
            {
                ProblemList.SetItemChecked(i, false);
            }
            DepositBox.Text = "";
            passwordBox.Clear();
            particularsBox.Items.Clear();
            warrantyT.Checked = false;
            nopayT.Checked = false;
            fullpayT.Checked = false;
            depositT.Checked = false;
            particularItem.Clear();
            itemQuantity.Clear();
            itemUnit.Clear();
            pictureBox1.Image = null;
            dateBox.Text = DateTime.Now.ToString("yyyy/MM/dd hh:mm");
            others.Clear();
            warrantyBox.Clear();
            // ID Checking
            DataTable dt = new DataTable();
            string SQL = "SELECT ID FROM InventoryTable ORDER BY ID DESC";
            OleDbCommand cmd = new OleDbCommand(SQL, con);
            try
            {
                con.Open();
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                adapter.Fill(dt);
                cmd.ExecuteNonQuery();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
            int newrow = 0;
            if (dt.Rows.Count > 0)
            {
                newrow = (Int32)dt.Rows[0][0];
            }
            newrow++;
            IDBox.Text = newrow.ToString();
        }

        private void createBtn_Click(object sender, EventArgs e)
        {
            ClearDGVText(sender, e);
            bool selectedrow = false;
            if (dataGridView1.SelectedRows.Count == 1)
            {
                selectedrow = true;
            }
            if (selectedrow)
            {
                dataGridView1.SelectedRows[0].Selected = false;
            }
        }
        private void dateBox_TextChanged(object sender, EventArgs e)
        {
            bool selectedrow = false;
            if (dataGridView1.SelectedRows.Count == 1)
            {
                selectedrow = true;
            }
            if (!selectedrow)
            {
                dateBox.Text = DateTime.Now.ToString("yyyy/MM/dd hh:mm");
            }
        }

        private void IDBox_TextChanged(object sender, EventArgs e)
        {
            bool selectedrow = false;
            if (dataGridView1.SelectedRows.Count == 1)
            {
                selectedrow = true;
            }
            if (!selectedrow)
            {
                DataTable dt = new DataTable();
                string SQL = "SELECT ID FROM InventoryTable ORDER BY ID DESC";
                OleDbCommand cmd = new OleDbCommand(SQL, con);
                try
                {
                    con.Open();
                    OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                    adapter.Fill(dt);
                    cmd.ExecuteNonQuery();
                    con.Close();
                }
                catch (Exception ex)
                {
                    MessageBox.Show(ex.Message);
                }
                int newrow = 0;
                if (dt.Rows.Count > 0)
                {
                    newrow = (Int32)dt.Rows[0][0];
                }
                newrow++;
                IDBox.Text = newrow.ToString();
            }
        }

        private void printBtn_Click(object sender, EventArgs e)
        {
            
            RCprintPreviewDialog1.Document = RCprintDocument1;
            RCprintDocument1.DefaultPageSettings.PaperSize = new System.Drawing.Printing.PaperSize("PaperA4", 812, 510);
            RCprintPreviewDialog1.ShowDialog();
        }

        private void RCprintDocument1_PrintPage(object sender, System.Drawing.Printing.PrintPageEventArgs e)
        {
            Image image = Image.FromFile( filepath + "/Phone_Inventory/Resource/Receipt.png");
            e.Graphics.DrawImage(image, 0, 0, image.Width, image.Height);
            if (pictureBox1.Image != null)
            {
                e.Graphics.DrawImage(pictureBox1.Image, 88, 350, pictureBox1.Image.Width - 75, pictureBox1.Image.Height - 75);
            }
            if (cNameBox.Text != "")
            {
                e.Graphics.DrawString(cNameBox.Text, new Font("Arial Narrow", 10, FontStyle.Regular), Brushes.Black, new Point(55, 110));
            }
            if (IDBox.Text != "")
            {
                e.Graphics.DrawString(IDBox.Text, new Font("Arial Narrow", 15, FontStyle.Regular), Brushes.Black, new Point(710, 37));
            }
            if (dateBox.Text != "")
            {
                e.Graphics.DrawString(dateBox.Text, new Font("Arial Narrow", 10, FontStyle.Regular), Brushes.Black, new Point(238, 78));
            }
            if (CPN1.Text != "")
            {
                e.Graphics.DrawString(CPN1.Text, new Font("Arial Narrow", 10, FontStyle.Regular), Brushes.Black, new Point(272, 110));
            }
            if (CPN2.Text != "")
            {
                e.Graphics.DrawString(CPN2.Text, new Font("Arial Narrow", 10, FontStyle.Regular), Brushes.Black, new Point(440, 110));
            }
            if (CPN3.Text != "")
            {
                e.Graphics.DrawString(CPN3.Text, new Font("Arial Narrow", 10, FontStyle.Regular), Brushes.Black, new Point(638, 110));
            }
            if (phoneBrandComboBox.Text != "")
            {
                e.Graphics.DrawString(phoneBrandComboBox.Text, new Font("Arial Narrow", 10, FontStyle.Regular), Brushes.Black, new Point(65, 186));
            }
            if (PhoneModelComboBox.Text != "")
            {
                e.Graphics.DrawString(PhoneModelComboBox.Text, new Font("Arial Narrow", 10, FontStyle.Regular), Brushes.Black, new Point(65, 206));
            }
            if (imeiBox.Text != "")
            {
                e.Graphics.DrawString(imeiBox.Text, new Font("Arial Narrow", 10, FontStyle.Regular), Brushes.Black, new Point(65, 229));
            }
            if (passwordBox.Text != "")
            {
                e.Graphics.DrawString(passwordBox.Text, new Font("Arial Narrow", 10, FontStyle.Regular), Brushes.Black, new Point(108, 305));
            }
            if (DepositBox.Text != "")
            {
                e.Graphics.DrawString(DepositBox.Text, new Font("Arial Narrow", 10, FontStyle.Regular), Brushes.Black, new Point(345, 343));
            }
            if (warrantyBox.Text != "")
            {
                e.Graphics.DrawString(warrantyBox.Text, new Font("Arial Narrow", 10, FontStyle.Regular), Brushes.Black, new Point(350, 370));
            }
            int i = 0;
            foreach (string text in ProblemList.CheckedItems)
            {
                e.Graphics.DrawString(text + ",", new Font("Arial", 10, FontStyle.Regular), Brushes.Black, new Point(30 + i, 270));
                int num = text.Length;
                i += 30 + (num * 3);
            }
            if (nopayT.CheckState.ToString() == "Checked")
            {
                e.Graphics.DrawString("☑", new Font("Arial Narrow", 10, FontStyle.Regular), Brushes.Black, new Point(706, 345));
            }
            if (fullpayT.CheckState.ToString() == "Checked")
            {
                e.Graphics.DrawString("☑", new Font("Arial Narrow", 10, FontStyle.Regular), Brushes.Black, new Point(569, 345));
            }
            if (warrantyT.CheckState.ToString() == "Checked")
            {
                e.Graphics.DrawString("☑", new Font("Arial Narrow", 10, FontStyle.Regular), Brushes.Black, new Point(454, 372));
            }
            if (depositT.CheckState.ToString() == "Checked")
            {
                e.Graphics.DrawString("☑", new Font("Arial Narrow", 10, FontStyle.Regular), Brushes.Black, new Point(454, 343));
            }
            if (others.Rtf != "")
            {
                e.Graphics.DrawString(others.Text, new Font("Arial Narrow", 10, FontStyle.Regular), Brushes.Black, new Point(557, 370));
            }
            if (particularsBox.Items.Count > 0)
            {
                float finaltotal = 0;
                int j = 0;
                foreach (string text in particularsBox.Items)
                {
                    string[] texts = text.Split('-');
                    int nums = texts[0].Length;
                    e.Graphics.DrawString(texts[0], new Font("Arial Narrow", 10, FontStyle.Regular), Brushes.Black, new Point(405 - (nums * 5), 185 + j));
                    int num = texts[1].Length;
                    e.Graphics.DrawString(texts[1], new Font("Arial Narrow", 10, FontStyle.Regular), Brushes.Black, new Point(572 - (num * 5), 185 + j));
                    int num2 = texts[2].Length;
                    e.Graphics.DrawString(texts[2], new Font("Arial Narrow", 10, FontStyle.Regular), Brushes.Black, new Point(658 - (num2 * 5), 185 + j));
                    float quantity;
                    float unitprice;
                    float.TryParse(texts[1], out quantity);
                    float.TryParse(texts[2], out unitprice);
                    string total = (quantity * unitprice).ToString();
                    int num3 = total.Length;
                    e.Graphics.DrawString(total, new Font("Arial Narrow", 10, FontStyle.Regular), Brushes.Black, new Point(750 - (num3 * 5), 185 + j));
                    finaltotal += quantity * unitprice;
                    j += 26;
                }
                double totals = Math.Round(finaltotal, 2);
                int num4 = finaltotal.ToString().Length;
                e.Graphics.DrawString(finaltotal.ToString(), new Font("Arial Narrow", 10, FontStyle.Regular), Brushes.Black, new Point(750 - (num4 * 5), 185 + 130));
            }
        }

        private void problemAdd_Click(object sender, EventArgs e)
        {
            if (problemText.Text != "")
            {
                String fileName =  filepath + "/Phone_Inventory/Problem.txt";
                using (FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(problemText.Text);
                }
                UpdateProblemBoxAndDepositbox(dataGridView2);
                problemText.Clear();
            }
        }

        private void DepositAdd_Click(object sender, EventArgs e)
        {
            if (depositText.Text != "")
            {
                String fileName = filepath + "/Phone_Inventory/Deposit.txt";
                using (FileStream fs = new FileStream(fileName, FileMode.Append, FileAccess.Write))
                using (StreamWriter sw = new StreamWriter(fs))
                {
                    sw.WriteLine(depositText.Text);
                }
                UpdateProblemBoxAndDepositbox(dataGridView2);
                depositText.Clear();
            }
        }

        private void particularAdd_Click(object sender, EventArgs e)
        {
            string item = particularItem.Text;
            string quantity = itemQuantity.Text;
            string unit = itemUnit.Text;
            particularItem.Clear();
            itemQuantity.Clear();
            itemUnit.Clear();
            particularsBox.Items.Add(item + "-" + quantity + "-" + unit);
        }

        private void particularRemove_Click(object sender, EventArgs e)
        {
            if (particularsBox.SelectedItem != null)
            {
                particularsBox.Items.Remove(particularsBox.SelectedItem);
            }
        }
        

        private void dataGridView1_CellContentClick(object sender, MouseEventArgs e)
        {
            if (dataGridView1.Rows.Count > 0)
            {
                bool selectedrow = false;
                ClearDGVText(sender, e);
                if (dataGridView1.SelectedRows.Count == 1)
                {
                    selectedrow = true;
                }
                if (dataGridView1.CurrentRow.Index == dataGridView1.Rows.Count - 1)
                {
                    selectedrow = false;
                }
                if (selectedrow)
                {
                    IDBox.Text = dataGridView1.SelectedRows[0].Cells[0].Value.ToString();
                    cNameBox.Text = dataGridView1.SelectedRows[0].Cells[1].Value.ToString();
                    dateBox.Text = dataGridView1.SelectedRows[0].Cells[2].Value.ToString();
                    CPN1.Text = dataGridView1.SelectedRows[0].Cells[3].Value.ToString();
                    CPN2.Text = dataGridView1.SelectedRows[0].Cells[4].Value.ToString();
                    CPN3.Text = dataGridView1.SelectedRows[0].Cells[5].Value.ToString();
                    phoneBrandComboBox.Text = dataGridView1.SelectedRows[0].Cells[6].Value.ToString();
                    PhoneModelComboBox.Text = dataGridView1.SelectedRows[0].Cells[7].Value.ToString();
                    imeiBox.Text = dataGridView1.SelectedRows[0].Cells[8].Value.ToString();
                    //ProblemBox.Text = 
                    if (dataGridView1.SelectedRows[0].Cells[9].Value.ToString() != "")
                    {
                        string[] problem = dataGridView1.SelectedRows[0].Cells[9].Value.ToString().Split(',');
                        if (problem.Length > 0)
                        {
                            foreach (string text in problem)
                            {
                                ProblemList.SetItemChecked(ProblemList.Items.IndexOf(text), true);
                            }
                        }
                    }
                    passwordBox.Text = dataGridView1.SelectedRows[0].Cells[10].Value.ToString();
                    pictureBox1.Image = null;
                    byte[] image = (byte[])dataGridView1.SelectedRows[0].Cells[11].Value;
                    if (image.Length > 0 && image != null)
                    {
                        MemoryStream ms = new MemoryStream(image);
                        pictureBox1.Image = Image.FromStream(ms);
                    }
                    string[] particular = dataGridView1.SelectedRows[0].Cells[12].Value.ToString().Split(',');
                    if (particular.Length > 0)
                    {
                        foreach (string text in particular)
                        {
                            particularsBox.Items.Add(text);
                        }
                    }
                    DepositBox.Text = dataGridView1.SelectedRows[0].Cells[13].Value.ToString();
                    string[] checkbox = dataGridView1.SelectedRows[0].Cells[14].Value.ToString().Split(',');
                    foreach (string text in checkbox)
                    {
                        if (text.Contains("Warranty"))
                        {
                            warrantyT.Checked = true;
                        }
                        if (text.Contains("Nopay"))
                        {
                            nopayT.Checked = true;
                        }
                        if (text.Contains("Fullpay"))
                        {
                            fullpayT.Checked = true;
                        }
                        if (text.Contains("Deposit"))
                        {
                            depositT.Checked = true;
                        }
                    }
                    string other = dataGridView1.SelectedRows[0].Cells[15].Value.ToString();
                    others.Rtf = other;
                    warrantyBox.Text = dataGridView1.SelectedRows[0].Cells[16].Value.ToString();
                }
            }
        }

        private void dbloadBtn_Click(object sender, EventArgs e)
        {
            dbload();
        }
        private void searchfilter(string searchby, string filter)
        {
            dataGridView1.Rows.Clear();
            dataGridView1.Columns.Clear();
            DataTable dt = new DataTable();
            string SQL = "SELECT * FROM InventoryTable WHERE [" + searchby + "] like ('%" + filter + "%')";
            OleDbCommand cmd = new OleDbCommand(SQL, con);
            try
            {
                con.Open();
                OleDbDataAdapter adapter = new OleDbDataAdapter(cmd);
                adapter.Fill(dt);
                foreach (DataColumn dc in dt.Columns)
                {
                    filterbox.Items.Add(dc.ColumnName);
                    dataGridView1.Columns.Add(dc.ColumnName, dc.ColumnName);
                }
                foreach (DataRow dr in dt.Rows)
                {
                    dataGridView1.Rows.Add(dr.ItemArray);
                }
                dt.Rows.Clear();
                dt.Columns.Clear();
                con.Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message);
            }
        }
        private void button1_Click(object sender, EventArgs e)
        {
            if (filterbox.Text != "")
            {
                searchfilter(filterbox.Text, searchText.Text);
            }
        }

        private void delBtm_Click(object sender, EventArgs e)
        {
            string id = IDBox.Text;
            DelFromDB(id);
            ClearDGVText(sender, e);
            dbload();
        }
    }
}

