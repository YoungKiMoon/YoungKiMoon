using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;

namespace PaperSetting.Utils
{
    public class CustomCursor : IDisposable
    {
        private Cursor _oldCursor;

        public CustomCursor()
        {
            _oldCursor = null;
        }
        public void WaitCursor()
        {
            _oldCursor = Mouse.OverrideCursor;
            Mouse.OverrideCursor = Cursors.Wait;
        }
        public void Dispose()
        {
            Mouse.OverrideCursor = _oldCursor;
        }
    }
}
