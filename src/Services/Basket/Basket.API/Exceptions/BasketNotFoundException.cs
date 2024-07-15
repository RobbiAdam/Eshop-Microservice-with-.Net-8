namespace Basket.API.Exceptions
{
    public class BasketNotFoundException : NotFoundException
    {
        public BasketNotFoundException(string userName) : base($"Basket with username: {userName} was not found.")
        {
        }
    }
}
