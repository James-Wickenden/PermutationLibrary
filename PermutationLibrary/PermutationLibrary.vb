Option Strict On
Imports System.Runtime.CompilerServices

Namespace PermutationLibrary

    ''' <summary>Handles the logic of generating permutations, and returning the result to the user.</summary>
    ''' <typeparam name="T">Type of the objects handled by that instance of the Permutor.</typeparam>
    Public Class Permutor(Of T)
        Implements IDisposable, IPermutorInterface(Of T)

        'https://github.com/James-Wickenden/VB-Permutor
        Private disposed As Boolean = False

        Private sizeOfPermutation As Integer
        Private possibleValues() As T
        Private possibleValueIndices As List(Of Integer)
        Private allowDuplicates As Boolean
        Private streamHandler As PermutorStreamHandler

        '/////////////////////////
        'These methods relate to initialisation and configuration of permutor attributes.
        '/////////////////////////

        ''' <summary>Instantiate a permutor of type T with the specified parameters.</summary>
        ''' <param name="sizeOfPermutation">The length of the returned permutations.</param>
        ''' <param name="possibleValues">An array of the possible values the permutations can take from.</param>
        ''' <param name="allowDuplicates">Whether returned permutations can use the same value multiple times.</param>
        Public Sub New(ByVal sizeOfPermutation As Integer, ByVal possibleValues() As T, ByVal allowDuplicates As Boolean)
            possibleValueIndices = New List(Of Integer)
            Configure(sizeOfPermutation, possibleValues, allowDuplicates)
        End Sub

        ''' <summary>Configure the permutor according to the new parameters.</summary>
        ''' <param name="sizeOfPermutation">The new length of the returned permutations.</param>
        ''' <param name="possibleValues">An array of the new possible values the permutations can take from.</param>
        ''' <param name="allowDuplicates">Whether returned permutations can use the same value multiple times.</param>
        Public Sub Configure(ByVal sizeOfPermutation As Integer, ByVal possibleValues() As T, ByVal allowDuplicates As Boolean) Implements IPermutorInterface(Of T).Configure
            Me.sizeOfPermutation = sizeOfPermutation
            Me.possibleValues = possibleValues
            Me.allowDuplicates = allowDuplicates
            ConfigIndicesList()
        End Sub

        ''' <summary>Validate the current permutor configuration to ensure that it is valid, and returns a corresponding error message otherwise.</summary>
        ''' <param name="fromList">An optional parameter that ensures the resulting permutations can be returned as a list.</param>
        Public Sub Validate(Optional fromList As Boolean = False) Implements IPermutorInterface(Of T).Validate
            If disposed Then Exit Sub

            Dim exceptionStr As String = ""
            If IsNothing(allowDuplicates) Then exceptionStr &=
                "ERROR: [allowDuplicates] attribute must not be null. "
            If IsNothing(sizeOfPermutation) Then exceptionStr &=
                "ERROR: [sizeOfPermutation] attribute must not be null. "
            If IsNothing(possibleValues) Then exceptionStr &=
                "ERROR: [possibleValues] attribute must not be null. "
            If IsNothing(possibleValueIndices) Then exceptionStr &=
                "ERROR: [possibleValueIndices] attribute must not be null. Should be configured by changing the [possibleValues] attribute. "

            If exceptionStr <> "" Then Throw New Exception(exceptionStr)

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

        ''' <summary>Set up the indices list which is used for permuting corresponding integer indices instead of objects of type T.</summary>
        Private Sub ConfigIndicesList()
            If possibleValues Is Nothing Then Exit Sub
            Dim x As Integer = 0
            possibleValueIndices.Clear()
            For Each elem As T In possibleValues
                possibleValueIndices.Add(x)
                x += 1
            Next
        End Sub

        ''' <summary>Establish the first permutation as an array size [sizeOfPermutation], with each element as [possibleValues.First].</summary>
        Private Function InitPermutingArray() As List(Of Integer)
            Dim permutingList As New List(Of Integer)
            For i As Integer = 0 To sizeOfPermutation - 1
                permutingList.Add(possibleValueIndices.First)
            Next
            Return permutingList
        End Function

        'Getters and setters for the permutor attributes. Not used in the code but added here for the user.

        ''' <summary>Set sizeOfPermutation to the specified value.</summary>
        ''' <param name="newPermutationSize">The length of the returned permutations.</param>
        Public Sub SetSizeOfPermutation(newPermutationSize As Integer) Implements IPermutorInterface(Of T).SetSizeOfPermutation
            sizeOfPermutation = newPermutationSize
        End Sub

        ''' <summary>Set possibleValues to the specified value.</summary>
        ''' <param name="newPossibleValues">An array of the possible values the permutations can take from.</param>
        Public Sub SetPossibleValues(newPossibleValues As T()) Implements IPermutorInterface(Of T).SetPossibleValues
            possibleValues = newPossibleValues
            ConfigIndicesList()
        End Sub

        ''' <summary>Set allowDuplicates to the specified value.</summary>
        ''' <param name="newAllowDuplicates">Whether returned permutations can use the same value multiple times.</param>
        Public Sub SetAllowDuplicates(newAllowDuplicates As Boolean) Implements IPermutorInterface(Of T).SetAllowDuplicates
            allowDuplicates = newAllowDuplicates
        End Sub

        ''' <summary>Get the value of sizeOfPermutation.</summary>
        ''' <returns>The current value of sizeOfPermutation.</returns>
        Public Function GetSizeOfPermutation() As Integer Implements IPermutorInterface(Of T).GetSizeOfPermutation
            Return sizeOfPermutation
        End Function

        ''' <summary>Get the value of possibleValues.</summary>
        ''' <returns>The current value of possibleValues.</returns>
        Public Function GetPossibleValues() As T() Implements IPermutorInterface(Of T).GetPossibleValues
            Return possibleValues.ToArray
        End Function

        ''' <summary>Get the value of allowDuplicates.</summary>
        ''' <returns>The current value of allowDuplicates.</returns>
        Public Function GetAllowDuplicates() As Boolean Implements IPermutorInterface(Of T).GetAllowDuplicates
            Return allowDuplicates
        End Function

        '/////////////////////////
        'These methods provide basic mathematical and logical functionality.
        '/////////////////////////

        ''' <summary>Calculates the factorial of a BigInteger. I don't know why. Probably to stop overflows.</summary>
        ''' <returns>The value of y = x!</returns>
        Private Function Factorial(x As System.Numerics.BigInteger) As System.Numerics.BigInteger
            If x <= 1 Then Return 1
            Return x * Factorial(x - 1)
        End Function

        ''' <summary>Swaps two integers. Is there not an inbuilt function provided by tuples or something?</summary>
        Private Shared Sub Swap(ByRef x As Integer, ByRef y As Integer)
            Dim temp As Integer = x
            x = y
            y = temp
        End Sub

        '/////////////////////////
        'These methods provide logic functions that together generate permuations.
        '/////////////////////////

        ''' <summary>Returns a boolean on whether a permutee only contains the final element in the possibleValueIndices list.
        ''' <para>Used to determine when the final permutation has been calculated.</para></summary>
        ''' <param name="permutee">The input permutee</param>
        ''' <returns>Whether the permutee contains only the final element at each index.</returns>
        Private Function PermuteeContainsOnlyFinalElement(ByVal permutee As List(Of Integer)) As Boolean
            If permutee.All(Function(elem) elem.Equals(possibleValueIndices.Last)) Then
                Return True
            End If
            Return False
        End Function

        ''' <summary>
        ''' Find the next value for the [index] element of [permutee].
        ''' <para>The next element loops around to the first if [permutee(index)] is [possibleValues.Last].</para>
        ''' <para>In this case, the [toNextColumn] flag is set to True.</para>
        ''' </summary>
        ''' <param name="permutee">The permutee to be incremented.</param>
        ''' <param name="toNextColumn">Whether the next column should then be incremented. Mutated by the function.</param>
        ''' <param name="index">The current index of the permutee being incremented.</param>
        Private Sub GetNextPossibleColummValue(ByRef permutee As List(Of Integer), ByRef toNextColumn As Boolean, ByVal index As Integer)
            If possibleValueIndices.Last.Equals(permutee(index)) Then
                permutee(index) = possibleValueIndices.First
                toNextColumn = True
            Else
                permutee(index) = possibleValueIndices.ElementAt(possibleValueIndices.IndexOf(permutee(index)) + 1)
            End If
        End Sub

        ''' <summary>
        ''' Generate the next permutation by counting up in base [possibleValues.count] a number with [sizeOfPermutation] digits.
        ''' <para>Each digit corresponds to the index of that element in possibleValues.</para>
        ''' <para>This generates every possible permutation with repetition.</para>
        ''' </summary>
        ''' <param name="permutee">The permutee to be incremented.</param>
        Private Sub FindNextPermutation(ByRef permutee As List(Of Integer))
            Dim traversalIndex As Integer = 0
            Dim toNextColumn As Boolean = False

            GetNextPossibleColummValue(permutee, toNextColumn, 0)

            While toNextColumn = True And traversalIndex < sizeOfPermutation - 1
                toNextColumn = False
                traversalIndex += 1
                GetNextPossibleColummValue(permutee, toNextColumn, traversalIndex)
            End While

            If Not allowDuplicates Then

                While permutee.Distinct.Count <> permutee.Count And Not PermuteeContainsOnlyFinalElement(permutee)
                    traversalIndex = 0
                    toNextColumn = False

                    GetNextPossibleColummValue(permutee, toNextColumn, 0)
                    While toNextColumn = True And traversalIndex < sizeOfPermutation - 1
                        toNextColumn = False
                        traversalIndex += 1
                        GetNextPossibleColummValue(permutee, toNextColumn, traversalIndex)
                    End While
                End While
            End If
        End Sub

        ''' <summary>
        ''' A recursive procedure to generate distinct permutations.
        ''' <para>Traverses down the permutation, setting each element as a valid choice then recursing on the next element.</para>
        ''' </summary>
        ''' <param name="permutee">The permutee being modified.</param>
        ''' <param name="banlist">A list of all the possible values currently being used by other elements in the permutee.</param>
        ''' <param name="curindex">The current index being iterated through.</param>
        ''' <param name="stream">The stream permutations are piped through.</param>
        ''' <param name="permutationAvle">Semaphore stating consumer availability.</param>
        ''' <param name="permutationPost">Semaphore stating producer posting.</param>
        ''' <param name="permutationLock">Semaphore stating consumer usage.</param>
        Private Sub FindDistinctPermutations(ByRef permutee As List(Of Integer), ByRef banlist As List(Of Integer), ByVal curindex As Integer,
                                             ByRef stream As System.IO.MemoryStream,
                                             ByRef permutationAvle As Threading.Semaphore,
                                             ByRef permutationPost As Threading.Semaphore,
                                             ByRef permutationLock As Threading.Semaphore)
            For i As Integer = 0 To possibleValueIndices.Count - 1
                If (banlist.Contains(i) And (curindex < permutee.Count)) Then Continue For
                If (curindex = permutee.Count) Then
                    OutputHandler(permutee.ToArray, stream, permutationAvle, permutationPost, permutationLock)
                    Return
                End If

                permutee(curindex) = possibleValueIndices(i)

                banlist.Add(i)
                FindDistinctPermutations(permutee, banlist, curindex + 1,
                                stream, permutationAvle, permutationPost, permutationLock)
                banlist.Remove(i)
            Next
        End Sub

        ''' <summary>
        ''' Stream the current permutation to the host thread securely using Semaphores, avoiding deadlock.
        ''' <para>Note that the corresponding code must be used in the host thread to receive the streamed data, given in the exampleStreamReceiver() method.</para>
        ''' </summary>
        ''' <param name="permutee">The permutee being modified.</param>
        ''' <param name="stream">The stream permutations are piped through.</param>
        ''' <param name="permutationAvle">Semaphore stating consumer availability.</param>
        ''' <param name="permutationPost">Semaphore stating producer posting.</param>
        ''' <param name="permutationLock">Semaphore stating consumer usage.</param>
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

        ''' <summary>Append a new permutation to the results list by first generating the corresponding object permutation.</summary>
        ''' <param name="res">The result list of permutations. Modified by the function.</param>
        ''' <param name="permutee">The permutee to be added; it is stored as an index list so must be converted to the corresponding objects.</param>
        Private Sub AddResultToList(ByRef res As List(Of T()), ByVal permutee As List(Of Integer))
            Dim permuteeAsTs As New List(Of T)
            For Each elem As Integer In permutee
                permuteeAsTs.Add(possibleValues.ElementAt(elem))
            Next
            res.Add(permuteeAsTs.ToArray)
        End Sub

        ''' <summary>
        ''' Convert the [permutee] to an array of Bytes to allow transmission over the defined stream.
        ''' <para>This is done by converting the element to its corresponding index value in [possibleValues].</para>
        ''' </summary>
        ''' <param name="permutee">The permutee to be sent.</param>
        ''' <returns>A Byte array of the permutee elements.</returns>
        Private Shared Function PermuteeToBytes(permutee() As Integer) As Byte()
            Dim permuteeCorrespondingIndices As New List(Of Byte)
            If permutee Is Nothing Then Throw New Exception
            For Each x As Integer In permutee
                permuteeCorrespondingIndices.Add(CByte(x))
            Next
            Return permuteeCorrespondingIndices.ToArray
        End Function

        ''' <summary>The inverse operation to permuteeToBytes. Used when reading data out of the stream.</summary>
        ''' <param name="bs">The received Byte array</param>
        ''' <returns>An object array of type T.</returns>
        Private Function BytesToPermutee(bs() As Byte) As T()
            Dim permuteeCorrespondingIndices As New List(Of T)
            If bs Is Nothing Then Throw New Exception
            For Each b As Byte In bs
                permuteeCorrespondingIndices.Add(possibleValues(b))
            Next
            Return permuteeCorrespondingIndices.ToArray
        End Function

        ''' <summary>A recursive sub for (O(n!)) permutation. Works by simply repeating the recurive procedure for every element in the permutee.</summary>
        ''' <param name="res">The generated list of permutations.</param>
        ''' <param name="permutee">The modified permutee.</param>
        ''' <param name="n">The length of the section being permuted.</param>
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

        ''' <summary>
        ''' Generate a random permutation of length [sizeOfPermutation]. Can allow or disallow repeated elements.
        ''' <para>The seed is specified by the wrapper function getRandomPermutation(seed) method.</para>
        ''' </summary>
        ''' <param name="generator">The generator of type Random used to generate the permutation.</param>
        ''' <returns>A new random permutation of type T.</returns>
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

        ''' <summary>
        ''' Return the number of permutations that the permutor will generate if called.
        ''' <para>Calucates differently depending on whether duplicate elements are allowed.</para>
        ''' <para>A return value of -1 indicates more than 2^64 results are returned by permuting.</para>
        ''' </summary>
        ''' <returns>Long value of the number of permutations that would be generated.</returns>
        Public Function GetNoOfPermutations() As Long Implements IPermutorInterface(Of T).GetNoOfPermutations
            Try
                If Not allowDuplicates Then
                    Dim numerator As System.Numerics.BigInteger = Factorial(possibleValues.Length)
                    Dim denominator As System.Numerics.BigInteger = Factorial(possibleValues.Length - sizeOfPermutation)
                    Dim res As System.Numerics.BigInteger = System.Numerics.BigInteger.Divide(numerator, denominator)

                    Return CLng(res)
                Else
                    Dim res As New System.Numerics.BigInteger(Math.Pow(possibleValues.Length, sizeOfPermutation))

                    Return CLng(res)
                End If

            Catch ex As ArgumentOutOfRangeException
                Return -1
            End Try
            Return -1
        End Function

        ''' <summary>Set up the stream; call this before referencing GetPermutationFromStream() or other stream functionality.</summary>
        Public Sub InitStreamPermutor() Implements IPermutorInterface(Of T).InitStreamPermutor
            streamHandler = New PermutorStreamHandler(Me)
        End Sub

        ''' <summary>Kill the stream permutor and its thread prematurely.</summary>
        Public Sub KillStreamPermutor() Implements IPermutorInterface(Of T).KillStreamPermutor
            If streamHandler IsNot Nothing Then streamHandler.Dispose()
        End Sub

        ''' <summary>Returns true if the stream is still active; use this to iterate through permutations.</summary>
        Public Function IsStreamActive() As Boolean Implements IPermutorInterface(Of T).IsStreamActive
            If streamHandler Is Nothing Then Return False
            Return streamHandler.StreamActive
        End Function

        ''' <summary>Returns an array of the permutation and sets up the stream to send the next permutation.</summary>
        Public Function GetPermutationFromStream() As T() Implements IPermutorInterface(Of T).GetPermutationFromStream
            If streamHandler Is Nothing Then Return Nothing
            If Not streamHandler.StreamActive Then Return Nothing
            Return streamHandler.GetPermutation
        End Function

        ''' <summary>Get an array of the random permutation generated using the given seed.</summary>
        ''' <param name = "generator" >The Random object used to generate the permutation.</param>
        Public Function GetRandomPermutation(ByRef generator As Random) As T() Implements IPermutorInterface(Of T).GetRandomPermutation
            If generator Is Nothing Then Throw New Exception
            Return RandomPermutation(generator).ToArray
        End Function

        ''' <summary>
        ''' Generate every permutation and streams it through [stream].
        ''' <para>The permutor is set up by the streamHandler created by InitStreamPermutor().</para>
        ''' </summary>
        ''' <param name="stream">The stream permutations are piped through.</param>
        ''' <param name="permutationAvle">Semaphore stating consumer availability.</param>
        ''' <param name="permutationPost">Semaphore stating producer posting.</param>
        ''' <param name="permutationLock">Semaphore stating consumer usage.</param>
        Private Sub StreamPermutor(ByRef stream As System.IO.MemoryStream,
                               ByRef permutationAvle As Threading.Semaphore,
                               ByRef permutationPost As Threading.Semaphore,
                               ByRef permutationLock As Threading.Semaphore)

            Validate(False)
            If stream Is Nothing Then Throw New Exception
            If permutationAvle Is Nothing Or permutationPost Is Nothing Or permutationLock Is Nothing Then Throw New Exception

            Dim permutee As List(Of Integer) = InitPermutingArray()
            stream.Capacity = sizeOfPermutation

            If allowDuplicates Then
                Do
                    OutputHandler(permutee.ToArray, stream, permutationAvle, permutationPost, permutationLock)
                    FindNextPermutation(permutee)
                Loop Until PermuteeContainsOnlyFinalElement(permutee)
                OutputHandler(permutee.ToArray, stream, permutationAvle, permutationPost, permutationLock)
            Else
                FindDistinctPermutations(permutee, New List(Of Integer), 0,
                                         stream, permutationAvle, permutationPost, permutationLock)
            End If

            permutationAvle.WaitOne()
            stream.Close()
            streamHandler = Nothing
        End Sub

        ''' <summary>
        ''' Generate every permutation and returns it using a list.
        ''' <para>This may fail if the number of permutations is too high and VB cannot handle the list; in this case, use PermuteToStream().</para>
        ''' <para>(This occurs when the list reaches a 2GB object size or contains 2^28 references.)</para>
        ''' </summary>
        ''' <returns>A list of permutations.</returns>
        Public Function PermuteToList() As List(Of T()) Implements IPermutorInterface(Of T).PermuteToList
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

        ''' <summary>
        ''' Faster but specific method of permuting an array of length [possibleValues.Count] without repetition. Works using recursion in BasicPermutation().
        ''' <para>Basic permuting through a stream is not implemented because I'm lazy.</para>
        ''' </summary>
        ''' <returns>A list of permutations.</returns>
        Public Function BasicPermuteToList() As List(Of T()) Implements IPermutorInterface(Of T).BasicPermuteToList
            Validate(True)
            Dim res As New List(Of T())
            BasicPermutation(res, possibleValueIndices.ToArray, possibleValueIndices.Count)
            Return res
        End Function

        ''' <summary>Disposes of the permutor.</summary>
        ''' <param name="disposing">Whether the permutor should be disposed.</param>
        Protected Overridable Sub Dispose(disposing As Boolean)
            If disposed Then Return
            If disposing Then
                If streamHandler IsNot Nothing Then streamHandler.Dispose()
            End If

            disposed = True
        End Sub

        ''' <summary>Disposes of the permutor.</summary>
        Public Sub Dispose() Implements IDisposable.Dispose, IPermutorInterface(Of T).Dispose
            Dispose(True)
            GC.SuppressFinalize(Me)
        End Sub

        '/////////////////////////
        'Provides a compact set of methods and attributes to make safely accessing the stream clean and simple.
        '/////////////////////////

        ''' <summary>
        ''' This class provides a compact set of methods and attributes to make safely accessing the stream clean and simple.
        ''' </summary>
        Private Class PermutorStreamHandler
            Implements IDisposable
            Private disposed As Boolean = False

            Private stream As System.IO.MemoryStream = New System.IO.MemoryStream()
            Private permutationThread As Threading.Thread
            Private permutationAvle, permutationPost, permutationLock As Threading.Semaphore
            Private ReadOnly permutor As Permutor(Of T)

            ''' <summary>
            ''' Constructor that configures semaphores for safe data transfer.
            ''' <para>Also initiates the new thread that computes permutations.</para>
            ''' </summary>
            ''' <param name="permutor">The corresponding permutor.</param>
            Public Sub New(permutor As Permutor(Of T))
                permutationAvle = New Threading.Semaphore(1, 1)
                permutationPost = New Threading.Semaphore(0, 1)
                permutationLock = New Threading.Semaphore(1, 1)
                permutationThread = New Threading.Thread(New Threading.ThreadStart(Sub() permutor.StreamPermutor(stream, permutationAvle, permutationPost, permutationLock)))

                Me.permutor = permutor
                permutationThread.Start()

            End Sub

            ''' <summary>Returns true is the stream is active.</summary>
            ''' <returns>Whether the stream is active.</returns>
            Public Function StreamActive() As Boolean
                If stream.CanRead Then Return True
                Me.Dispose()
                Return False
            End Function

            ''' <summary>Opens the lock, gets data from the stream, and closes it again for the permutor to post the next permutation.</summary>
            ''' <returns>The next permutation pulled from the stream.</returns>
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

            ''' <summary>Disposes of the streamPermutor.</summary>
            ''' <param name="disposing">Whether the streamPermutor should be disposed.</param>
            Protected Overridable Sub Dispose(disposing As Boolean)
                If disposed Then Return

                If disposing Then
                    permutationAvle.Dispose()
                    permutationPost.Dispose()
                    permutationLock.Dispose()
                    permutationThread = Nothing
                    stream.Dispose()
                End If

                disposed = True
            End Sub

            ''' <summary>Disposes of the streamPermutor.</summary>
            Public Sub Dispose() Implements IDisposable.Dispose
                Dispose(True)
                GC.SuppressFinalize(Me)
            End Sub
        End Class
    End Class

End Namespace