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
                Console.Write(letter & " ")
            Next
            Console.WriteLine()
        Next
        Console.WriteLine()
        Console.WriteLine("/////////////////////////")
        Console.WriteLine()
    End Sub

    Private Sub testToStream()
        printVarArray("STREAM")
        Dim stream As System.IO.Stream = New System.IO.MemoryStream()

        Dim permutationAvle As New Threading.Semaphore(1, 1)
        Dim permutationPost As New Threading.Semaphore(0, 1)
        Dim permutationLock As New Threading.Semaphore(1, 1)
        Dim permutationThread As New Threading.Thread(New Threading.ThreadStart(Sub() permutor.permuteToStream(stream, permutationAvle, permutationPost, permutationLock)))

        permutationThread.Start()

        Dim permutationBytes(permutor.getSizeOfPermutation) As Byte

        Do
            permutationPost.WaitOne()
            permutationLock.WaitOne()
            stream.Position = 0
            stream.Read(permutationBytes, 0, permutor.getSizeOfPermutation)
            permutationAvle.Release()
            permutationLock.Release()

            Dim streamedPermutation() As Char = permutor.bytesToPermutee(permutationBytes)
            For i As Integer = 0 To streamedPermutation.Length - 2
                Console.Write(streamedPermutation(i) & " ")
            Next
            Console.WriteLine()
        Loop Until Not stream.CanRead
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
                Console.Write(letter & " ")
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
        Dim ALLOW_DUPLICATES As Boolean = False
        'setInputAsAlphabet(INPUT_VARARRAY)

        permutor = New PermutationLibrary(Of Char)(PERMUTATION_SIZE, INPUT_VARARRAY, ALLOW_DUPLICATES)

        testNumberPermsCounter()
        testToStream()
        testToList()
        testBasicToList()

        Console.WriteLine("FINISHED TESTS")
        Console.ReadLine()
    End Sub
End Module
