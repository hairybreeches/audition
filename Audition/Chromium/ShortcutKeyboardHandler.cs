using System;
using System.Collections.Generic;
using CefSharp;

namespace Audition.Chromium
{
    public class ShortcutKeyboardHandler : IKeyboardHandler
    {        
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

        public bool OnKeyEvent(IWebBrowser browser, KeyType type, int code, CefEventFlags modifiers, bool isSystemKey)
        {
            if (type == KeyType.Char && ControlShift(modifiers))
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

        private bool ControlShift(CefEventFlags modifiers)
        {
            return KeyDown(modifiers, CefEventFlags.ControlDown) && KeyDown(modifiers, CefEventFlags.ShiftDown);
        }

        private static bool KeyDown(CefEventFlags modifiers, CefEventFlags key)
        {
            return (modifiers & key) != CefEventFlags.None;
        }

        public bool OnPreKeyEvent(IWebBrowser browser, KeyType type, int windowsKeyCode, int nativeKeyCode, CefEventFlags modifiers,
            bool isSystemKey, bool isKeyboardShortcut)
        {
            return false;            
        }
    }
}