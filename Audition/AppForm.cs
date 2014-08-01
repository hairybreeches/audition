using System.Drawing;
using System.Windows.Forms;
using Audition.Chromium;
using NLog;

namespace Audition
{
    public class AppForm : Form
    {
        public AppForm(ChromiumControl control)
        {
            Controls.Add(control);
            
            Text = "Audition software is awesome";
            Icon = Resources.AuditionIcon;
            Size = new Size(820, 420);
            FormBorderStyle = FormBorderStyle.FixedDialog;
            StartPosition = FormStartPosition.CenterParent;
        }

    }
}


