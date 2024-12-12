using System;
using System.Collections;
using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] int playerNumber;


    UIPlayerText uiPlayerText;

    public event Action<Character> OnCharacterChanged = delegate { };

    public bool HasController { get { return Controller != null; } }
    public int PlayerNumber => playerNumber;
    public Controller Controller { get; private set; }

    public Character CharacterPrefab { get; set; }

    void Awake()
    {
        uiPlayerText = GetComponentInChildren<UIPlayerText>();
    }

    public void InitizlizePlayer(Controller controller)
    {
        Controller = controller;

        gameObject.name = string.Format("Player {0} - {1}", playerNumber, controller.gameObject.name);

        uiPlayerText.HandlePlayerInitialized();
    }

    public void SpawnCharacter(Vector3 spawnPosition)
    {
        var character = CharacterPrefab.Get<Character>(spawnPosition, Quaternion.identity);
        character.SetController(Controller);
        character.OnDied += Character_OnDied;

        OnCharacterChanged(character);
    }

    void Character_OnDied(IDie character)
    {
        character.OnDied -= Character_OnDied;

        character.gameObject.SetActive(false);

        StartCoroutine(RespawnAfterDelay());
    }

    IEnumerator RespawnAfterDelay()
    {
        yield return new WaitForSeconds(5f);
        SpawnCharacter(transform.position);
    }
}