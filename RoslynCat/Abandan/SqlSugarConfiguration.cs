//using SqlSugar;
//namespace RoslynCat.Abandan
//{
//    public static class SqlSugarConfiguration
//    {
//        public static ISqlSugarClient Configure()
//        {
//            var db = new SqlSugarClient(new ConnectionConfig()
//            {
//                ConnectionString = new GetConfig().ConnectionString,
//                DbType = DbType.SqlServer,
//                IsAutoCloseConnection = true
//            });

//            db.Aop.OnLogExecuting = (sql, pars) =>
//            {
//                string currentDate = DateTime.Now.ToString("yyyyMMdd");

//                string logFileName = $"{currentDate}-sqlLog.txt";

//                string logFilePath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, logFileName);

//                // 如果日志文件不存在，则创建日志文件
//                if (!File.Exists(logFilePath))
//                {
//                    File.Create(logFilePath).Close();
//                }

//                // 记录日志到文件中
//                string logContent = $"执行SQL：{sql}，参数：{pars.ToString()}";
//                File.AppendAllText(logFilePath, logContent);
//            };
//            return db;
//        }
//    }
//}
