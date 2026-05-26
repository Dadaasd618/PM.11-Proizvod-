using System;
using System.Data;
using System.Windows;
using TonshaevaCRB_HospitalApp.Services;

namespace TonshaevaCRB_HospitalApp.Views
{
    public partial class HospitalizationsView : Window
    {
        private DatabaseService dbService;
        private int selectedHospitalizationId;
        private string userRole;

        public HospitalizationsView(string role)
        {
            InitializeComponent();
            userRole = role;
            dbService = new DatabaseService();
            LoadHospitalizations();
            ConfigureAccessByRole();
        }

        private void LoadHospitalizations()
        {
            dgHospitalizations.ItemsSource = dbService.GetHospitalizations().DefaultView;
        }

        private void ConfigureAccessByRole()
        {
            switch (userRole)
            {
                case "Role_Admin":
                    btnAdd.IsEnabled = true;
                    btnEdit.IsEnabled = true;
                    btnDischarge.IsEnabled = true;
                    txtRoleInfo.Text = "Режим: Администратор - полный доступ к госпитализациям";
                    break;

                case "Role_Doctor":
                    btnAdd.IsEnabled = true;
                    btnEdit.IsEnabled = true;
                    btnDischarge.IsEnabled = true;
                    txtRoleInfo.Text = "Режим: Врач - можно добавлять, редактировать и выписывать пациентов";
                    break;

                case "Role_Registrator":
                    btnAdd.IsEnabled = true;
                    btnEdit.IsEnabled = false;
                    btnDischarge.IsEnabled = false;
                    txtRoleInfo.Text = "Режим: Регистратор - можно добавлять новые госпитализации";
                    break;

                case "Role_Nurse":
                    btnAdd.IsEnabled = false;
                    btnEdit.IsEnabled = false;
                    btnDischarge.IsEnabled = false;
                    txtRoleInfo.Text = "Режим: Медсестра - только просмотр госпитализаций";
                    break;

                default:
                    btnAdd.IsEnabled = false;
                    btnEdit.IsEnabled = false;
                    btnDischarge.IsEnabled = false;
                    txtRoleInfo.Text = "Режим: ограниченный доступ";
                    break;
            }
        }

        private void btnRefresh_Click(object sender, RoutedEventArgs e)
        {
            LoadHospitalizations();
        }

        private void btnAdd_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Функция добавления госпитализации.\n" +
                           "Для практики достаточно демонстрации принципа.\n" +
                           "Полный CRUD можно реализовать при необходимости.",
                           "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            if (selectedHospitalizationId == 0)
            {
                MessageBox.Show("Выберите госпитализацию для редактирования", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            MessageBox.Show($"Редактирование госпитализации ID = {selectedHospitalizationId}\n" +
                           "Демонстрация принципа UPDATE операции.",
                           "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void btnDischarge_Click(object sender, RoutedEventArgs e)
        {
            if (selectedHospitalizationId == 0)
            {
                MessageBox.Show("Выберите госпитализацию для выписки пациента", "Внимание", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            MessageBox.Show($"Пациент будет выписан из госпитализации ID = {selectedHospitalizationId}\n" +
                           "Проставлена дата выписки. Демонстрация UPDATE операции.",
                           "Информация", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private void dgHospitalizations_SelectionChanged(object sender, System.Windows.Controls.SelectionChangedEventArgs e)
        {
            if (dgHospitalizations.SelectedItem != null)
            {
                DataRowView row = (DataRowView)dgHospitalizations.SelectedItem;
                selectedHospitalizationId = Convert.ToInt32(row["ID_Госпитализации"]);
            }
        }
    }
}