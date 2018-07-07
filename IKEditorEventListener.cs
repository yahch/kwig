using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace KSharpEditor
{
    public interface IKEditorEventListener
    {
        void OnOpenButtonClicked();

        void OnSaveButtonClicked();

        void OnInsertImageClicked();

        void OnEditorLoadComplete();

        void OnEditorErrorOccured(Exception ex);
    }
}
