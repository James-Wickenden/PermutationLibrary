# VB-Permutor

## About

---

A Visual Basic Permutation library to allow complex and custom permuting of generic objects.

PermutationLibrary Version 1.8 (16/02/2020)

The online repository is available at <https://github.com/James-Wickenden/VB-Permutor>

Provides framework to allow generic permuting of arrays, either with or without repetition.
The permutator can handle up to 255 possible values when streaming.
A list of supported functionality is provided below, as well as a TODO for future functionality.

A class testing the library and demonstrating some of its functionality can be seen in the `TestPermutor.vb` class.

## TODO

---

- Input validation for typing
- Add parameter guidelines/ descriptions
- Config library as DLL (?)
- Make streamPermutor() private!
- Clean Up Permutor Interface; Public Subs and Method Formatting

## Functionality

---

- Constructor (for generic type T):

    ```VB
    Dim permutor As PermutationLibrary(Of T)
    Dim INPUT_VARARRAY() As T
    Dim PERMUTATION_SIZE As Integer
    Dim ALLOW_DUPLICATES As Boolean
    permutor = New PermutationLibrary(Of T)(PERMUTATION_SIZE, INPUT_VARARRAY, ALLOW_DUPLICATES)
    ```
