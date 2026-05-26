using System;
using System.Data;
using System.Windows;
using System.Windows.Controls;
using TonshaevaCRB_HospitalApp.Models;
using TonshaevaCRB_HospitalApp.Services;

namespace TonshaevaCRB_HospitalApp.Views
{
    public partial class ServicesView : Window
    {
        private DatabaseService dbService;
        private string selectedServiceCode;
        private string userRole;

        public ServicesView(string role)
        {
            InitializeComponent();
            userRole = role;
            dbService = new DatabaseService();
            LoadServices();
            ConfigureAccessByRole();
        }

        private void LoadServices()
        {
            dgServices.ItemsSource = dbService.GetServices().DefaultView;
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
                    txtRoleInfo.Text = "Режим: Администратор - полный доступ к услугам (CRUD)";
                    break;

                case "Role_Doctor":
                    btnAdd.IsEnabled = false;
                    btnUpdate.IsEnabled = false;
                    btnDelete.IsEnabled = false;
                    btnClear.IsEnabled = false;
                    txtRoleInfo.Text = "Режим: Врач - только просмотр справочника услуг";
                    break;

                case "Role_Nurse":
                    btnAdd.IsEnabled = false;
                    btnUpdate.IsEnabled = false;
                    btnDelete.IsEnabled = false;
                    btnClear.IsEnabled = false;
                    txtRoleInfo.Text = "Режим: Медсестра - только просмотр справочника услуг";
                    break;

                default:
                    btnAdd.IsEnabled = false;
                    btnUpdate.IsEnabled = false;
                    btnDelete.IsEnabled = false;
                    btnClear.IsEnabled = false;
                    txtRoleInfo.Text = "Режим: ограниченный доступ";
                    break;
            }
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(txtCode.Text) ||
                string.IsNullOrWhiteSpace(txtName.Text) ||
                cmbCategory.SelectedItem == null)
            {
                MessageBox.Show("Заполните обязательные поля: Код услуги, Название услуги, Категория",
                                "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Service service = new Service
                {
                    КодУслуги = txtCode.Text.Trim(),
                    НазваниеУслуги = txtName.Text.Trim(),
                    Категория = (cmbCategory.SelectedItem as ComboBoxItem)?.Content.ToString(),
                    Стоимость = string.IsNullOrWhiteSpace(txtPrice.Text) ? (decimal?)null : decimal.Parse(txtPrice.Text),
                    ПоОМС = chkIsCompulsory.IsChecked ?? true
                };
                dbService.AddService(service);
                LoadServices();
                ClearForm();
                MessageBox.Show("Услуга добавлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnUpdate_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(selectedServiceCode))
            {
                MessageBox.Show("Выберите услугу для обновления", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            try
            {
                Service service = new Service
                {
                    КодУслуги = selectedServiceCode,
                    НазваниеУслуги = txtName.Text.Trim(),
                    Категория = (cmbCategory.SelectedItem as ComboBoxItem)?.Content.ToString(),
                    Стоимость = string.IsNullOrWhiteSpace(txtPrice.Text) ? (decimal?)null : decimal.Parse(txtPrice.Text),
                    ПоОМС = chkIsCompulsory.IsChecked ?? true
                };
                dbService.UpdateService(service);
                LoadServices();
                ClearForm();
                MessageBox.Show("Услуга обновлена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка: " + ex.Message, "Ошибка", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnDelete_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(selectedServiceCode))
            {
                MessageBox.Show("Выберите услугу для удаления", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            if (MessageBox.Show("Удалить выбранную услугу? Это действие нельзя отменить.",
                                "Подтверждение", MessageBoxButton.YesNo, MessageBoxImage.Question) == MessageBoxResult.Yes)
            {
                try
                {
                    dbService.DeleteService(selectedServiceCode);
                    LoadServices();
                    ClearForm();
                    MessageBox.Show("Услуга удалена!", "Успех", MessageBoxButton.OK, MessageBoxImage.Information);
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

        private void dgServices_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (dgServices.SelectedItem != null)
            {
                DataRowView row = (DataRowView)dgServices.SelectedItem;
                selectedServiceCode = row["КодУслуги"].ToString();
                txtCode.Text = row["КодУслуги"].ToString();
                txtName.Text = row["НазваниеУслуги"].ToString();

                string category = row["Категория"].ToString();
                for (int i = 0; i < cmbCategory.Items.Count; i++)
                {
                    if ((cmbCategory.Items[i] as ComboBoxItem)?.Content.ToString() == category)
                    {
                        cmbCategory.SelectedIndex = i;
                        break;
                    }
                }

                txtPrice.Text = row["Стоимость"]?.ToString() ?? "";
                chkIsCompulsory.IsChecked = Convert.ToBoolean(row["ПоОМС"]);
            }
        }

        private void ClearForm()
        {
            selectedServiceCode = null;
            txtCode.Text = "";
            txtName.Text = "";
            cmbCategory.SelectedIndex = -1;
            txtPrice.Text = "";
            chkIsCompulsory.IsChecked = true;
        }
    }
}