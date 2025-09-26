using Day_3.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Day_3
{
    public static class Helper
    {
        public static void LoadDb(StudentContext context,DataGridView view)
        {
            view.DataSource = context.Students.ToList();
        }
        public static void ClearFields(Control.ControlCollection controls)
        {
            foreach (Control control in controls)
            {
                if (control is TextBox textBox)
                    textBox.Clear();
                else if (control is NumericUpDown numeric)
                    numeric.Value = 0;
            }
        }
        public static void ValidateForm(Control.ControlCollection controls, params Button[] buttons)
        {
            bool isValid = true;

            foreach (Control control in controls)
            {
                if (control is TextBox textBox && string.IsNullOrWhiteSpace(textBox.Text))
                {
                    isValid = false;
                    break;
                }
                else if (control is NumericUpDown numeric && numeric.Value == 0)
                {
                    isValid = false;
                    break;
                }
            }

            foreach (Button button in buttons)
            {
                button.Enabled = isValid;
            }
        }
        public static bool ValidateDataTypes(TextBox nameTextBox, TextBox cityTextBox)
        {
            if (nameTextBox.Text.Any(char.IsDigit))
            {
                MessageBox.Show("Name cannot contain numbers");
                return false;
            }
            if (cityTextBox.Text.Any(char.IsDigit))
            {
                MessageBox.Show("City cannot contain numbers");
                return false;
            }
            return true;
                
        }
    }
}
