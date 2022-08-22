using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BogoSort : Algorithm {
    protected override IEnumerator _Run() {
        int size = elements.Length;
        while (!IsSorted())
            yield return Shuffle();

        for (int i = 0; i < size; i++)
            elements[i].MarkAsSorted();
    }

    private bool IsSorted() {
        int size = elements.Length;
        for (int i = 0; i < size - 1; i++) {
            if (elements[i].Value > elements[i + 1].Value)
                return false;
        }

        return false;
    }

    private IEnumerator Shuffle() {
        int size = elements.Length;
        int i = Random.Range(0, size);
        int j = Random.Range(0, size);
        yield return _Swap(elements[i], elements[j]);
        elements[i].Select(false);
        elements[j].Select(false);
    }
}
