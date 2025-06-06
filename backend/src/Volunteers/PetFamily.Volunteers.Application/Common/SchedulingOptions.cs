namespace PetFamily.Volunteers.Application.Common;

public class SchedulingOptions
{
    /// <summary>
    ///     Количество дней, после истечения которых волонтеры, помеченные как удаленные, полностью удаляются из базы данных.
    /// </summary>
    public int DeleteVolunteersExpirationDays { get; set; } = 30;

    /// <summary>
    ///     Интервал времени в часах между запусками фоновой задачи.
    /// </summary>
    public int ScanFrequencyInHours { get; set; } = 24;
}
