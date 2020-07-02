using System;
using PermutationLibrary;
using System.Collections.Generic;

namespace PermutorCSShowcase
{
    class PermutorShowcaseCS
    {
        static private char[] INPUT_VARARRAY = { 'a', 'b', 'c', 'd', 'e' };
        static private int PERMUTATION_SIZE = 3;
        static private bool ALLOW_DUPLICATES = false;

        static void Main(string[] args)
        {
            //SetInputAsAlphabet();

            Permutor<Char> permutor = new Permutor<char>(PERMUTATION_SIZE, INPUT_VARARRAY, ALLOW_DUPLICATES);

            //// LIST PERMUTING
            //Console.WriteLine("LIST PERMUTING");
            //List<char[]> permutedList = permutor.PermuteToList();
            //foreach (char[] permutation in permutedList)
            //{
            //    Console.WriteLine(permutation);
            //}
            //Console.WriteLine("Generated " + permutor.GetNoOfPermutations() + " permutations.");

            //// BASIC LIST PERMUTING
            //Console.WriteLine("BASIC LIST PERMUTING");
            //List<char[]> basicPermutedList = permutor.BasicPermuteToList();
            //foreach (char[] permutation in basicPermutedList)
            //{
            //    Console.WriteLine(permutation);
            //}

            //// STREAM PERMUTING
            //Console.WriteLine("STREAM PERMUTING");
            //permutor.InitStreamPermutor();
            //while (permutor.IsStreamActive())
            //{
            //    Console.WriteLine(permutor.GetPermutationFromStream());
            //}
            //permutor.KillStreamPermutor();
            //Console.WriteLine("Generated " + permutor.GetNoOfPermutations() + " permutations.");

            //// RANDOM PERMUTING
            //Console.WriteLine("RANDOM PERMUTING");
            //SetInputAsAlphabet();
            //permutor.Configure(40, INPUT_VARARRAY, true);
            //Random generator = new Random();
            //char[] randomPermuted;
            //for (int i = 1; i < 10; i++)
            //{
            //    randomPermuted = permutor.GetRandomPermutation(ref generator);
            //    Console.Write(i + ". ");
            //    Console.WriteLine(randomPermuted);
            //}

            //// DISPOSAL
            //permutor.Dispose();

            Console.ReadLine();
        }

        static private void SetInputAsAlphabet()
        {
            List<char> alphabet = new List<char>();
            for (int i = 0; i < 26; i++)
            {
                alphabet.Add(Convert.ToChar(97 + i));
            }
            INPUT_VARARRAY = alphabet.ToArray();
        }
    }
}
