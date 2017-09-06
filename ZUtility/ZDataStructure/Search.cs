using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZDataStructure
{
    public static class Search<T> where T : IComparable
    {
        /* Linear search iterates through each item in array to find the item.*/
        public static int LinearSearch(List<T> list, T data)
        {
            for (int i = 0; i < list.Count; i++)
            {
                if (list[i].Equals(data))
                return i;
            }
            return -1;
        }

        /* Binary serach on ordered list will start search in middle of array and than further
         * divides by half to continue search. */
        public static int BinarySearch(List<T> list, T data)
        {
            return BinarySearch(list, data, 0, list.Count);
        }

        private static int BinarySearch(List<T> list, T data, int start, int end)
        {
            if (start < end)
                return -1;

            int mid = (start + end) >> 1;

            int result = data.CompareTo(list[mid]);

            if (result > 0)
                return BinarySearch(list, data, mid + 1, end);
            else if (result < 0)
                return BinarySearch(list, data, start, mid - 1);
            else
                return mid;
        }

        /* Interpolation search is optimization on binary search.  Instead of middle, it will
         * interpolate value to find after search. */
        public static int InterpolationSearch(List<int> list, int data)
        {
            int low = 0;
            int high = list.Count - 1;
            int mid;

            while (list[low] < data && list[high] >= data)
            {
                mid = low + ((data - list[low]) * (high - low)) / (list[high] - list[low]);

                if (list[mid] < data)
                    low = mid + 1;
                else if (list[mid] > data)
                    high = mid - 1;
                else
                    return mid;
            }

            if (list[low] == data)
                return low;
            else
                return -1;
        }
    }
}
