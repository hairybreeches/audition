using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Input;
using CefSharp;
using CefSharp.WinForms;
using Webapp;

namespace Audition.Chromium
{
    [ComVisible(true)]
    public class ChromiumControl : UserControl, IMenuHandler
    {
        private readonly ChromiumWebBrowser webView;

        public ChromiumControl(IEnumerable<CefCustomScheme> schemes)
        {
            var address = Routing.GetViewUrl("login.html");
            var cefSettings = new CefSettings();
            foreach (var scheme in schemes)
            {
                cefSettings.RegisterScheme(scheme);
            }
            Cef.Initialize(cefSettings);
            Dock = DockStyle.Fill;
            webView = new ChromiumWebBrowser(address)
                          {
                              Dock = DockStyle.Fill,                               
                          };            
            

            webView.KeyboardHandler = new ShortcutKeyboardHandler();            
            Controls.Add(webView);

            webView.MenuHandler = this;
        }        

        internal void ShowDevTools()
        {
            webView.ShowDevTools();
        }

        public bool OnBeforeMenu(IWebBrowser browser)
        {
            return !Keyboard.IsKeyDown(Key.LeftShift);
        }
    }
}
