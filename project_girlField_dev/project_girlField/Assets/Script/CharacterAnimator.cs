using Cysharp.Text;
using Cysharp.Threading.Tasks;
using System;
using System.Threading;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Animator animator;
	private bool isRunning;
	private int attackCount = 0;
	private float duration;
	private CancellationTokenSource cts;

	private const int AttackCountMax = 4;

	private const float AttackDuration_1 = 1.5f;
	private const float AttackDuration_2 = 2.067f;
	private const float AttackDuration_3 = 1.5f;
	private const float AttackDuration_4 = 1.833f;

	private const int idx_cts_animation = 1000;
	private const int idx_cts_attack_duration = 1001;
	private static float GetAttackDuration(int _attackCount)
	{
		switch(_attackCount)
		{
			case 1: return AttackDuration_1;
			case 2: return AttackDuration_2;
			case 3: return AttackDuration_3;
			case 4: return AttackDuration_4;
			default:return 0.0f;
		}
	}

	private void Start()
	{
		UpdateCountAsync();
	}


	private void Awake()
	{
		animator = GetComponent<Animator>();
	}
	private void OnDisable()
	{
		cts?.Clear();
		cts = null;
	}
	private void PlayAnim(string _key)
	{
		animator.SetTrigger(_key);
	}
	public void PlayRun()
	{
		if(!isRunning)
		{
			isRunning = true;
			PlayAnim("RUN");
			ResetAttack();
		}
	}
	public void PlayIdle()
	{
		PlayAnim("IDLE");
		isRunning = false;
	}

	public void AddAttackCount()
	{
		if (attackCount >= AttackCountMax) return;
		attackCount++;
		duration = GetAttackDuration(attackCount);

		string _str = ZString.Format("ATTACK{0}", attackCount);
		Debug.Log(_str);
		PlayAnim(_str);
	}
	private async UniTask UpdateCountAsync()
	{
		cts?.Clear();
		cts = new CancellationTokenSource();

		try
		{
			while (true)
			{
				if (duration > 0.0)
				{
					duration -= Time.deltaTime;
					if(duration <= 0.0f)
					{
						ResetAttack();
					}
				}

				await UniTask.Yield(cts.Token);
			}
		}
		catch (OperationCanceledException)
		{
		}
	}
	private void ResetAttack()
	{
		duration = 0.0f;
		attackCount = 0;
		Debug.Log("Stop");
	}
}
