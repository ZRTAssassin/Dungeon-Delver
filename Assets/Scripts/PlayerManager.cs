using System.Linq;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    public static PlayerManager Instance { get; private set; }
    Player[] _players;
    

    void Awake()
    {
        Instance = this;
        _players = FindObjectsOfType<Player>();
    }

    public void AddPlayerToGame(Controller controller)
    {
        var firstNonActivePlayer = _players
            .OrderBy(t => t.PlayerNumber)
            .FirstOrDefault(t => t.HasController == false);
        firstNonActivePlayer.InitizlizePlayer(controller);
    }

    public void SpawnPlayerCharacters()
    {
        var spawnPosition = FindObjectOfType<PlayerSpawner>().SpawnPosition;
        foreach (var player in _players)
        {
            if (player.HasController && player.CharacterPrefab != null)
            {
                player.SpawnCharacter(spawnPosition);
            }
        }
    }
    
}
