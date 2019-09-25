Module Module1
    Private INPUT_VARARRAY() As Char = {"a", "b", "c", "d"}
    Private PERMUTATION_SIZE As Integer = 3
    Private ALLOW_DUPLICATES As Boolean = True

    Private permutor As New PermutationLibrary(Of Char)(PERMUTATION_SIZE, INPUT_VARARRAY, ALLOW_DUPLICATES)

    Public Sub Main()

        Dim perms As List(Of Char()) = permutor.basicPermuteToList()
        For Each elem As Char() In perms
            For Each letter As Char In elem
                Console.Write(letter)
            Next
            Console.WriteLine()
        Next
        Console.ReadLine()

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

            Dim streamedPermutation() As Char = permutor.bytesToPermutee(permutationBytes)
            For i As Integer = 0 To streamedPermutation.Length - 2
                Console.Write(streamedPermutation(i))
            Next
            Console.WriteLine()
        Loop Until Not stream.CanRead
        Console.ReadLine()
    End Sub
End Module
