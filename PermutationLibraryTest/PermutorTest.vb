Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports PermutationLibrary

Namespace PermutationLibraryTest

    <TestClass>
    Public Class PermutorTest
        Private Class TestObject
            Public val As Decimal
            Public Sub New(val)
                Me.val = val
            End Sub
        End Class

        <TestMethod>
        Sub TestWellFormedCharPermutor_ToList()
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
        Sub TestWellFormedIntPermutor_ToList()
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

            ' Duplicate elements not allowed
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

        Private Sub MatchStringResults(permuted_ACTUAL As List(Of Char()), permuted_EXPECTED As List(Of Char()))
            For Each elem_EXPECTED As Char() In permuted_EXPECTED
                Dim foundMatch As Boolean = False
                For Each elem_ACTUAL As Char() In permuted_ACTUAL
                    If elem_EXPECTED = elem_ACTUAL Then foundMatch = True
                Next
                Assert.IsTrue(foundMatch)
            Next
        End Sub

        Private Sub MatchIntegerResults(permuted_ACTUAL As List(Of Integer()), permuted_EXPECTED As List(Of Integer()))
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
            If x = 1 Then Return 1
            Return x * Factorial(x - 1)
        End Function
    End Class
End Namespace

