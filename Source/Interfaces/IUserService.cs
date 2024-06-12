using AirlineTicketingSystemWebApp.Model;

namespace AirlineTicketingSystemWebApp.Source.Interfaces
{
    public interface IUserService
    {
        public Task<User> GetUserAsync();
    }
}
