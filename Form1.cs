using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace _22_day
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        string connectionString = Properties.Settings.Default.ConfectioneryDBConnectionString;



  

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            // Создаем экземпляр OpenFileDialog
            OpenFileDialog openFileDialog = new OpenFileDialog();

            // Устанавливаем фильтры файлов
            openFileDialog.Filter = "Изображения (*.jpg; *.jpeg; *.png; *.bmp)|*.jpg; *.jpeg; *.png; *.bmp|Все файлы (*.*)|*.*";
            openFileDialog.FilterIndex = 1; // Устанавливаем индекс выбранного фильтра
            openFileDialog.Multiselect = false; // Запрещаем выбор нескольких файлов

            // Открываем диалоговое окно выбора файла
            DialogResult result = openFileDialog.ShowDialog();

            // Если пользователь выбрал файл и нажал "OK"
            if (result == DialogResult.OK)
            {
                // Получаем путь к выбранному файлу
                string filePath = openFileDialog.FileName;

                try
                {
                    // Загружаем изображение в PictureBox
                    pictureBox1.Image = Image.FromFile(filePath);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка загрузки изображения: " + ex.Message);
                }
            }
        }
        

        private void button1_Click(object sender, EventArgs e)
        {
            // Проверка наличия названия продукта
            if (string.IsNullOrWhiteSpace(textBox1.Text))
            {
                MessageBox.Show("Введите название продукта.");
                return;
            }

            // Проверка наличия цены
            if (!decimal.TryParse(textBox3.Text, out decimal price))
            {
                MessageBox.Show("Введите корректную цену.");
                return;
            }

            try
            {
                // Чтение данных из текстовых полей и изображения из PictureBox
                string name = textBox1.Text;
                string description = textBox2.Text;
                byte[] image = null;
                if (pictureBox1.Image != null)
                {
                    using (MemoryStream ms = new MemoryStream())
                    {
                        pictureBox1.Image.Save(ms, pictureBox1.Image.RawFormat);
                        image = ms.ToArray();
                    }
                }

                // Создание подключения и SQL-запроса
                using (SqlConnection connection = new SqlConnection(connectionString))
                {
                    connection.Open();

                    // SQL-запрос для вставки новой записи в таблицу Продукты
                    string query = "INSERT INTO Products (ProductName, Description, Price, Image) VALUES (@ProductName, @Description, @Price, @Image)";
                    using (SqlCommand command = new SqlCommand(query, connection))
                    {
                        // Добавление параметров к запросу
                        command.Parameters.AddWithValue("@ProductName", name);
                        command.Parameters.AddWithValue("@Description", description);
                        command.Parameters.AddWithValue("@Price", price);
                        command.Parameters.AddWithValue("@Image", image ?? (object)DBNull.Value);

                        // Выполнение запроса
                        int rowsAffected = command.ExecuteNonQuery();
                        if (rowsAffected > 0)
                        {
                            MessageBox.Show("Продукт успешно добавлен.");
                            // Очистка полей после успешного добавления
                            ClearFields();
                        }
                        else
                        {
                            MessageBox.Show("Произошла ошибка при добавлении продукта.");
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при добавлении продукта: " + ex.Message);
            }
        }
        private void ClearFields()
        {
            textBox1.Text = "";
            textBox2.Text = "";
            textBox3.Text = "";
            pictureBox1.Image = null;
        }

        private void productsBindingNavigatorSaveItem_Click(object sender, EventArgs e)
        {
            this.Validate();
            this.productsBindingSource.EndEdit();
            this.tableAdapterManager.UpdateAll(this.confectioneryDBDataSet);

        }

        private void Form1_Load(object sender, EventArgs e)
        {
            // TODO: данная строка кода позволяет загрузить данные в таблицу "confectioneryDBDataSet.Products". При необходимости она может быть перемещена или удалена.
            this.productsTableAdapter.Fill(this.confectioneryDBDataSet.Products);

        }

        private void ordersToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Form2 frm = new Form2();
            frm.Show();
        }
    }
}
