using FluentAssertions;
using TrackerApi.Infrastructure.Mongo.Models;

namespace TrackerApi.Proximity.Tests
{
    public class ProximityCalculatorTests
    {
        [Fact]

        public void ProximityCalculator_WhenProximityResultsAsViolation_ReturnTrue()
        {
            // arrange
            var latitude = 50f;
            var longitude = 15f;
            var zone = new Zone()
            {
                Points = new List<Coordinates>()
                {
                    new Coordinates(53f, 4f),
                    new Coordinates(54f, 30f),
                    new Coordinates(45f, 32f),
                    new Coordinates(44f, 2f),
                }
            };

            // act
            var response = ProximityCalculator.Calculate(latitude, longitude, zone);

            // assert
            response.Should().Be(true);
        }

        [Fact]
        public void ProximityCalculator_WhenProximityResultsAsNotViolation_ReturnFalse()
        {
            // arrange
            var latitude = 8f;
            var longitude = 17f;
            var zone = new Zone()
            {
                Points = new List<Coordinates>()
                {
                    new Coordinates(53f, 4f),
                    new Coordinates(54f, 30f),
                    new Coordinates(45f, 32f),
                    new Coordinates(44f, 2f),
                }
            };

            // act
            var response = ProximityCalculator.Calculate(latitude, longitude, zone);

            // assert
            response.Should().Be(false);
        }
    }
}