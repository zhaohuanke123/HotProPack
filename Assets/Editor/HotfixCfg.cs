using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;
using vanko;
using XLua;

public static class HotfixCfg
{
    [Hotfix] public static List<Type> by_field = new()
    {
        typeof(Bootstrap),
    };
}