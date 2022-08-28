using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoutingSort : Algorithm {
    [SerializeField] protected GameObject input;
    [SerializeField] protected GameObject output;
    [SerializeField] protected GameObject hist;

    protected ArrayElement[] histData = new ArrayElement[0];
    protected ArrayElement[] outputData = new ArrayElement[0];
    protected ArrayElement[] inputData = new ArrayElement[0];
    private readonly int RANGE = 10;

    public override void Init(int[] dataset) {
        foreach (var e in inputData)
            Destroy(e.gameObject);

        foreach (var e in outputData)
            Destroy(e.gameObject);

        foreach (var e in histData)
            Destroy(e.gameObject);

        int size = dataset.Length;
        inputData = new ArrayElement[size];
        outputData = new ArrayElement[size];
        histData = new ArrayElement[RANGE];

        for (int i = 0; i < size; i++) {
            inputData[i] = Instantiate(elementPrefab, input.transform).GetComponent<ArrayElement>();
            inputData[i].Value = dataset[i] % RANGE;

            outputData[i] = Instantiate(elementPrefab, output.transform).GetComponent<ArrayElement>();
        }

        for (int i = 0; i < RANGE; i++)
            histData[i] = Instantiate(elementPrefab, hist.transform).GetComponent<ArrayElement>();
    }

    protected override IEnumerator _Run() {
        foreach (var e in inputData) {
            e.Select(true);
            yield return _WaitStep();

            histData[e.Value].Select(true);
            histData[e.Value].Value++;
            yield return _WaitStep();

            e.Select(false);
            histData[e.Value].Select(false);
        }

        for (int i = 1; i < RANGE; i++) {
            histData[i].Value += histData[i - 1].Value;
            histData[i].Select(true);
            yield return _WaitStep();
            histData[i].Select(false);
        }

        for (int i = inputData.Length - 1; i >= 0; i--) {
            var inData = inputData[i];
            var htData = histData[inData.Value];
            var outData = outputData[htData.Value - 1];

            inData.Select(true);
            htData.Select(true);
            outData.Select(true);
            yield return _WaitStep();

            outData.Value = inData.Value;
            histData[inData.Value].Value--;

            yield return _WaitStep();
            inData.Select(false);
            htData.Select(false);
            outData.Select(false);
        }

        foreach (var e in outputData)
            e.MarkAsSorted();
    }
}
