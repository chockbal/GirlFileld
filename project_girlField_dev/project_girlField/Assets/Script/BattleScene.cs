using Cysharp.Threading.Tasks;
using UnityEngine;

public class BattleScene : MonoSingleton<BattleScene>
{
	[SerializeField] private UIBattle mUIBattle;
	[SerializeField] private CCharacterController mCharacter;
	[SerializeField] private CharacterCamera characterCamera;

	protected override void Awake()
	{
		Application.targetFrameRate = 120;

		base.Awake();
	}

	private async UniTask Start()
	{
		mUIBattle.SetCharacter(mCharacter);
		characterCamera.Set(mCharacter);
	}
}
