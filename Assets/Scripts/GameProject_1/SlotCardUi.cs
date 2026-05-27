using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using DG.Tweening;
using System.Collections.Generic;

public class SlotCardUi : UiBase, IPointerEnterHandler, IPointerExitHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField] private Text text_Name;
    [SerializeField] private Text text_Description;
    [SerializeField] private Image image_Card;
    [SerializeField] private Image image_Icon;
    
    private CardData myCardData;
    private bool isDragging = false;
    private RectTransform rectTransform;

    private LineRenderer targetingLine;
    public void OnEnable()
    {
        rectTransform = GetComponent<RectTransform>();
        //button_Active.BindOnClickButtonEvent(OnClickCardButton);
    }
    public void SetUp(CardData cardData)
    {
        myCardData = cardData;
        if (cardData == null) return;

        text_Name.text = cardData.Name;
        text_Description.text = cardData.Description;

        Sprite iCard = Resources.Load<Sprite>(cardData.ImageCardAddress);
        Sprite iIcon = Resources.Load<Sprite>(cardData.ImageIconAddress);
        if (iCard != null) image_Card.sprite = iCard;
        if (iIcon != null) image_Icon.sprite = iIcon;

        transform.localScale = Vector3.zero;
        transform.DOScale(Vector3.one, 0.5f).SetEase(Ease.OutBack);
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isDragging) return;

        transform.DOScale(Vector3.one * 1.1f, 0.2f).SetEase(Ease.OutBounce);
        rectTransform.DOAnchorPosY(30f, 0.2f).SetRelative(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (isDragging) return;

        transform.DOScale(Vector3.one, 0.2f).SetEase(Ease.OutQuad);
        rectTransform.DOAnchorPosY(-30f, 0.2f).SetRelative(true);
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
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
            mouseWorldPos.z = transform.position.z;

            targetingLine.SetPosition(0, transform.position);
            targetingLine.SetPosition(1, mouseWorldPos);
        }
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;

        if (targetingLine != null) Destroy(targetingLine.gameObject);

        PointerEventData pointerData = new PointerEventData(EventSystem.current)
        {
            position = Input.mousePosition
        };

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
            bool isUsed = GameManager.Instance.UseCardOnMonster(myCardData, hitMonster);

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

        targetingLine.startWidth = 10f;
        targetingLine.endWidth = 20f;

        Shader uiShader = Shader.Find("UI/Default");
        if (uiShader != null)
        {
            targetingLine.material = new Material(uiShader);
        }

        targetingLine.startColor = Color.yellow;
        targetingLine.endColor = Color.red;

        targetingLine.sortingLayerName = "Default";
        targetingLine.sortingOrder = 100000;

        targetingLine.positionCount = 2;
    }
}