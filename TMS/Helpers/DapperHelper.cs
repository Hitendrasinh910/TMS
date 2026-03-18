using Dapper;
using Microsoft.Data.SqlClient;
using Serilog;
using System.Data;

namespace TMS.Helpers
{
    public interface IDapperHelper
    {
        Task<IEnumerable<T>> QueryAsync<T>(string sp, object? param = null);
        Task<T?> QueryFirstOrDefaultAsync<T>(string sp, object? param = null);
        Task<T?> QuerySingleOrDefaultAsync<T>(string sp, object? param = null);
        Task<T> QuerySingleAsync<T>(string sp, object? param = null);
        Task<int> ExecuteAsync(string sp, object? param = null);
        Task<int> ExecuteAsync(string sp, object? param, SqlConnection conn, SqlTransaction tran);
        Task<T?> ExecuteScalarAsync<T>(string sp, object? param = null);
        Task<(SqlMapper.GridReader Reader, SqlConnection Conn)> QueryMultipleAsync(string sp, object? param = null);
        Task ExecuteInTransaction(Func<SqlConnection, SqlTransaction, Task> action);
    }
    public class DapperHelper : IDapperHelper
    {
        private readonly string _connectionString;
        private readonly int _defaultTimeout = 60;
        private readonly bool _showSqlError;

        public DapperHelper(IConfiguration config)
        {
            _connectionString = config.GetConnectionString("DefaultConnection")
                ?? throw new Exception("Connection string 'DefaultConnection' not found");

            _showSqlError =
                (config["ASPNETCORE_ENVIRONMENT"] ?? "Production")
                .Equals("Development", StringComparison.OrdinalIgnoreCase);
        }

        private SqlConnection CreateConnection() => new SqlConnection(_connectionString);

        // ---------------- QUERY ----------------
        public async Task<IEnumerable<T>> QueryAsync<T>(string sp, object? param = null)
        {
            try
            {
                using var conn = CreateConnection();
                await conn.OpenAsync();
                return await conn.QueryAsync<T>(sp, param,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: _defaultTimeout);
            }
            catch (Exception ex)
            {
                ThrowFormattedException(sp, param, ex);
                throw;
            }
        }

        public async Task<T?> QueryFirstOrDefaultAsync<T>(string sp, object? param = null)
        {
            try
            {
                using var conn = CreateConnection();
                await conn.OpenAsync();
                return await conn.QueryFirstOrDefaultAsync<T>(sp, param,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: _defaultTimeout);
            }
            catch (Exception ex)
            {
                ThrowFormattedException(sp, param, ex);
                throw;
            }
        }

        public async Task<T?> QuerySingleOrDefaultAsync<T>(string sp, object? param = null)
        {
            try
            {
                using var conn = CreateConnection();
                await conn.OpenAsync();
                return await conn.QuerySingleOrDefaultAsync<T>(sp, param,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: _defaultTimeout);
            }
            catch (Exception ex)
            {
                ThrowFormattedException(sp, param, ex);
                throw;
            }
        }

        public async Task<T> QuerySingleAsync<T>(string sp, object? param = null)
        {
            try
            {
                using var conn = CreateConnection();
                await conn.OpenAsync();
                return await conn.QuerySingleAsync<T>(sp, param,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: _defaultTimeout);
            }
            catch (Exception ex)
            {
                ThrowFormattedException(sp, param, ex);
                throw;
            }
        }

        // ---------------- EXECUTE ----------------
        public async Task<int> ExecuteAsync(string sp, object? param = null)
        {
            try
            {
                using var conn = CreateConnection();
                await conn.OpenAsync();
                return await conn.ExecuteAsync(sp, param,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: _defaultTimeout);
            }
            catch (Exception ex)
            {
                ThrowFormattedException(sp, param, ex);
                throw;
            }
        }

        public async Task<int> ExecuteAsync(string sp, object? param, SqlConnection conn, SqlTransaction tran)
        {
            try
            {
                return await conn.ExecuteAsync(sp, param, tran,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: _defaultTimeout);
            }
            catch (Exception ex)
            {
                ThrowFormattedException(sp, param, ex);
                throw;
            }
        }

        public async Task<T?> ExecuteScalarAsync<T>(string sp, object? param = null)
        {
            try
            {
                using var conn = CreateConnection();
                await conn.OpenAsync();
                return await conn.ExecuteScalarAsync<T>(sp, param,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: _defaultTimeout);
            }
            catch (Exception ex)
            {
                ThrowFormattedException(sp, param, ex);
                throw;
            }
        }

        public async Task<(SqlMapper.GridReader Reader, SqlConnection Conn)> QueryMultipleAsync(string sp, object? param = null)
        {
            var conn = CreateConnection();
            try
            {
                await conn.OpenAsync();
                var reader = await conn.QueryMultipleAsync(sp, param,
                    commandType: CommandType.StoredProcedure,
                    commandTimeout: _defaultTimeout);

                return (reader, conn);
            }
            catch (Exception ex)
            {
                ThrowFormattedException(sp, param, ex);
                conn.Dispose();
                throw;
            }
        }

        public async Task ExecuteInTransaction(Func<SqlConnection, SqlTransaction, Task> action)
        {
            using var conn = CreateConnection();
            await conn.OpenAsync();
            using var tran = conn.BeginTransaction();

            try
            {
                await action(conn, tran);
                tran.Commit();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                ThrowFormattedException("TRANSACTION", null, ex);
                throw;
            }
        }

        // ---------------- ERROR HANDLING ----------------
        private void ThrowFormattedException(string sp, object? param, Exception ex)
        {
            Log.Error(ex, "SQL Error in {StoredProcedure} with params {@Params}", sp, param);

            if (ex is SqlException sqlEx && _showSqlError)
            {
                var detailedMessage =
                    $"SQL Error {sqlEx.Number} at Line {sqlEx.LineNumber} " +
                    $"in {sqlEx.Procedure}: {sqlEx.Message}";

                throw new Exception(detailedMessage, sqlEx);
            }

            throw new Exception("Database operation failed. Please contact administrator.");
        }
    }

}
