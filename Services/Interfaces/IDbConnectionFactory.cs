
using System.Data;
namespace DoctorTrack.Services.Interfaces
{
    public interface IDbConnectionFactory
    {
        IDbConnection CreateConnection();
    }
}
