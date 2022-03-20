using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SqlDB
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            panel1.Hide();
            SettingsBtn.BackColor = panel1.BackColor;

            // Сохраненные настройки //
            ServerBox.Text = Properties.Settings.Default.SettingsSRV;
            PortBox.Text = Properties.Settings.Default.SettingsPort;
            UsernameBox.Text = Properties.Settings.Default.SettingsLogin;
            PasswordBox.Text = Properties.Settings.Default.SettingsPassword;
            DatabaseBox.Text = Properties.Settings.Default.SettingsDB;
            checkBox1.Checked = Properties.Settings.Default.SettingsAutoClear;

            // Для удобного отображения полей //
            this.Controls.Add(textBox1);
            this.Controls.Add(label16);
            this.Controls.Add(buttonAdd);
            this.Controls.Add(comboBox2);
            this.Controls.Add(comboBox3);
            this.Controls.Add(labelVersion);

            labelVersion.Text = "Version 1.0 сука";
        }

        private void buttonAdd_Click_1(object sender, EventArgs e)
        {
            MySqlConnection connection = new MySqlConnection($"server={Properties.Settings.Default.SettingsSRV};port={Properties.Settings.Default.SettingsPort};username={Properties.Settings.Default.SettingsLogin};password={Properties.Settings.Default.SettingsPassword};database={Properties.Settings.Default.SettingsDB}");
            //"server=localhost;port=3306;username=root;password=;database=sqldb"
            void openConnection()
            {
                if (connection.State == System.Data.ConnectionState.Closed)
                {
                    connection.Open();
                }
            }

            void closeConnection()
            {
                if (connection.State == System.Data.ConnectionState.Open)
                {
                    connection.Close();
                }
            }

            MySqlConnection getConnection()
            {
                return connection;
            }

            // Сбор информации с заполненных полей //

            string date = dateTimePicker1.Text;
            string subscriber_number = textBox2.Text;
            string name = textBox3.Text;
            string address = textBox4.Text;
            string contact_person = textBox5.Text;
            string technician = comboBox3.Text;
            string mark = comboBox2.Text;
            string malfunction = textBox8.Text;
            string replacement = comboBox1.Text;
            string note = textBox1.Text;

            DB db = new DB();

            //MySqlCommand command = new MySqlCommand("INSERT INTO `work` (`Дата`, `№ Абонента`, `Наименование`) VALUES (@date, @subscriber_number, @name);", db.getConnection());

            try
            {
                MySqlCommand command = new MySqlCommand("INSERT INTO `work` (`Дата`, `№ Абонента`, `Наименование`, `Адрес`, `Контактное лицо`, `Техник`, `Отметка о выполнении`, `Неисправность`, `Замена оборудования`, `Примечание`) VALUES (@date, @subscriber_number, @name, @address, @contact_person, @technician, @mark, @malfunction, @replacement, @note);", getConnection());
                command.Parameters.Add("@date", MySqlDbType.VarChar).Value = date;
                command.Parameters.Add("@subscriber_number", MySqlDbType.VarChar).Value = subscriber_number;
                command.Parameters.Add("@name", MySqlDbType.VarChar).Value = name;
                command.Parameters.Add("@address", MySqlDbType.VarChar).Value = address;
                command.Parameters.Add("@contact_person", MySqlDbType.VarChar).Value = contact_person;
                command.Parameters.Add("@technician", MySqlDbType.VarChar).Value = technician;
                command.Parameters.Add("@mark", MySqlDbType.VarChar).Value = mark;
                command.Parameters.Add("@malfunction", MySqlDbType.VarChar).Value = malfunction;
                command.Parameters.Add("@replacement", MySqlDbType.VarChar).Value = replacement;
                command.Parameters.Add("@note", MySqlDbType.VarChar).Value = note;

                openConnection();
                if (command.ExecuteNonQuery() == 1)
                {
                    // Успешная отправка //

                    MessageBox.Show("Запись Добавлена", "Успешно", MessageBoxButtons.OK, MessageBoxIcon.Information);


                    // Автоочистка //

                    if (Properties.Settings.Default.SettingsAutoClear == true) 
                    {
                        textBox1.Text = "";textBox2.Text = "";textBox3.Text = "";
                        textBox4.Text = "";textBox5.Text = "";textBox8.Text = "";comboBox1.Text = "";
                        comboBox2.Text = "";comboBox3.Text = "";
                    }
                }

                // Поля базы данных не совпадают с полями программы //

                else
                {
                    MessageBox.Show("Ошибка создания записи", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
                closeConnection();
            }


            // Ошибка подключения к базе данных //

            catch 
            {
                MessageBox.Show("Не удалось подключиться к базе данных", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }

            
        }

        // Отображение панели настроек //
        private void SettingsBtn_Click(object sender, EventArgs e)
        {
            panel1.Visible = true;
            
        }

        // Сохранение настроек //
        private void SaveSettingsBtn_Click(object sender, EventArgs e)
        {
            Properties.Settings.Default.SettingsSRV = ServerBox.Text;
            Properties.Settings.Default.SettingsPort = PortBox.Text;
            Properties.Settings.Default.SettingsLogin = UsernameBox.Text;
            Properties.Settings.Default.SettingsPassword = PasswordBox.Text;
            Properties.Settings.Default.SettingsDB = DatabaseBox.Text;
            Properties.Settings.Default.SettingsAutoClear = checkBox1.Checked;
            Properties.Settings.Default.Save();

            panel1.Hide();
        }

        // Восстановление последних сохраненных настроек //
        private void CancelSettings_Click(object sender, EventArgs e)
        {
            ServerBox.Text = Properties.Settings.Default.SettingsSRV;
            PortBox.Text = Properties.Settings.Default.SettingsPort;
            UsernameBox.Text = Properties.Settings.Default.SettingsLogin;
            PasswordBox.Text = Properties.Settings.Default.SettingsPassword;
            DatabaseBox.Text = Properties.Settings.Default.SettingsDB;
            checkBox1.Checked = Properties.Settings.Default.SettingsAutoClear;

            panel1.Hide();
        }

        // Отображение кнопки отмены настроек //

        private void CancelSettings_MouseMove(object sender, MouseEventArgs e)
        {
            Font myFont = new Font(CancelSettings.Font,FontStyle.Underline);
            CancelSettings.Font = myFont;
        }

        private void CancelSettings_MouseLeave(object sender, EventArgs e)
        {
            Font myFont = new Font(CancelSettings.Font, FontStyle.Regular);
            CancelSettings.Font = myFont;
        }


    }
}
