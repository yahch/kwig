namespace KSharpEditor
{
    public class KBrowser : System.Windows.Forms.WebBrowser
    {
        private SHDocVw.IWebBrowser2 Iwb2;

        public KBrowser()
        {
            NewWindow += KBrowser_NewWindow;
        }

        private void KBrowser_NewWindow(object sender, System.ComponentModel.CancelEventArgs e)
        {
            KBrowser kb = sender as KBrowser;
            if (kb == null) return;
            Navigate(kb.StatusText);
        }

        protected override void AttachInterfaces(object nativeActiveXObject)
        {
            Iwb2 = (SHDocVw.IWebBrowser2)nativeActiveXObject;
            Iwb2.Silent = true;
            base.AttachInterfaces(nativeActiveXObject);
        }

        protected override void DetachInterfaces()
        {
            Iwb2 = null;
            base.DetachInterfaces();
        }
    }
}
