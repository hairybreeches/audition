using System;
using System.Collections.Generic;
using CefSharp;

namespace Audition.Chromium
{
    public class ShortcutKeyboardHandler : IKeyboardHandler
    {
        private const CefEventFlags Ctrlshift = CefEventFlags.ControlDown | CefEventFlags.ShiftDown;
        private const int W = 23;
        private const int E = 5;
        private const int R = 18;
        private readonly Dictionary<int, Action<IWebBrowser>> shortcuts = new Dictionary<int, Action<IWebBrowser>>();

        public ShortcutKeyboardHandler()
        {
            shortcuts.Add(W, browser => browser.Back());
            shortcuts.Add(E, browser => browser.Reload(true));
            shortcuts.Add(R, browser => browser.ShowDevTools());
        }
        
        public bool OnKeyEvent(IWebBrowser browser, KeyType type, int code, CefEventFlags modifiers, bool isSystemKey)
        {
            if (type == KeyType.Char && modifiers == Ctrlshift)
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

        public bool OnPreKeyEvent(IWebBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers,
            bool isSystemKey, bool isKeyboardShortcut)
        {
            return false;
        }
    }
}