using System;
using System.Collections.Generic;
using CefSharp;

namespace Audition.Chromium
{
    public class ShortcutKeyboardHandler : IKeyboardHandler
    {        
        private const int ctrlshift = 1027;
        private const int w = 23;
        private const int e = 5;
        private const int r = 18;
        private readonly Dictionary<int, Action<IWebBrowser>> shortcuts = new Dictionary<int, Action<IWebBrowser>>();

        public ShortcutKeyboardHandler()
        {
            shortcuts.Add(w, browser => browser.Back());
            shortcuts.Add(e, browser => browser.Reload(true));
            shortcuts.Add(r, browser => browser.ShowDevTools());
        }

        public bool OnKeyEvent(IWebBrowser browser, KeyType type, int code, int modifiers, bool isSystemKey, bool isAfterJavaScript)
        {
            if (type == KeyType.Char && isAfterJavaScript == false && modifiers == ctrlshift)
            {
                Action<IWebBrowser> action;
                if (shortcuts.TryGetValue(code, out action))
                {
                    action(browser);
                    return true;
                }
            }


            return false;
        }


    }
}