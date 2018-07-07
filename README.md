# KWIG

KIWG 是一个基于 summernote 的 WinForm 编辑器控件。

[](/yahch/kwig/blob/master/screenshots/screenshot-1.png?raw=true)

**使用方法：**

1.添加 kwig.dll 的引用

2.界面添加 KEditor 的编辑器控件

3.运行

**事件:**

```
// 打开文件按钮事件
void OnOpenButtonClicked();
// 保存按钮事件
void OnSaveButtonClicked();
// 插入图片按钮事件
void OnInsertImageClicked();
// 编辑器加载成功事件
void OnEditorLoadComplete();
// 编辑器错误事件
void OnEditorErrorOccured(Exception ex);
```

**属性:**

```
// 编辑器版本，同 summernote 版本号
KEditor.Version
// 设置或获取编辑器 Html 内容
KEditor.Html
```

**方法:**

```
// 编辑器清空重置
KEditor.Reset();
// 插入 html 代码
KEditor.InsertNode(string html)
// 插入文本
KEditor.InsertText(string text)
```


