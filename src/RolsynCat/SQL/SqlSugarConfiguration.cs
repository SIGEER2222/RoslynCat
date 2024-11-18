using SqlSugar;
namespace RoslynCat.SQL
{
    public static class SqlSugarConfiguration
    {
        public static ISqlSugarClient Configure() {
            var db = new SqlSugarClient(new ConnectionConfig()
            {
                ConnectionString = "Data Source=RoslynCat.db",
                DbType = DbType.Sqlite,
                IsAutoCloseConnection = true
            });

            db.Aop.OnLogExecuting = (sql,pars) =>
            {
                Console.WriteLine(sql + "\r\n" + db.Utilities.SerializeObject(pars.ToDictionary(p => p.ParameterName,p => p.Value)));
            };
            return db;
        }
    }
}
