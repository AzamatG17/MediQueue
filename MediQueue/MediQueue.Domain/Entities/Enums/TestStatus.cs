namespace MediQueue.Domain.Entities.Enums;

public enum TestStatus
{
    Pending = 0,      // Ожидание результата
    Completed = 1,    // Завершено
    InProgress = 2,   // В процессе
    Canceled = 3      // Отменено
}
