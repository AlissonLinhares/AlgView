using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class Menu : MonoBehaviour {
    [SerializeField] private Slider slider;
    [SerializeField] private TMP_Dropdown algDropDown;
    [SerializeField] private TMP_Dropdown dataDropDown;
    [SerializeField] private Transform display;

    [SerializeField] private int size = 5;
    [SerializeField] private int minValue = 0;
    [SerializeField] private int maxValue = 100;
    [SerializeField] private GameObject[] prefabs;

    private enum DataSet {
        RANDOM,
        REVERSE,
        SORTED
    }

    private Algorithm algorithm = null;
    private int[] values = new int[0];

    public void Start() {
        GenArray();
    }

    public void OnPlay() {
        algorithm.Run();
    }

    public void OnPause() {
        algorithm.Paused = true;
    }

    public void OnStep() {
        algorithm.Step();
    }

    public void OnSpeedChange() {
        algorithm.Speed = slider.value;
    }

    public void OnStop() {
        algorithm.Stop();
        algorithm.Init(values);
    }

    public void OnAddElements() {
        if (size < 50) {
            size += 5;
            GenArray();
        }
    }

    public void OnRemoveElements() {
        if (size > 5) {
            size -= 5;
            GenArray();
        }
    }

    public void OnAlgorithmChanged() {
        GenArray();
    }

    public void OnDataSetChanged() {
        GenArray();
    }

    private void ResizeArray() {
        int[] result = new int[size];

        for (int i = 0; i < size; i++)
            result[i] = UnityEngine.Random.Range(this.minValue, this.maxValue);

        this.values = result;
    }

    private void GenArray() {
        ResizeArray();

        if (this.dataDropDown.value == (int) DataSet.REVERSE)
            Sort(false);
        else if (this.dataDropDown.value == (int) DataSet.SORTED)
            Sort(true);

        if (algorithm != null) {
            algorithm.Stop();
            Destroy(algorithm.gameObject);
        }

        GameObject obj = Instantiate(prefabs[algDropDown.value], display);
        this.algorithm = obj.GetComponent<Algorithm>();
        this.algorithm.Init(this.values);
        this.algorithm.Speed = this.slider.value;
    }

    private void Sort(bool reverse = false) {
        int len = this.values.Length;

        for (int i = 1; i < len; ++i) {
            for (int j = 0; j < len - i; ++j) {
                int a = this.values[j];
                int b = this.values[j + 1];

                if (reverse ? a < b : a > b) {
                    this.values[j] = b;
                    this.values[j + 1] = a;
                }
            }
        }
    }
}
