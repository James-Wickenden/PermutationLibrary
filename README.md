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

- Add parameter guidelines/ descriptions
- Config library (as DLL (?))
- Ensure mostly compliant with Standards given by [https://en.wikibooks.org/wiki/Visual_Basic/Coding_Standards](https://en.wikibooks.org/wiki/Visual_Basic/Coding_Standards)

## Functionality

---

## Parameters

There are several parameters held by the Permutor which determine the output of functions.

- `Integer sizeOfPermutation`

    This controls the number of elements in the returned permutations.

    For example, with `sizeOfPermutation = 3`, a returned permutation could be `{a, b, c}`, but not `{a, b, c, d}` or `{a, b}`.

- `T possibleValues()`

    This is the set of possible values that can be returned from. The typing of this must be the same as the type given to the permutor when instantiating.

    For example, with `possibleValues = {a, b, c}`, a returned permutation could be `{c, a, b}` but not `{c, a, d}`.

- `Boolean allowDuplicates`

    This boolean flag controls whether duplicate elements are allowed in returned permutations.

    For example, if `allowDuplicates = True`, a returned permutation could be `{a, a, a}`, but not if the permutor was set to `allowDuplicates = False`.

    The `allowDuplicates` flag is ignored when calling `basicPermuteToList()`. This is explained in more detail below.

## Methods

- Constructor (for generic type T):

    ```VB
    Dim permutor As PermutationLibrary(Of T)
    Dim INPUT_VARARRAY() As T
    Dim PERMUTATION_SIZE As Integer
    Dim ALLOW_DUPLICATES As Boolean

    permutor = New PermutationLibrary(Of T)(PERMUTATION_SIZE, INPUT_VARARRAY, ALLOW_DUPLICATES)
    ```

    The constructor instantiates the permutor using the given parameters. T is a generic typing.

- Get number of permutations as a Long:

    ```VB
    Dim noOfPermutations as Long = permutor.getNoOfPermutations()
    ```

    This function will return -1 in case of overflow; if there more than 2^64 permutations generated by the current parameters.

- Get a list of permutations:

    ```VB
    Dim permutations As New List(Of T())
    permutations = permutor.permuteToList()
    ```

    This function will return a list of the permutations generated by `permutor` with its current parameters. This is only suitable for smaller scale permutations, as the list returned by the function can only return max 2^28 references or a 2GB list.

    In this case, permute via a stream instead. This also allows computation on returned objects while generating new permutations in the permutor thread.

- Get a stream of permutations:

    ```VB
    Dim permutation as T()
    permutor.initStreamPermutor()

    While permutor.isStreamActive
        permutation = permutor.getPermutationFromStream()
    End While
    ```

    Here, the initStreamPermutor() will create an instance of the `StreamHandler` class. This will create a new thread that is initialised at the `permutor.streamPermutor()` function. This thread will generate permutations in order and place them in the stream.

    The `getPermutationFromStream()` function will pull the new permutation from the stream and return it. This call is safely looped through with the `isStreamActive()` function.

- Get a list of permutations using a faster algorithm:

    ```VB
     Dim permutations As List(Of T())

     permutations = permutor.basicPermuteToList()
    ```

    This provides a faster algorithm for returning a list of permutations than the `permuteToList()` uses.

    However, this will only generate permutations with no duplicate elements regardless of the state of the `allowDuplicates` flag in the Permutor! This function is given to cater for a more stereotypical use of permutations in better time complexity.

- Get a random permutation:

    ```VB
    Dim generator As New Random
    Dim randomPermutation() As T

    randomPermutation = permutor.getRandomPermutation(generator)
    ```

    This function returns a random permutation using the given Random object and the parameters held by the Permutor.

- Reconfigure the parameters:

    ```VB
    Dim INPUT_VARARRAY() As T
    Dim PERMUTATION_SIZE As Integer
    Dim ALLOW_DUPLICATES As Boolean

    permutor.configure(PERMUTATION_SIZE, INPUT_VARARRAY, ALLOW_DUPLICATES)
    ```

    This function sets the flags in the permutor to the new values given as parameters.

- Validate the permutor:

    ```VB
    permutor.validate(True)
    ```

    Validates the permutor given the current internal parameters, causing an exception if any errors are found that would prevent the permutor from working. This function is called before permuting anyway so it is not necessary to call before every permutation.

    The parameter `fromList As Boolean` should be set to true if you intend to call `permuteToList()` to ensure that it can return the List safely without running out of memory.
    