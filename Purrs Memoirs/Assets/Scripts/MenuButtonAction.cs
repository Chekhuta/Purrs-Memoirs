using UnityEngine;
using UnityEngine.EventSystems;

public class MenuButtonAction : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerClickHandler {

    public int actionId;
    private SpriteRenderer button;
    private Transform buttonTransform;

    void Start() {
        button = GetComponent<SpriteRenderer>();
        buttonTransform = GetComponent<Transform>();
    }

    public void OnPointerDown(PointerEventData eventData) {
        buttonTransform.localPosition -= new Vector3(0, 0.02f, 0);
        button.size -= new Vector2(0, 0.02f);
    }

    public void OnPointerUp(PointerEventData eventData) {
        buttonTransform.localPosition += new Vector3(0, 0.02f, 0);
        button.size += new Vector2(0, 0.02f);
    }

    public void OnPointerClick(PointerEventData eventData) {
        FindObjectOfType<Menu>().ButtonAction(actionId);
    }
}
