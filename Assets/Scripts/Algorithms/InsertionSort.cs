using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsertionSort : Algorithm {
    protected override IEnumerator _Run() {
        int size = elements.Length;
        for (int i = 1; i < size; ++i) {
            ArrayElement a = elements[i];
            a.Select(true);
            yield return _WaitStep();

            for (int j = i - 1; j >= 0; j--) {
                ArrayElement b = elements[j];
                b.Select(true);
                yield return _WaitStep();

                if (a.Value < b.Value) {
                    yield return _Swap(a, b);

                    a.Select(false);
                    a = b;

                    if (!Running)
                        yield break;
                } else {
                    b.Select(false);
                    break;
                }
            }

            a.Select(false);
        }

        for (int i = 0; i < size; i++)
            elements[i].MarkAsSorted();
    }
}
