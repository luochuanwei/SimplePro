namespace MT.Core.DB
{
    /// <summary>
    /// 数据访问基类
    /// </summary>
    public abstract class DataAccessBase
    {
        /// <summary>
        /// 连接超时时间
        /// </summary>
        private const int TimeOut = 600;

        /// <summary>
        /// 数据数据访问上下文，用于数据操作。
        /// </summary>
        protected IDbContext DbContext;

        /// <summary>
        /// 默认构造函数。使用连接字符串名称为"ScholarsBridge",Oracle数据访问提供程序
        /// </summary>
        protected DataAccessBase() : this("LQQ", new OracleProvider())
        {
        }

        /// <summary>
        /// 构造函数,使用连接字符串名称为"ScholarsBridge"
        /// </summary>
        /// <param name="dbProvider">提供程序</param>
        protected DataAccessBase(IDbProvider dbProvider) : this("LQQ", dbProvider)
        {
        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="connectionStringName">连接字符串名称</param>
        /// <param name="dbProvider">提供程序</param>
        protected DataAccessBase(string connectionStringName, IDbProvider dbProvider) :
            this(
                new DataAccessConfig
                {
                    DbProvider = dbProvider,
                    IsConnectionStringName = true,
                    Text = connectionStringName
                })
        {


        }

        /// <summary>
        /// 构造函数
        /// </summary>
        /// <param name="config">配置信息</param>
        protected DataAccessBase(DataAccessConfig config)
        {
            DbContext = config.IsConnectionStringName
                ? new DbContext().ConnectionStringName(config.Text, config.DbProvider)
                : new DbContext().ConnectionString(config.Text, config.DbProvider);
            DbContext.CommandTimeout(config.CommandTimeOut < 1 ? TimeOut : config.CommandTimeOut);
        }
    }
}