using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;
using MT.LQQ.Models.Param;
using MT.Model.Model;
using MT.WCF.User.Request;

namespace MT.WCF.User
{
    [ServiceContract]
    public interface IUserService
    {
        /// <summary>
        /// 获取问答列表
        /// </summary> 
        /// <param name="param"></param>
        /// <returns></returns>
        [OperationContract]
        ResponseData<UserEntity> GetUser(ParamForGetUser param);
    }
}
