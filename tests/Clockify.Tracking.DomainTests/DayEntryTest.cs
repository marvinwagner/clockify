using Clockify.Tracking.Domain.Models;
using System;
using Xunit;

namespace Clockify.Tracking.DomainTests
{
    public class DayEntryTest
    {
        private static Guid _userId = Guid.NewGuid();
        private readonly Configuration configuration;
       
        public DayEntryTest()
        {
            var lunchTime = new TimeSpan(1, 0, 0);
            var toleranceTime = new TimeSpan(0, 5, 0);
            var workingTime = new TimeSpan(4, 0, 0);
            configuration = new Configuration(_userId, lunchTime, toleranceTime, workingTime);
        }

        [Fact]
        public void TestExtraTime_NineMinutes()
        {
            // Arrange
            var day = new DayEntry(_userId, DateTime.Now);

            var point1 = new TimeEntry(day.Id, new DateTime(2020, 10, 1, 8, 0, 0));
            var point2 = new TimeEntry(day.Id, new DateTime(2020, 10, 1, 12, 9, 0));

            // Act
            day.AddPoint(point1, configuration);
            day.AddPoint(point2, configuration);

            // Test
            Assert.Equal(new TimeSpan(0, 9, 0), day.ExtraTime);
            Assert.Equal(new TimeSpan(0, 0, 0), day.MissingTime);
        }

        [Fact]
        public void TestMissingTime_NineMinutes()
        {
            // Arrange
            var day = new DayEntry(_userId, DateTime.Now);

            var point1 = new TimeEntry(day.Id, new DateTime(2020, 10, 1, 8, 0, 0));
            var point2 = new TimeEntry(day.Id, new DateTime(2020, 10, 1, 11, 51, 0));

            // Act
            day.AddPoint(point1, configuration);
            day.AddPoint(point2, configuration);

            // Test
            Assert.Equal(new TimeSpan(0, 9, 0), day.MissingTime);
            Assert.Equal(new TimeSpan(0, 0, 0), day.ExtraTime);
        }

        [Fact]
        public void TestDefaultTime_NoExtraNoMissing()
        {
            // Arrange
            var day = new DayEntry(_userId, DateTime.Now);

            var point1 = new TimeEntry(day.Id, new DateTime(2020, 10, 1, 8, 0, 0));
            var point2 = new TimeEntry(day.Id, new DateTime(2020, 10, 1, 11, 56, 0));

            // Act
            day.AddPoint(point1, configuration);
            day.AddPoint(point2, configuration);

            // Test
            Assert.Equal(new TimeSpan(0, 0, 0), day.MissingTime);
            Assert.Equal(new TimeSpan(0, 0, 0), day.ExtraTime);
        }
    }
}
