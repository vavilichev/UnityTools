using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace VavilichevGD.Extensions {
    public static class ExtensionsClass {

        public static T GetRandom<T>(this T[] array, T currentItem) {
            List<T> availableList = new List<T>(array);
            availableList.Remove(currentItem);
            int rIndex = Random.Range(0, availableList.Count);
            return availableList[rIndex];
        }
        
        public static DateTime Trim(this DateTime date, long ticks) {
            return new DateTime(date.Ticks - (date.Ticks % ticks), date.Kind);
        }
    }
}