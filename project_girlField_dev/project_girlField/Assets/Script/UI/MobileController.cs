using System;
using UniRx;
using UnityEngine;
using UnityEngine.EventSystems;

public class MobileController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
	[SerializeField] private GameObject controllerRoot;
	[SerializeField] private RectTransform stick;
	private ReactiveProperty<Vector3> mVector = new ReactiveProperty<Vector3>();
	private BoolReactiveProperty running = new BoolReactiveProperty();
	private float mControllerDistance;

	public IObservable<Vector3> Vector => mVector.AsObservable();
	public IObservable<bool> Running => running.AsObservable();
	private void Awake()
	{
#if UNITY_EDITOR
		controllerRoot.SetActive(false);

		Observable.EveryUpdate().Select(_ =>
		{
			float _x = Input.GetKey(KeyCode.D) ? 1f : 0f - (Input.GetKey(KeyCode.A) ? 1f : 0.0f);
			float _z = Input.GetKey(KeyCode.W) ? 1f : 0f - (Input.GetKey(KeyCode.S) ? 1f : 0.0f);
			return new Vector3(_x, 0.0f, _z).normalized;
		}).DistinctUntilChanged()
		.Subscribe(_moveDirection =>
		{
			if (_moveDirection == Vector3.zero)
			{
				running.Value = false;
			}
			else
			{
				// rotation
				//
				float _degree = 0.0f;
				if (_moveDirection == Vector3.forward)
				{
				}
				else if (_moveDirection == Vector3.right)
				{
					_degree = 90f;
				}
				else if (_moveDirection == Vector3.back)
				{
					_degree = 180f;
				}
				else if (_moveDirection == Vector3.left)
				{
					_degree = 270f;
				}
				else if (_moveDirection == (Vector3.right + Vector3.forward).normalized)
				{
					_degree = 45f;
				}
				else if (_moveDirection == (Vector3.right + Vector3.back).normalized)
				{
					_degree = 135f;
				}
				else if (_moveDirection == (Vector3.left + Vector3.back).normalized)
				{
					_degree = 225f;
				}
				else if (_moveDirection == (Vector3.left + Vector3.forward).normalized)
				{
					_degree = 315;
				}

				mVector.Value = _moveDirection;
				running.Value = true;

			}
		}).AddTo(this);
#else
		RectTransform _r = GetComponent<RectTransform>();
		mControllerDistance = (_r.sizeDelta.x * 0.5f) - (stick.sizeDelta.x * 0.5f);
#endif


	}

	public void OnBeginDrag(PointerEventData _eventData)
	{
#if UNITY_EDITOR
#else
		Debug.Log("[MobileController/OnBeginDrag] Drag Start");
		SetPosition(_eventData);
		running.Value = true;
#endif
	}

	public void OnDrag(PointerEventData _eventData)
	{
#if UNITY_EDITOR
#else
		SetPosition(_eventData);
#endif
	}

	public void OnEndDrag(PointerEventData _eventData)
	{
#if UNITY_EDITOR
#else
		Debug.Log("[MobileController/OnEndDrag] Drag End");
		stick.localPosition = Vector3.zero;
		running.Value = false;
#endif
	}

#if !UNITY_EDITOR
	private void SetPosition(PointerEventData _eventData)
	{
		Vector3 _vector = (stick.anchoredPosition + _eventData.delta);
		Vector3 _normalized;
		_vector = Vector3.Distance(Vector3.zero, _vector) <= mControllerDistance ? _vector : _vector.normalized * mControllerDistance;
		_normalized = _vector.normalized;
		stick.localPosition = _vector;
		mVector.Value = new Vector3(_normalized.x, 0.0f, _normalized.y);
	}
#endif
}
