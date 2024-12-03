using UnityEngine;
using DG.Tweening;
public class Popup : MonoBehaviour
{
    [SerializeField] Transform DialogBox;
    [SerializeField] [Range(0.1f, 1f)] float Duration;
    public void PopupAction(bool Open)
    {
        if (Open) gameObject.SetActive(true);
        else TransformDialogBox(Open);
    }
    void OnEnable()
    {
        TransformDialogBox();
    }
    void TransformDialogBox(bool Open=true)
    {
        DialogBox.DOScale(Open ? Vector3.one : Vector3.zero, Duration).onComplete = Open ? () => { return; } : () => { gameObject.SetActive(false); };
    }
}
