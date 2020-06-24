using UnityEngine;
using UnityEngine.EventSystems;

public class DraggablePokemonSlot : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public Transform NewParent { get; set; }
    private Transform originalParent;
    private int originalSiblingIndex;
    private float originalTransparency;
    private const float dragTransparency = 0.75f;
    private CanvasGroup canvasGroup;
    private GameObject swapText;

    private void Start()
    {
        this.swapText = this.transform.parent.parent.Find("SwapText").gameObject;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        this.swapText.SetActive(true);

        originalParent = this.transform.parent;
        originalSiblingIndex = this.transform.GetSiblingIndex();

        this.transform.SetParent(originalParent.parent);

        canvasGroup = GetComponent<CanvasGroup>();
        canvasGroup.blocksRaycasts = false;
        originalTransparency = canvasGroup.alpha;
        canvasGroup.alpha = dragTransparency;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.pointerCurrentRaycast.worldPosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Player player = Player.Instance;
        if(player == null)
        {
            Debug.LogError("Cant do drag swap because Player is null");
        }

        this.swapText.SetActive(false);

        var hoveredObjects = eventData.hovered;
        hoveredObjects.Reverse();
        GameObject replacedObject = null;
        foreach (GameObject obj in hoveredObjects)
        {
            if (obj.CompareTag("PartyPokemonSlot"))
            {
                replacedObject = obj;
                break;
            }
        }

        if (NewParent != null && !NewParent.Equals(originalParent))
        {
            this.transform.SetParent(NewParent);
            Pokemon removedPokemon = player.Party.GetPokemonAtIndex(originalSiblingIndex);
            player.Party.Remove(removedPokemon);
        }
        else
        {
         
            if (replacedObject != null)
            {
                int replaceIndex = replacedObject.transform.GetSiblingIndex();
                this.transform.SetParent(originalParent);
                replacedObject.transform.SetSiblingIndex(originalSiblingIndex);
                this.transform.SetSiblingIndex(replaceIndex);
                //Debug.Log($"swap replace. from: {originalSiblingIndex} to: {replaceIndex}");
                player.Party.Swap(originalSiblingIndex, replaceIndex+1);
                player.Party.Swap(replaceIndex, replaceIndex + 1);                


            }
            else
            {
                this.transform.SetParent(originalParent); 
                var endPosition = eventData.pointerCurrentRaycast.worldPosition;
                transform.position = new Vector3(Mathf.Round(endPosition.x),
                                             Mathf.Round(endPosition.y),
                                             Mathf.Round(endPosition.z));
                int newIndex = transform.GetSiblingIndex();
                //Debug.Log($"swap new pos. from: {originalSiblingIndex} to: {newIndex}");
                player.Party.Swap(originalSiblingIndex, newIndex);

            }
        }

        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = originalTransparency;
    }
}