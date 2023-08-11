using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Utli
{
    public class FileListData : INotifyPropertyChanged
    {
        public event PropertyChangedEventHandler? PropertyChanged;

        /// <summary>
        /// 文件目录 和文件名
        /// </summary>
        public string _path;
        public string path { get { return _path; } set { _path = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("path")); } }

        /// <summary>
        /// 文件大小
        /// </summary>
        public string _size;
        
        public string size { get { return _size; } set { _size = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("size")); } }

        /// <summary>
        /// 文件日期
        /// </summary>
        public string _time;
        public string time { get { return _time; } set { _time = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("time")); } }

        /// <summary>
        /// 文件属性
        /// </summary>

        public string _attribute;
        public string attribute { get { return _attribute; } set { _attribute = value; PropertyChanged?.Invoke(this, new PropertyChangedEventArgs("attribute")); } }



        public FileListData()
        {
            _path = path;
            _size = size;
            _time = time;
            _attribute = attribute;
        }

    }
}
