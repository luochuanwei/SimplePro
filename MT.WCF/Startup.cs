using System.IO;
using System.Linq;
using System.Reflection;
using System.Web.Compilation;
using Autofac;
using Autofac.Integration.Wcf;
using log4net.Config;

namespace MT.WCF
{
    /// <summary>
    /// 启动器
    /// </summary>
    public class Startup
    {
        /// <summary>
        /// 配置Autofac
        /// </summary>
        public static void AutofacConfiguration()
        {
            var builder = new ContainerBuilder();
            RegisterModule(builder);
            var container = builder.Build();
            AutofacHostFactory.Container = container;
        }

        /// <summary>
        /// 注册模块
        /// </summary>
        /// <param name="builder">容器</param>
        private static void RegisterModule(ContainerBuilder builder)
        {
            var assemblies = BuildManager.GetReferencedAssemblies().Cast<Assembly>();
            var assemblieList = assemblies.Where(x => x.FullName.Contains("MT")).ToArray();
            builder.RegisterAssemblyModules(assemblieList);
        }

        /// <summary>
        /// 日志记录器配置
        /// </summary>
        /// <param name="logConfig">日志的配置文件信息</param>
        public static void UseLog4Net(FileInfo logConfig)
        {
            XmlConfigurator.ConfigureAndWatch(logConfig);
        }
    }
}