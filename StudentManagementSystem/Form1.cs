using System.Data;
using System.Data.OleDb;

namespace StudentManagementSystem
{
    public partial class Form1 : Form
    {
        private string connStr = @"Provider=Microsoft.ACE.OLEDB.16.0; Data Source=" +
            Path.Combine(Application.StartupPath, "StudentDatabase.accdb") + ";";
        private int selectedID = -1;

        public Form1()
        {
            InitializeComponent();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            LoadData();
        }

        private void LoadData()
        {
            try
            {
                using (var conn = new OleDbConnection(connStr))
                using (var adapter = new OleDbDataAdapter("SELECT * FROM StudentInfo", conn))
                {
                    var table = new DataTable();
                    adapter.Fill(table);
                    dgvStudents.DataSource = table;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error loading students: " + ex.Message);
            }
        }

        private bool ValidateInputs()
        {
            if (string.IsNullOrWhiteSpace(txtName.Text) ||
                string.IsNullOrWhiteSpace(txtRoll.Text) ||
                string.IsNullOrWhiteSpace(txtSection.Text) ||
                string.IsNullOrWhiteSpace(txtAddress.Text) ||
                string.IsNullOrWhiteSpace(txtPhone.Text))
            {
                MessageBox.Show("Please enter all required student information.");
                return false;
            }
            if (!int.TryParse(txtAge.Text, out int age) || age <= 0)
            {
                MessageBox.Show("Age must be a valid positive number.");
                return false;
            }
            return true;
        }

        private void ClearFields()
        {
            txtName.Clear();
            txtRoll.Clear();
            txtSection.Clear();
            txtAddress.Clear();
            txtPhone.Clear();
            txtAge.Clear();
            selectedID = -1;
            txtName.Focus();
        }

        private void label2_Click(object sender, EventArgs e)
        {
        }

        private void label4_Click(object sender, EventArgs e)
        {
        }

        private void dgvStudents_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            ClearFields();
        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadData();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {
            if (!ValidateInputs()) return;

            try
            {
                using (var conn = new OleDbConnection(connStr))
                using (var cmd = new OleDbCommand(
                   "INSERT INTO [StudentInfo] ([Name], [Roll_number], [Section], [Age], [Address], [Phone_number]) VALUES (?, ?, ?, ?, ?, ?)", conn))
                {
                    cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Roll_number", txtRoll.Text.Trim());
                    cmd.Parameters.AddWithValue("@Section", txtSection.Text.Trim());
                    cmd.Parameters.AddWithValue("@Age", int.Parse(txtAge.Text));
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@Phone_number", txtPhone.Text.Trim());

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Student added successfully.");
                LoadData();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error adding student: " + ex.Message);
            }
        }
    }
}