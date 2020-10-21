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

        [Fact]
        public void TestMultiplePoints()
        {
            // Arrange
            var day = new DayEntry(_userId, DateTime.Now);

            var point1 = new TimeEntry(day.Id, new DateTime(2020, 10, 17, 9, 1, 0));
            var point2 = new TimeEntry(day.Id, new DateTime(2020, 10, 17, 16, 42, 0));
            var point3 = new TimeEntry(day.Id, new DateTime(2020, 10, 17, 16, 44, 0));
            var point4 = new TimeEntry(day.Id, new DateTime(2020, 10, 17, 16, 48, 0));
            var point5 = new TimeEntry(day.Id, new DateTime(2020, 10, 17, 17, 49, 0));
            var point6 = new TimeEntry(day.Id, new DateTime(2020, 10, 17, 18, 2, 0));

            var lunchTime = new TimeSpan(1, 0, 0);
            var toleranceTime = new TimeSpan(0, 10, 0);
            var workingTime = new TimeSpan(8, 0, 0);
            var configuration = new Configuration(_userId, lunchTime, toleranceTime, workingTime);

            // Act
            day.AddPoint(point1, configuration);
            day.AddPoint(point2, configuration);
            day.AddPoint(point3, configuration);
            day.AddPoint(point4, configuration);
            day.AddPoint(point5, configuration);
            day.AddPoint(point6, configuration);

            // Test
            Assert.Equal(new TimeSpan(0, 0, 0), day.MissingTime);
            Assert.Equal(new TimeSpan(0, 0, 0), day.ExtraTime);
        }

    }
}
