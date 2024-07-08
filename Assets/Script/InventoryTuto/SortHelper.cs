using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public interface ISwapper<T>
{
    void Swap(T[] array, int i, int j);
}

public static class SortHelper
{
    private const int InsertionSortThreshold = 8;//이 크기를 못 넘으면 삽입정렬 사용.
    public static void Sort<T>(T[] array, int left, int right, IComparer<T> comparer, ISwapper<T> swapper)
    {
        if (array == null || comparer == null || swapper == null) 
            throw new ArgumentNullException();
        if (left < 0 || right > array.Length) 
            throw new ArgumentOutOfRangeException();

        if (right - left < InsertionSortThreshold)
        {
            InsertionSort(array, left, right, comparer, swapper);
            Debug.Log("Execute InsertionSort");
        }
        else
        {
            QuickSort(array, left, right, comparer, swapper);
            Debug.Log("Execute QuickSort");
        }
    }

    private static void QuickSort<T>(T[] array, int left, int right, IComparer<T> comparer, ISwapper<T> swapper)
    {
        if (left < right)
        {
            int pivot = Partition(array, left, right, comparer, swapper);

            QuickSort(array, left, pivot - 1, comparer, swapper);
            QuickSort(array, pivot + 1, right, comparer, swapper);
        }
    }

    private static int Partition<T>(T[] array, int left, int right, IComparer<T> comparer, ISwapper<T> swapper)
    {
        T pivot = array[right];
        int i = left - 1;
        for (int j = left; j < right; j++)
        {
            if (comparer.Compare(array[j], pivot) < 0)
            {
                i++;
                swapper.Swap(array, i, j);
            }
        }
        swapper.Swap(array, i + 1, right);
        return i + 1;
    }

private static void InsertionSort<T>(T[] array, int left, int right, IComparer<T> comparer, ISwapper<T> swapper)
    {
        for (int i = left + 1; i <= right; i++)
        {
            int j = i;
            while (j > left && comparer.Compare(array[j - 1], array[j]) > 0)
            {
                swapper.Swap(array, j - 1, j);
                j--;
            }
        }
    }
}