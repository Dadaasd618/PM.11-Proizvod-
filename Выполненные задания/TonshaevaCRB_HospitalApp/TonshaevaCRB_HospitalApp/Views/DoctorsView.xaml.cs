using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using TonshaevaCRB_HospitalApp.Models;
using TonshaevaCRB_HospitalApp.Services;

namespace TonshaevaCRB_HospitalApp.Views
{
    public partial class DoctorsView : Window
    {
        private DatabaseService dbService;
        private int selectedDoctorId;
        private string userRole;

        public DoctorsView(string role)
        {
            InitializeComponent();
            userRole = role;
            dbService = new DatabaseService();
            LoadDoctors();
            ConfigureAccessByRole();
        }

        private void LoadDoctors()
        {
            dgDoctors.ItemsSource = dbService.GetDoctors().DefaultView;
        }

        private void ConfigureAccessByRole()
        {
            switch (userRole)
            {
                case "Role_Admin":
                    btnAdd.IsEnabled = true;
                    btnUpdate.IsEnabled = true;
                    btnDelete.IsEnabled = true;
                    btnClear.IsEnabled = true;
                    txtRoleInfo.Text = "Режим: Администратор - полный доступ (CRUD)";
                    break;

                case "Role_Doctor":
                    btnAdd.IsEnabled = false;
                    btnUpdate.IsEnabled = false;
                    btnDelete.IsEnabled = false;
                    btnClear.IsEnabled = false;
                    txtRoleInfo.Text = "Режим: Врач - только просмотр списка врачей";
                    break;

                default:
                    btnAdd.IsEnabled = false;
                    btnUpdate.IsEnabled = false;
                    btnDelete.IsEnabled = false;
                    btnClear.IsEnabled = false;
                    txtRoleInfo.Text = "Режим: ограниченный доступ - только просмотр";
                    break;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtLastName.Text) ||
                string.IsNullOrWhiteSpace(txtFirstName.Text) ||
                string.IsNullOrWhiteSpace(txtMiddleName.Text) ||
                string.IsNullOrWhiteSpace(txtSpecialization.Text) ||
                dpHireDate.SelectedDate == null)
            {
                MessageBox.Show("Заполните обязательные поля: Фамилия, Имя, Отчество, Специализация, Дата приёма",
                                "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Doctor doctor = new Doctor
                {
                    Фамилия = txtLastName.Text.Trim(),
                    Имя = txtFirstName.Text.Trim(),
                    Отчество = txtMiddleName.Text.Trim(),
                    Специализация = txtSpecialization.Text.Trim(),
                    Категория = (cmbCategory.SelectedItem as ComboBoxItem)?.Content.ToString(),
                    НомерКабинета = string.IsNullOrWhiteSpace(txtOfficeNumber.Text) ? null : txtOfficeNumber.Text.Trim(),
                    ВнутреннийТелефон = string.IsNullOrWhiteSpace(txtPhoneInternal.Text) ? null : txtPhoneInternal.Text.Trim(),
                    ДатаПриема = dpHireDate.SelectedDate.Value
                };
                dbService.AddDoctor(doctor);
                LoadDoctors();
                ClearForm();
                MessageBox.Show("Врач добавлен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (selectedDoctorId == 0)
            {
                MessageBox.Show("Выберите врача для обновления", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Doctor doctor = new Doctor
                {
                    ID_Врача = selectedDoctorId,
                    Фамилия = txtLastName.Text.Trim(),
                    Имя = txtFirstName.Text.Trim(),
                    Отчество = txtMiddleName.Text.Trim(),
                    Специализация = txtSpecialization.Text.Trim(),
                    Категория = (cmbCategory.SelectedItem as ComboBoxItem)?.Content.ToString(),
                    НомерКабинета = string.IsNullOrWhiteSpace(txtOfficeNumber.Text) ? null : txtOfficeNumber.Text.Trim(),
                    ВнутреннийТелефон = string.IsNullOrWhiteSpace(txtPhoneInternal.Text) ? null : txtPhoneInternal.Text.Trim(),
                    ДатаПриема = dpHireDate.SelectedDate.Value
                };
                dbService.UpdateDoctor(doctor);
                LoadDoctors();
                ClearForm();
                MessageBox.Show("Врач обновлён!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (selectedDoctorId == 0)
            {
                MessageBox.Show("Выберите врача для увольнения", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show("Уволить выбранного врача? (Он будет помечен как уволенный)",
                                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    dbService.DeleteDoctor(selectedDoctorId);
                    LoadDoctors();
                    ClearForm();
                    MessageBox.Show("Врач уволен!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void dgDoctors_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (dgDoctors.SelectedItem != null)
            {
                DataRowView row = (DataRowView)dgDoctors.SelectedItem;
                selectedDoctorId = Convert.ToInt32(row["ID_Врача"]);
                txtLastName.Text = row["Фамилия"].ToString();
                txtFirstName.Text = row["Имя"].ToString();
                txtMiddleName.Text = row["Отчество"].ToString();
                txtSpecialization.Text = row["Специализация"].ToString();

                string category = row["Категория"].ToString();
                if (!string.IsNullOrEmpty(category))
                {
                    for (int i = 0; i < cmbCategory.Items.Count; i++)
                    {
                        if ((cmbCategory.Items[i] as ComboBoxItem)?.Content.ToString() == category)
                        {
                            cmbCategory.SelectedIndex = i;
                            break;
                        }
                    }
                }
                dpHireDate.SelectedDate = Convert.ToDateTime(row["ДатаПриема"]);
                txtOfficeNumber.Text = row["НомерКабинета"]?.ToString() ?? "";
                txtPhoneInternal.Text = row["ВнутреннийТелефон"]?.ToString() ?? "";
            }
        }

        private void ClearForm()
        {
            selectedDoctorId = 0;
            txtLastName.Text = "";
            txtFirstName.Text = "";
            txtMiddleName.Text = "";
            txtSpecialization.Text = "";
            cmbCategory.SelectedIndex = -1;
            txtOfficeNumber.Text = "";
            txtPhoneInternal.Text = "";
            dpHireDate.SelectedDate = DateTime.Now;
        }
    }
}