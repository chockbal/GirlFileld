using System;
using UniRx;
using UnityEngine;

public class CCharacterController : MonoBehaviour
{
	private Transform mTransform;
	private BoolReactiveProperty walk= new BoolReactiveProperty();

	private CharacterAnimator characterAnimator;
	private CompositeDisposable disposables;
	public IObservable<bool> Walk => walk.AsObservable();

	private void Awake()
	{
		mTransform = transform;
		characterAnimator = GetComponent<CharacterAnimator>();
		disposables = new CompositeDisposable();

		walk.Value = false;
	}

	public void Dispose()
	{
		disposables?.Dispose();
		disposables = null;
	}

	public void ToggleWalk()
	{
		walk.Value = !walk.Value;
	}

	public void Set(MobileController _controller)
	{
		_controller.Vector.Subscribe(_vector => 
		{ 
			SetRotation(_vector);
		}).AddTo(disposables);
		_controller.Running.Subscribe(_isRunning =>
		{
			SetRunningAnim(_isRunning);
		}).AddTo(disposables);
	}
	public void Set(ButtonAttack _buttonAttack)
	{
		_buttonAttack.OnClicked.Subscribe(_unit =>
		{
			characterAnimator.AddAttackCount();
		}).AddTo(disposables);
	}
	private void SetControllerEvent()
	{
		Observable.EveryUpdate().Select(_ =>
		{
			float _x = Input.GetKey(KeyCode.D) ? 1f : 0f - (Input.GetKey(KeyCode.A) ? 1f : 0.0f);
			float _z = Input.GetKey(KeyCode.W) ? 1f : 0f - (Input.GetKey(KeyCode.S) ? 1f : 0.0f);
			return new Vector3(_x, 0.0f, _z).normalized;
		}).DistinctUntilChanged()
		.Subscribe(_moveDirection =>
		{
			//if (_moveDirection == Vector3.zero)
			//{
			//	PlayAnim("IDLE");
			//}
			//else
			//{
			//	// rotation
			//	//
			//	float _degree = 0.0f;
			//	Debug.Log((_moveDirection));
			//	if (_moveDirection == Vector3.forward)
			//	{
			//	}
			//	else if (_moveDirection == Vector3.right)
			//	{
			//		_degree = 90f;
			//	}
			//	else if (_moveDirection == Vector3.back)
			//	{
			//		_degree = 180f;
			//	}
			//	else if (_moveDirection == Vector3.left)
			//	{
			//		_degree = 270f;
			//	}
			//	else if (_moveDirection == (Vector3.right + Vector3.forward).normalized)
			//	{
			//		_degree = 45f;
			//	}
			//	else if (_moveDirection == (Vector3.right + Vector3.back).normalized)
			//	{
			//		_degree = 135f;
			//	}
			//	else if (_moveDirection == (Vector3.left + Vector3.back).normalized)
			//	{
			//		_degree = 225f;
			//	}
			//	else if (_moveDirection == (Vector3.left + Vector3.forward).normalized)
			//	{
			//		_degree = 315;
			//	}

			//	if (mTransform.localRotation.y != _degree)
			//		mTransform.localRotation = Quaternion.Euler(0.0f, _degree, 0.0f);
			//	//
			//	PlayAnim("RUN");
			//}

		}).AddTo(this);
	}

	private void SetRotation(Vector3 _controllerDirection)
	{
		Vector3 _default = Vector3.forward;
		float _degree = Vector3.Angle(_default, _controllerDirection);
		if (mTransform.localRotation.y != _degree)
			mTransform.localRotation = Quaternion.Euler(0.0f, _controllerDirection.x > 0 ? _degree : _degree*-1, 0.0f);
	}
	private void SetRunningAnim(bool _running)
	{
		if(_running)
			characterAnimator.PlayRun();
		else
			characterAnimator.PlayIdle();
	}

	//private void SetRotation(Vector3 _moveDirection)
	//{
	//	float _degree = 0.0f;
	//	Debug.Log((_moveDirection));
	//	if (_moveDirection == Vector3.forward)
	//	{
	//	}
	//	else if (_moveDirection == Vector3.right)
	//	{
	//		_degree = 90f;
	//	}
	//	else if (_moveDirection == Vector3.back)
	//	{
	//		_degree = 180f;
	//	}
	//	else if (_moveDirection == Vector3.left)
	//	{
	//		_degree = 270f;
	//	}
	//	else if (_moveDirection == (Vector3.right + Vector3.forward).normalized)
	//	{
	//		_degree = 45f;
	//	}
	//	else if (_moveDirection == (Vector3.right + Vector3.back).normalized)
	//	{
	//		_degree = 135f;
	//	}
	//	else if (_moveDirection == (Vector3.left + Vector3.back).normalized)
	//	{
	//		_degree = 225f;
	//	}
	//	else if (_moveDirection == (Vector3.left + Vector3.forward).normalized)
	//	{
	//		_degree = 315;
	//	}

	//	if (mTransform.localRotation.y != _degree)
	//		mTransform.localRotation = Quaternion.Euler(0.0f, _degree, 0.0f);
	//}
}
