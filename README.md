# PyPyFaster
Right-click on the web page and click on Translate to 'your language' in the popup.

-本Mod涉及到IO和网络下载操作，如有介意，请勿使用！！！！！！！！

---
# 功能
在视频URL更新后比对本地文件，若以有该视频的缓存则直接从本地读取播放，否则会通过异步下载未缓存的视频到本地（仅限PyPyDance列表中的舞蹈视频）。

---
# 安装方法
安装最新版Melon Loader至`steamapps\common\VRChat`，并将`PyPyFaster.dll`放在`steamapps\common\VRChat\Mods`目录下。

初次安装本Mod并在首次运行VRChat后,在`steamapps\common\VRChat\UserData\MelonPreferences.cfg`中找到

[PyPyFaster]

"Cache Path" = *"your cache path"*

修改 *"your cache path"* 至你想要缓存的目录，VRChat目录下自动被生成的PyPyCache文件夹也可以删掉。

修改后示范：

[PyPyFaster]

"Cache Path" = "D:\\PyPyCache"

---
# 注意事项
-系统路径`\`分割符默认只有一个，由于c#转义符的原因，需要将分隔符修改为`\\`

-路径尾不需要再添加分隔符

正确示范：

"D:\\PyPyCache"

错误示范：

"D:\PyPyCache"

"D:\\\PyPyCache\\\\"
