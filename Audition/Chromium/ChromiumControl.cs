using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.Windows.Input;
using CefSharp;
using CefSharp.WinForms;

namespace Audition.Chromium
{
    [ComVisible(true)]
    public class ChromiumControl : UserControl, IMenuHandler
    {
        private readonly WebView webView;

        public ChromiumControl(IRequestHandler requestHandler)
        {
            var address = Routing.GetViewUrl("login.html");
            CEF.Initialize(new Settings());
            Dock = DockStyle.Fill;
            webView = new WebView(address, new BrowserSettings
                                             {
                                                 WebSecurityDisabled = true
                                             })
                          {
                              Dock = DockStyle.Fill,
                              RequestHandler = requestHandler
                          };

            webView.PropertyChanged += (sender, args) => webView.Address = address;
            

            webView.KeyboardHandler = new ShortcutKeyboardHandler();            

            // Without this:
            //  The webview isn't initialized for a while after the tab is shown, so we have to wait, polling it.
            //  About 1/3 times when the tab gets created the webview only fills the top left hand corner of it.
            SizeChanged += ReinitializeAndResize;

            Controls.Add(webView);

            webView.MenuHandler = this;
        }

        private void ReinitializeAndResize(object o, EventArgs a)
        {
            if (webView.IsHandleCreated)
            {
                ReinitializeAndResizeInner(o,a);
            }
            else
            {
                webView.HandleCreated += ReinitializeAndResizeInner;
            }
        }

        private void ReinitializeAndResizeInner(object o, EventArgs a)
        {
            //This method is oddly named but seems to work.
            webView.OnInitialized();
        }

        protected override void Dispose(bool disposing)
        {
            SizeChanged -= ReinitializeAndResize;
            webView.HandleCreated -= ReinitializeAndResizeInner;
            base.Dispose(disposing);
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
