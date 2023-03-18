using cah.models;
using cah.services.repositories;
using cah.services.services;
using FluentAssertions;

namespace cah.tests
{
    public class CardRepositoryTests
    {


        /// <summary>
        /// Selects a random black card from the specified set
        /// </summary>
        /// <returns></returns>
        [Theory]
        [InlineData("CAH Base Set")]
        public async Task GetRandomBlackCard__ShouldReturnARandomBlackCard(string setName)
        {

            // Arrange
            var _sut = new CardsRepository();


            // Act
            var response = await _sut.GetRandomBlackCard(setName);

            // Assert
            response.Should().NotBeNull();
            response.text.Should().NotBeNull();
        }

        [Theory]
        [InlineData("Bad Set Name")]
        [InlineData("Some Other Set Name")]
        public async Task GetRandomBlackCard__ShouldThrowExceptionWhenInvalidSetNameUsed(string setName)
        {
            // Arrange
            var _sut = new CardsRepository();


            // Act
            Func<Task> act = async () => await _sut.GetRandomBlackCard(setName);

            // Assert
            await act.Should().ThrowAsync<Exception>();
        }



        /// <summary>
        /// Should return unique white cards, GameSettings.CardsPerPlayer, for the number of players
        /// </summary>
        /// <returns></returns>
        [Theory]
        [InlineData("CAH Base Set", 1)]
        [InlineData("CAH Base Set", 2)]
        [InlineData("CAH Base Set", 3)]
        public async Task GetRandomWhiteCards__ShouldReturnRandomWhiteCards(string setName, int playerCount)
        {
            // Arrange
            var _sut = new CardsRepository();

            // Act
            var response = await _sut.GetRandomWhiteCards(setName, playerCount);

            // Assert
            response.Should().NotBeNull();
            response.Count.Should().Be(playerCount * GameSettings.CardsPerPerson);
            response.Should().OnlyHaveUniqueItems();
        }


        [Theory]
        [InlineData("Bad Set Name", 1)]
        [InlineData("Some Other Set Name", 1)]
        [InlineData(null, 1)]
        [InlineData("", 1)]
        public async Task GetRandomWhiteCards__ShouldThrowExceptionWhenInvalidSetNameUsed(string setName, int playerCount)
        {
            // Arrange
            var _sut = new CardsRepository();


            // Act
            Func<Task> act = async () => await _sut.GetRandomWhiteCards(setName, playerCount);

            // Assert
            await act.Should().ThrowAsync<Exception>();
        }


        [Theory]
        [InlineData("CAH Base Set", -1)]
        [InlineData("CAH Base Set", 0)]        
        public async Task GetRandomWhiteCards__ShouldThrowExceptionWhenInvalidPlayerCountUsed(string setName, int playerCount)
        {
            // Arrange
            var _sut = new CardsRepository();


            // Act
            Func<Task> act = async () => await _sut.GetRandomWhiteCards(setName, playerCount);

            // Assert
            await act.Should().ThrowAsync<Exception>();
        }


        [Fact]
        public async Task GetSetList__ShouldReturnListOfSets()
        {
            // Arrange
            var sut = new CardsRepository();

            // Act
            var response = await sut.GetSets();

            // Assert
            response.Should().NotBeNull();
            response.Should().HaveCountGreaterThan(0);
        }
    }
}