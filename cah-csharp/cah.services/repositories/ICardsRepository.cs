using cah.models;

namespace cah.services.repositories
{
    public interface ICardsRepository
    {
        Task<BlackCard> GetRandomBlackCard(string setId);

        Task<List<WhiteCard>> GetRandomWhiteCards(string setId, int playerCount);

        Task<List<string>> GetSets();

	}
}