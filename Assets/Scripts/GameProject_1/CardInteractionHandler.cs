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

    private float originY;
    private bool isOriginSet = false;

    public GameMonster currentTargetedMonster;
    public void OnEnable()
    {
        slotUi = GetComponent<SlotCardUi>();
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (!enabled || isDragging) return;

        if (!isOriginSet)
        {
            originY = rectTransform.anchoredPosition.y;
            isOriginSet = true;
        }
        transform.DOKill();
        transform.DOScale(Vector3.one * 1.1f, 0.2f).SetEase(Ease.OutBounce);
        rectTransform.DOAnchorPosY(originY + 30f, 0.2f);
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
        
        PointerEventData pointerData = new PointerEventData(EventSystem.current) { position = Input.mousePosition };
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(pointerData, results);

        GameMonster hitMonster = null;
        foreach (RaycastResult result in results)
        {
            GameMonster monster = result.gameObject.GetComponent<GameMonster>();
            if (monster != null)
            {
                hitMonster = monster;
                break;
            }
        }

        if (hitMonster != currentTargetedMonster)
        {
            if (currentTargetedMonster != null)
            {
                currentTargetedMonster.SetTargetOutline(false);
            }

            if (hitMonster != null)
            {
                hitMonster.SetTargetOutline(true);
            }

            currentTargetedMonster = hitMonster;
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        if (!enabled) return;
        isDragging = false;

        if (targetingLine != null) Destroy(targetingLine.gameObject);

        if (currentTargetedMonster != null)
        {
            currentTargetedMonster.SetTargetOutline(false);
            currentTargetedMonster = null;
        }
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
        transform.DOKill();
        transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutQuad);

        if (isOriginSet)
        {
            rectTransform.DOAnchorPosY(originY, 0.2f);
        }
    }

    private void CreateTargetingLine()
    {
        GameObject lineObj = new GameObject("TargetingLine");
        targetingLine = lineObj.AddComponent<LineRenderer>();
        Texture2D arrowTexture = Resources.Load<Texture2D>("Image/RedArrow");
        
        //셰이더가 아니라 유니티에서 제공하는 재료 그대로 사용
        Material defaultUIMaterial = Canvas.GetDefaultCanvasMaterial();
        Material arrowMaterial = new Material(defaultUIMaterial);

        if (arrowTexture != null)
        {
            arrowMaterial.mainTexture = arrowTexture;
        }
        else
        {
            Debug.LogError("이미지 주소가 잘못 되었습니다 이름과 주소를 다시 확인하세요");
        }

        targetingLine.material = arrowMaterial;
        //원래 이미지 색상이 빨간색이므로 하얀색으로 해줘야함
        targetingLine.startColor = Color.white;
        targetingLine.endColor = Color.white;

        targetingLine.startWidth = 1.0f;
        targetingLine.endWidth = 1.0f;

        targetingLine.sortingLayerName = "Default";
        targetingLine.sortingOrder = 32754;
        targetingLine.positionCount = 2;
    }
}