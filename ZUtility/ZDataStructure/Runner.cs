using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.CompilerServices;

namespace ZDataStructure
{
    class Runner
    {
        static void Main()
        {
            //long[] array = Sort.CreateArray(20);
            long[] array = new long[11] { 5, 2, 9, 1, 10, 7, 8, 3, 6, 4, 11 };

            Print("Intial array: " + Sort.Array2String(array));

            Action<string> printer = Print;

            //Sort.BubbleSort(array, printer);
            //Sort.SelectionSort(array, printer);
            //Sort.InsertionSort(array, printer);
            //Sort.ShellSort(array, printer);
            //Sort.QuickSort(array, printer);
            //Sort.HeapSort(array, printer);
            //Sort.TreeSort(array, printer);
            //Sort.MergeSort(array, printer);

            string str = "ABCD";
            char[] charArray = str.ToCharArray();
            Permute(charArray, 0, charArray.Length - 1);

            Console.Read();
        }

        private static void Print(string value)
        {
            Console.WriteLine(value);
        }

        private static void Permute(char[] array, int start, int end)
        {
            int i;
            if (start == end)
                Console.WriteLine(array);
            else
            {
                for(i = start; i <= end; i++)
                {
                    Swap(ref array[start], ref array[i]);
                    Permute(array, start + 1, end);
                    Swap(ref array[start], ref array[i]); //backtrack
                }
            }
        }

        private static void Swap(ref char a, ref char b)
        {
            char tmp;
            tmp = a;
            a = b;
            b = tmp;
        }
    }
}
