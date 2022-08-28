using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

[RequireComponent(typeof(Image))]
public class ArrayElement : MonoBehaviour {
    [SerializeField] private TMP_Text label;
    [SerializeField] private Color defaultColor = Color.white;
    [SerializeField] private Color selectedColor = Color.white;
    [SerializeField] private Color swappingColor = Color.white;
    [SerializeField] private Color sortedColor = Color.white;

    private Image image;
    private int data = 0;

    public int Value {get => data; set {data = value; label.text = string.Format("{0:00}", value);}}
    public RectTransform Label {get => (RectTransform) label.transform;}

    public void MarkAsSorted() {
        image.color = sortedColor;
    }

    public void Select(bool value) {
        if (value)
            image.color = selectedColor;
        else
            image.color = defaultColor;
    }

    public void Fade(bool fade) {
        Color color = image.color;
        color.a = fade ? 0.5f : 1f;
        image.color = color;
    }

    public void Swapping(bool value) {
        if (value)
            image.color = swappingColor;
        else
            image.color = defaultColor;
    }

    private void Start() {
        image = GetComponent<Image>();
        Select(false);
    }
}
