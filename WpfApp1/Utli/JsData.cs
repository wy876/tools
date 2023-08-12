using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Utli
{
    /// <summary>
    /// JSON数据的实体类
    /// </summary>
    public class JsData
    {
        /// <summary>
        /// 列表 id
        /// </summary>
        public string ID;
        /// <summary>
        /// 一句话链接
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// 密码
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// 编码器
        /// </summary>
        public string Encode { get; set; }

        /// <summary>
        /// 脚本类型
        /// </summary>
        public string Script { get; set; }

        /// <summary>
        /// Ip地址
        /// </summary>
        public string Ip { get; set; }

        /// <summary>
        /// 物理地址
        /// </summary>
        public string address { get; set; }

        /// <summary>
        /// 创建时间
        /// </summary>
        public DateTime Creation_time { get; set; }
        /// <summary>
        /// 修改时间
        /// </summary>
        public DateTime Modification_time { get; set; }

        /// <summary>
        ///  treeview 列表
        /// </summary>
        public string defaults { get; set; }
       
    }

}
