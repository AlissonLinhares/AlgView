using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Algorithm : MonoBehaviour {
    [SerializeField] protected GameObject elementPrefab;
    protected ArrayElement[] elements = new ArrayElement[0];
    protected RectTransform srcObj = null;
    protected RectTransform tgtObj = null;

    public int NextStep {get; protected set;}
    public int CurrStep {get; protected set;}
    public float Speed {get; set;}
    public bool Paused {get; set;}
    public bool Running {get; set;}

    public virtual void Init(int[] dataset) {
        foreach (var e in elements)
            Destroy(e.gameObject);

        int size = dataset.Length;
        elements = new ArrayElement[size];

        for (int i = 0; i < size; i++) {
            elements[i] = Instantiate(elementPrefab, this.transform).GetComponent<ArrayElement>();
            elements[i].Value = dataset[i];
        }
    }

    public void Step() {
        if (Running)
            NextStep = CurrStep + 1;
        else
            Run();

        Paused = true;
    }

    public void Stop() {
        Clear();
        Running = false;
        NextStep = 0;
        CurrStep = 0;
    }

    public void Run() {
        if (!Running) {
            Running = true;
            CurrStep = 0;
            NextStep = 0;
            StartCoroutine(_Run());
        }

        Paused = false;
    }

    public void Clear() {
        StopAllCoroutines();

        if (tgtObj != null)
            Destroy(tgtObj.gameObject);

        if (srcObj != null)
            Destroy(srcObj.gameObject);
    }

    protected IEnumerator _WaitStep() {
        float time = 0;

        while (CurrStep >= NextStep) {
            if (Running)
                yield return null;
            else
                yield break;

            if (!Paused) {
                time += Speed * Time.deltaTime;

                if (time > 1)
                    NextStep++;
            }
        }

        CurrStep++;
    }

    protected IEnumerator _Swap(ArrayElement eSrc, ArrayElement eTgt) {
        eSrc.Swapping(true);
        eTgt.Swapping(true);
        yield return _WaitStep();

        RectTransform a = eSrc.Label;
        a.SetParent(this.transform.parent.parent, true);
        srcObj = a;
        Vector3 src = a.position;

        RectTransform b = eTgt.Label;
        b.SetParent(this.transform.parent.parent, true);
        tgtObj = b;
        Vector3 tgt = b.position;

        while (true) {
            a.position = Vector3.MoveTowards(a.position, tgt, Time.deltaTime * Speed);
            b.position = Vector3.MoveTowards(b.position, src, Time.deltaTime * Speed);
            yield return null;

            float dist = Vector3.Distance(tgt, a.position) + Vector3.Distance(src, b.position);
            if (dist < 0.001f || NextStep > CurrStep)
                break;
        }

        a.SetParent(eSrc.transform, false);
        a.offsetMin = Vector3.zero;
        a.offsetMax = Vector3.zero;

        b.SetParent(eTgt.transform, false);
        b.offsetMin = Vector3.zero;
        b.offsetMax = Vector3.zero;

        int aux = eSrc.Value;
        eSrc.Value = eTgt.Value;
        eTgt.Value = aux;

        eSrc.Select(true);
        eTgt.Select(true);

        srcObj = null;
        tgtObj = null;
        yield return _WaitStep();
    }

    protected virtual IEnumerator _Run() {
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
