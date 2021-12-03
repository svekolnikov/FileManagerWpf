using System.Collections.Generic;
using System.Windows;

namespace FileManagerWpf.Extensions
{
    public static class ObservableCollectionExt
    {
        public static void AddOnUI<T>(this ICollection<T> collection, T item)
        {
            var addMethod = collection.Add;
            Application.Current.Dispatcher.BeginInvoke(addMethod, item);
        }

        public static void ClearOnUI<T>(this ICollection<T> collection)
        {
            var clearMethod = collection.Clear;
            Application.Current.Dispatcher.BeginInvoke(clearMethod);
        }
    }
}
