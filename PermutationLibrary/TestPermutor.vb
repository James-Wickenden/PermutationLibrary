Public Module TestPermutor
    Private permutor As PermutationLibrary(Of Char)

    Private Sub printVarArray(transferType As String)
        Console.Write(transferType & " PERMUTATION SIZE " & permutor.getSizeOfPermutation & " OF {")
        Console.Write(permutor.getPossibleValues(0))
        For i As Integer = 1 To permutor.getPossibleValues.Length - 1
            Console.Write(", " & permutor.getPossibleValues(i).ToString)
        Next
        Console.Write("}")
        If permutor.getAllowDuplicates Then Console.WriteLine("; DUPLICATE ELEMENTS ALLOWED")
        If Not permutor.getAllowDuplicates Then Console.WriteLine("; DUPLICATE ELEMENTS NOT ALLOWED")

    End Sub

    Private Sub testNumberPermsCounter()
        Console.WriteLine("NO. PERMUTATIONS: " & permutor.getNoOfPermutations)
        Console.WriteLine("/////////////////////////")
        Console.WriteLine()
    End Sub

    Private Sub testToList()
        printVarArray("LIST")
        Dim perms As List(Of Char()) = permutor.permuteToList()
        For Each elem As Char() In perms
            For Each letter As Char In elem
                Console.Write(letter)
            Next
            Console.WriteLine()
        Next
        Console.WriteLine()
        Console.WriteLine("/////////////////////////")
        Console.WriteLine()
    End Sub

    Private Sub testToStream()
        printVarArray("STREAM")

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

    Private Sub testBasicToList()
        printVarArray("FASTLIST")
        Dim perms As List(Of Char()) = permutor.basicPermuteToList()
        For Each elem As Char() In perms
            For Each letter As Char In elem
                Console.Write(letter)
            Next
            Console.WriteLine()
        Next
        Console.WriteLine()
        Console.WriteLine("/////////////////////////")
        Console.WriteLine()
    End Sub

    Private Sub setInputAsAlphabet(ByRef INPUT_VARARRAY As Char())
        Dim alphabet As New List(Of Char)
        For i As Integer = 0 To 25
            alphabet.Add(Microsoft.VisualBasic.Chr(97 + i))
        Next
        INPUT_VARARRAY = alphabet.ToArray
    End Sub

    Public Sub Main()
        Dim INPUT_VARARRAY() As Char = {"a", "b", "c", "d", "e"}
        Dim PERMUTATION_SIZE As Integer = 3
        Dim ALLOW_DUPLICATES As Boolean = True
        'setInputAsAlphabet(INPUT_VARARRAY)

        permutor = New PermutationLibrary(Of Char)(PERMUTATION_SIZE, INPUT_VARARRAY, ALLOW_DUPLICATES)

        testNumberPermsCounter()
        testToStream()
        testToList()
        testBasicToList()

        Console.WriteLine("FINISHED")
        Console.ReadLine()
    End Sub
End Module
