namespace BookingsLibraryUTests;

using BookingsLibrary;
using PublicHoliday;
using NodaTime;
using NodaTime.Testing;
using NodaTime.Text;

[TestClass]
public sealed class BookingsUTests
{
    [TestMethod]
    [DataRow("2025-04-15T09:00:00Z", "2025-04-16")]
    [DataRow("2025-04-15T11:59:00Z", "2025-04-16")]
    [DataRow("2025-04-15T12:00:00Z", "2025-04-16")]
    [DataRow("2025-04-16T11:59:00Z", "2025-04-22")]
    [DataRow("2025-04-16T12:00:00Z", "2025-04-23")]
    [DataRow("2025-04-16T12:01:00Z", "2025-04-23")]
    [DataRow("2025-04-17T09:00:00Z", "2025-04-23")]
    [DataRow("2025-04-17T12:00:00Z", "2025-04-23")]
    public void TestRequestBooking(string input, string output)
    {
        DateTime outputDate = DateTime.Parse(output);
        Instant result = InstantPattern.General.Parse(input).Value;
        Bookings BookingsInstance = new Bookings(AustraliaPublicHoliday.States.QLD, new FakeClock(result));

        Assert.AreEqual<DateTime>(outputDate.Date, BookingsInstance.RequestBooking().Date);
    }
}
