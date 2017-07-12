namespace MT.LQQ.Utilitys.Helpers
{
    /// <summary>
    /// 配置助手
    /// </summary>
    public static class ConfigureHelper
    {
        /// <summary>
        /// 获取RabbitMQ主机地址
        /// </summary>
        /// <param name="ip">IP地址</param>
        /// <param name="vhost">vhost</param>
        /// <returns></returns>
        public static string GetRabbitMQHostAddress(string ip, string vhost)
        {
            if (string.IsNullOrEmpty(vhost) || vhost == "/")
            {
                return $"rabbitmq://{ip}";
            }
            return $"rabbitmq://{ip}/{vhost}";
        }
    }
}