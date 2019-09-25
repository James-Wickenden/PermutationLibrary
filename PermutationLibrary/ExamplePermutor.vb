Public Class ExamplePermutor
    Private INPUT_VARARRAY() As Integer = {0, 1, 2}
    Private PERMUTATION_SIZE As Integer = 3
    Private ALLOW_DUPLICATES As Boolean = False

    Private permutor As New PermutationLibrary(Of Integer)(PERMUTATION_SIZE, INPUT_VARARRAY, ALLOW_DUPLICATES)

    Private Sub printVarArray(transferType As String)
        Console.Write(transferType & " PERMUTATION SIZE " & PERMUTATION_SIZE & " OF {")
        Console.Write(INPUT_VARARRAY(0))
        For i As Integer = 1 To INPUT_VARARRAY.Length - 1
            Console.Write(", " & INPUT_VARARRAY(i).ToString)
        Next
        Console.Write("}")
        If ALLOW_DUPLICATES Then Console.WriteLine("; DUPLICATE ELEMENTS ALLOWED")
        If Not ALLOW_DUPLICATES Then Console.WriteLine("; DUPLICATE ELEMENTS NOT ALLOWED")
    End Sub

    Private Sub testNumberPermsCounter()
        Console.WriteLine("NO. PERMUTATIONS: " & permutor.getNoOfPermutations)
        Console.WriteLine()
    End Sub

    Private Sub testToList()
        printVarArray("LIST")
        Dim permutations As List(Of Integer()) = permutor.permuteToList()
        For Each permutation As Integer() In permutations
            For i As Integer = 0 To permutation.Length - 1
                Console.Write(permutation(i) & ", ")
            Next
            Console.WriteLine()
        Next
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

        Dim permutationBytes(PERMUTATION_SIZE) As Byte

        Do
            permutationPost.WaitOne()
            permutationLock.WaitOne()
            stream.Position = 0
            stream.Read(permutationBytes, 0, PERMUTATION_SIZE)
            permutationAvle.Release()
            permutationLock.Release()

            Dim streamedPermutation() As Integer = permutor.bytesToPermutee(permutationBytes)
            For i As Integer = 0 To streamedPermutation.Length - 2
                Console.Write(streamedPermutation(i) & ", ")
            Next
            Console.WriteLine()
        Loop Until Not stream.CanRead
        Console.WriteLine("STREAM TERMINATED")
    End Sub

    Public Sub ExampleMain()
        testNumberPermsCounter()
        testToList()
        testToStream()
        Console.ReadLine()
    End Sub
End Class
