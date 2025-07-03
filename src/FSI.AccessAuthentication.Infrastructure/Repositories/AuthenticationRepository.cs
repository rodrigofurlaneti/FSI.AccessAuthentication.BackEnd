using Dapper;
using FSI.AccessAuthentication.Infrastructure.Interfaces;
using FSI.AccessAuthentication.Infrastructure.Repositories;
using FSI.AccessAuthentication.Domain.Entities;
using FSI.AccessAuthentication.Domain.Interfaces;
using System.Data;

namespace FSI.AccessAuthentication.Infrastructure.Repositories
{
    public class AuthenticationRepository : BaseRepository, IAuthenticationRepository
    {
        private const string PROCEDURE_NAME = "usp_Authentication";
        private const string ACTION_GETALL = "GetAll";
        private const string ACTION_GETBYID = "GetById";
        private const string ACTION_GETBYAUTHENTICATION = "GetByAuthentication";
        private const string ACTION_INSERT = "Insert";
        private const string ACTION_UPDATE = "Update";
        private const string ACTION_DELETE = "Delete";

        public AuthenticationRepository(IDbContext context) : base(context) { }

        public async Task<IEnumerable<AuthenticationEntity>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<AuthenticationEntity>(
                PROCEDURE_NAME,
                new { Action = ACTION_GETALL },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<AuthenticationEntity?> GetByIdAsync(long id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<AuthenticationEntity>(
                PROCEDURE_NAME,
                new { Action = ACTION_GETBYID, Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<long> InsertAsync(AuthenticationEntity entity)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<long>(
                PROCEDURE_NAME,
                new
                {
                    Action = ACTION_INSERT,
                    entity.SystemId,
                    entity.Username,
                    entity.Password,
                    entity.IsAuthentication,
                    entity.CreatedAt,
                    entity.UpdatedAt,
                    entity.Expiration,
                    entity.IsActive
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<bool> UpdateAsync(AuthenticationEntity entity)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<bool>(
                PROCEDURE_NAME,
                new
                {
                    Action = ACTION_UPDATE,
                    entity.Id,
                    entity.SystemId,
                    entity.Username,
                    entity.Password,
                    entity.IsAuthentication,
                    entity.CreatedAt,
                    entity.UpdatedAt,
                    entity.Expiration,
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

        public async Task<AuthenticationEntity> GetByAuthenticationAsync(AuthenticationRequestEntity authenticationRequestEntity)
        {
            using var connection = CreateConnection();

            var user = await connection.QueryFirstOrDefaultAsync<dynamic>(
                PROCEDURE_NAME,
                new
                {
                    Action = ACTION_GETBYAUTHENTICATION,
                    authenticationRequestEntity.Username,
                    authenticationRequestEntity.Password
                },
                commandType: CommandType.StoredProcedure
            );

            // ❌ Não encontrou o usuário → retorna autenticação inválida
            bool isAuth = user?.IsAuthentication == true;

            if (!isAuth)
            {
                return new AuthenticationEntity
                {
                    Username = authenticationRequestEntity.Username,
                    Password = authenticationRequestEntity.Password,
                    IsAuthentication = false,
                    Expiration = DateTime.Now,
                    CreatedAt = DateTime.Now,
                    UpdatedAt = DateTime.Now,
                    ErrorMessage = "Invalid username or password",
                    IsActive = false
                };
            }

            return new AuthenticationEntity
            {
                SystemId = user.SystemId,
                Username = user.Username,
                Password = user.Password,
                IsAuthentication = true,
                Expiration = user.Expiration,
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now,
                ErrorMessage = string.Empty,
                IsActive = true
            };
        }
    }
}