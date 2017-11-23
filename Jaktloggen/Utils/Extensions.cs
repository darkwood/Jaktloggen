using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using Newtonsoft.Json;

namespace Jaktloggen
{
    public static class Extensions
    {
        public static T DeepClone<T>(this T toClone) where T : class
        {
            string tmp = JsonConvert.SerializeObject(toClone);
            return JsonConvert.DeserializeObject<T>(tmp);
        }

        public static void Replace<T>(this ObservableCollection<T> toReplace, T item) where T : BaseViewModel
        {
            var index = toReplace.ToList().FindIndex(i => i.ID == item.ID);
            toReplace[index] = item;
        }
    }
}
