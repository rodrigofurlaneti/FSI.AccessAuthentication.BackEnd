using Dapper;
using FSI.AccessAuthentication.Domain.Entities;
using FSI.AccessAuthentication.Domain.Interfaces;
using FSI.AccessAuthentication.Infrastructure.Interfaces;

using System.Data;

namespace FSI.AccessAuthentication.Infrastructure.Repositories
{
    public class MessagingRepository : BaseRepository, IMessagingRepository
    {
        private const string PROCEDURE_NAME = "usp_Messaging";
        private const string ACTION_GETALL = "GetAll";
        private const string ACTION_GETBYID = "GetById";
        private const string ACTION_INSERT = "Insert";
        private const string ACTION_UPDATE = "Update";
        private const string ACTION_DELETE = "Delete";

        public MessagingRepository(IDbContext context) : base(context) { }

        public async Task<IEnumerable<MessagingEntity>> GetAllAsync()
        {
            using var connection = CreateConnection();
            return await connection.QueryAsync<MessagingEntity>(
                PROCEDURE_NAME,
                new { Action = ACTION_GETALL },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<MessagingEntity?> GetByIdAsync(long id)
        {
            using var connection = CreateConnection();
            return await connection.QueryFirstOrDefaultAsync<MessagingEntity>(
                PROCEDURE_NAME,
                new { Action = ACTION_GETBYID, Id = id },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<long> InsertAsync(MessagingEntity entity)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<long>(
                PROCEDURE_NAME,
                new
                {
                    Action = ACTION_INSERT,
                    OperationMessage = entity.OperationMessage,
                    entity.QueueName,
                    entity.MessageRequest,
                    entity.MessageResponse,
                    entity.ErrorMessage,
                    entity.CreatedAt,
                    entity.UpdatedAt,
                    entity.IsActive
                },
                commandType: CommandType.StoredProcedure
            );
        }

        public async Task<bool> UpdateAsync(MessagingEntity entity)
        {
            using var connection = CreateConnection();
            return await connection.ExecuteScalarAsync<bool>(
                PROCEDURE_NAME,
                new
                {
                    Action = ACTION_UPDATE,
                    OperationMessage = entity.OperationMessage,
                    entity.Id,
                    entity.QueueName,
                    entity.MessageRequest,
                    entity.MessageResponse,
                    entity.IsProcessed,
                    entity.ErrorMessage,
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
                new { OperationMessage = ACTION_DELETE, entity.Id },
                commandType: CommandType.StoredProcedure
            );
        }
    }
}