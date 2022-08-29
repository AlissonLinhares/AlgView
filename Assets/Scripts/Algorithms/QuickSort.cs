using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class QuickSort : Algorithm {
    [SerializeField] protected bool centerAsPivot = false;
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

    private IEnumerator Sort(int begin, int end) {
        if (begin == end) {
            data[begin].MarkAsSorted();
            yield break;
        } else if (begin > end) {
            yield break;
        }

        Fade(0, data.Length-1, true);
        Fade(begin, end, false);

        int pivot = end;
        if (centerAsPivot) {
            int center = (begin + end)/2;
            data[center].Select(true);
            yield return _WaitStep();

            yield return _Swap(data[center], data[end]);
            data[center].Select(false);
            data[pivot].Select(true);
            yield return _WaitStep();
        } else {
            data[pivot].Select(true);
            yield return _WaitStep();
        }

        int firstBigger = begin;
        for (int i = begin; i < end; i++) {
            data[i].Select(true);
            yield return _WaitStep();

            if (data[pivot].Value > data[i].Value) {
                if (firstBigger < i) {
                    yield return _Swap(data[i], data[firstBigger]);
                    data[firstBigger].Select(false);
                }

                firstBigger++;
            }

            data[i].Select(false);
        }

        if (firstBigger != pivot)
            yield return _Swap(data[firstBigger], data[pivot]);
        data[pivot].Select(false);

        yield return _WaitStep();
        data[firstBigger].MarkAsSorted();

        yield return Sort(begin, firstBigger - 1);
        yield return Sort(firstBigger + 1, end);
    }

    private void Fade(int begin, int end, bool fade) {
        while (begin <= end)
            data[begin++].Fade(fade);
    }

    protected override IEnumerator _Run() {
        int size = data.Length;
        yield return Sort(0, size - 1);

        foreach (var e in data)
            e.MarkAsSorted();
    }
}
