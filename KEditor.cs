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

        public IKEditorEventListener KEditorEventListener { get; set; }

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
                Stream sm = Assembly.GetExecutingAssembly().GetManifestResourceStream("KSharpEditor.Resources.editor.html");
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
        }


        public void OnSaveButtonClicked()
        {
            if (KEditorEventListener != null) KEditorEventListener.OnSaveButtonClicked();
        }


        public void OnOpenFileButtonClicked()
        {
            if (KEditorEventListener != null) KEditorEventListener.OnOpenButtonClicked();
        }


        public void OnInsertImageButtonClicked()
        {
            if (KEditorEventListener != null) KEditorEventListener.OnInsertImageClicked();
        }

        public void OnEditorLoadComplete()
        {
            if (KEditorEventListener != null) KEditorEventListener.OnEditorLoadComplete();
        }

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
    }
}
