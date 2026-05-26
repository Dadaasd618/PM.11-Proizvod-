using System;
using System.Windows;
using Microsoft.Data.SqlClient;
using System.Configuration;

namespace TonshaevaCRB_HospitalApp
{
    public partial class LoginWindow : Window
    {
        private string connectionString;

        public LoginWindow()
        {
            InitializeComponent();
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        private void btnLogin_Click(object sender, RoutedEventArgs e)
        {
            string login = txtLogin.Text.Trim();
            string password = txtPassword.Password;

            if (string.IsNullOrEmpty(login) || string.IsNullOrEmpty(password))
            {
                txtStatus.Text = "Введите логин и пароль!";
                return;
            }

            // Преобразуем логин в имя пользователя БД
            string dbUserName = GetDbUserName(login);

            if (dbUserName == null)
            {
                txtStatus.Text = "Неверный логин!";
                txtPassword.Clear();
                return;
            }

            // Проверяем пароль
            string userRole = AuthenticateUser(login, password, dbUserName);

            if (userRole != null)
            {
                txtStatus.Text = "Успешный вход! Загрузка...";
                MainWindow mainWindow = new MainWindow(userRole, login);
                mainWindow.Show();
                this.Close();
            }
            else
            {
                txtStatus.Text = "Неверный пароль!";
                txtPassword.Clear();
            }
        }

        private string GetDbUserName(string login)
        {
            switch (login)
            {
                case "RegistratorLogin":
                    return "RegistratorUser";
                case "DoctorLogin":
                    return "DoctorUser";
                case "NurseLogin":
                    return "NurseUser";
                case "AdminLogin":
                    return "AdminUser";
                default:
                    return null;
            }
        }

        private string GetRoleByDbUserName(string dbUserName)
        {
            switch (dbUserName)
            {
                case "RegistratorUser":
                    return "Role_Registrator";
                case "DoctorUser":
                    return "Role_Doctor";
                case "NurseUser":
                    return "Role_Nurse";
                case "AdminUser":
                    return "Role_Admin";
                default:
                    return null;
            }
        }

        private string AuthenticateUser(string login, string password, string dbUserName)
        {
            try
            {
                // Используем .\SQLEXPRESS - то же имя, что в App.config
                string testConnectionString = $"Server=.\\SQLEXPRESS;Database=TonshaevaCRB_Hospital;User Id={login};Password={password};TrustServerCertificate=True;";

                using (SqlConnection conn = new SqlConnection(testConnectionString))
                {
                    conn.Open();
                    // Если подключились — пароль верный
                    return GetRoleByDbUserName(dbUserName);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine("Ошибка авторизации: " + ex.Message);
                return null;
            }
        }
    }
}