using UniRx;
using UnityEngine;
using Cysharp.Text;
using System.Collections.Generic;

public class TouchController : MonoBehaviour
{
    private CompositeDisposable disposables = new CompositeDisposable();
    void Start()
    {
#if !UNITY_EDITOR
        Observable.EveryUpdate().Where(_ => Input.GetMouseButtonDown(0)).Subscribe(_ => 
        {
			ShowTouchInfo(Input.mousePosition);
		}).AddTo(disposables);
#else
        Observable.EveryUpdate().Where(_ => Input.touchCount > 0).Select(_ =>
        {
            List<Touch> _touches = new List<Touch>();
			_touches.Add(Input.GetTouch(0));
            if(Input.touchCount > 1) _touches.Add(Input.GetTouch(1));
			return _touches;
		}).Subscribe(_touches => 
        {
            for(int i = 0; i < _touches.Count; i++)
            {
                if (_touches[0].phase == TouchPhase.Began)
				    ShowTouchInfo(_touches[i].position);
			}
                
        }).AddTo(disposables);
#endif
    }
	private void OnDisable()
	{
        disposables.Clear();
	}

    private void ShowTouchInfo(Vector3 _touchPosition)
    {
		Debug.Log(ZString.Format("터치 위치 :{0}", _touchPosition));
		Ray _ray = Camera.main.ScreenPointToRay(_touchPosition);
		RaycastHit _hit;
		if (Physics.Raycast(_ray, out _hit))
			Debug.Log(ZString.Format("터치 위치1 :{0}", _hit.point));
	}
}
