using System;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using CefSharp;
using CefSharp.WinForms;
using System.Windows.Input;
using Microsoft.Owin.FileSystems;
using NLog;

namespace Audition.Chromium
{
    [ComVisible(true)]
    public class ChromiumControl : UserControl, IMenuHandler
    {
        private readonly Logger log;
        private readonly WebView webView;

        public ChromiumControl(Logger log, IRequestHandler requestHandler)
        {
            var address = WebConstants.GetViewUrl("login.html");
            this.log = log;
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

            webView.PropertyChanged += (sender, args) =>
                                         {
                                             webView.Address = address;

                                            if (args.PropertyName == "IsBrowserInitialized" && Keyboard.IsKeyDown(Key.LeftShift))
                                            {
                                                
                                                webView.ShowDevTools();
                                            }
                                        };
            webView.ConsoleMessage += LogConsoleMessage;

            // Without this:
            //  The webview isn't initialized for a while after the tab is shown, so we have to wait, polling it.
            //  About 1/3 times when the tab gets created the webview only fills the top left hand corner of it.
            SizeChanged += ReinitializeAndResize;

            Controls.Add(webView);

            webView.MenuHandler = this;
        }

        private void LogConsoleMessage(object sender, ConsoleMessageEventArgs e)
        {
            log.Warn("{0} at {1}:{2}", e.Message, new Uri(e.Source).LocalPath.TrimStart('/'), e.Line);
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

        /// <summary>
        /// The tab must be made visible before calling this method.
        /// </summary>
        /// <param name="url"></param>
        public void NavigateToUrl(string url)
        {
            webView.Load(WebConstants.AddInternalDomain(url));
        }

        protected override void Dispose(bool disposing)
        {
            SizeChanged -= ReinitializeAndResize;
            webView.HandleCreated -= ReinitializeAndResizeInner;
            base.Dispose(disposing);
        }

        internal void EvaluateJavaScript(string script)
        {
            webView.EvaluateScript(script);
        }

        public bool OnBeforeMenu(IWebBrowser browser)
        {
            return !Keyboard.IsKeyDown(Key.LeftShift);
        }
    }
}
