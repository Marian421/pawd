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
                            textBox1.Text = "";
                            textBox2.Text = "";
                            textBox3.Text = "";

                        } else
                        {
                            MessageBox.Show("Nu s-au putut aduaga datele.");
                            textBox1.Text = "";
                            textBox2.Text = "";
                            textBox3.Text = "";
                        }
                    }
                } catch (Exception ex) { 
                    MessageBox.Show("Eroare: " + ex.Message);
                }
            }

            actualizare_date();
        }

        private void actualizare_date()
        {
            comboBox1.Items.Clear();

            this.noteTableAdapter.Fill(this.studentiDataSet.note);

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                try
                {
                    connection.Open();
                    string query = "SELECT id_student FROM note";

                    using(SqlCommand command = new SqlCommand(query, connection))
                    {
                        using(SqlDataReader reader = command.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                comboBox1.Items.Add(reader["id_student"]).ToString();
                            }
                        }
                    }
                } catch (Exception ex)
                {
                    MessageBox.Show("eroare " + ex.Message);
                }
            }
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: This line of code loads data into the 'studentiDataSet.note' table. You can move, or remove it, as needed.
            this.noteTableAdapter.Fill(this.studentiDataSet.note);
            actualizare_date(); 

        }

        private void button3_Click(object sender, EventArgs e)
        {
            actualizare_date();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            string selectedItem = comboBox1.Text;

            if (!int.TryParse(selectedItem,out int result)) {
                MessageBox.Show("It has to be an integer");
                return ;
            }

            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                try
                {
                    conn.Open();
                        string query = "DELETE FROM note WHERE id_student = @id";
                                      
                    using (SqlCommand command = new SqlCommand(query, conn))
                    {

                        command.Parameters.AddWithValue("@id", selectedItem);


                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Row deleted");
                            actualizare_date();

                        }
                        else
                        {
                            MessageBox.Show("Nu este un id valid");
                        }
                    }
                     
                } catch (Exception ex) {
                    MessageBox.Show("Eroare: " + ex.Message);
                }
            }
        }
    }
}
