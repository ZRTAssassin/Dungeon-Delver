using UnityEngine;

public class UICharacterSelectionPanel : MonoBehaviour
{
    [SerializeField] Character characterPrefab;

    public Character CharacterPrefab => characterPrefab;
}
