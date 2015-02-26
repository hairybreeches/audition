using System.Drawing;
using System.Windows.Forms;
using Audition.Chromium;

namespace Audition
{
    public class AppForm : Form
    {
        public AppForm(ChromiumControl control)
        {
            Controls.Add(control);
            
            Text = "Audition";
            Icon = Resources.AuditionIcon;
            Size = new Size(800, 880);
            MinimumSize = new Size(800,300);
            FormBorderStyle = FormBorderStyle.Sizable;
            StartPosition = FormStartPosition.CenterParent;
        }

    }
}


