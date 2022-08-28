using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MergeSort : Algorithm {
    [SerializeField] protected GameObject input;
    [SerializeField] protected GameObject output;

    private ArrayElement[] inputData = new ArrayElement[0];
    private ArrayElement[] outputData  = new ArrayElement[0];
    private Stack<GameObject> separators = new Stack<GameObject>();

    public override void Init(int[] dataset) {
        foreach (var e in inputData)
            Destroy(e.gameObject);

        foreach (var e in outputData)
            Destroy(e.gameObject);

        foreach (var s in separators)
            Destroy(s.gameObject);

        separators.Clear();

        int size = dataset.Length;
        inputData = new ArrayElement[size];
        outputData = new ArrayElement[size];

        for (int i = 0; i < size; i++) {
            inputData[i] = Instantiate(elementPrefab, this.input.transform).GetComponent<ArrayElement>();
            inputData[i].Value = dataset[i];
            outputData[i] = Instantiate(elementPrefab, this.output.transform).GetComponent<ArrayElement>();
            outputData[i].Value = 0;
        }
    }

    private void Split(int pos) {
        GameObject obj = new GameObject("Separator");

        RectTransform transf = obj.AddComponent<RectTransform>();
        transf.anchorMax = Vector2.one;
        transf.anchorMin = Vector2.zero;
        transf.anchoredPosition = Vector3.zero;

        obj.transform.SetParent(this.input.transform);
        separators.Push(obj);

        obj.transform.SetSiblingIndex(inputData[pos].transform.GetSiblingIndex());

        Fade(pos, inputData.Length - 1, true);
        Fade(0, pos-1, false);
    }

    private void Fade(int begin, int end, bool fade) {
        while (begin <= end)
            inputData[begin++].Fade(fade);
    }

    private IEnumerator Sort(ArrayElement[] inputData, int begin, int end) {
        if (begin < end) {
            int center = (begin + end) / 2;
            Split(center+1);

            yield return _WaitStep();
            yield return Sort(inputData, begin, center);
            yield return Sort(inputData, center + 1, end);

            Fade(begin, end, false);
            yield return _WaitStep();

            int i = begin;
            int j = center + 1;
            int k = 0;

            while (i <= center && j <= end) {
                inputData[i].Select(true);
                inputData[j].Select(true);
                yield return _WaitStep();

                if (inputData[i].Value < inputData[j].Value)
                    yield return _Move(inputData[i++], outputData[k++]);
                else
                    yield return _Move(inputData[j++], outputData[k++]);
            }

            while (i <= center)
                yield return _Move(inputData[i++], outputData[k++]);

            while (j <= end)
                yield return _Move(inputData[j++], outputData[k++]);

            GameObject obj = separators.Pop();
            Destroy(obj);

            while (k > 0) {
                yield return _Move(outputData[--k], inputData[end--]);
                outputData[k].Value = 0;
            }
        }
    }

    protected override IEnumerator _Run() {
        int size = inputData.Length;
        yield return Sort(inputData, 0, size - 1);

        foreach (var e in inputData)
            e.MarkAsSorted();
    }
}
