using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectionSort : Algorithm {
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
        for (int i = 0; i < size; i++) {
            int min = i;

            for (int j = i + 1; j < size; j++) {
                ArrayElement a = data[min];
                a.Select(true);

                ArrayElement b = data[j];
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
                yield return _Swap(data[min], data[i]);

            data[min].Select(false);
            data[i].Select(false);

            data[i].MarkAsSorted();
        }
    }
}
