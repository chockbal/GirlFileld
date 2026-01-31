using Cysharp.Threading.Tasks;
using UnityEngine;

public class BattleScene : MonoSingleton<BattleScene>
{
	[SerializeField] private UIBattle mUIBattle;
	[SerializeField] private CCharacterController mCharacter;

	private async UniTask Start()
	{
		mUIBattle.SetCharacter(mCharacter);
	}
}
