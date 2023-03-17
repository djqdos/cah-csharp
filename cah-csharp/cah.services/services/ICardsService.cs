using cah.models;

namespace cah.services.services
{
    public interface ICardsService
    {
        Task<BlackCard> GetRandomBlackCard(string setId);

        Task<List<WhiteCard>> GetRandomWhiteCards(string setId, int playerCount);
    }
}