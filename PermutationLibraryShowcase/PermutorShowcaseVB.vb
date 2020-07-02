Imports System
Imports PermutationLibrary

Module PermutorShowcaseVB
    Sub Main()
        Dim PERMUTATION_SIZE As Integer = 25
        Dim INPUT_VARARRAY() As Char = {"a", "b", "c", "d", "e"}
        Dim ALLOW_DUPLICATES As Boolean = False
        SetInputAsAlphabet(INPUT_VARARRAY)

        Dim permutor As New Permutor(Of Char)(PERMUTATION_SIZE, INPUT_VARARRAY, ALLOW_DUPLICATES)



        '' LIST PERMUTING
        'Console.WriteLine("LIST PERMUTING")
        'Dim permutedList As List(Of Char()) = permutor.PermuteToList
        'For Each permutation As String In permutedList
        '    Console.WriteLine(permutation)
        'Next
        'Console.WriteLine("Generated " & permutor.GetNoOfPermutations & " permutations.")

        '' BASIC LIST PERMUTING
        'Console.WriteLine("BASIC LIST PERMUTING")
        'Dim basicPermutedList As List(Of Char()) = permutor.BasicPermuteToList
        'For Each permutation As String In basicPermutedList
        '    Console.WriteLine(permutation)
        'Next

        '' STREAM PERMUTING
        'Console.WriteLine("STREAM PERMUTING")
        'Dim permutedStreamReceiver As Char()
        'permutor.InitStreamPermutor()
        'While permutor.IsStreamActive
        '    permutedStreamReceiver = permutor.GetPermutationFromStream
        '    Console.WriteLine(permutedStreamReceiver)
        'End While
        'Console.WriteLine("Generated " & permutor.GetNoOfPermutations & " permutations.")

        '' RANDOM PERMUTING
        'Console.WriteLine("RANDOM PERMUTING")
        'SetInputAsAlphabet(INPUT_VARARRAY)
        'permutor.Configure(40, INPUT_VARARRAY, True)
        'Dim generator As New Random
        'Dim randomPermuted As Char()
        'For i As Integer = 1 To 9
        '    randomPermuted = permutor.GetRandomPermutation(generator)
        '    Console.WriteLine(i & ". " & randomPermuted)
        'Next

        '' DISPOSAL
        'permutor.Dispose()

        Console.ReadLine()
    End Sub

    Private Sub SetInputAsAlphabet(ByRef INPUT_VARARRAY As Char())
        Dim alphabet As New List(Of Char)
        For i As Integer = 0 To 26
            alphabet.Add(Microsoft.VisualBasic.Chr(97 + i))
        Next
        INPUT_VARARRAY = alphabet.ToArray
    End Sub
End Module
