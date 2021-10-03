using System;
using System.Data.Common;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using Npgsql;
using Ozon.DotNetCourse.SupplyService.Infrastructure.Postgres.Abstractions;

namespace Ozon.DotNetCourse.SupplyService.Infrastructure.Postgres.Database
{
    public class DbConnectionFactory: IDbConnectionFactory, IDisposable
    {
        private readonly string _connectionString;

        private DbConnection _connection;
        private bool _disposed;

        public DbConnectionFactory(IOptions<Configuration> dbConfiguration)
        {
            _connectionString = dbConfiguration.Value.ConnectionString;
        }
        
        
        
        public async Task<DbConnection> GetConnectionAsync(CancellationToken cancellationToken)
        {
            if (_connection != null)
            {
                return _connection;
            }

            _connection = new NpgsqlConnection(_connectionString);
            await _connection.OpenAsync(cancellationToken);

            _connection.Disposed += (_, _) => { _connection = null; };

            return _connection;
        }
        
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        private void Dispose(bool disposing)
        {
            if (_disposed)
            {
                return;
            }

            // Free managed objects here
            if (disposing)
            {
            }

            // Free unmanaged objects here
            _connection?.Dispose();
            _connection= null;

            _disposed = true;
        }

        ~DbConnectionFactory()
        {
            Dispose(false);
        }
    }
}