using UnityEngine;
using System.Collections;
using UnityEngine.EventSystems;

public class EventTriggerListener : EventTrigger {

	public delegate void VoidDelegate (GameObject go);
	public delegate void DragDelegate(Vector2 pos,GameObject go);
	public delegate void MoveDelegate(Vector2 pos,MoveDirection dir,GameObject go);

	public VoidDelegate onClick;
	public VoidDelegate onDown;
	public VoidDelegate onEnter;
	public VoidDelegate onExit;
	public VoidDelegate onUp;
	public VoidDelegate onSelect;
	public VoidDelegate onUnSelect;
	public VoidDelegate onUpdateSelect;

	public DragDelegate onBeginDrag;
	public DragDelegate onDrag;
	public DragDelegate onEndDrag;
	public DragDelegate onDrop;
	public MoveDelegate onMove;
	public object parameter { get; set;}

	static public EventTriggerListener Get (GameObject go)
	{
		EventTriggerListener listener = go.GetComponent<EventTriggerListener>();
		if (listener == null) listener = go.AddComponent<EventTriggerListener>();
		return listener;
	}

	public override void OnPointerClick (PointerEventData eventData) {

		if (onClick == null) return;

		onClick (gameObject);
		//UEDC.Record (pp.UEDC_EVENT_TYPE.UI_BTN_CLICK, gameObject);
	}

	public override void OnPointerDown (PointerEventData eventData) {
		if (onDown == null) return;
		onDown (gameObject);
	}

	public override void OnPointerUp (PointerEventData eventData) {
		if (onUp == null) return;
		onUp (gameObject);
	}

	public override void OnPointerEnter (PointerEventData eventData){
		if(onEnter != null) onEnter(gameObject);
	}
	public override void OnPointerExit (PointerEventData eventData){
		if(onExit != null) onExit(gameObject);
	}
	public override void OnSelect (BaseEventData eventData){
		if(onSelect != null) onSelect(gameObject);
	}
	public override void OnDeselect(BaseEventData eventData)
	{
		if (onUnSelect != null) onUnSelect(gameObject); 
	}
	public override void OnUpdateSelected (BaseEventData eventData){
		if(onUpdateSelect != null) onUpdateSelect(gameObject);
	}

	public override void OnBeginDrag(PointerEventData eventData)
	{
		if (onBeginDrag != null) {
			onBeginDrag (eventData.position, gameObject);
		}
	}

	public override void OnDrag (PointerEventData eventData)
	{
		if (onDrag != null) {
			onDrag (eventData.position, gameObject);
		}
		//base.OnDrag (eventData);
	}

	public override void OnEndDrag (PointerEventData eventData)
	{
		if (onEndDrag != null) {
			onEndDrag (eventData.position, gameObject);
		}
	}

	public override void OnDrop (PointerEventData eventData)
	{
		if (onDrop != null) {
			onDrop (eventData.position, gameObject);
		}
	}

	public override void OnMove (AxisEventData eventData)
	{
		if (onMove != null) {
			onMove (eventData.moveVector, eventData.moveDir, gameObject);
		}
	}
}
