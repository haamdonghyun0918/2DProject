using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SlotCardUi : UiBase
{
    [SerializeField] private Text text_Name;
    [SerializeField] private Text text_Description;
    [SerializeField] private Image image_Card;
    [SerializeField] private Image image_Icon;
    
    public CardData MyCardData { get; private set; }
    public void SetUp(CardData cardData)
    {
        MyCardData = cardData;
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
}