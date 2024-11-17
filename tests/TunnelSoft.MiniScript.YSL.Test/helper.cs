using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TunnelSoft.MiniScript.YSL.SL;

namespace TunnelSoft.MiniScript.YSL.Test; 
public static class Debug {
    public static void Assert(bool condition, string message = null) {
        if (!condition) {
            throw new AssertionException(message ?? "Assertion failed");
        }
    }

    public static void Contains(string? expected, string? actual) {
        var contains = false;
        if (expected == actual 
            || (actual!= null && expected != null && actual.Contains(expected))) {
            contains = true;
        }

        if (!contains) {
            throw new AssertFailedException("Contains.Contains");
        }
    }


    public static void DoesNotContain(string? expected, string? actual) {
        if (expected == actual
            || (actual != null && expected != null && actual.Contains(expected))) {
            throw new AssertFailedException("Contains.Contains");
        }
    }



    public static void Log(object value, string level = "info") {
        // Implement logging logic
    }
}
