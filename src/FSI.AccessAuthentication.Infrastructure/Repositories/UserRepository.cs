using Dapper;
using FSI.AccessAuthentication.Domain.Entities;
using FSI.AccessAuthentication.Domain.Interfaces;
using FSI.AccessAuthentication.Infrastructure.Interfaces;
using System.Data;

namespace FSI.AccessAuthentication.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository, IUserRepository
    {
        private const string PROCEDURE_NAME = "usp_User";
        private const string ACTION_GETALL = "GetAll";
        private const string ACTION_GETBYID = "GetById";
        private const string ACTION_INSERT = "Insert";
        private const string ACTION_UPDATE = "Update";
        private const string ACTION_DELETE = "Delete";

        public UserRepository(IDbContext context) : base(context) { }

        public async Task<IEnumerable<UserEntity>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<UserEntity>(
                PROCEDURE_NAME,
                new { Action = ACTION_GETALL },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<UserEntity?> GetByIdAsync(long id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<UserEntity>(
                PROCEDURE_NAME,
                new { Action = ACTION_GETBYID, Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<long> InsertAsync(UserEntity entity)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<long>(
                PROCEDURE_NAME,
                new
                {
                    Action = ACTION_INSERT,
                    entity.Name,
                    entity.Username,
                    entity.Password,
                    entity.Email,
                    entity.PhoneNumber,
                    entity.SystemId,
                    entity.ProfileId,
                    entity.CreatedAt,
                    entity.UpdatedAt,
                    entity.IsActive
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<bool> UpdateAsync(UserEntity entity)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<bool>(
                PROCEDURE_NAME,
                new
                {
                    Action = ACTION_UPDATE,
                    entity.Id,
                    entity.Name,
                    entity.Username,
                    entity.Password,
                    entity.Email,
                    entity.PhoneNumber,
                    entity.SystemId,
                    entity.ProfileId,
                    entity.CreatedAt,
                    entity.UpdatedAt,
                    entity.IsActive
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<bool> DeleteAsync(UserEntity entity)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<bool>(
                PROCEDURE_NAME,
                new { OperationMessage = ACTION_DELETE, entity.Id },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}