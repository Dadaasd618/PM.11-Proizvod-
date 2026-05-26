using System;
using System.Data;
using Microsoft.Data.SqlClient;
using System.Configuration;
using TonshaevaCRB_HospitalApp.Models;

namespace TonshaevaCRB_HospitalApp.Services
{
    public class DatabaseService
    {
        private string connectionString;

        public DatabaseService()
        {
            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString;
        }

        // =====================================================
        // CRUD для Пациентов
        // =====================================================
        public DataTable GetPatients()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT [ID_Пациента], [Фамилия], [Имя], [Отчество], [ДатаРождения], [Пол], [НомерПолиса], [Телефон], [Адрес], [Email] FROM [Пациент] ORDER BY [Фамилия], [Имя]";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public void AddPatient(Patient patient)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO [Пациент] (Фамилия, Имя, Отчество, ДатаРождения, Пол, НомерПолиса, Адрес, Телефон, Email)
                                 VALUES (@Фамилия, @Имя, @Отчество, @ДатаРождения, @Пол, @НомерПолиса, @Адрес, @Телефон, @Email)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Фамилия", patient.Фамилия);
                cmd.Parameters.AddWithValue("@Имя", patient.Имя);
                cmd.Parameters.AddWithValue("@Отчество", (object)patient.Отчество ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ДатаРождения", patient.ДатаРождения);
                cmd.Parameters.AddWithValue("@Пол", patient.Пол);
                cmd.Parameters.AddWithValue("@НомерПолиса", patient.НомерПолиса);
                cmd.Parameters.AddWithValue("@Адрес", patient.Адрес);
                cmd.Parameters.AddWithValue("@Телефон", (object)patient.Телефон ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", (object)patient.Email ?? DBNull.Value);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdatePatient(Patient patient)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE [Пациент] SET Фамилия=@Фамилия, Имя=@Имя, Отчество=@Отчество, 
                                 ДатаРождения=@ДатаРождения, Пол=@Пол, НомерПолиса=@НомерПолиса, 
                                 Адрес=@Адрес, Телефон=@Телефон, Email=@Email 
                                 WHERE ID_Пациента=@ID_Пациента";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID_Пациента", patient.ID_Пациента);
                cmd.Parameters.AddWithValue("@Фамилия", patient.Фамилия);
                cmd.Parameters.AddWithValue("@Имя", patient.Имя);
                cmd.Parameters.AddWithValue("@Отчество", (object)patient.Отчество ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ДатаРождения", patient.ДатаРождения);
                cmd.Parameters.AddWithValue("@Пол", patient.Пол);
                cmd.Parameters.AddWithValue("@НомерПолиса", patient.НомерПолиса);
                cmd.Parameters.AddWithValue("@Адрес", patient.Адрес);
                cmd.Parameters.AddWithValue("@Телефон", (object)patient.Телефон ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@Email", (object)patient.Email ?? DBNull.Value);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeletePatient(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM [Пациент] WHERE ID_Пациента=@ID_Пациента";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID_Пациента", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // =====================================================
        // CRUD для Врачей
        // =====================================================
        public DataTable GetDoctors()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT [ID_Врача], [Фамилия], [Имя], [Отчество], [Специализация], [Категория], [НомерКабинета], [ВнутреннийТелефон], [ДатаПриема] FROM [Врач] WHERE [Уволен]=0 ORDER BY [Фамилия]";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public void AddDoctor(Doctor doctor)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO [Врач] (Фамилия, Имя, Отчество, Специализация, Категория, НомерКабинета, ВнутреннийТелефон, ДатаПриема, Уволен)
                                 VALUES (@Фамилия, @Имя, @Отчество, @Специализация, @Категория, @НомерКабинета, @ВнутреннийТелефон, @ДатаПриема, 0)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@Фамилия", doctor.Фамилия);
                cmd.Parameters.AddWithValue("@Имя", doctor.Имя);
                cmd.Parameters.AddWithValue("@Отчество", doctor.Отчество);
                cmd.Parameters.AddWithValue("@Специализация", doctor.Специализация);
                cmd.Parameters.AddWithValue("@Категория", (object)doctor.Категория ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@НомерКабинета", (object)doctor.НомерКабинета ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ВнутреннийТелефон", (object)doctor.ВнутреннийТелефон ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ДатаПриема", doctor.ДатаПриема);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateDoctor(Doctor doctor)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE [Врач] SET Фамилия=@Фамилия, Имя=@Имя, Отчество=@Отчество, 
                                 Специализация=@Специализация, Категория=@Категория, 
                                 НомерКабинета=@НомерКабинета, ВнутреннийТелефон=@ВнутреннийТелефон, ДатаПриема=@ДатаПриема 
                                 WHERE ID_Врача=@ID_Врача";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID_Врача", doctor.ID_Врача);
                cmd.Parameters.AddWithValue("@Фамилия", doctor.Фамилия);
                cmd.Parameters.AddWithValue("@Имя", doctor.Имя);
                cmd.Parameters.AddWithValue("@Отчество", doctor.Отчество);
                cmd.Parameters.AddWithValue("@Специализация", doctor.Специализация);
                cmd.Parameters.AddWithValue("@Категория", (object)doctor.Категория ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@НомерКабинета", (object)doctor.НомерКабинета ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ВнутреннийТелефон", (object)doctor.ВнутреннийТелефон ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ДатаПриема", doctor.ДатаПриема);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteDoctor(int id)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "UPDATE [Врач] SET Уволен=1 WHERE ID_Врача=@ID_Врача";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@ID_Врача", id);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        // =====================================================
        // CRUD для Госпитализаций
        // =====================================================
        public DataTable GetHospitalizations()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"SELECT h.[ID_Госпитализации], 
                                        p.[Фамилия] + ' ' + p.[Имя] + ' ' + ISNULL(p.[Отчество], '') AS Пациент,
                                        d.[Фамилия] + ' ' + d.[Имя] + ' ' + d.[Отчество] AS Врач,
                                        o.[НазваниеОтделения] AS Отделение,
                                        pa.[НомерПалаты] AS Палата,
                                        h.[НомерКойки], h.[ТипПоступления], 
                                        h.[ДатаВремяПоступления], h.[ДатаВыписки], ISNULL(h.[Исход], 'лечится') AS Исход
                                 FROM [Госпитализация] h
                                 LEFT JOIN [Пациент] p ON h.[ID_Пациента] = p.[ID_Пациента]
                                 LEFT JOIN [Врач] d ON h.[ID_Врача] = d.[ID_Врача]
                                 LEFT JOIN [Отделение] o ON h.[ID_Отделения] = o.[ID_Отделения]
                                 LEFT JOIN [Палата] pa ON h.[ID_Палаты] = pa.[ID_Палаты]
                                 ORDER BY h.[ДатаВремяПоступления] DESC";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        // =====================================================
        // CRUD для Услуг
        // =====================================================
        public DataTable GetServices()
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "SELECT [КодУслуги], [НазваниеУслуги], [Категория], [Стоимость], [ПоОМС] FROM [Услуга] ORDER BY [НазваниеУслуги]";
                SqlDataAdapter adapter = new SqlDataAdapter(query, conn);
                DataTable dt = new DataTable();
                adapter.Fill(dt);
                return dt;
            }
        }

        public void AddService(Service service)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"INSERT INTO [Услуга] (КодУслуги, НазваниеУслуги, Категория, Стоимость, ПоОМС)
                                 VALUES (@КодУслуги, @НазваниеУслуги, @Категория, @Стоимость, @ПоОМС)";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@КодУслуги", service.КодУслуги);
                cmd.Parameters.AddWithValue("@НазваниеУслуги", service.НазваниеУслуги);
                cmd.Parameters.AddWithValue("@Категория", service.Категория);
                cmd.Parameters.AddWithValue("@Стоимость", (object)service.Стоимость ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ПоОМС", service.ПоОМС);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void UpdateService(Service service)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = @"UPDATE [Услуга] SET НазваниеУслуги=@НазваниеУслуги, Категория=@Категория, 
                                 Стоимость=@Стоимость, ПоОМС=@ПоОМС WHERE КодУслуги=@КодУслуги";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@КодУслуги", service.КодУслуги);
                cmd.Parameters.AddWithValue("@НазваниеУслуги", service.НазваниеУслуги);
                cmd.Parameters.AddWithValue("@Категория", service.Категория);
                cmd.Parameters.AddWithValue("@Стоимость", (object)service.Стоимость ?? DBNull.Value);
                cmd.Parameters.AddWithValue("@ПоОМС", service.ПоОМС);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }

        public void DeleteService(string code)
        {
            using (SqlConnection conn = new SqlConnection(connectionString))
            {
                string query = "DELETE FROM [Услуга] WHERE КодУслуги=@КодУслуги";
                SqlCommand cmd = new SqlCommand(query, conn);
                cmd.Parameters.AddWithValue("@КодУслуги", code);
                conn.Open();
                cmd.ExecuteNonQuery();
            }
        }
    }
}