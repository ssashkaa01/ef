using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleAppUnsafe
{
    class Program
    {
     
        static unsafe void Main(string[] args)
        {
            /*
             Завдання 1:
            Дано два масиви: А [M] і B [N] (M і N вводяться з клавіатури). Необхідно створити
            третій масив мінімально можливого розміру, в якому потрібно зібрати елементи
            обох масивів. Використати stackalloc.
            */

            int j = 0;

            const int size = 3;
            int* arr = stackalloc int[size];

            const int size2 = 2;
            int* arr2 = stackalloc int[size2];

            const int size3 = size + size2;
            int* arr3 = stackalloc int[size3];

            for (int i = 0; i < size; i++, j++)
            {
                arr[i] = Convert.ToInt32(Console.ReadLine());
                *(arr3 + j) = *(arr + i);
            }

            for (int i = 0; i < size2; i++, j++)
            {
                arr2[i] = Convert.ToInt32(Console.ReadLine());
                *(arr3 + j) = *(arr2 + i);
            }

            for (int i = 0; i < size3; i++)
            {
                Console.WriteLine(arr3[i]);
            }

            /*
            Завдання 2:
            Написати метод, який отримує вказівник на масив і його розмір. Метод повинен
            створити новий масив, видаливши всі непарні числа та повернути його адресу.'
            */

            //int[] arr = new int[8] { 7, 7, 3, 4, 5, 6, 7, 8 };

            //fixed (int* arrPtr = arr)
            //{
            //    int* arr2 = DelUnmatched(arrPtr, arr.Length);
            //}


            /*
            Завдання 3:
            Написати метод, який буде приймати масив та створювати новий видаливши всі
            дублікати. Метод повертає адресу нового масива.
            */

            //int[] arr = new int[9] { 7, 7, 3, 3, 4, 5, 6, 7, 8 };

            //fixed (int* arrPtr = arr)
            //{
            //    int* arr2 = OnlyUnique(arrPtr, arr.Length);
            //}


            /*
            Завдання 4:
            Написати метод, який приймає два масив та створювати новий, в якому містяться
            спільні елементи для обох масиві. Метод повертає адресу нового масива.
             */


            //int[] arr1 = new int[7] { 7, 3, 4, 5, 6, 7, 8 };
            //int[] arr2 = new int[7] { 7, 1, 2, 5, 6, 10, 8 };

            //fixed (int* arrPtr1 = arr1)
            //{
            //    fixed (int* arrPtr2 = arr2)
            //    {
            //        int* arr3 = CommonArray(arrPtr1, arr1.Length, arrPtr2, arr2.Length);
            //    }
            //}






        }

        public static unsafe int* CommonArray(int* arr1, int size1, int* arr2, int size2)
        {
            int size = (size1 > size2) ? size1 : size2;

            int* newArr = stackalloc int[size];
            int j = 0;

            for (int i = 0; i < size; i++)
            {
                if(size1 > size2)
                {
                    if (InArray(arr1, size1, arr2[i]))
                    {
                        *(newArr + j) = *(arr2 + i);
                        j++;
                        //Console.WriteLine(*(arr2 + i));
                    }
                } else
                {
                    if (InArray(arr2, size2, arr1[i]))
                    {
                        *(newArr + j) = *(arr1 + i);
                        j++;
                        //Console.WriteLine(*(arr1 + i));
                    }
                }
            }

            return newArr;
        }

        public static unsafe int* DelUnmatched(int* arr, int size)
        {
            int count = 0;

            for (int i = 0; i < size; i++)
            {
                if (arr[i] % 2 == 0)
                {
                    count++;
                }
            }

            int* newArr = stackalloc int[count];
            int j = 0;

            for (int i = 0; i < size; i++)
            {
                if (*(arr + i) % 2 == 0)
                {
                    *(newArr + j) = *(arr + i);
                    Console.WriteLine(*(newArr + j));
                    j++;
                }
            }

            return newArr;
        }

        public static unsafe bool InArray(int* arr, int size, int value)
        {
            for (int i = 0; i < size; i++)
            {
                if (value == arr[i])
                {
                    return true;
                }
            }

            return false;
        }

        public static unsafe int* OnlyUnique(int* arr, int size)
        {

            int* newArr = stackalloc int[size];
            int j = 0;

            for (int i = 0; i < size; i++)
            {      
                if (!InArray(newArr, j, arr[i]))
                {
                    *(newArr + j) = arr[i];
                    j++;
                    //Console.WriteLine(*(newArr + j));
                }
            }

            return newArr;
        }
    }
}
