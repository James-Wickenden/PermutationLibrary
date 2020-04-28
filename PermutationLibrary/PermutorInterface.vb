Public Interface IPermutorInterface(Of T)

    Sub Configure(ByVal sizeOfPermutation As Integer, ByVal possibleValues() As T, ByVal allowDuplicates As Boolean)

    Sub Validate(Optional fromList As Boolean = Nothing)

    Sub SetSizeOfPermutation(newPermutationSize As Integer)
    Sub SetPossibleValues(newPossibleValues As T())
    Sub SetAllowDuplicates(newAllowDuplicates As Boolean)

    Function GetSizeOfPermutation() As Integer
    Function GetPossibleValues() As T()
    Function GetAllowDuplicates() As Boolean

    Function GetNoOfPermutations() As Long

    Sub InitStreamPermutor()
    Function IsStreamActive() As Boolean
    Function GetPermutationFromStream() As T()

    Function GetRandomPermutation(ByRef generator As Random) As T()

    Function PermuteToList() As List(Of T())

    Function BasicPermuteToList() As List(Of T())

    Sub Dispose()

End Interface
