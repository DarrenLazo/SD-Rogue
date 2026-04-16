using System;
using System.Collections.Generic;
using System.Text;

namespace RogueLib.Utilities;
public static class LogSystem
{
    private static string _message = "";

    public static void Log(string msg)
    {
        _message = msg;
    }

    public static string Message => _message;
}