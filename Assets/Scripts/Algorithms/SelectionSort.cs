using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionSort : Algorithm {
    protected override IEnumerator _Run() {
        int size = elements.Length;
        for (int i = 0; i < size; i++) {
            int min = i;

            for (int j = i + 1; j < size; j++) {
                ArrayElement a = elements[min];
                a.Select(true);

                ArrayElement b = elements[j];
                b.Select(true);
                yield return _WaitStep();

                if (b.Value < a.Value) {
                    yield return _WaitStep();
                    min = j;
                }

                a.Select(false);
                b.Select(false);
            }

            if (i != min)
                yield return _Swap(elements[min], elements[i]);

            elements[min].Select(false);
            elements[i].Select(false);

            elements[i].MarkAsSorted();
        }
    }
}
