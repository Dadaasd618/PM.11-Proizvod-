using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using TonshaevaCRB_HospitalApp.Models;
using TonshaevaCRB_HospitalApp.Services;

namespace TonshaevaCRB_HospitalApp.Views
{
    public partial class PatientsView : Window
    {
        private DatabaseService dbService;
        private int selectedPatientId;
        private string userRole;

        public PatientsView(string role)
        {
            InitializeComponent();
            userRole = role;
            dbService = new DatabaseService();
            LoadPatients();
            ConfigureAccessByRole();
        }

        private void LoadPatients()
        {
            dgPatients.ItemsSource = dbService.GetPatients().DefaultView;
        }

        private void ConfigureAccessByRole()
        {
            switch (userRole)
            {
                case "Role_Registrator":
                    btnUpdate.IsEnabled = false;
                    btnDelete.IsEnabled = false;
                    txtRoleInfo.Text = "Режим: Регистратор - можно добавлять новых пациентов";
                    break;

                case "Role_Doctor":
                    btnAdd.IsEnabled = false;
                    btnUpdate.IsEnabled = false;
                    btnDelete.IsEnabled = false;
                    btnClear.IsEnabled = false;
                    txtRoleInfo.Text = "Режим: Врач - только просмотр пациентов";
                    break;

                case "Role_Nurse":
                    btnAdd.IsEnabled = false;
                    btnUpdate.IsEnabled = false;
                    btnDelete.IsEnabled = false;
                    btnClear.IsEnabled = false;
                    txtRoleInfo.Text = "Режим: Медсестра - только просмотр пациентов";
                    break;

                case "Role_Admin":
                    txtRoleInfo.Text = "Режим: Администратор - полный доступ (CRUD)";
                    break;

                default:
                    txtRoleInfo.Text = "Режим: ограниченный доступ";
                    break;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                dpBirthDate.SelectedDate == null ||
                cmbGender.SelectedItem == null ||
                string.IsNullOrWhiteSpace(txtPolicy.Text) ||
                string.IsNullOrWhiteSpace(txtAddress.Text))
            {
                MessageBox.Show("Заполните обязательные поля: Фамилия, Имя, Дата рождения, Пол, Номер полиса, Адрес",
                                "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Patient patient = new Patient
                {
                    Фамилия = txtLastName.Text.Trim(),
                    Имя = txtFirstName.Text.Trim(),
                    Отчество = string.IsNullOrWhiteSpace(txtMiddleName.Text) ? null : txtMiddleName.Text.Trim(),
                    ДатаРождения = dpBirthDate.SelectedDate.Value,
                    Пол = (cmbGender.SelectedItem as ComboBoxItem)?.Content.ToString(),
                    НомерПолиса = txtPolicy.Text.Trim(),
                    Адрес = txtAddress.Text.Trim(),
                    Телефон = string.IsNullOrWhiteSpace(txtPhone.Text) ? null : txtPhone.Text.Trim(),
                    Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim()
                };
                dbService.AddPatient(patient);
                LoadPatients();
                ClearForm();
                MessageBox.Show("Пациент добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPatientId == 0)
            {
                MessageBox.Show("Выберите пациента для обновления", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Patient patient = new Patient
                {
                    ID_Пациента = selectedPatientId,
                    Фамилия = txtLastName.Text.Trim(),
                    Имя = txtFirstName.Text.Trim(),
                    Отчество = string.IsNullOrWhiteSpace(txtMiddleName.Text) ? null : txtMiddleName.Text.Trim(),
                    ДатаРождения = dpBirthDate.SelectedDate.Value,
                    Пол = (cmbGender.SelectedItem as ComboBoxItem)?.Content.ToString(),
                    НомерПолиса = txtPolicy.Text.Trim(),
                    Адрес = txtAddress.Text.Trim(),
                    Телефон = string.IsNullOrWhiteSpace(txtPhone.Text) ? null : txtPhone.Text.Trim(),
                    Email = string.IsNullOrWhiteSpace(txtEmail.Text) ? null : txtEmail.Text.Trim()
                };
                dbService.UpdatePatient(patient);
                LoadPatients();
                ClearForm();
                MessageBox.Show("Пациент обновлён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedPatientId == 0)
            {
                MessageBox.Show("Выберите пациента для удаления", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show("Удалить выбранного пациента? Это действие нельзя отменить.",
                                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    dbService.DeletePatient(selectedPatientId);
                    LoadPatients();
                    ClearForm();
                    MessageBox.Show("Пациент удалён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

        private void btnClear_Click(object sender, RoutedEventArgs e)
        {
            ClearForm();
        }

        private void dgPatients_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (dgPatients.SelectedItem != null)
            {
                DataRowView row = (DataRowView)dgPatients.SelectedItem;
                selectedPatientId = Convert.ToInt32(row["ID_Пациента"]);
                txtLastName.Text = row["Фамилия"].ToString();
                txtFirstName.Text = row["Имя"].ToString();
                txtMiddleName.Text = row["Отчество"]?.ToString() ?? "";
                dpBirthDate.SelectedDate = Convert.ToDateTime(row["ДатаРождения"]);

                string gender = row["Пол"].ToString();
                cmbGender.SelectedItem = gender == "М" ? cmbGender.Items[0] : cmbGender.Items[1];

                txtPolicy.Text = row["НомерПолиса"].ToString();
                txtAddress.Text = row["Адрес"].ToString();
                txtPhone.Text = row["Телефон"]?.ToString() ?? "";
                txtEmail.Text = row["Email"]?.ToString() ?? "";
            }
        }

        private void ClearForm()
        {
            selectedPatientId = 0;
            txtLastName.Text = "";
            txtFirstName.Text = "";
            txtMiddleName.Text = "";
            dpBirthDate.SelectedDate = DateTime.Now.AddYears(-30);
            cmbGender.SelectedIndex = -1;
            txtPolicy.Text = "";
            txtAddress.Text = "";
            txtPhone.Text = "";
            txtEmail.Text = "";
        }
    }
}