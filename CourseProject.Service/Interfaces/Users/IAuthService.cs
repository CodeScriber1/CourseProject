
using System.Threading.Tasks;

namespace CourseProject.Service.Interfaces.Users;

public interface IAuthService
{
	ValueTask<string> GenerateToken(string email, string password);
}
	