using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections;

public class DropZone : MonoBehaviour, IDropHandler, IPointerEnterHandler, IPointerExitHandler
{
	public Draggable.ItemTypes dropType = Draggable.ItemTypes.INVENTORY;


	public void OnDrop (PointerEventData eventData)
	{
		Debug.Log (eventData.pointerDrag.name + " Was dropped on " + gameObject.name);

		Draggable droppedItem = eventData.pointerDrag.GetComponent<Draggable> ();

		if (droppedItem != null)
		{
			if (dropType == Draggable.ItemTypes.INVENTORY || droppedItem.itemType == dropType)
			{
				droppedItem.parentToReturnTo = transform;
			}
		}


	}

	public void OnPointerEnter (PointerEventData eventData)
	{
		if (eventData.pointerDrag == null)
			return;

		Draggable droppedItem = eventData.pointerDrag.GetComponent<Draggable> ();
		
		if (droppedItem != null)
		{
			if (dropType == Draggable.ItemTypes.INVENTORY || droppedItem.itemType == dropType)
			{
				droppedItem.placeholderParent = transform;
			}
		}
	}

	public void OnPointerExit (PointerEventData eventData)
	{
		if (eventData.pointerDrag == null)
			return;

		Draggable droppedItem = eventData.pointerDrag.GetComponent<Draggable> ();
		
		if (droppedItem != null && droppedItem.placeholderParent == transform)
		{
			if (dropType == Draggable.ItemTypes.INVENTORY || droppedItem.itemType == dropType)
			{
				droppedItem.placeholderParent = droppedItem.parentToReturnTo;
			}
		}
	}

}
