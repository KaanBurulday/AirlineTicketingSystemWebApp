using AirlineTicketingSystemWebApp.Model;

namespace AirlineTicketingSystemWebApp.Source.Svc.Interfaces
{
    public interface IJwtTokenService
    {
        string GenerateToken(User user);
    }
}
