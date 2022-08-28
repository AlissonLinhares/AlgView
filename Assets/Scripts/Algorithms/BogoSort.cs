using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BogoSort : Algorithm {
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
        while (!IsSorted())
            yield return Shuffle();

        for (int i = 0; i < size; i++)
            data[i].MarkAsSorted();
    }

    private bool IsSorted() {
        int size = data.Length;
        for (int i = 0; i < size - 1; i++) {
            if (data[i].Value > data[i + 1].Value)
                return false;
        }

        return true;
    }

    private IEnumerator Shuffle() {
        int size = data.Length;
        int i = Random.Range(0, size);
        int j = Random.Range(0, size);
        yield return _Swap(data[i], data[j]);
        data[i].Select(false);
        data[j].Select(false);
    }
}
