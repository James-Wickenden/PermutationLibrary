Imports Microsoft.VisualStudio.TestTools.UnitTesting
Imports PermutationLibrary

Namespace PermutationLibraryTest
    <TestClass>
    Public Class PermutorTest

        <TestMethod>
        Sub TestWellFormedCharPermutor()
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
        Sub TestWellFormedIntPermutor()
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

        Sub MatchStringResults(permuted_ACTUAL As List(Of Char()), permuted_EXPECTED As List(Of Char()))
            For Each elem_EXPECTED As Char() In permuted_EXPECTED
                Dim foundMatch As Boolean = False
                For Each elem_ACTUAL As Char() In permuted_ACTUAL
                    If elem_EXPECTED = elem_ACTUAL Then foundMatch = True
                Next
                Assert.IsTrue(foundMatch)
            Next
        End Sub

        Sub MatchIntegerResults(permuted_ACTUAL As List(Of Integer()), permuted_EXPECTED As List(Of Integer()))
            For Each elem_EXPECTED As Integer() In permuted_EXPECTED
                Dim foundMatch As Boolean = False
                For Each elem_ACTUAL As Integer() In permuted_ACTUAL
                    Dim foundMatch_elemWise As Boolean = True
                    For i As Integer = 0 To elem_EXPECTED.Length - 1
                        If elem_EXPECTED(i) <> elem_ACTUAL(i) Then foundMatch_elemWise = False
                    Next
                    If foundMatch_elemWise Then foundMatch = True
                Next
                Assert.IsTrue(foundMatch)
            Next
        End Sub

        Function GetExpectedIntegerList(ByVal duplicates As Boolean) As List(Of Integer())
            Dim permuted_EXPECTED As New List(Of Integer())
            For i As Integer = 0 To 5
                For j As Integer = 0 To 5
                    If i <> j Then
                        permuted_EXPECTED.Add({i, j})
                    ElseIf duplicates Then
                        permuted_EXPECTED.Add({i, j})
                    End If

                Next
            Next
            Return permuted_EXPECTED
        End Function
    End Class
End Namespace

