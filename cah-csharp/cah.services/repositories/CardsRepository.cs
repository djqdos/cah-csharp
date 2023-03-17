using cah.models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace cah.services.repositories
{
    public class CardsRepository : ICardsRepository
    {
        // holds the list of all the cards
        List<Set> _cards = new List<Set>();



        /// <summary>
        /// Load the cards from json file
        /// </summary>
        public CardsRepository()
        {
            string cardsJson = File.ReadAllText("Assets/cards.json");

            _cards = JsonSerializer.Deserialize<List<Set>>(cardsJson);
        }


        public async Task<BlackCard> GetRandomBlackCard(string setId)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(setId)) throw new Exception("Set Id must be provided");

                var set = _cards.Where(x => x.name == setId).FirstOrDefault();

                int min = 0;
                int max = set.Black.Count();

                Random r = new Random();
                int selected = r.Next(min, max + 1);

                BlackCard b = set.Black[selected];

                return b;
            }
            catch (Exception ex)
            {
                // TODO: Do something with exception here
                // eg: logging

                throw;
            }
        }


        public async Task<List<WhiteCard>> GetRandomWhiteCards(string setId, int playerCount)
        {
            try
            {
                if (playerCount <= 0) throw new Exception("Player count must be greater than 0");
                if (string.IsNullOrWhiteSpace(setId)) throw new Exception("Set Id must be provided");

                var set = _cards.Where(x => x.name == setId).FirstOrDefault();

                // set temp list of white cards
                List<WhiteCard> _tempCards = set.White;

                // empty list of white cards, to populate with selected cards
                List<WhiteCard> _selectedCards = new List<WhiteCard>();

                int min = 0;
                int max = set.White.Count();

                // Loop over the player count, and pick cards from the available list.
                // each iteration, remove the selected cards from the available pile
                // so that each user gets distinct cards
                for (var i = 1; i <= playerCount; i++)
                {
                    for (var innerLoop = 0; innerLoop < GameSettings.CardsPerPerson; innerLoop++)
                    {
                        Random r = new Random();
                        int randNum = r.Next(min, max + 1);
                        var selectedCard = _tempCards[randNum];

                        // remove the card, so it can't be selected again
                        _tempCards.Remove(selectedCard);
                        max = _tempCards.Count();

                        _selectedCards.Add(selectedCard);
                    }
                }

                return _selectedCards;
            }
            catch (Exception ex)
            {
                // TODO: Do something with exception here
                // eg: logging

                throw;
            }
        }

    }
}
