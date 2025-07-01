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
        private const string ACTION_GETBYUSERNAME = "GetByUsername";
        private const string ACTION_GETBYPASSWORD = "GetByPassword";
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

        public async Task<bool> DeleteAsync(long id)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<bool>(
                PROCEDURE_NAME,
                new { Action = ACTION_DELETE, id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<UserEntity?> GetByUsernameAsync(string username)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<UserEntity>(
                PROCEDURE_NAME,
                new { Action = ACTION_GETBYUSERNAME, Username = username },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<UserEntity?> GetByPasswordAsync(string password)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<UserEntity>(
                PROCEDURE_NAME,
                new { Action = ACTION_GETBYPASSWORD, Password = password },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}