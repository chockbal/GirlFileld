using Cysharp.Text;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class AttackAnimation
{
    private float duration;
    private int count;
	private Action<string> onPlayAnimation;

	private const int AttackCountMax = 4;
	private const float AttackDuration_1 = 1.5f;
	private const float AttackDuration_2 = 2.067f;
	private const float AttackDuration_3 = 1.5f;
	private const float AttackDuration_4 = 1.833f;

	private static float GetAttackDuration(int _attackCount)
	{
		switch (_attackCount)
		{
			case 1: return AttackDuration_1;
			case 2: return AttackDuration_2;
			case 3: return AttackDuration_3;
			case 4: return AttackDuration_4;
			default: return 0.0f;
		}
	}

	public void Set(Action<string> _onPlayAnimation)
	{
		onPlayAnimation = _onPlayAnimation;
	}

	public void Add()
	{
		if (count >= AttackCountMax)
			return;

		count++;
		duration = GetAttackDuration(count);
		onPlayAnimation?.Invoke(ZString.Format("ATTACK{0}", count));
	}

	public async UniTask UpdateAsync(CancellationToken _token)
	{
		try
		{
			while (true)
			{
				if (duration > 0.0)
				{
					duration -= Time.deltaTime;
					if (duration <= 0.0f)
						ResetState();
				}

				await UniTask.Yield(_token);
			}
		}
		catch (OperationCanceledException)
		{
		}
	}

	public void ResetState()
	{
		duration = 0.0f;
		count = 0;
	}
}
