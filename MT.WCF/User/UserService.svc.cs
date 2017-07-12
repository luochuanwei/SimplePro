using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using Autofac;
using Autofac.Integration.Wcf;
using log4net;
using MT.BLL.User;
using MT.LQQ.Models;
using MT.LQQ.Models.Param;
using MT.Model;
using MT.Model.Model;
using MT.WCF.User.Request;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace MT.WCF.User
{
    // 注意: 使用“重构”菜单上的“重命名”命令，可以同时更改代码、svc 和配置文件中的类名“User”。
    // 注意: 为了启动 WCF 测试客户端以测试此服务，请在解决方案资源管理器中选择 User.svc 或 User.svc.cs，然后开始调试。
    public class UserService : IUserService
    {
        private readonly ILog _logger = LogManager.GetLogger(typeof(UserService));

        public ResponseData<UserEntity> GetUser(ParamForGetUser param)
        {
            try
            {
                if (param == null)
                {
                    return ResponseData<UserEntity>.Failed(MessageConstant.ArgumentIsNull);
                }

                string message;
                if (!param.Validate(out message))
                {
                    return ResponseData<UserEntity>.Failed(message);
                }
                using (var scope = AutofacHostFactory.Container.BeginLifetimeScope())
                {
                    var bll = scope.Resolve<IUserBLL>();
                    var list = bll.GetUserInfoByUserId(param);
                    return ResponseData<UserEntity>.Success(message, list);
                }
            }
            catch (Exception ex)
            {
                _logger.Error("GetUser()异常", ex);
                return ResponseData<UserEntity>.Error(MessageConstant.ErrorByWcf);
            }
        }

        /// <summary>
        /// 格式化返回结果
        /// </summary>
        /// <param name="jObject"></param>
        /// <returns></returns>
        private ResponseData<string> FormatResult(JObject jObject)
        {
            if (jObject == null)
            {
                return ResponseData<string>.Failed("没有结果");
            }

            var statusValue = jObject.GetValue("status", StringComparison.OrdinalIgnoreCase).Value<int>();
            if (statusValue == 0)
            {
                var jsonToken = jObject.GetValue("data", StringComparison.OrdinalIgnoreCase);
                var jsonString = JsonConvert.SerializeObject(jsonToken);
                var result = ResponseData<string>.Success(value: jsonString);
                return result;
            }
            var message = jObject.GetValue("message", StringComparison.OrdinalIgnoreCase).Value<string>();

            return ResponseData<string>.Failed(message ?? "没有数据，请稍后再试！");
        }
    }
}
