using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DropZone : MonoBehaviour, IDropHandler
{
    public void OnDrop(PointerEventData eventData)
    {
        DraggablePokemonSlot draggable = eventData.pointerDrag.GetComponent<DraggablePokemonSlot>();
        if (draggable != null)
        {
            draggable.NewParent = this.transform;
        }
    }
}
