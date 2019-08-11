using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PortraitEditor.Extensions
{
    public static class CollectionExtensions
    {
        public static bool Contains<TSource>(this IEnumerable<TSource> collection, TSource itemTofind, Func<TSource, TSource, bool> equalizer)
        {
            foreach (var item in collection)
            {
                if (equalizer(item, itemTofind))
                {
                    return true;
                }
            }
            return false;
        }

        public static List<int> FindAll<TSource>(this IEnumerable<TSource> collection, TSource itemTofind) where TSource : IEquatable<TSource>
        {
            List<int> Position = new List<int>();
            int PosCounter = 0;
            foreach (var item in collection)
            {
                if (item.Equals(itemTofind))
                {
                    Position.Add(PosCounter);
                }
                PosCounter++;
            }
            if (Position.Count > 0)
                return Position;
            else
                return null;
        }




    }
}
