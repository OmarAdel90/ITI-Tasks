using Day_2.Models;
using Microsoft.EntityFrameworkCore;
using System.Runtime.CompilerServices;
using System.Windows.Forms;
using System.Xml.Linq;

namespace Day_2
{
    public partial class Form1 : Form
    {
        private StudentContext context = new StudentContext();
        private int selectedStudentId = 0;
        public Form1()
        {
            InitializeComponent();
            NameT.TextChanged += (s, e) => Helper.ValidateForm(this.Controls, button1);
            AgeT.TextChanged += (s, e) => Helper.ValidateForm(this.Controls, button1);
            CityT.TextChanged += (s, e) => Helper.ValidateForm(this.Controls, button1);
            // Intiallialy make buttons disabbled till input is given
            ValidateAllButtons();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            Helper.LoadDb(context, DGV);
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            bool isValid = Helper.ValidateDataTypes(NameT, CityT);
            if (!isValid) return;
            var student = new Student
            {
                Name = NameT.Text.Trim(),
                Age = (int)AgeT.Value,
                City = CityT.Text.Trim()
            };
            context.Add(student);
            context.SaveChanges();
            Helper.LoadDb(context, DGV);
            Helper.ClearFields(this.Controls);
            ValidateAllButtons();

        }

        private void button3_Click(object sender, EventArgs e)
        {
            bool isValid = Helper.ValidateDataTypes(NameT, CityT);
            if (!isValid) return;
            var student = context.Students.Find(selectedStudentId);
            if (student != null)
            {
                context.Students.Remove(student);
                context.SaveChanges();
                Helper.LoadDb(context, DGV);
                Helper.ClearFields(this.Controls);
                selectedStudentId = 0;
                ValidateAllButtons();

            }
        }

        private void button2_Click(object sender, EventArgs e)
        {
            var selectedStudent = context.Students.Find(selectedStudentId);
            bool isValid = Helper.ValidateDataTypes(NameT, CityT);
            if (!isValid) return;
            if (selectedStudent != null)
            {
                selectedStudent.Name = NameT.Text.Trim();
                selectedStudent.Age = (int)AgeT.Value;
                selectedStudent.City = CityT.Text.Trim();
                context.SaveChanges();
                Helper.LoadDb(context, DGV);
                Helper.ClearFields(this.Controls);
                selectedStudentId = 0;
                ValidateAllButtons();
            }
        }

        private void DGV_CellClick(object sender, DataGridViewCellEventArgs e)
        {

            if (e.RowIndex >= 0)
            {
                DataGridViewRow row = DGV.Rows[e.RowIndex];
                NameT.Text = row.Cells["Name"].Value?.ToString() ?? "";

                if (row.Cells["Age"].Value != null && int.TryParse(row.Cells["Age"].Value.ToString(), out int age))
                {
                    AgeT.Value = age;
                }

                CityT.Text = row.Cells["City"].Value?.ToString() ?? "";

                // Store the selected student ID in variable
                if (row.Cells["Id"].Value != null && int.TryParse(row.Cells["Id"].Value.ToString(), out int id))
                {
                    selectedStudentId = id;
                }

                ValidateAllButtons();
            }
        }
        private void ValidateAllButtons()
        {
            bool isValid = !string.IsNullOrWhiteSpace(NameT.Text) &&
                           !string.IsNullOrWhiteSpace(CityT.Text) &&
                           AgeT.Value > 0;

            if (selectedStudentId > 0)
            {
                // Update mode: Disable Add, enable Update/Delete based on validation
                button1.Enabled = false;
                button2.Enabled = isValid;  // Update button requires valid data
                button3.Enabled = true;     // Delete button always enabled when student selected
            }
            else
            {
                // Add mode: Enable Add based on validation, disable Update/Delete
                button1.Enabled = isValid;
                button2.Enabled = false;
                button3.Enabled = false;
            }
        }

        private void button4_Click(object sender, EventArgs e)
        {
            NameT.Text = "";
            AgeT.Value = 0;
            CityT.Text = "";
            selectedStudentId = 0;
            ValidateAllButtons();
        }
    }
}
