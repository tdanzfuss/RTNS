using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace TMB.Controls
{
    public interface IPopupFormControl
    {
        bool SaveControl();
        void CancelControl();
        string HeadingText { get; }
    }
}
