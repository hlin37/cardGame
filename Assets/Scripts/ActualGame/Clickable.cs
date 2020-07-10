using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Clickable : MonoBehaviour, ISelectHandler, IDeselectHandler, IPointerClickHandler
{
    public static List<Clickable> allSelectables = new List<Clickable>();

    public static List<Clickable> current = new List<Clickable>();

    private Image img;

    [SerializeField]
    private Color selectBlue;

    [SerializeField]
    private Color unSelectRed;

    [SerializeField]
    private Color unSelectWhite;

    void Awake() {
        allSelectables.Add(this);
        img = GetComponent<Image>();
    }

    public void OnPointerClick(PointerEventData eventData) {
        OnSelect(eventData);
    }

    public void OnSelect(BaseEventData eventData) {
        if (this.gameObject.name.Contains("White")) {
            if (current.Count == 2 && !current.Contains(this)) {
                current[0].OnDeselect(eventData);
                current.RemoveAt(0);
            }
            if (!current.Contains(this)) {
                current.Add(this);
                img.color = selectBlue;
            }
            else {
                int selectedNumber = 0;
                for (int i = 0; i < current.Count; i++) {
                    Clickable select = current[i];
                    if (select == this) {
                        select.OnDeselect(eventData);
                        selectedNumber = i;
                    }
                }
                current.RemoveAt(selectedNumber);
            }
        }
        else {
            if (current.Count == 1 && !current.Contains(this)) {
                current[0].OnDeselect(eventData);
                current.RemoveAt(0);
            }
            if (!current.Contains(this)) {
                current.Add(this);
                img.color = selectBlue;
            }
            else {
                int selectedNumber = 0;
                for (int i = 0; i < current.Count; i++) {
                    Clickable select = current[i];
                    if (select == this) {
                        select.OnDeselect(eventData);
                        selectedNumber = i;
                    }
                }
                current.RemoveAt(selectedNumber);
            }
        }
    }

    public void OnDeselect(BaseEventData eventData) {
        if (this.gameObject.name.Contains("White")) {
            img.color = unSelectWhite;
        }
        else {
            img.color = unSelectRed;
        }
    }
}
