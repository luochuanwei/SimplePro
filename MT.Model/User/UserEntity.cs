using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MT.Model.Model
{
    public class UserEntity
    {
        /// <summary>
        /// 用户ID
        /// </summary>
        public string UserID { get; set; }
        /// <summary>
        /// 登录账户
        /// </summary>
        public string LoginAccount { get; set; }
        /// <summary>
        ///  登录密码 
        /// </summary>
        public string LoginPassword { get; set; }
        /// <summary>
        ///  头像 
        /// </summary>
        public string Portrait { get; set; }
        /// <summary>
        ///  性别;直接保存名称（男、女），不要保存1,0以及其他奇怪的东西 
        /// </summary>
        public string Sex { get; set; }
        /// <summary>
        ///  生日 
        /// </summary>
        public DateTime? Birthday { get; set; }
        /// <summary>
        ///  手机 
        /// </summary>
        public string Mobile { get; set; }
        /// <summary>
        ///  是否启用 
        /// </summary>
        public bool IsEnable { get; set; }
    }
}
