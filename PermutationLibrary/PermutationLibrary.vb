Option Strict On
Imports System.Runtime.CompilerServices

Namespace PermutationLibrary
    Public Class Permutor(Of T)
        Implements IDisposable

        'https://github.com/James-Wickenden/VB-Permutor
        Private disposed As Boolean = False

        Private sizeOfPermutation As Integer
        Private possibleValues() As T
        Private possibleValueIndices As List(Of Integer)
        Private allowDuplicates As Boolean
        Private streamHandler As StreamHandler(Of T)

        '/////////////////////////
        'These methods relate to initialisation and configuration of permutor attributes.
        '/////////////////////////

        'Initialise the attributes that classify fully an instance of the permutor.
        Public Sub New(ByVal sizeOfPermutation As Integer, ByVal possibleValues() As T, ByVal allowDuplicates As Boolean)
            possibleValueIndices = New List(Of Integer)
            Configure(sizeOfPermutation, possibleValues, allowDuplicates)
        End Sub

        Public Sub Configure(ByVal sizeOfPermutation As Integer, ByVal possibleValues() As T, ByVal allowDuplicates As Boolean)
            Me.sizeOfPermutation = sizeOfPermutation
            Me.possibleValues = possibleValues
            Me.allowDuplicates = allowDuplicates
            ConfigIndicesList()
        End Sub

        Public Sub Validate(fromList As Boolean)
            Dim exceptionStr As String = ""
            If possibleValues.Length = 0 Then exceptionStr &=
            "ERROR: [possibleValues] attribute must contain elements. "
            If possibleValues.Length >= 255 Then exceptionStr &=
            "ERROR: [possibleValues] attribute contains too many elements. "
            If possibleValueIndices.Count = 0 Then exceptionStr &=
            "ERROR: [possibleValueIndices] attribute must contain elements. Should be configured by changing the [possibleValues] attribute. "
            If possibleValueIndices.Count >= 255 Then exceptionStr &=
            "ERROR: [possibleValueIndices] attribute contains too many elements. Should be configured by changing the [possibleValues] attribute. "

            If sizeOfPermutation < 0 Then exceptionStr &=
            "ERROR: [sizeOfPermutation] attribute must be a positive integer. "
            If sizeOfPermutation > possibleValues.Length And allowDuplicates = False Or
           sizeOfPermutation > possibleValueIndices.Count And allowDuplicates = False Then exceptionStr &=
            "ERROR: [sizeOfPermutation] attribute must be lower than magnitude of [possibleValues] if duplicate elements are not allowed. "

            If fromList = True Then
                Dim noPerms As Long = GetNoOfPermutations()
                If noPerms = -1 Or noPerms >= Math.Pow(2, 28) Then exceptionStr &= "ERROR: Too many permutations generated."
            End If

            If exceptionStr <> "" Then Throw New Exception(exceptionStr)
        End Sub

        'Sets up the indices list which is used for permuting corresponding integer indices instead of objects of type T.
        Private Sub ConfigIndicesList()
            Dim x As Integer = 0
            possibleValueIndices.Clear()
            For Each elem As T In possibleValues
                possibleValueIndices.Add(x)
                x += 1
            Next
        End Sub

        'Establishes the first permutation as an array size [sizeOfPermutation], with each element as [possibleValues.First]
        Private Function InitPermutingArray() As List(Of Integer)
            Dim permutingList As New List(Of Integer)
            For i As Integer = 0 To sizeOfPermutation - 1
                permutingList.Add(possibleValueIndices.First)
            Next
            Return permutingList
        End Function

        'Getters and setters for the permutor attributes. Not used in the code but added here for the user.
        Public Sub SetSizeOfPermutation(newPermutationSize As Integer)
            sizeOfPermutation = newPermutationSize
        End Sub

        Public Sub SetPossibleValues(newPossibleValues As T())
            possibleValues = newPossibleValues
            ConfigIndicesList()
        End Sub

        Public Sub SetAllowDuplicates(newAllowDuplicates As Boolean)
            allowDuplicates = newAllowDuplicates
        End Sub

        Public Function GetSizeOfPermutation() As Integer
            Return sizeOfPermutation
        End Function

        Public Function GetPossibleValues() As T()
            Return possibleValues.ToArray
        End Function

        Public Function GetAllowDuplicates() As Boolean
            Return allowDuplicates
        End Function

        '/////////////////////////
        'These methods provide basic mathematical and logical functionality.
        '/////////////////////////

        Private Function Factorial(x As Long) As Long
            If x <= 1 Then Return 1
            Return x * Factorial(x - 1)
        End Function

        Private Shared Sub Swap(ByRef x As Integer, ByRef y As Integer)
            Dim temp As Integer = x
            x = y
            y = temp
        End Sub

        '/////////////////////////
        'These methods provide logic functions that together generate permuations.
        '/////////////////////////

        'Used to determine when the final permutation has been calculated
        'The final permutation is reached when each element in [permutee] is the final value in [possibleValues]
        Private Function PermuteeContainsOnlyFinalElement(ByVal permutee As List(Of Integer)) As Boolean
            If permutee.All(Function(elem) elem.Equals(possibleValueIndices.Last)) Then
                Return True
            End If
            Return False
        End Function

        'Finds the next value for the [index] element of [permutee]
        'The next element loops around to the first if [permutee(index)] is [possibleValues.Last]
        'In this case, the [toNextColumn] flag is set to True.
        Private Sub GetNextPossibleColummValue(ByRef permutee As List(Of Integer), ByRef toNextColumn As Boolean, ByVal index As Integer)
            If possibleValueIndices.Last.Equals(permutee(index)) Then
                permutee(index) = possibleValueIndices.First
                toNextColumn = True
            Else
                permutee(index) = possibleValueIndices.ElementAt(possibleValueIndices.IndexOf(permutee(index)) + 1)
            End If
        End Sub

        'Generates the next permutation by counting up in base [possibleValues.count] a number with [sizeOfPermutation] digits.
        'Each digit corresponds to the index of that element in possibleValues.
        'This generates every possible permutation with repetition.
        Private Sub FindNextPermutation(ByRef permutee As List(Of Integer))
            Dim traversalIndex As Integer = 0
            Dim toNextColumn As Boolean = False

            GetNextPossibleColummValue(permutee, toNextColumn, 0)
            While toNextColumn = True And traversalIndex < sizeOfPermutation - 1
                toNextColumn = False
                traversalIndex += 1
                GetNextPossibleColummValue(permutee, toNextColumn, traversalIndex)
            End While
        End Sub

        'Streams the current permutation to the host thread securely using Semaphores, avoiding deadlock.
        'Note that the corresponding code must be used in the host thread to receive the streamed data, given in the exampleStreamReceiver() method.
        Private Sub OutputHandler(ByVal permutee() As Integer, ByRef stream As System.IO.MemoryStream,
                              ByRef permutationAvle As Threading.Semaphore,
                              ByRef permutationPost As Threading.Semaphore,
                              ByRef permutationLock As Threading.Semaphore)
            If Not allowDuplicates And permutee.Distinct.Count <> permutee.Length Then Exit Sub
            Dim permuteeIndexBytes() As Byte = PermuteeToBytes(permutee)

            permutationAvle.WaitOne()
            permutationLock.WaitOne()
            '/// CRITICAL SECTION START
            stream.Position = 0
            stream.Write(permuteeIndexBytes, 0, permuteeIndexBytes.Length)
            '/// CRITICAL SECTION END
            permutationPost.Release()
            permutationLock.Release()
        End Sub

        'Appends a new permutation to the results list by first generating the corresponding object permutation.
        Private Sub AddResultToList(ByRef res As List(Of T()), ByVal permutee As List(Of Integer))
            Dim permuteeAsTs As New List(Of T)
            For Each elem As Integer In permutee
                permuteeAsTs.Add(possibleValues.ElementAt(elem))
            Next
            res.Add(permuteeAsTs.ToArray)
        End Sub

        'Converts the [permutee] to an array of Bytes to allow transmission over the defined stream.
        'This is done by converting the element to its corresponding index value in [possibleValues].
        Public Function PermuteeToBytes(permutee() As Integer) As Byte()
            Dim permuteeCorrespondingIndices As New List(Of Byte)
            If permutee Is Nothing Then Throw New Exception
            For Each x As Integer In permutee
                permuteeCorrespondingIndices.Add(CByte(x))
            Next
            Return permuteeCorrespondingIndices.ToArray
        End Function

        'The inverse operation to permuteeToBytes.
        'Use this when reading data out of the stream.
        Public Function BytesToPermutee(bs() As Byte) As T()
            Dim permuteeCorrespondingIndices As New List(Of T)
            If bs Is Nothing Then Throw New Exception
            For Each b As Byte In bs
                permuteeCorrespondingIndices.Add(possibleValues(b))
            Next
            Return permuteeCorrespondingIndices.ToArray
        End Function

        'A recursive sub for (O(n!)) permutation. Works by simply repeating the recurive procedure for every element in the permutee.
        Private Sub BasicPermutation(ByRef res As List(Of T()), ByRef permutee As Integer(), ByRef n As Integer)
            If n = 1 Then
                AddResultToList(res, permutee.ToList)
                Exit Sub
            End If
            For i As Integer = 0 To n - 1
                Swap(permutee(i), permutee(n - 1))
                BasicPermutation(res, permutee, n - 1)
                Swap(permutee(i), permutee(n - 1))
            Next
        End Sub

        'A simple function to generate a random permutation of length [sizeOfPermutation]. Can allow or disallow repeated elements.
        'The seed is specified by the wrapper function getRandomPermutation(seed) method.
        Private Function RandomPermutation(ByVal generator As Random) As List(Of T)
            Dim res As New List(Of T)

            If (Not allowDuplicates) And (sizeOfPermutation > possibleValues.Length) Then
                'Dim errMsg As String = "ERROR: [sizeOfPermutation] attribute must be lower than magnitude of [possibleValues] if duplicate elements are not allowed. "
                Throw New Exception()
            End If

            For i As Integer = 1 To sizeOfPermutation
                Dim index As Integer = generator.Next(0, possibleValues.Length)
                If allowDuplicates Then
                    res.Add(possibleValues(index))
                Else
                    Dim newval As Boolean = False
                    While Not newval
                        If Not res.Contains(possibleValues(index)) Then
                            newval = True
                            res.Add(possibleValues(index))
                        End If
                        If Not newval Then index = generator.Next(0, possibleValues.Length)
                    End While
                End If
            Next
            Return res
        End Function

        '/////////////////////////
        'These methods provide the main interface for meaningful utilisation of the code.
        '/////////////////////////

        'Returns the number of permutations that the permutor will generate if called.
        'Calucates differently depending on whether duplicate elements are allowed.
        'A return value of -1 indicates more than 2^64 results are returned by permuting.
        Public Function GetNoOfPermutations() As Long
            Try
                If Not allowDuplicates Then Return CLng((Factorial(possibleValues.Length)) / Factorial(possibleValues.Length - sizeOfPermutation))
                Return CLng(Math.Pow(possibleValues.Length, sizeOfPermutation))
            Catch ex As Exception
                Throw New Exception
            End Try
            Return -1
        End Function

        'Sets up the stream; call this first
        Public Sub InitStreamPermutor()
            streamHandler = New StreamHandler(Of T)(Me)
        End Sub

        'Returns true if the stream is still active; use this to iterate through permutations
        Public Function IsStreamActive() As Boolean
            If streamHandler Is Nothing Then Return False
            Return streamHandler.StreamActive
        End Function

        'Returns an array of the permutation and sets up the stream to send the next permutation
        Public Function GetPermutationFromStream() As T()
            If streamHandler Is Nothing Then Return Nothing
            If Not streamHandler.StreamActive Then Return Nothing
            Return streamHandler.GetPermutation
        End Function

        'Returns an array of the random permutation generated using the given seed. The seed defaults to using the system tick counter if not specified.
        Public Function GetRandomPermutation(ByRef generator As Random) As T()
            If generator Is Nothing Then Throw New Exception
            Return RandomPermutation(generator).ToArray
        End Function

        'Generates every permutation and streams it through [stream].
        'The permutor is set up by the [streamHandler] created by initStreamPermutor().
        'You should NOT call this function.
        Public Sub StreamPermutor(ByRef stream As System.IO.MemoryStream,
                               ByRef permutationAvle As Threading.Semaphore,
                               ByRef permutationPost As Threading.Semaphore,
                               ByRef permutationLock As Threading.Semaphore)

            Validate(False)
            If stream Is Nothing Then Throw New Exception
            If permutationAvle Is Nothing Or permutationPost Is Nothing Or permutationLock Is Nothing Then Throw New Exception

            Dim permutee As List(Of Integer) = InitPermutingArray()
            stream.Capacity = sizeOfPermutation
            Do
                OutputHandler(permutee.ToArray, stream, permutationAvle, permutationPost, permutationLock)
                FindNextPermutation(permutee)
            Loop Until PermuteeContainsOnlyFinalElement(permutee)
            OutputHandler(permutee.ToArray, stream, permutationAvle, permutationPost, permutationLock)

            permutationAvle.WaitOne()
            stream.Close()
            streamHandler = Nothing
        End Sub

        'Generates every permutation and returns it using a list.
        'This may fail if the number of permutations is too high and VB cannot handle the list; in this case, use permuteToStream().
        '   (This occurs when the list reaches a 2GB object size or contains 2^28 references.)
        Public Function PermuteToList() As List(Of T())
            Validate(True)

            Dim permutee As List(Of Integer) = InitPermutingArray()
            Dim res As New List(Of T())
            Do
                If Not allowDuplicates And permutee.Distinct.Count = permutee.Count Then AddResultToList(res, permutee)
                If allowDuplicates Then AddResultToList(res, permutee)
                FindNextPermutation(permutee)
            Loop Until PermuteeContainsOnlyFinalElement(permutee)
            If Not allowDuplicates And permutee.Distinct.Count = permutee.Count Then AddResultToList(res, permutee)
            If allowDuplicates Then AddResultToList(res, permutee)

            Return res
        End Function

        'Faster but specific method of permuting an array of length [possibleValues.Count] without repetition. Works using recursion in basicPermutation().
        'Basic permuting through a stream is not implemented because I'm lazy.
        Public Function BasicPermuteToList() As List(Of T())
            Validate(True)
            Dim res As New List(Of T())
            BasicPermutation(res, possibleValueIndices.ToArray, possibleValueIndices.Count)
            Return res
        End Function

        Protected Overridable Sub Dispose(disposing As Boolean)
            If disposed Then Return
            If disposing Then
                streamHandler.Dispose()
            End If

            disposed = True
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
    End Class

    '/////////////////////////
    'This class provides a compact set of methods and attributes to make safely accessing the stream clean and simple.
    '/////////////////////////
    Public Class StreamHandler(Of T)
        Implements IDisposable
        Private disposed As Boolean = False

        Private stream As System.IO.MemoryStream = New System.IO.MemoryStream()
        Private permutationAvle, permutationPost, permutationLock As Threading.Semaphore
        Private ReadOnly permutor As Permutor(Of T)

        'Constructor that configures semaphores for safe data transfer.
        'Also initiates the new thread that computes permutations
        Public Sub New(permutor As Permutor(Of T))
            permutationAvle = New Threading.Semaphore(1, 1)
            permutationPost = New Threading.Semaphore(0, 1)
            permutationLock = New Threading.Semaphore(1, 1)
            Dim permutationThread As New Threading.Thread(New Threading.ThreadStart(Sub() permutor.StreamPermutor(stream, permutationAvle, permutationPost, permutationLock)))

            Me.permutor = permutor
            permutationThread.Start()

        End Sub

        'Returns true is the stream is active
        Public Function StreamActive() As Boolean
            If stream.CanRead Then Return True
            Return False
        End Function

        'Opens the lock, gets data from the stream, and closes it again for the permutor to post the next permutation.
        'BUG: The returned bytes are of the wrong length and shortening it returns erroneous data.
        '   A fix has been applied but its efficacy is not guaranteed!
        Public Function GetPermutation() As T()
            Dim permutationBytes(permutor.GetSizeOfPermutation) As Byte

            permutationPost.WaitOne()
            permutationLock.WaitOne()
            stream.Position = 0
            stream.Read(permutationBytes, 0, permutor.GetSizeOfPermutation)
            permutationAvle.Release()
            permutationLock.Release()

            Return permutor.BytesToPermutee(permutationBytes.Take(permutationBytes.Length - 1).ToArray)
        End Function

        Protected Overridable Sub Dispose(disposing As Boolean)
            If disposed Then Return

            If disposing Then
                permutationAvle.Dispose()
                permutationPost.Dispose()
                permutationLock.Dispose()
                stream.Dispose()
            End If

            disposed = True
        End Sub

        Public Sub Dispose() Implements IDisposable.Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub
    End Class
End Namespace