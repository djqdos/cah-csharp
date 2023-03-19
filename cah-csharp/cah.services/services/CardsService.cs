using cah.models;
using cah.services.repositories;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cah.services.services
{
    public class CardsService : ICardsService
    {

        private readonly ICardsRepository _cardsRepository;

        public CardsService(ICardsRepository cardsRepository)
        {
            _cardsRepository = cardsRepository;
        }

        public async Task<BlackCard> GetRandomBlackCard(string setId)
        {
            return await _cardsRepository.GetRandomBlackCard(setId);
        }

        public async Task<List<WhiteCard>> GetRandomWhiteCards(string setId, int playerCount)
        {
            return await _cardsRepository.GetRandomWhiteCards(setId, playerCount);
        }

        public async Task<List<string>> GetSets()
        {
            return await _cardsRepository.GetSets();
        }

	}
}
