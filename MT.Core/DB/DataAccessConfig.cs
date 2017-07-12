namespace MT.Core.DB
{
    /// <summary>
    /// 数据访问配置
    /// </summary>
    public class DataAccessConfig
    {
        /// <summary>
        /// 是否为连接字符串名称
        /// </summary>
        public bool IsConnectionStringName { get; set; }

        /// <summary>
        /// 连接字符串文本，IsConnectionStringName为False时，为数据库连接字符串，负责为连接字符串的名称
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// 数据库提供程序，比如：OracleProvider，MySQlProvider,SqlServerProvider等
        /// </summary>
        public IDbProvider DbProvider { get; set; }

        /// <summary>
        /// 命令超时时间
        /// </summary>
        public int CommandTimeOut { get; set; }

    }
}
