using DoctorTrack.Models;
using DoctorTrack.Services.Interfaces;
using System.Data;
using Dapper;
namespace DoctorTrack.Repositories
{
    public class DoctorRepository : IDoctorRepository
    {
        private readonly IDbConnectionFactory _connectionFactory;

        public DoctorRepository(IDbConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Doctor>> GetAllAsync(
            string? search,
            string? status)
        {
            using var connection = _connectionFactory.CreateConnection();

            var doctors = await connection.QueryAsync<Doctor>(
                "sp_GetDoctors",
                new
                {
                    Search = search,
                    Status = status
                },
                commandType: CommandType.StoredProcedure);

            return doctors;
        }

        public async Task<Doctor?> GetByIdAsync(int id)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = @"
            SELECT *
            FROM Doctor
            WHERE Id = @Id
            AND IsDeleted = 0";

            return await connection.QueryFirstOrDefaultAsync<Doctor>(
                sql,
                new { Id = id });
        }

        public async Task<bool> ExistsByLicenseAsync(string licenseNumber)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = @"
            SELECT COUNT(1)
            FROM Doctor
            WHERE LicenseNumber = @LicenseNumber";

            var count = await connection.ExecuteScalarAsync<int>(
                sql,
                new { LicenseNumber = licenseNumber });

            return count > 0;
        }

        public async Task CreateAsync(Doctor doctor)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = @"
            INSERT INTO Doctor
            (
               
                FullName,
                Email,
                Specialization,
                LicenseNumber,
                LicenseExpiryDate,
                Status,
                CreatedDate,
                IsDeleted
            )
            VALUES
            (
                
                @FullName,
                @Email,
                @Specialization,
                @LicenseNumber,
                @LicenseExpiryDate,
                @Status,
                @CreatedDate,
                @IsDeleted
            )";

            await connection.ExecuteAsync(sql, doctor);
        }

        public async Task UpdateAsync(Doctor doctor)
        {
            using var connection = _connectionFactory.CreateConnection();

            var sql = @"
            UPDATE Doctor
            SET
                FullName = @FullName,
                Email = @Email,
                Specialization = @Specialization,
                LicenseExpiryDate = @LicenseExpiryDate,
                Status = @Status,
                IsDeleted = @IsDeleted
            WHERE Id = @Id";

            await connection.ExecuteAsync(sql, doctor);
        }
    }
}
