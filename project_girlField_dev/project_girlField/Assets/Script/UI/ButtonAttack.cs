using Cysharp.Threading.Tasks;
using DG.Tweening;
using System;
using System.Threading;
using UniRx;
using UnityEngine;

public class ButtonAttack : MonoBehaviour
{
    [SerializeField] private CanvasGroup canvasGroup;
    private FloatReactiveProperty cooltime = new FloatReactiveProperty();
	private Subject<Unit> onClicked = new Subject<Unit>();
    private CancellationTokenSource cts;
    public IObservable<float> CoolTime => cooltime.AsObservable();
	public IObservable<Unit> OnClicked => onClicked.AsObservable();

	private void Start()
	{
		UpdateCountAsync();
	}
	private void OnDisable()
	{
		cts?.Clear();
		cts = null;
	}

	public void OnClickButton()
    {
        canvasGroup.alpha = 1.0f;
		canvasGroup.DOKill();
        canvasGroup.DOFade(0.8f, 0.1f).OnComplete( () => canvasGroup.alpha = 1.0f);
        if(cooltime.Value <= 0)
		{
			cooltime.Value = 0.5f;
			onClicked.OnNext(default);
		}
	}

    private async UniTask UpdateCountAsync()
    {
        cts?.Clear();
		cts = new CancellationTokenSource();

        try
        {
			while (true)
			{
				if (cooltime.Value > 0.0)
				{
					float _cooltime = cooltime.Value - Time.deltaTime;
					cooltime.Value = _cooltime <= 0.0f ? 0.0f : _cooltime;
				}

				await UniTask.Yield(cts.Token);
			}
		}
		catch(OperationCanceledException)
        {
        }
    }
}
