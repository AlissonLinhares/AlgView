using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InsertionSort : Algorithm {
    protected ArrayElement[] data = new ArrayElement[0];

    public override void Init(int[] dataset) {
        foreach (var e in data)
            Destroy(e.gameObject);

        int size = dataset.Length;
        data = new ArrayElement[size];

        for (int i = 0; i < size; i++) {
            data[i] = Instantiate(elementPrefab, this.transform).GetComponent<ArrayElement>();
            data[i].Value = dataset[i];
        }
    }

    protected override IEnumerator _Run() {
        int size = data.Length;
        for (int i = 1; i < size; ++i) {
            ArrayElement a = data[i];
            a.Select(true);
            yield return _WaitStep();

            for (int j = i - 1; j >= 0; j--) {
                ArrayElement b = data[j];
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
            data[i].MarkAsSorted();
    }
}
