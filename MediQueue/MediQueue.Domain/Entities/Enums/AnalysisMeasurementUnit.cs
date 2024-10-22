namespace MediQueue.Domain.Entities.Enums;

public enum AnalysisMeasurementUnit
{
    MilligramsPerDeciliter = 0, // Мг/дл для анализов крови
    Millimeters = 1,            // Мм для MRT
    UnitsPerMilliliter = 2,     // Ед/мл для гормональных анализов
    Percent = 3,                // Процент
    Count = 4                   // Количество клеток
}
