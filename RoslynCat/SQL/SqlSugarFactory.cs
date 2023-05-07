using Microsoft.Extensions.Options;
using SqlSugar;

namespace RoslynCat.SQL
{
    public class SqlSugarFactory
    {
        public ISqlSugarClient Create(IServiceProvider provider) {
            var appSettings = provider.GetRequiredService<IOptions<AppSettings>>().Value;
            string connectionString = "Data Source = TestIDDDD.sqlite";

            //string connectionString = appSettings.ConnectionStrings.SqliteDb;
            var db = new SqlSugarScope(new ConnectionConfig()
            {
                DbType = DbType.Sqlite,
                ConnectionString = connectionString,
                IsAutoCloseConnection = true
            });
            return db;
        }
    }
}
