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
            
            dgvStudents.CellClick += dgvStudents_CellClick;
            btnUpdate.Click += btnUpdate_Click;
            btnDelete.Click += btnDelete_Click;

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

        private void dgvStudents_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            DataGridViewRow row = dgvStudents.Rows[e.RowIndex];
            selectedID = Convert.ToInt32(row.Cells["ID"].Value);
            txtName.Text = row.Cells["Name"].Value.ToString();
            txtRoll.Text = row.Cells["Roll_number"].Value.ToString();
            txtSection.Text = row.Cells["Section"].Value.ToString();
            txtAge.Text = row.Cells["Age"].Value.ToString();
            txtAddress.Text = row.Cells["Address"].Value.ToString();
            txtPhone.Text = row.Cells["Phone_number"].Value.ToString();
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {
            if (selectedID == -1)
            {
                MessageBox.Show("Please select a student from the table first.");
                return;
            }
            if (!ValidateInputs()) return;

            try
            {
                using (var conn = new OleDbConnection(connStr))
                using (var cmd = new OleDbCommand(
                    "UPDATE [StudentInfo] SET [Name]=?, [Roll_number]=?, [Section]=?, [Age]=?, [Address]=?, [Phone_number]=? WHERE [ID]=?", conn))
                {
                    cmd.Parameters.AddWithValue("@Name", txtName.Text.Trim());
                    cmd.Parameters.AddWithValue("@Roll_number", txtRoll.Text.Trim());
                    cmd.Parameters.AddWithValue("@Section", txtSection.Text.Trim());
                    cmd.Parameters.AddWithValue("@Age", int.Parse(txtAge.Text));
                    cmd.Parameters.AddWithValue("@Address", txtAddress.Text.Trim());
                    cmd.Parameters.AddWithValue("@Phone_number", txtPhone.Text.Trim());
                    cmd.Parameters.AddWithValue("@ID", selectedID);

                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Student updated successfully.");
                LoadData();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error updating student: " + ex.Message);
            }
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            if (selectedID == -1)
            {
                MessageBox.Show("Please select a student from the table first.");
                return;
            }

            DialogResult result = MessageBox.Show(
                "Are you sure you want to delete this student?",
                "Confirm Delete", MessageBoxButtons.YesNo, MessageBoxIcon.Warning);
            if (result != DialogResult.Yes) return;

            try
            {
                using (var conn = new OleDbConnection(connStr))
                using (var cmd = new OleDbCommand("DELETE FROM [StudentInfo] WHERE [ID]=?", conn))
                {
                    cmd.Parameters.AddWithValue("@ID", selectedID);
                    conn.Open();
                    cmd.ExecuteNonQuery();
                }

                MessageBox.Show("Student deleted successfully.");
                LoadData();
                ClearFields();
            }
            catch (Exception ex)
            {
                MessageBox.Show("Error deleting student: " + ex.Message);
            }
        }
    }
}