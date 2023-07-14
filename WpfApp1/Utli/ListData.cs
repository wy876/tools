using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Utli
{
    public class ListData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// 列表 id
        /// </summary>
        public string _ID;

        public string ID
        {
            get { return _ID; }
            set { _ID = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("ID")); }
        }

        /// <summary>
        /// 一句话链接
        /// </summary>
        public string _url;
        public string url { 
            get { return _url; }
            set { _url = value;
                PropertyChanged?.Invoke(this,new PropertyChangedEventArgs("url")); }
            
        }

        /// <summary>
        /// 密码
        /// </summary>
        public string _Password;
        public string Password { 
            get { return _Password; }
            set { _Password = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Password"));
            }
        }

        /// <summary>
        /// 编码器
        /// </summary>
        public string _Encode;
        public string Encode {
            get { return _Encode; }
            set
            {
                _Encode = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Encode"));
            }
        }

        /// <summary>
        /// 脚本类型
        /// </summary>
        public string _Script;
        public string Script
        {
            get { return _Script; }
            set
            {
                _Script = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Script"));
            }
        }

        /// <summary>
        /// Ip地址
        /// </summary>
        public string _Ip;
        public string Ip
        {
            get { return _Ip; }
            set
            {
                _Ip = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Ip"));
            }
        }

        /// <summary>
        /// 物理地址
        /// </summary>
        public string _address;
        public string address {
            get { return _address; }
            set { _address = value; 
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("address"));
            
            }
        }

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
        public string _defaults;
        public string defaults { 
            get { return _defaults; }
            set { _defaults = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("defaults"));
            }
        }

        public ListData() {
            _ID = ID;
            _url = url;
            _Password = Password;
            _Script = Script;
            _Ip = Ip;
            _Encode = Encode;
            _defaults = defaults;
            _address = address;


        }

    }


}
