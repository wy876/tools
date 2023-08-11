using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.ComponentModel;

namespace WpfApp1.Utli
{
    public class ProxyData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// 代理协议
        /// </summary>
        public string _Proxy_Agreement { get; set; }
        public string Proxy_Agreement
        {
            get { return _Proxy_Agreement; }
            set {
                _Proxy_Agreement = value;
                PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Proxy_Agreement"));
            }
        }
        /// <summary>
        /// 代理服务器
        /// </summary>
        public string _Proxy_Host { get; set; }
        public string Proxy_Host
        {
            get { return _Proxy_Host; }
            set { _Proxy_Host = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Proxy_Host")); }
        }

        /// <summary>
        /// 代理端口
        /// </summary>
        public static string _Proxy_Prot { get; set; }
        public string Proxy_Prot
        {
            get { return _Proxy_Prot; }
            set { _Proxy_Prot = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("Proxy_Prot")); }
        }


        public ProxyData()
        {
            _Proxy_Agreement = Proxy_Agreement;
            _Proxy_Host = Proxy_Host;
            _Proxy_Prot = Proxy_Prot;
        }
    }
}
