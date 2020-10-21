using Clockify.Tracking.Domain.Models;
using System;
using Xunit;

namespace Clockify.Tracking.DomainTests
{
    public class TimeEntryTest
    {
        [Fact]
        public void TestNoSecondsOnTime()
        {
            // Arrange
            var date = DateTime.Now;
            var day = new TimeEntry(Guid.NewGuid(), date);

            // Test
            Assert.Equal(0, day.Date.Second);
            Assert.Equal(0, day.Date.Millisecond);
        }
    }
}
