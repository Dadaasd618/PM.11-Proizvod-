using System.Windows;
using TonshaevaCRB_HospitalApp.Views;

namespace TonshaevaCRB_HospitalApp
{
    public partial class MainWindow : Window
    {
        private string userRole;
        private string userName;

        public MainWindow(string role, string login)
        {
            InitializeComponent();
            userRole = role;
            userName = login;

            Title = $"Тоншаевская ЦРБ - {userName} ({GetRoleDisplayName(role)})";
            ConfigureAccessByRole();
        }

        private string GetRoleDisplayName(string role)
        {
            switch (role)
            {
                case "Role_Registrator": return "Регистратор";
                case "Role_Doctor": return "Врач";
                case "Role_Nurse": return "Медсестра";
                case "Role_Admin": return "Администратор";
                default: return role;
            }
        }

        private void ConfigureAccessByRole()
        {
            switch (userRole)
            {
                case "Role_Registrator":
                    btnDoctors.Visibility = Visibility.Collapsed;
                    btnServices.Visibility = Visibility.Collapsed;
                    break;

                case "Role_Doctor":
                    btnServices.Visibility = Visibility.Collapsed;
                    break;

                case "Role_Nurse":
                    btnPatients.Visibility = Visibility.Collapsed;
                    btnDoctors.Visibility = Visibility.Collapsed;
                    break;

                case "Role_Admin":
                    // Все кнопки видны
                    break;

                default:
                    btnPatients.Visibility = Visibility.Collapsed;
                    btnDoctors.Visibility = Visibility.Collapsed;
                    btnHospitalizations.Visibility = Visibility.Collapsed;
                    btnServices.Visibility = Visibility.Collapsed;
                    break;
            }
        }

        private void btnPatients_Click(object sender, RoutedEventArgs e)
        {
            PatientsView window = new PatientsView(userRole);
            window.ShowDialog();
        }

        private void btnDoctors_Click(object sender, RoutedEventArgs e)
        {
            DoctorsView window = new DoctorsView(userRole);
            window.ShowDialog();
        }

        private void btnHospitalizations_Click(object sender, RoutedEventArgs e)
        {
            HospitalizationsView window = new HospitalizationsView(userRole);
            window.ShowDialog();
        }

        private void btnServices_Click(object sender, RoutedEventArgs e)
        {
            ServicesView window = new ServicesView(userRole);
            window.ShowDialog();
        }
    }
}