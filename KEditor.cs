using KSharpEditor.Args;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace KSharpEditor
{
    [ComVisible(true)]
    public class KEditor : System.Windows.Forms.UserControl
    {
        private KBrowser kBrowserEditor;
        private string _lang = "zh-CN";

        public event EventHandler<EditorArgs> OpenButtonClick;
        public event EventHandler<EditorArgs> SaveButtonClick;
        public event EventHandler<EditorArgs> InsertImageClick;
        public event EventHandler<EditorArgs> EditorLoadComplete;
        public event EventHandler<ErrorArgs> EditorError;

        private void InitializeComponent()
        {
            this.kBrowserEditor = new KBrowser();
            this.SuspendLayout();
            // 
            // kBrowserEditor
            // 
            this.kBrowserEditor.Dock = System.Windows.Forms.DockStyle.Fill;
            this.kBrowserEditor.Location = new System.Drawing.Point(0, 0);
            this.kBrowserEditor.MinimumSize = new System.Drawing.Size(20, 20);
            this.kBrowserEditor.Name = "kBrowserEditor";
            this.kBrowserEditor.ScriptErrorsSuppressed = true;
            this.kBrowserEditor.Size = new System.Drawing.Size(465, 252);
            this.kBrowserEditor.TabIndex = 0;
            // 
            // KEditor
            // 
            this.Controls.Add(this.kBrowserEditor);
            this.Name = "KEditor";
            this.Size = new System.Drawing.Size(465, 252);
            this.Load += new System.EventHandler(this.KEditor_Load);
            this.ResumeLayout(false);

        }

        public KEditor()
        {
            InitializeComponent();
            kBrowserEditor.IsWebBrowserContextMenuEnabled = false;
            kBrowserEditor.ObjectForScripting = this;
        }

        /// <summary>
        /// Sets the language of the browser. Default is zh-CN
        /// </summary>
        /// <param name="lang">The language to set. Supported are en-US, zh-CN</param>
        public KEditor(string lang): this()
        {
            _lang = lang;
        }

        public IKEditorEventListener KEditorEventListener { get; set; }

        /// <summary>
        /// Sets the language of the editor.
        /// </summary>
        public string Language
        {
            set
            {
                try
                {
                    kBrowserEditor?.Document?.InvokeScript("setLang", new string[] { value });
                }
                catch (Exception ex)
                {
                    OnError(ex);
                }
            }
        }

        /// <summary>
        /// Returns the version of the editor.
        /// </summary>
        public string Version
        {
            get
            {
                try
                {
                    return kBrowserEditor.Document.InvokeScript("getVersion").ToString();
                }
                catch (Exception ex)
                {
                    OnError(ex);
                    return "";
                }
            }
        }

        /// <summary>
        /// Gets or sets the html in the editor
        /// </summary>
        public string Html
        {
            get
            {
                try
                {
                    return kBrowserEditor.Document.InvokeScript("getHtml").ToString();
                }
                catch (Exception ex)
                {
                    OnError(ex);
                    return "";
                }
            }
            set
            {
                try
                {
                    kBrowserEditor.Document.InvokeScript("setHtml", new string[] { value });
                }
                catch (Exception ex)
                {
                    OnError(ex);
                }
            }
        }

        private void KEditor_Load(object sender, EventArgs e)
        {
            try
            {
                string resourcename = Assembly.GetExecutingAssembly().GetManifestResourceNames().FirstOrDefault(x => x.Contains("editor.html"));
                Stream sm = Assembly.GetExecutingAssembly().GetManifestResourceStream(resourcename);
                byte[] bs = new byte[sm.Length];
                sm.Read(bs, 0, (int)sm.Length);
                sm.Close();
                UTF8Encoding con = new UTF8Encoding();
                string str = con.GetString(bs);
                kBrowserEditor.DocumentText = str;
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        private void OnError(Exception ex)
        {
            if (KEditorEventListener != null) KEditorEventListener.OnEditorErrorOccured(ex);
            if (EditorError != null)
            {
                EditorError(this, new ErrorArgs(ex));
            }
        }


        public void OnSaveButtonClicked()
        {
            if (KEditorEventListener != null) KEditorEventListener.OnSaveButtonClicked();
            if (SaveButtonClick != null)
            {
                SaveButtonClick(this, GetEditArgs());
            }
        }


        public void OnOpenFileButtonClicked()
        {
            if (KEditorEventListener != null) KEditorEventListener.OnOpenButtonClicked();
            if (OpenButtonClick != null)
            {
                OpenButtonClick(this, GetEditArgs());
            }
        }

        private EditorArgs GetEditArgs()
        {
            return new EditorArgs()
            {
                Html = this.Html,
                Version = this.Version
            };
        }

        public void OnInsertImageButtonClicked()
        {
            if (KEditorEventListener != null) KEditorEventListener.OnInsertImageClicked();
            if (InsertImageClick != null)
            {
                InsertImageClick(this, GetEditArgs());
            }
        }

        public void OnEditorLoadComplete()
        {
            if (KEditorEventListener != null) KEditorEventListener.OnEditorLoadComplete();
            if (EditorLoadComplete != null)
            {
                EditorLoadComplete(this, GetEditArgs());
            }
        }

        public void OnEditorLoadStart() {
            Language = _lang;
        }

        /// <summary>
        /// Clears the editor.
        /// </summary>
        public void Reset()
        {
            try
            {
                kBrowserEditor.Document.InvokeScript("reset");
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        /// <summary>
        /// Inserts provided html to the editor.
        /// </summary>
        /// <param name="html">the html to insert</param>
        public void InsertNode(string html)
        {
            try
            {
                kBrowserEditor.Document.InvokeScript("insertNode", new string[] { html });
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        /// <summary>
        /// Inserts a text in the editor
        /// </summary>
        /// <param name="text">the text to insert</param>
        public void InsertText(string text)
        {
            try
            {
                kBrowserEditor.Document.InvokeScript("insertText", new string[] { text });
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }

        /// <summary>
        /// Inserts image in the editor.
        /// </summary>
        /// <param name="path">absolute path of the image file to insert</param>
        /// <param name="base64">insert image as base64 encoded string. Image is inserted in png format</param>
        public void InsertImage(string path, bool base64 = false)
        {
            try
            {
                string sImg = string.Empty;
                if (base64)
                {
                    Byte[] bytes = File.ReadAllBytes(path);
                    String file = Convert.ToBase64String(bytes);
                    sImg = $"<p><img src=\"data:image/png;base64,{file}\" /></p>";
                }
                else
                {
                    sImg = $"<p><img src='{path}' /></p>";
                }

                kBrowserEditor.Document.InvokeScript("insertNode", new string[] { sImg });
            }
            catch (Exception ex)
            {
                OnError(ex);
            }
        }
    }
}
