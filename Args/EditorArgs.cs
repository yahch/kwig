using System;
using System.Collections.Generic;
using System.Text;

namespace KSharpEditor.Args
{
    public class EditorArgs: EventArgs
    {
        public string Version { get; set; }
        public string Html { get; set; }
    }
}
