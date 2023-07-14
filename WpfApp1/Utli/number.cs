using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Utli
{
    public class number
    {
        public string GetDom()
        {
            Random r = new Random();
            string figure = string.Empty;
            for (int i = 0; i < 10; i++)
            {
                figure = r.Next(1, 10000).ToString();
               
            }
            return figure;
           
        }
    }
}
