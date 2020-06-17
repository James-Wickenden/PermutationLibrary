Option Strict On

''' <summary>Defines the user interface for interacting with the Permutor's functionality.</summary>
''' <typeparam name="T">Type of the objects handled by that instance of the Permutor.</typeparam>
Public Interface IPermutorInterface(Of T)

    ''' <summary>Configure the permutor according to the new parameters.</summary>
    ''' <param name="sizeOfPermutation">The new length of the returned permutations.</param>
    ''' <param name="possibleValues">An array of the new possible values the permutations can take from.</param>
    ''' <param name="allowDuplicates">Whether returned permutations can use the same value multiple times.</param>
    Sub Configure(ByVal sizeOfPermutation As Integer, ByVal possibleValues() As T, ByVal allowDuplicates As Boolean)

    ''' <summary>Validate the current permutor configuration to ensure that it is valid, and returns a corresponding error message otherwise.</summary>
    ''' <param name="fromList">An optional parameter that ensures the resulting permutations can be returned as a list.</param>
    Sub Validate(Optional fromList As Boolean = Nothing)

    ''' <summary>Set sizeOfPermutation to the specified value.</summary>
    ''' <param name="newPermutationSize">The length of the returned permutations.</param>
    Sub SetSizeOfPermutation(newPermutationSize As Integer)
    ''' <summary>Set possibleValues to the specified value.</summary>
    ''' <param name="newPossibleValues">An array of the possible values the permutations can take from.</param>
    Sub SetPossibleValues(newPossibleValues As T())
    ''' <summary>Set allowDuplicates to the specified value.</summary>
    ''' <param name="newAllowDuplicates">Whether returned permutations can use the same value multiple times.</param>
    Sub SetAllowDuplicates(newAllowDuplicates As Boolean)

    ''' <summary>Get the value of sizeOfPermutation.</summary>
    ''' <returns>The current value of sizeOfPermutation.</returns>
    Function GetSizeOfPermutation() As Integer
    ''' <summary>Get the value of possibleValues.</summary>
    ''' <returns>The current value of possibleValues.</returns>
    Function GetPossibleValues() As T()
    ''' <summary>Get the value of allowDuplicates.</summary>
    ''' <returns>The current value of allowDuplicates.</returns>
    Function GetAllowDuplicates() As Boolean

    ''' <summary>
    ''' Return the number of permutations that the permutor will generate if called.
    ''' <para>Calucates differently depending on whether duplicate elements are allowed.</para>
    ''' <para>A return value of -1 indicates more than 2^64 results are returned by permuting.</para>
    ''' </summary>
    ''' <returns>Long value of the number of permutations that would be generated.</returns>
    Function GetNoOfPermutations() As Long

    ''' <summary>Set up the stream; call this before referencing GetPermutationFromStream() or other stream functionality.</summary>
    Sub InitStreamPermutor()
    ''' <summary>Kill the stream permutor and its thread prematurely.</summary>
    Sub KillStreamPermutor()
    ''' <summary>Returns true if the stream is still active; use this to iterate through permutations.</summary>
    Function IsStreamActive() As Boolean
    ''' <summary>Returns an array of the permutation and sets up the stream to send the next permutation.</summary>
    Function GetPermutationFromStream() As T()

    ''' <summary>Get an array of the random permutation generated using the given seed.</summary>
    ''' <param name = "generator" >The Random object used to generate the permutation.</param>
    Function GetRandomPermutation(ByRef generator As Random) As T()

    ''' <summary>
    ''' Generate every permutation and returns it using a list.
    ''' <para>This may fail if the number of permutations is too high and VB cannot handle the list; in this case, use PermuteToStream().</para>
    ''' <para>(This occurs when the list reaches a 2GB object size or contains 2^28 references.)</para>
    ''' </summary>
    ''' <returns>A list of permutations.</returns>
    Function PermuteToList() As List(Of T())

    ''' <summary>
    ''' Faster but specific method of permuting an array of length [possibleValues.Count] without repetition. Works using recursion in BasicPermutation().
    ''' <para>Basic permuting through a stream is not implemented because I'm lazy.</para>
    ''' </summary>
    ''' <returns>A list of permutations.</returns>
    Function BasicPermuteToList() As List(Of T())

    ''' <summary>Disposes of the permutor.</summary>
    Sub Dispose()

End Interface
