Imports PermutationLibrary

Public Module TestPermutor
    Private permutor As PermutationLibrary.Permutor(Of Char)

    Private Class TestObject
        Public val As Decimal
        Public Sub New(val)
            Me.val = val
        End Sub
    End Class

    Private Sub printCharArray(A As Char())
        Console.Write(A(0))
        For i As Integer = 1 To A.Length - 1
            Console.Write(", " & A(i))
        Next
    End Sub

    Private Sub PrintVarArray(transferType As String, printAD As Boolean)
        Console.Write(transferType & " PERMUTATION SIZE " & permutor.getSizeOfPermutation & " OF {")
        Console.Write(permutor.GetPossibleValues(0))
        For i As Integer = 1 To permutor.getPossibleValues.Length - 1
            Console.Write(", " & permutor.GetPossibleValues(i).ToString)
        Next
        Console.Write("}")

        If printAD Then
            If permutor.getAllowDuplicates Then Console.WriteLine("; DUPLICATE ELEMENTS ALLOWED")
            If Not permutor.getAllowDuplicates Then Console.WriteLine("; DUPLICATE ELEMENTS NOT ALLOWED")
        Else
            Console.WriteLine()
        End If
    End Sub

    Private Sub TestNumberPermsCounter()
        Console.WriteLine("NO. PERMUTATIONS: " & permutor.getNoOfPermutations)
        Console.WriteLine("/////////////////////////")
        Console.WriteLine()
    End Sub

    Private Sub TestToList()
        PrintVarArray("LIST", True)
        Dim perms As List(Of Char()) = permutor.permuteToList()
        For Each elem As Char() In perms
            Console.WriteLine(elem)
        Next
        Console.WriteLine()
        Console.WriteLine("/////////////////////////")
        Console.WriteLine()
    End Sub

    Private Sub TestToStream()
        PrintVarArray("STREAM", True)

        permutor.initStreamPermutor()
        While permutor.isStreamActive
            Dim str As String = permutor.getPermutationFromStream()
            Console.WriteLine(str)
        End While

        Console.WriteLine("STREAM TERMINATED")
        Console.WriteLine()
        Console.WriteLine("/////////////////////////")
        Console.WriteLine()
    End Sub

    Private Sub TestBasicToList()
        PrintVarArray("FASTLIST", True)
        Dim perms As List(Of Char()) = permutor.BasicPermuteToList()
        For Each elem As Char() In perms
            Console.WriteLine(elem)
        Next
        Console.WriteLine(perms.Count)
        Console.WriteLine()
        Console.WriteLine("/////////////////////////")
        Console.WriteLine()
    End Sub

    Private Sub TestRandomPermutation()
        PrintVarArray("RANDOM", False)

        Dim rndPerm() As Char
        Dim generator As New Random

        permutor.setAllowDuplicates(True)
        Console.WriteLine("DUPLICATES: ALLOWED")
        For i As Integer = 0 To 10
            rndPerm = permutor.getRandomPermutation(generator)
            Console.Write(i & ": ")
            printCharArray(rndPerm)
        Next

        permutor.setAllowDuplicates(False)
        Console.WriteLine("DUPLICATES: NOT ALLOWED")
        For i As Integer = 0 To 10
            rndPerm = permutor.getRandomPermutation(generator)
            Console.Write(i & ": ")
            printCharArray(rndPerm)
        Next
    End Sub

    Private Sub SetInputAsAlphabet(ByRef INPUT_VARARRAY As Char())
        Dim alphabet As New List(Of Char)
        For i As Integer = 0 To 25
            alphabet.Add(Microsoft.VisualBasic.Chr(97 + i))
        Next
        INPUT_VARARRAY = alphabet.ToArray
    End Sub

    Private Sub CustomTest()

        'Dim testObjects(5) As TestObject
        'For i As Integer = 0 To 5
        '    testObjects(i) = New TestObject(CDec(i))
        'Next

        'Dim permutor2 As New Permutor(Of TestObject)(2, testObjects, True)
        'Dim permuted_ACTUAL As List(Of TestObject()) = permutor2.PermuteToList()

        'For Each elem As TestObject() In permuted_ACTUAL
        '    For i As Integer = 0 To elem.Length - 1
        '        Console.Write(elem(i).val)

        '    Next
        '    Console.WriteLine()
        'Next

    End Sub

    Public Sub Main()
        Dim INPUT_VARARRAY() As Char = {"a", "b", "c", "d", "e"}
        Dim PERMUTATION_SIZE As Integer = 3
        Dim ALLOW_DUPLICATES As Boolean = True
        'setInputAsAlphabet(INPUT_VARARRAY)

        permutor = New Permutor(Of Char)(PERMUTATION_SIZE, INPUT_VARARRAY, ALLOW_DUPLICATES)
        permutor.validate(True)

        CustomTest()
        'TestNumberPermsCounter()
        'TestToStream()
        'TestToList()
        'TestBasicToList()
        'TestRandomPermutation()

        Console.WriteLine("FINISHED")
        Console.ReadLine()
    End Sub
End Module
