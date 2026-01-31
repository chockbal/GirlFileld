using System.Threading;
using UnityEngine;

public class CharacterAnimator : MonoBehaviour
{
    private Animator animator;
	private AttackAnimation attack = new AttackAnimation();
	private CancellationTokenSource cts;
	private bool isRunning;

	private void Start()
	{
		animator = GetComponent<Animator>();
		cts = new CancellationTokenSource();
		attack.UpdateAsync(cts.Token);
		attack.Set(PlayAnim);
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
			attack.ResetState();
		}
	}
	public void PlayIdle()
	{
		PlayAnim("IDLE");
		isRunning = false;
	}

	public void AddAttackCount()
	{
		attack.Add();
	}
}
