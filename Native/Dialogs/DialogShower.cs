using System;
using System.Windows.Forms;

namespace Native.Dialogs
{
    public class DialogShower
    {
        private readonly Func<Form> mainFormFactory;

        public DialogShower(Func<Form> mainFormFactory)
        {
            this.mainFormFactory = mainFormFactory;
        }

        
        public DialogResult ShowDialog(CommonDialog dialog)
        {
            var form = mainFormFactory();
            return (DialogResult) form.Invoke(GetMethod(dialog, form));            
        }

        private static Func<DialogResult> GetMethod(CommonDialog dialogToShow, IWin32Window mainForm)
        {
            return () => dialogToShow.ShowDialog(mainForm);
        }
    }
}
