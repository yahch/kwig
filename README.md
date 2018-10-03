# KWIG

KIWG is a WYSIWYG editor for WinForm based on summernote.

![](https://raw.githubusercontent.com/yahch/kwig/master/screenshots/screenshot-1.png)

**Instructions:**

1. Add a reference to kwig.dll

2. Add KEditor control to your form

3. Buid & Run

**Events:**

```
// open file button event
Void OnOpenButtonClicked();
// save button event
Void OnSaveButtonClicked();
// Insert picture button event
Void OnInsertImageClicked();
// Editor loads success event
Void OnEditorLoadComplete();
// editor error event
Void OnEditorErrorOccured(Exception ex);
```

**Attributes:**

```
// editor version, same as summernote version number
KEditor.Version
// Set or get the editor Html content
KEditor.Html
```

**Methods:**

```
// editor clear reset
KEditor.Reset();
// insert html code
KEditor.InsertNode(string html)
// insert text
KEditor.InsertText(string text)
```
