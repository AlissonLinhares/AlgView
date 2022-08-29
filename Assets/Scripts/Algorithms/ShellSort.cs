using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShellSort : InsertionSort {
    protected override IEnumerator _Run() {
        int size = data.Length;

        for (int k = size/2; k > 0; k /= 2) {
            for (int i = k; i < size; i++) {
                for (int j = i - k; j >= 0; j -= k) {
                    ArrayElement a = data[j + k];
                    a.Select(true);
                    ArrayElement b = data[j];
                    b.Select(true);
                    yield return _WaitStep();

                    if (a.Value < b.Value) {
                        yield return _Swap(a, b);

                        a.Select(false);
                        b.Select(false);
                        a = b;

                        if (!Running)
                            yield break;
                    } else {
                        b.Select(false);
                        a.Select(false);
                        break;
                    }
                }
            }
        }

        for (int i = 0; i < size; i++)
            data[i].MarkAsSorted();
    }
}
