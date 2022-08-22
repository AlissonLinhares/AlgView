using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSort : Algorithm {
    protected override IEnumerator _Run() {
        int size = elements.Length;
        for (int i = 1; i < size; ++i) {
            for (int j = 0; j < size - i; ++j) {
                ArrayElement a = elements[j];
                ArrayElement b = elements[j + 1];

                a.Select(true);
                b.Select(true);
                yield return _WaitStep();

                if (a.Value > b.Value)
                    yield return _Swap(a, b);

                a.Select(false);
                b.Select(false);

                if (!Running)
                    yield break;
            }

            elements[size - i].MarkAsSorted();
        }

        elements[0].MarkAsSorted();
    }
}
