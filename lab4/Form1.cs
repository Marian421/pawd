using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace lab4
{
    public partial class Form1 : Form
    {
        private string connectionString = "Server=DESKTOP-Q0BAVTV\\SQLEXPRESS;Database=studenti;Integrated Security=True;";
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string nume = textBox1.Text;
            float nota1;
            float nota2;

            if(!float.TryParse(textBox2.Text, out nota1) || !float.TryParse(textBox3.Text,out nota2))
            {
                MessageBox.Show("Te rog introdu valori numerice pentru note.");
                return;
            }
            using(SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "INSERT INTO note (nume, nota_1, nota_2) VALUES (@Nume, @Nota1, @Nota2)";

                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        command.Parameters.AddWithValue("@Nume", nume);
                        command.Parameters.AddWithValue("@Nota1", nota1);
                        command.Parameters.AddWithValue("@Nota2", nota2);

                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Datele au fost adaugate cu succes.");

                        } else
                        {
                            MessageBox.Show("Nu s-au putut aduaga datele.");
                        }
                    }
                } catch (Exception ex) { 
                    MessageBox.Show("Eroare: " + ex.Message);
                }
            }
        }
    }
}
