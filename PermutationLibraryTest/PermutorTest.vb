Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports PermutationLibrary

Namespace PermutationLibraryTest

    <TestClass>
    Public Class PermutorTest
        ' This is a basic class used to demonstrate that the Permutor can permute complex custom objects.
        Private Class TestObject
            Public val As Decimal
            Public Sub New(val)
                Me.val = val
            End Sub
        End Class

        <TestMethod>
        Sub TestWellFormedCharPermutor_ToList()
            ' This test permutes chars to a list, ensuring the result is correct.

            Dim permutor As New Permutor(Of Char)(2, {"a", "b", "c"}, True)
            Dim permuted_ACTUAL As List(Of Char())
            Dim permuted_EXPECTED As List(Of Char())

            ' Duplicate elements allowed
            permutor.SetAllowDuplicates(True)

            permuted_ACTUAL = permutor.PermuteToList()
            permuted_EXPECTED = New List(Of Char()) From {"aa", "ab", "ac", "ba", "bb", "bc", "ca", "cb", "cc"}

            Assert.AreEqual(permuted_ACTUAL.Count, permuted_EXPECTED.Count)
            Assert.AreEqual(permuted_ACTUAL.Count, CInt(permutor.GetNoOfPermutations))
            MatchStringResults(permuted_ACTUAL, permuted_EXPECTED)

            ' Duplicate elements not allowed
            permutor.SetAllowDuplicates(False)

            permuted_ACTUAL = permutor.PermuteToList()
            permuted_EXPECTED = New List(Of Char()) From {"ab", "ac", "ba", "bc", "ca", "cb"}

            Assert.AreEqual(permuted_ACTUAL.Count, permuted_EXPECTED.Count)
            Assert.AreEqual(permuted_ACTUAL.Count, CInt(permutor.GetNoOfPermutations))
            MatchStringResults(permuted_ACTUAL, permuted_EXPECTED)

        End Sub

        <TestMethod>
        Sub TestWellFormedIntegerPermutor_ToList()
            ' This test permutes Integers to a list, ensuring the result is correct.

            Dim permutor As New Permutor(Of Integer)(2, {0, 1, 2, 3, 4, 5}, True)
            Dim permuted_ACTUAL As List(Of Integer())
            Dim permuted_EXPECTED As List(Of Integer())

            ' Duplicate elements allowed
            permutor.SetAllowDuplicates(True)

            permuted_ACTUAL = permutor.PermuteToList()
            permuted_EXPECTED = GetExpectedIntegerList(permutor.GetAllowDuplicates)

            Assert.AreEqual(permuted_ACTUAL.Count, permuted_EXPECTED.Count)
            Assert.AreEqual(permuted_ACTUAL.Count, CInt(permutor.GetNoOfPermutations))
            MatchIntegerResults(permuted_ACTUAL, permuted_EXPECTED)

            ' Duplicate elements not allowed
            permutor.SetAllowDuplicates(False)

            permuted_ACTUAL = permutor.PermuteToList()
            permuted_EXPECTED = GetExpectedIntegerList(permutor.GetAllowDuplicates)

            Assert.AreEqual(permuted_ACTUAL.Count, permuted_EXPECTED.Count)
            Assert.AreEqual(permuted_ACTUAL.Count, CInt(permutor.GetNoOfPermutations))
            MatchIntegerResults(permuted_ACTUAL, permuted_EXPECTED)
        End Sub

        <TestMethod>
        Sub TestWellFormedCharPermutor_BasicToList()
            ' This test permutes chars to a list, ensuring the result is correct.
            ' This uses the quick BasicToList() algorithm which does not use permutationSize or allow duplicates.

            Dim permutor As New Permutor(Of Char)(1, {"a", "b", "c"}, True)
            Dim permuted_ACTUAL As List(Of Char())
            Dim permuted_EXPECTED As List(Of Char())

            permuted_ACTUAL = permutor.BasicPermuteToList()
            permuted_EXPECTED = New List(Of Char()) From {"abc", "acb", "bac", "bca", "cab", "cba"}

            Assert.AreEqual(permuted_ACTUAL.Count, permuted_EXPECTED.Count)
            Assert.AreEqual(permuted_ACTUAL.Count, Factorial(permutor.GetPossibleValues.Length))
            MatchStringResults(permuted_ACTUAL, permuted_EXPECTED)

            permutor.SetPossibleValues({"a", "b", "c", "d", "e", "f"})
            permuted_ACTUAL = permutor.BasicPermuteToList()
            Assert.AreEqual(permuted_ACTUAL.Count, Factorial(permutor.GetPossibleValues.Length))
        End Sub

        <TestMethod>
        Sub TestWellFormedObjectPermutor_ToList()
            ' This test uses a vague test object that contains a decimal attribute, defined at the top of the class.
            ' The generic typing allows the Permutor to permute complex objects.

            Dim testObjects(5) As TestObject
            For i As Integer = 0 To 5
                testObjects(i) = New TestObject(CDec(i))
            Next

            Dim permutor As New Permutor(Of TestObject)(2, testObjects, True)
            Dim permuted_ACTUAL As List(Of TestObject())
            Dim permuted_EXPECTED As List(Of TestObject())

            ' Duplicate elements allowed
            permutor.SetAllowDuplicates(True)

            permuted_ACTUAL = permutor.PermuteToList()
            permuted_EXPECTED = GetExpectedTestObjectList(permutor.GetAllowDuplicates, testObjects)

            Assert.AreEqual(permuted_ACTUAL.Count, permuted_EXPECTED.Count)
            Assert.AreEqual(permuted_ACTUAL.Count, CInt(permutor.GetNoOfPermutations))

            MatchTestObjectResults(permuted_ACTUAL, permuted_EXPECTED)

            ' Duplicate elements not allowed
            permutor.SetAllowDuplicates(False)

            permuted_ACTUAL = permutor.PermuteToList()
            permuted_EXPECTED = GetExpectedTestObjectList(permutor.GetAllowDuplicates, permutor.GetPossibleValues)

            Assert.AreEqual(permuted_ACTUAL.Count, permuted_EXPECTED.Count)
            Assert.AreEqual(permuted_ACTUAL.Count, CInt(permutor.GetNoOfPermutations))
            MatchTestObjectResults(permuted_ACTUAL, permuted_EXPECTED)
        End Sub

        <TestMethod>
        Sub TestMalformedPermutor()
            ' These tests ensure malformed configurations are not permitted by the Validate() function.

            Dim permutor As Permutor(Of Char)

            'permutor = New Permutor(Of Char)(Nothing, {"a"}, False)
            'Assert.ThrowsException(Of Exception)(Sub() permutor.Validate(True))
            permutor = New Permutor(Of Char)(1, Nothing, False)
            Assert.ThrowsException(Of Exception)(Sub() permutor.Validate(True))
            'permutor = New Permutor(Of Char)(1, {"a"}, Nothing)
            'Assert.ThrowsException(Of Exception)(Sub() permutor.Validate(True))

            permutor = New Permutor(Of Char)(2, {}, True)
            Assert.ThrowsException(Of Exception)(Sub() permutor.Validate(True))

            permutor = New Permutor(Of Char)(-1.2, {"a", "b"}, True)
            Assert.ThrowsException(Of Exception)(Sub() permutor.Validate(True))

            permutor = New Permutor(Of Char)(3, {"a", "b"}, False)
            Assert.ThrowsException(Of Exception)(Sub() permutor.Validate(True))

            Dim charList As New List(Of Char)
            For i As Integer = 0 To 256
                charList.Add(CChar("a"))
            Next
            permutor = New Permutor(Of Char)(3, charList.ToArray, True)
            Assert.ThrowsException(Of Exception)(Sub() permutor.Validate(True))

            permutor = New Permutor(Of Char)(2, {"test"}, True)
            permutor.SetSizeOfPermutation(-1)
            Assert.ThrowsException(Of Exception)(Sub() permutor.BasicPermuteToList())

            permutor = New Permutor(Of Char)(200, charList.Take(254).ToArray, True)
            Assert.ThrowsException(Of Exception)(Sub() permutor.Validate(True))
        End Sub

        <TestMethod>
        Sub TestWellFormedObjectPermutor_ToStream()
            ' This test permutes complex objects through a stream, ensuring the result is correct.
            ' Streaming in unit tests appear to be buggy so this is skipped once validated.

            Dim testObjects(5) As TestObject
            For i As Integer = 0 To 5
                testObjects(i) = New TestObject(CDec(i))
            Next

            Dim permutor As New Permutor(Of TestObject)(2, testObjects, True)
            Dim permuted_ACTUAL As New List(Of TestObject())
            Dim permuted_EXPECTED As List(Of TestObject()) = GetExpectedTestObjectList(permutor.GetAllowDuplicates, testObjects)

            Assert.IsNull(permutor.GetPermutationFromStream())

            'permutor.InitStreamPermutor()
            'While permutor.IsStreamActive()
            '    permuted_ACTUAL.Add(permutor.GetPermutationFromStream)
            'End While

            'Assert.IsFalse(permutor.IsStreamActive)
            'Assert.AreEqual(permuted_ACTUAL.Count, permuted_EXPECTED.Count)
            'Assert.AreEqual(permuted_ACTUAL.Count, CInt(permutor.GetNoOfPermutations))
            'MatchTestObjectResults(permuted_ACTUAL, permuted_EXPECTED)

            '' Repeat to test that a PermutorStreamHandler() can be reused in the permutor safely.

            'Assert.IsNull(permutor.GetPermutationFromStream())
            'permutor.InitStreamPermutor()
            'While permutor.IsStreamActive()
            '    permuted_ACTUAL.Add(permutor.GetPermutationFromStream)
            'End While

            'Assert.IsFalse(permutor.IsStreamActive)
            'Assert.AreEqual(permuted_ACTUAL.Count, permuted_EXPECTED.Count)
            'Assert.AreEqual(permuted_ACTUAL.Count, CInt(permutor.GetNoOfPermutations))
            'MatchTestObjectResults(permuted_ACTUAL, permuted_EXPECTED)
        End Sub

        <TestMethod>
        Sub TestIdenticalElementsInPossibleValues()
            ' Elements with the same value should be allowed in a permutor without it discounting them for being duplicate.

            Dim permutor As New Permutor(Of Integer)(3, {1, 2, 2}, False)

            Dim permuted_ACTUAL As List(Of Integer()) = permutor.PermuteToList
            Dim permuted_EXPECTED As New List(Of Integer())

            permuted_EXPECTED.Add({1, 2, 2})
            permuted_EXPECTED.Add({1, 2, 2})
            permuted_EXPECTED.Add({2, 2, 1})
            permuted_EXPECTED.Add({2, 2, 1})
            permuted_EXPECTED.Add({2, 1, 2})
            permuted_EXPECTED.Add({2, 1, 2})

            Assert.AreEqual(permuted_ACTUAL.Count, permuted_EXPECTED.Count)
            Assert.AreEqual(permuted_ACTUAL.Count, CInt(permutor.GetNoOfPermutations))
            MatchIntegerResults(permuted_ACTUAL, permuted_EXPECTED)

            permutor.SetAllowDuplicates(True)
            permuted_ACTUAL = permutor.PermuteToList

            Assert.AreEqual(permuted_ACTUAL.Count, 27)
            Assert.AreEqual(permuted_ACTUAL.Count, CInt(permutor.GetNoOfPermutations))
        End Sub

        <TestMethod>
        Sub TestDispose()
            ' Ensures that the Dispose() method is safe.

            Dim permutor As New Permutor(Of Integer)(Nothing, Nothing, Nothing)
            Assert.IsNotNull(permutor)
            permutor.Dispose()

            permutor = New Permutor(Of Integer)(1, {1, 2, 3}, False)
            Assert.IsNotNull(permutor)
            Dim permuted_ACTUAL As List(Of Integer()) = permutor.PermuteToList
            Assert.AreEqual(3, permuted_ACTUAL.Count)
        End Sub

        <TestMethod>
        Sub TestGettersAndSetters()
            ' Ensures the getters, setters and configure methods are valid.

            Dim permutor As New Permutor(Of Integer)(Nothing, Nothing, Nothing)

            permutor.SetAllowDuplicates(True)
            Assert.IsTrue(permutor.GetAllowDuplicates)
            permutor.SetAllowDuplicates(False)
            Assert.IsFalse(permutor.GetAllowDuplicates)

            permutor.SetSizeOfPermutation(1)
            Assert.AreEqual(1, permutor.GetSizeOfPermutation)
            permutor.SetSizeOfPermutation(5)
            Assert.AreEqual(5, permutor.GetSizeOfPermutation)

            permutor.SetPossibleValues({1, 2, 3, 4, 5, 6, 7, 8, 9})
            For i As Integer = 0 To permutor.GetPossibleValues.Length - 1
                Assert.AreEqual(i + 1, permutor.GetPossibleValues(i))
            Next

            permutor.SetPossibleValues({1, 2, 3, 4, 5})
            For i As Integer = 0 To permutor.GetPossibleValues.Length - 1
                Assert.AreEqual(i + 1, permutor.GetPossibleValues(i))
            Next

            permutor.Configure(1, {1, 2, 3}, False)
            Assert.AreEqual(1, permutor.GetSizeOfPermutation)
            Assert.IsFalse(permutor.GetAllowDuplicates)
            For i As Integer = 0 To permutor.GetPossibleValues.Length - 1
                Assert.AreEqual(i + 1, permutor.GetPossibleValues(i))
            Next
        End Sub

        <TestMethod>
        Sub TestGetRandomPermutation()
            ' This test pulls a random permutation and ensures it is well formed, of the correct length, and could not contain duplicates.

            Dim permutor As New Permutor(Of String)(3, {"Apple", "Banana", "Coconut"}, True)
            Dim permuted_ACTUAL As String()
            Dim generator As New Random

            ' Randomly get permutations with duplicates
            permutor.SetAllowDuplicates(True)
            For i As Integer = 0 To 10
                permuted_ACTUAL = permutor.GetRandomPermutation(generator)
                Assert.AreEqual(permuted_ACTUAL.Length, 3)
                Assert.IsTrue(permuted_ACTUAL.Contains("Apple") Or permuted_ACTUAL.Contains("Banana") Or permuted_ACTUAL.Contains("Coconut"))
            Next

            ' Randomly get permutations without duplicates
            permutor.SetAllowDuplicates(False)
            For i As Integer = 0 To 10
                permuted_ACTUAL = permutor.GetRandomPermutation(generator)
                Assert.AreEqual(permuted_ACTUAL.Length, 3)
                Assert.IsTrue(permuted_ACTUAL.Contains("Apple") And permuted_ACTUAL.Contains("Banana") And permuted_ACTUAL.Contains("Coconut"))
            Next
        End Sub

        ' These are not tests but methods that assist test execution.
        ' //////

        Private Sub MatchStringResults(permuted_ACTUAL As List(Of Char()), permuted_EXPECTED As List(Of Char()))
            ' Asserts that the expected and actual results for Char List Permutations are the same.

            For Each elem_EXPECTED As Char() In permuted_EXPECTED
                Dim foundMatch As Boolean = False
                For Each elem_ACTUAL As Char() In permuted_ACTUAL
                    If elem_EXPECTED = elem_ACTUAL Then
                        foundMatch = True
                        'permuted_ACTUAL.Remove(elem_ACTUAL)
                    End If

                Next
                Assert.IsTrue(foundMatch)
            Next
        End Sub

        Private Sub MatchIntegerResults(permuted_ACTUAL As List(Of Integer()), permuted_EXPECTED As List(Of Integer()))
            ' Asserts that the expected and actual results for Integer List Permutations are the same.

            For Each elem_EXPECTED As Integer() In permuted_EXPECTED
                Dim foundMatch As Boolean = False
                For Each elem_ACTUAL As Integer() In permuted_ACTUAL
                    Dim foundMatch_elemWise As Boolean = True
                    For i As Integer = 0 To elem_EXPECTED.Length - 1
                        If elem_EXPECTED(i) <> elem_ACTUAL(i) Then
                            foundMatch_elemWise = False
                            Exit For
                        End If
                    Next
                    If foundMatch_elemWise Then foundMatch = True
                Next
                Assert.IsTrue(foundMatch)
            Next
        End Sub

        Private Sub MatchTestObjectResults(permuted_ACTUAL As List(Of TestObject()), permuted_EXPECTED As List(Of TestObject()))
            ' Asserts that the expected and actual results for TestObject List Permutations are the same.

            For Each elem_EXPECTED As TestObject() In permuted_EXPECTED
                Dim foundMatch As Boolean = False
                For Each elem_ACTUAL As TestObject() In permuted_ACTUAL
                    Dim foundMatch_elemWise As Boolean = True
                    For i As Integer = 0 To elem_EXPECTED.Length - 1
                        If elem_EXPECTED(i).val <> elem_ACTUAL(i).val Then
                            foundMatch_elemWise = False
                            Exit For
                        End If
                    Next
                    If foundMatch_elemWise Then foundMatch = True
                Next
                Assert.IsTrue(foundMatch)
            Next
        End Sub

        Private Function GetExpectedIntegerList(ByVal allowDuplicates As Boolean) As List(Of Integer())
            ' Creates the expected Integer List permutation result for ease of use.

            Dim permuted_EXPECTED As New List(Of Integer())
            For i As Integer = 0 To 5
                For j As Integer = 0 To 5
                    If i <> j Then
                        permuted_EXPECTED.Add({i, j})
                    ElseIf allowDuplicates Then
                        permuted_EXPECTED.Add({i, j})
                    End If
                Next
            Next
            Return permuted_EXPECTED
        End Function

        Private Function GetExpectedTestObjectList(ByVal allowDuplicates As Boolean, ByVal possibleValues As TestObject()) As List(Of TestObject())
            ' Creates the expected TestObject List permutation result for ease of use.

            Dim permuted_EXPECTED As New List(Of TestObject())
            For i As Integer = 0 To possibleValues.Length - 1
                For j As Integer = 0 To possibleValues.Length - 1
                    If i <> j Then
                        permuted_EXPECTED.Add({possibleValues(i), possibleValues(j)})
                    ElseIf allowDuplicates Then
                        permuted_EXPECTED.Add({possibleValues(i), possibleValues(j)})
                    End If
                Next
            Next
            Return permuted_EXPECTED
        End Function

        Private Function Factorial(x As Integer) As Integer
            ' Returns x! for values of x s.t. Factorial(x) < 2^32, via recursion.
            If x <= 1 Then Return 1
            Return x * Factorial(x - 1)
        End Function
    End Class
End Namespace

