using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ZDataStructure
{
    class Sort
    {
        public static long[] CreateArray(long count)
        {
            long[] array = new long[count];

            Random rnd = new Random();

            for (long i = 0; i < count; i++)
                array[i] = rnd.Next();

            return array;
        }

        public static string Array2String(long[] array)
        {
            StringBuilder output = new StringBuilder();

            for (long i = 0; i < array.LongLength; i++)
                output.Append(array[i] + " ");

            return output.ToString();
        }

        private static void Swap(ref long val1, ref long val2)
        {
            long temp = val1;
            val1 = val2;
            val2 = temp;
        }

        /// <summary>
        /// Parent Category - Comparison sort
        /// Category - Exchange sort
        /// Bubble sort - It is a straightforward and simplistic method of sorting data. The algorithm starts at the beginning of the data set. 
        /// It compares the first two elements, and if the first is greater than the second, then it swaps them. 
        /// It continues doing this for each pair of adjacent elements to the end of the data set. 
        /// It then starts again with the first two elements, repeating until no swaps have occurred on the last pass. 
        /// This algorithm is highly inefficient, and is rarely used.
        /// Best case - O(n)
        /// Average case - O(n^2)
        /// Worst case - O(n^2)
        /// </summary>
        /// <param name="array"></param>
        public static void BubbleSort(long[] array, Action<string> printer)
        {
            printer("BubbleSort Start.");
            for (int i = 0; i < array.LongLength; i++)
            {
                printer("Outer loop: " + i);
                for (int j = 0; j < array.LongLength - 1; j++)
                {
                    if (array[j] > array[j + 1])
                        Swap(ref array[j], ref array[j + 1]);

                    printer("\tInner loop: " + j);
                    printer("\t" + Array2String(array));
                }
            }
            printer("BubbleSort Finish.");
        }

        /// <summary>
        /// Parent Category - Comparison sort
        /// Category - Selection sort
        /// Selection sort - in-place comparison sort.
        /// It has O(n2) complexity, making it inefficient on large lists, and generally performs worse than the similar insertion sort. 
        /// Selection sort is noted for its simplicity, and also has performance advantages over more complicated algorithms in certain situations.
        /// Better than bubble sort in almost all cases
        /// Best case - O(n^2)
        /// Average case - O(n^2)
        /// Worst case - O(n^2)  
        /// </summary>
        /// <param name="array"></param>
        public static void SelectionSort(long[] array, Action<string> printer)
        {
            printer("SelectionSort Start.");
            for (int i = 0; i < array.LongLength; i++)
            {
                printer("Outer loop: " + i);
                long minIndex = i;

                for (int j = i + 1; j < array.LongLength; j++)
                {
                    if (array[j] < array[minIndex])
                        minIndex = j;
                    printer.Invoke("\tInner loop: " + j + " min index: " + minIndex);
                }
                Swap(ref array[i], ref array[minIndex]);
                printer(Array2String(array));
            }
            printer("SelectionSort Finish.");
        }

        /// <summary>
        /// Parent Category - Comparison sort
        /// Category - Insertion sort
        /// Insertion sort - It is a simple sorting algorithm that is relatively efficient for small lists and mostly-sorted lists, 
        /// and often is used as part of more sophisticated algorithms. 
        /// It works by taking elements from the list one by one and inserting them in their correct position into a new sorted list.
        /// Shell sort is a variant of insertion sort which is more efficient for larger lists because in arrays, insertion is expensive 
        /// and requires shifting of all elements over by one.
        /// Best case - O(n)
        /// Average case - O(n^2)
        /// Worst case - O(n^2) 
        /// </summary>
        /// <param name="array"></param>
        public static void InsertionSort(long[] array, Action<string> printer)
        {
            printer("InsertionSort Start.");

            long j = 0;
            long temp = 0;

            for (int i = 1; i < array.LongLength; i++)
            {
                printer("Outer loop: " + i);
                j = i;
                temp = array[i];

                while((j > 0) && (array[j - 1] > temp))
                {
                    printer.Invoke("\tInner loop: " + j);

                    array[j] = array[j - 1];
                    j = j - 1;
                }
                array[j] = temp;
                printer.Invoke(Array2String(array));
            }

            printer("InsertionSort Finish.");
        }

        /// <summary>
        /// Parent Category - Comparison sort
        /// Category - Insertion sort
        /// Shell sort - It improves upon bubble sort and insertion sort by moving out of order elements more than one position at a time.
        /// It compares elements separated by a gap of several positions. This lets an element take "bigger steps" toward its expected position. 
        /// Multiple passes over the data are taken with smaller and smaller gap sizes. 
        /// The last step of Shell sort is a plain insertion sort, but by then, the array of data is guaranteed to be almost sorted.
        /// Best case - O(n)
        /// Average case - depends on gap sequence
        /// Worst case - O(n^2) or O(nlog^2 n) depends on gap sequence 
        /// </summary>
        /// <param name="array"></param>
        public static void ShellSort(long[] array, Action<string> printer)
        {
            printer("ShellSort Start.");

            int iterator = 0;
            long j, temp = 0;
            int increment = (array.Length) / 2;
            while (increment > 0)
            {
                for (int i = 0; i < array.Length; i++)
                {
                    j = i;
                    temp = array[i];
                    while ((j >= increment) && array[j - increment] > temp)
                    {
                        array[j] = array[j - increment];
                        j = j - increment;
                    }
                    array[j] = temp;
                    printer("\tAfter pass " + (i + 1) + ":");
                    printer("\t" + Array2String(array));
                }

                if (increment / 2 != 0)
                    increment = increment / 2;
                else if (increment == 1)
                    increment = 0;
                else
                    increment = 1;
                iterator++;

                printer("\nAfter iteration " + (iterator) + "");
                printer(Array2String(array));
            }

            printer("ShellSort Finish.");
        }

        /// <summary>
        /// Parent Category - Comparison sort
        /// Category - Exchange sort
        /// Quick Sort - It is a divide and conquer algorithm which relies on a partition operation: 
        /// To partition an array, choose an element, called a pivot, move all smaller elements before the pivot, 
        /// and move all greater elements after it. This can be done efficiently in linear time and in-place. 
        /// Later recursively sort the lesser and greater sublists.
        /// Best case - O(n log n)
        /// Average case - O(n log n)
        /// Worst case - O(n^2)  
        /// </summary>
        /// <param name="array"></param>
        public static void QuickSort(long[] array, Action<string> printer)
        {
            printer("QuickSort Start.");
            InternalQuickSort(array, 0, array.Length - 1, printer);
            printer(Array2String(array));
            printer("QuickSort Finish.");
        }

        /// <summary>
        /// Internal recursive sort algorithm for quick sort using divide and conquer. Sorting is done based on pivot
        /// </summary>
        /// <param name="array"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        private static void InternalQuickSort(long[] array, int left, int right, Action<string> printer)
        {
            int pivotNewIndex = Partition(array, left, right, printer);

            long pivot = array[(left + right) / 2];

            if (left < pivotNewIndex - 1)
                InternalQuickSort(array, left, pivotNewIndex - 1, printer);

            if (pivotNewIndex < right)
                InternalQuickSort(array, pivotNewIndex, right, printer);
        }

        /// <summary>
        /// This operation returns a new pivot everytime it is called recursively and swaps the array elements based on pivot value comparison
        /// </summary>
        /// <param name="array"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static int Partition(long[] array, int left, int right, Action<string> printer)
        {
            int i = left, j = right;
            long pivot = array[(left + right) / 2];

            while (i <= j)
            {
                while (array[i] < pivot)
                    i++;
                while (array[j] > pivot)
                    j--;
                if (i <= j)
                {
                    Swap(ref array[i], ref array[j]);
                    i++; j--;
                    printer(Array2String(array));
                }
            }
            return i;
        }

        /// <summary>
        /// Parent Category - Comparison sort
        /// Category - Merge sort
        /// Merge sort - It takes advantage of the ease of merging already sorted lists into a new sorted list. 
        /// It starts by comparing every two elements (i.e., 1 with 2, then 3 with 4...) and swapping them if the first should come after the second. 
        /// It then merges each of the resulting lists of two into lists of four, then merges those lists of four, and so on; 
        /// until at last two lists are merged into the final sorted list.
        /// In most implementations it is stable, meaning that it preserves the input order of equal elements in the sorted output. 
        /// It is an example of the divide and conquer algorithmic paradigm.
        /// Best case - O(n) or O(n log n)
        /// Average case - O(n log n)
        /// Worst case - O(n log n)  
        /// </summary>
        /// <param name="array"></param>
        public static void MergeSort(long[] array, Action<string> printer)
        {
            printer("MergeSort Start.");
            InternalMergeSort(array, 0, array.Length - 1, printer);
            printer(Array2String(array));
            printer("MergeSort Finish.");
        }

        /// <summary>
        /// Internal recursive sort algorithm for merge sort using divide and conquer
        /// </summary>
        /// <param name="array"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        private static void InternalMergeSort(long[] array, int left, int right, Action<string> printer)
        {
            int mid = 0;

            if (left < right)
            {
                mid = (left + right) / 2;
                InternalMergeSort(array, left, mid, printer);
                InternalMergeSort(array, (mid + 1), right, printer);

                MergeSortedArray(array, left, mid, right, printer);
            }
        }

        /// <summary>
        /// Merging two sorted lists into a big sorted list step by step i.e. each of the resulting lists of two into lists of four, 
        /// then merges those lists of four, and so on; 
        /// </summary>
        /// <param name="array"></param>
        /// <param name="left"></param>
        /// <param name="mid"></param>
        /// <param name="right"></param>
        private static void MergeSortedArray(long[] array, int left, int mid, int right, Action<string> printer)
        {
            int index = 0;
            int total_elements = right - left + 1; //BODMAS rule
            int right_start = mid + 1;
            int temp_location = left;
            long[] tempArray = new long[total_elements];

            while ((left <= mid) && right_start <= right)
            {
                if (array[left] <= array[right_start])
                {
                    tempArray[index++] = array[left++];
                }
                else
                {
                    tempArray[index++] = array[right_start++];
                }
            }

            if (left > mid)
            {
                for (int j = right_start; j <= right; j++)
                    tempArray[index++] = array[right_start++];
            }
            else
            {
                for (int j = left; j <= mid; j++)
                    tempArray[index++] = array[left++];
            }

            //Array.Copy(tempArray, 0, inputArray, temp_location, total_elements); // just another way of accomplishing things (in-built copy)
            for (int i = 0, j = temp_location; i < total_elements; i++, j++)
            {
                array[j] = tempArray[i];
            }
        }

        /// <summary>
        /// Parent Category - Comparison sort
        /// Category - Selection sort
        /// Heap sort - Create Heap
        /// Best case - O(n log n)
        /// Average case - O(n log n)
        /// Worst case - O(n log n)  
        /// </summary>
        /// <param name="array"></param>
        public static void HeapSort(long[] array, Action<string> printer)
        {
            printer("HeapSort Start.");
            //Create Heap.
            //Heap.GetMax();
            printer("HeapSort Finish.");
        }

        public static void TreeSort(long[] array, Action<string> printer)
        {
            printer("TreeSort Start.");
            //Create Binary Search Tree.
            //Run Inorder traversal.
            printer("TreeSort Finish.");
        }
    }
}
