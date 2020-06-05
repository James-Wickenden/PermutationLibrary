Imports System
Imports PermutationLibrary

Module Program
    Sub Main()
        Dim PERMUTATION_SIZE As Integer = 3
        Dim INPUT_VARARRAY() As Char = {"a", "b", "c", "d", "e"}
        Dim ALLOW_DUPLICATES As Boolean = False
        'SetInputAsAlphabet(INPUT_VARARRAY)
        'INPUT_VARARRAY = {"a", "b", "c"}

        Dim permutor As New Permutor(Of Char)(PERMUTATION_SIZE, INPUT_VARARRAY, ALLOW_DUPLICATES)

        ' List Permuting
        'permutor.GetNoOfPermutations()
        'Dim permutedList As List(Of Char()) = permutor.PermuteToList
        'For Each permutation As String In permutedList
        '    Console.WriteLine(permutation)
        'Next

        ' Stream Permuting
        'Dim permutedStreamReceiver As Char()
        'permutor.InitStreamPermutor()
        'While permutor.IsStreamActive
        '    permutedStreamReceiver = permutor.GetPermutationFromStream
        '    Console.WriteLine(permutedStreamReceiver)
        'End While

        Console.ReadLine()
    End Sub

    Private Sub SetInputAsAlphabet(ByRef INPUT_VARARRAY As Char())
        Dim alphabet As New List(Of Char)
        For i As Integer = 0 To 25
            alphabet.Add(Microsoft.VisualBasic.Chr(97 + i))
        Next
        INPUT_VARARRAY = alphabet.ToArray
    End Sub
End Module
