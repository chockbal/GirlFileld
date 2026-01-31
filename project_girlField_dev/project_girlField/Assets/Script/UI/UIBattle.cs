using UnityEngine;

public class UIBattle : MonoBehaviour
{
    [SerializeField] private MobileController controller;
	[SerializeField] private ButtonAttack buttonAttack;
	public void SetCharacter(CCharacterController _character)
    {
        _character.Set(controller);
		_character.Set(buttonAttack);
	}
}
