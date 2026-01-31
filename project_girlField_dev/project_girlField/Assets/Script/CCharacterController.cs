using System;
using UniRx;
using UnityEngine;

public class CCharacterController : MonoBehaviour
{
	[SerializeField] private Transform cameraRoot;

	private Transform mTransform;
	private BoolReactiveProperty walk= new BoolReactiveProperty();

	private CharacterAnimator characterAnimator;
	private CompositeDisposable disposables;
	private Vector3 position = new Vector3(0.0f, 1.61f, -2f);
	private Vector3 rotation = new Vector3(10f, 0.0f, 0.0f);
	private const float DefaultSpeed = 5.0f;
	public IObservable<bool> Walk => walk.AsObservable();
	public Transform CameraRoot => cameraRoot;

	private void Awake()
	{
		mTransform = transform;
		characterAnimator = GetComponent<CharacterAnimator>();
		disposables = new CompositeDisposable();

		Observable.EveryUpdate().Subscribe(_ =>
		{
			Vector3 _targetPosition = mTransform.position + position;
			float _delta = Time.deltaTime * DefaultSpeed;

			cameraRoot.position = Vector3.Lerp(cameraRoot.position, _targetPosition, _delta);
			//mTransform.rotation = Quaternion.Lerp(mTransform.rotation, _targetRotation, _delta);
			cameraRoot.rotation = Quaternion.Euler(rotation);
		}).AddTo(disposables);

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
		//Observable.EveryUpdate().Select(_ =>
		//{
		//	float _x = Input.GetKey(KeyCode.D) ? 1f : 0f - (Input.GetKey(KeyCode.A) ? 1f : 0.0f);
		//	float _z = Input.GetKey(KeyCode.W) ? 1f : 0f - (Input.GetKey(KeyCode.S) ? 1f : 0.0f);
		//	return new Vector3(_x, 0.0f, _z).normalized;
		//}).DistinctUntilChanged()
		//.Subscribe(_moveDirection =>
		//{
		//}).AddTo(this);
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

}
