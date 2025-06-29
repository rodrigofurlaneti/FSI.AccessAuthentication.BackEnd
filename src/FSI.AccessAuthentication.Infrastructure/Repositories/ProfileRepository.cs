using Dapper;
using FSI.AccessAuthentication.Domain.Entities;
using FSI.AccessAuthentication.Domain.Interfaces;
using FSI.AccessAuthentication.Infrastructure.Interfaces;
using System.Data;

namespace FSI.AccessAuthentication.Infrastructure.Repositories
{
    public class ProfileRepository : BaseRepository, IProfileRepository
    {
        private const string PROCEDURE_NAME = "usp_Profile";
        private const string ACTION_GETALL = "GetAll";
        private const string ACTION_GETBYID = "GetById";
        private const string ACTION_INSERT = "Insert";
        private const string ACTION_UPDATE = "Update";
        private const string ACTION_DELETE = "Delete";

        public ProfileRepository(IDbContext context) : base(context) { }

        public async Task<IEnumerable<ProfileEntity>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<ProfileEntity>(
                PROCEDURE_NAME,
                new { Action = ACTION_GETALL },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<ProfileEntity?> GetByIdAsync(long id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<ProfileEntity>(
                PROCEDURE_NAME,
                new { Action = ACTION_GETBYID, Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<long> InsertAsync(ProfileEntity entity)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<long>(
                PROCEDURE_NAME,
                new
                {
                    Action = ACTION_INSERT,
                    entity.Name,
                    entity.CreatedAt,
                    entity.UpdatedAt,
                    entity.IsActive
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<bool> UpdateAsync(ProfileEntity entity)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<bool>(
                PROCEDURE_NAME,
                new
                {
                    Action = ACTION_UPDATE,
                    OperationMessage = entity.OperationMessage,
                    entity.Id,
                    entity.Name,
                    entity.UpdatedAt,
                    entity.IsActive
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<bool> DeleteAsync(long id)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<bool>(
                PROCEDURE_NAME,
                new { Action = ACTION_DELETE, entity.Id },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}