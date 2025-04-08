namespace BookingsLibrary;

using PublicHoliday;
using NodaTime;

/// <summary>
/// Class <c>Bookings</c> offers the first available date for making a booking according to Australian public holidays and company policy.
/// </summary>
public class Bookings
{
    /// <summary>
    /// Maximum number of days before the next public holiday in which the booking in made the day after the requested.
    /// </summary>
    public const int PublicHolidayDaysOffset = 2;
    public const int NextDay = 1;
    /// <summary>
    /// 12pm hour.
    /// </summary>
    public const int AfternoonTime = 12;

    /// <summary>
    /// PublicHoliday object whith the public holidays calendar from certain state.
    /// </summary>
    public AustraliaPublicHoliday AustralianCalendar { get; private set; }

    /// <summary>
    /// NodaTime object which contains the desired current date and time.
    /// </summary>
    public Instant CurrentInstant { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="Bookings"/> class.
    /// </summary>
    /// <param name="state">Australia state where the supplier is located (necessary for public holidays).</param>
    /// <param name="clock">Injection of desired clock for retrieving the current date and time (usually for test purposes).</param>
    public Bookings(AustraliaPublicHoliday.States state, IClock clock)
    {
        AustralianCalendar = new AustraliaPublicHoliday { State = state };
        CurrentInstant = clock.GetCurrentInstant();
    }

    /// <summary>
    /// Method <c>RequestBooking</c> calculates the first available date to make a booking.
    /// </summary>
    /// <returns>First available date to make a booking.</returns>
    public DateTime RequestBooking()
    {
        DateTime currentTime = CurrentInstant.ToDateTimeUtc();

        IList<Holiday> holsWithinTwoDays = AustralianCalendar.GetHolidaysInDateRange(currentTime, currentTime.AddDays(PublicHolidayDaysOffset));

        if (holsWithinTwoDays.Any())
        {
            bool requestedAfterCutoff = currentTime.Hour >= AfternoonTime || (currentTime.AddDays(NextDay).Date == holsWithinTwoDays.First().HolidayDate);
            currentTime = AustralianCalendar.NextWorkingDay(holsWithinTwoDays.First());
            if (!requestedAfterCutoff)
            {
                return currentTime;
            }
        }

        return currentTime.AddDays(NextDay);
    }
}