using UniRx;
using UnityEngine;
using UnityEngine.UIElements;

public class CharacterCamera : MonoBehaviour
{
	[SerializeField] private float speed;
	private Transform mCharacterRoot;
	private CompositeDisposable mDisposable;
	private Transform mTransform;

	public void Set(CCharacterController _character)
	{
		mCharacterRoot = _character.CameraRoot;
		mDisposable = new CompositeDisposable();
		mTransform = transform;

		Observable.EveryUpdate().Subscribe(_ =>
		{
			mTransform.position = mCharacterRoot.position;
			mTransform.rotation = mCharacterRoot.rotation;
		}).AddTo(mDisposable);
	}

	public void Release()
	{
		mDisposable?.Dispose();
		mDisposable = null;
		mCharacterRoot = null;
	}

}
