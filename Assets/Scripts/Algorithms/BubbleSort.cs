using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BubbleSort : Algorithm {
    private ArrayElement[] data = new ArrayElement[0];

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
            for (int j = 0; j < size - i; ++j) {
                ArrayElement a = data[j];
                ArrayElement b = data[j + 1];

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

            data[size - i].MarkAsSorted();
        }

        data[0].MarkAsSorted();
    }
}
