using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Extension
{
    public static class OtherExtension
    {
        public static string ToJson(this object target) => JsonConvert.SerializeObject(target);
        public static T ToObject<T>(this string Data) => JsonConvert.DeserializeObject<T>(Data);
        public static T Clone<T>(this T Object) => Object.ToJson().ToObject<T>();

        public static void ForEach<T>(this IEnumerable<T> enumerable, Action<T> action) => enumerable.ToList().ForEach(action);
        public static void ForEach<T>(this IEnumerable<T> enumerable, Func<T> action) => enumerable.ToList().ForEach(action);
        //public static void To<T>(this T param, Action<T> action) => action(param) ;
        public static void To<T>(this T param, Action<T, object[]> action, params object[] paramas) => action(param, paramas);
        public static Color SetR(this Color color, float r) => new Color(r, color.g, color.b, color.a);
        public static Color SetG(this Color color, float g) => new Color(color.r, g, color.b, color.a);
        public static Color SetB(this Color color, float b) => new Color(color.r, color.g, b, color.a);
        public static Color SetA(this Color color, float a) => new Color(color.r, color.g, color.b, a);
    }
}


