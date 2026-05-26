using System;

namespace TonshaevaCRB_HospitalApp.Models
{
    public class Patient
    {
        public int ID_Пациента { get; set; }
        public string Фамилия { get; set; }
        public string Имя { get; set; }
        public string Отчество { get; set; }
        public DateTime ДатаРождения { get; set; }
        public string Пол { get; set; }
        public string НомерПолиса { get; set; }
        public string СНИЛС { get; set; }
        public string Телефон { get; set; }
        public string Адрес { get; set; }
        public string Email { get; set; }
    }

    public class Doctor
    {
        public int ID_Врача { get; set; }
        public string Фамилия { get; set; }
        public string Имя { get; set; }
        public string Отчество { get; set; }
        public string Специализация { get; set; }
        public string Категория { get; set; }
        public string НомерКабинета { get; set; }
        public string ВнутреннийТелефон { get; set; }
        public DateTime ДатаПриема { get; set; }
        public bool Уволен { get; set; }
    }

    public class Hospitalization
    {
        public int ID_Госпитализации { get; set; }
        public int ID_Пациента { get; set; }
        public string ПациентФИО { get; set; }
        public int ID_Врача { get; set; }
        public string ВрачФИО { get; set; }
        public int ID_Отделения { get; set; }
        public string Отделение { get; set; }
        public int ID_Палаты { get; set; }
        public string Палата { get; set; }
        public int НомерКойки { get; set; }
        public string ТипПоступления { get; set; }
        public string КодПредварительногоДиагноза { get; set; }
        public string Диагноз { get; set; }
        public DateTime ДатаВремяПоступления { get; set; }
        public DateTime? ДатаВыписки { get; set; }
        public string Исход { get; set; }
        public string НаправившееУчреждение { get; set; }
    }

    public class Service
    {
        public string КодУслуги { get; set; }
        public string НазваниеУслуги { get; set; }
        public string Категория { get; set; }
        public decimal? Стоимость { get; set; }
        public bool ПоОМС { get; set; }
    }
}