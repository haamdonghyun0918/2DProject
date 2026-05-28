using DG.Tweening;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CardInteractionHandler : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private SlotCardUi slotUi;
    private RectTransform rectTransform;
    private bool isDragging = false;
    private LineRenderer targetingLine;
    public void OnEnable()
    {
        slotUi = GetComponent<SlotCardUi>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!enabled || isDragging) return;

        transform.DOScale(Vector3.one * 1.1f, 0.2f).SetEase(Ease.OutBounce);
        rectTransform.DOAnchorPosY(30f, 0.2f).SetRelative(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!enabled || isDragging) return;

        ResetCardPosition();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (!enabled) return;
        isDragging = true;
        CreateTargetingLine();
    }
    public void OnDrag(PointerEventData eventData)
    {
        if (targetingLine != null)
        {
            Vector3 mousePos = Input.mousePosition;
            mousePos.z = 10f;

            Vector3 mouseWorldPos = Camera.main.ScreenToWorldPoint(mousePos);

            Vector3 startPos = transform.position;
            startPos.z -= 1f;
            mouseWorldPos.z = startPos.z;

            targetingLine.SetPosition(0, startPos);
            targetingLine.SetPosition(1, mouseWorldPos);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!enabled) return;
        isDragging = false;

        if (targetingLine != null) Destroy(targetingLine.gameObject);

        PointerEventData pointerData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        GameMonster hitMonster = null;
        foreach (RaycastResult result in results)
        {
            GameMonster monster = result.gameObject.GetComponentInParent<GameMonster>();
            if (monster != null)
            {
                hitMonster = monster;
                break;
            }
        }

        if (hitMonster != null)
        {
            // 분리된 SlotCardUi에서 데이터를 가져옵니다.
            bool isUsed = GameManager.Instance.UseCardOnMonster(slotUi.MyCardData, hitMonster);

            if (isUsed)
            {
                transform.DOScale(Vector3.zero, 0.2f).OnComplete(() => Destroy(gameObject));
            }
            else
            {
                ResetCardPosition();
            }
        }
        else
        {
            ResetCardPosition();
        }
    }

    private void ResetCardPosition()
    {
        transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutQuad);
        rectTransform.DOAnchorPosY(-30f, 0.2f).SetRelative(true);
    }
    private void CreateTargetingLine()
    {
        GameObject lineObj = new GameObject("TargetingLine");
        targetingLine = lineObj.AddComponent<LineRenderer>();

        AnimationCurve arrowCurve = new AnimationCurve();

        arrowCurve.AddKey(new Keyframe(0f, 0.15f));
        arrowCurve.AddKey(new Keyframe(0.85f, 0.15f));
        arrowCurve.AddKey(new Keyframe(0.85f, 0.6f));
        arrowCurve.AddKey(new Keyframe(1f, 0f));
        targetingLine.widthCurve = arrowCurve;
        targetingLine.widthMultiplier = 1.5f;

        Shader uiShader = Shader.Find("Sprites/Default");
        if (uiShader != null)
        {
            targetingLine.material = new Material(uiShader);
        }

        targetingLine.startColor = Color.red;
        targetingLine.endColor = Color.red;

        targetingLine.sortingLayerName = "Default";
        targetingLine.sortingOrder = 555500000;

        targetingLine.positionCount = 2;
    }
}