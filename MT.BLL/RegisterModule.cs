using System;
using System.Reflection;
using Autofac;

namespace MT.BLL
{
    /// <summary>
    /// 注册模块
    /// </summary>
    public class RegisterModule : Autofac.Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            var dataAccess = Assembly.GetExecutingAssembly();
            builder.RegisterAssemblyTypes(dataAccess)
                .Where(t => TypeNameEndsWith(t.Name))
                .AsImplementedInterfaces();
        }

        /// <summary>
        /// 类型结尾包含 BLL或者DAL 
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        private bool TypeNameEndsWith(string name)
        {
            return name.EndsWith("BLL", StringComparison.OrdinalIgnoreCase);
        }
    }
}