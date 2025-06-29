using Dapper;
using FSI.AccessAuthentication.Domain.Entities;
using FSI.AccessAuthentication.Domain.Interfaces;
using FSI.AccessAuthentication.Infrastructure.Interfaces;
using System.Data;

namespace FSI.AccessAuthentication.Infrastructure.Repositories
{
    public class SystemRepository : BaseRepository, ISystemRepository
    {
        private const string PROCEDURE_NAME = "usp_System";
        private const string ACTION_GETALL = "GetAll";
        private const string ACTION_GETBYID = "GetById";
        private const string ACTION_INSERT = "Insert";
        private const string ACTION_UPDATE = "Update";
        private const string ACTION_DELETE = "Delete";

        public SystemRepository(IDbContext context) : base(context) { }

        public async Task<IEnumerable<SystemEntity>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<SystemEntity>(
                PROCEDURE_NAME,
                new { Action = ACTION_GETALL },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<SystemEntity?> GetByIdAsync(long id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<SystemEntity>(
                PROCEDURE_NAME,
                new { Action = ACTION_GETBYID, Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<long> InsertAsync(SystemEntity entity)
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

        public async Task<bool> UpdateAsync(SystemEntity entity)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<bool>(
                PROCEDURE_NAME,
                new
                {
                    Action = ACTION_UPDATE,
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
                new { Action = ACTION_DELETE, id },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}