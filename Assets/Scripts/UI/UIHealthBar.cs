using UnityEngine;
using UnityEngine.UI;

public class UIHealthBar : MonoBehaviour
{

    [SerializeField] Image foregroundImage;


    Character currentCharacter;

    void Awake()
    {
        var player = GetComponentInParent<Player>();
        player.OnCharacterChanged += Player_OnCharacterChanged;
        gameObject.SetActive(false);
    }

    void Player_OnCharacterChanged(Character character)
    {
        currentCharacter = character;
        currentCharacter.OnHealthChanged += HandleHealthChanged;
        currentCharacter.OnDied += CurrentCharacter_OnDied;
        gameObject.SetActive(true);
    }

    void CurrentCharacter_OnDied(IDie character)
    {
        currentCharacter.OnHealthChanged -= HandleHealthChanged;
        currentCharacter.OnDied -= CurrentCharacter_OnDied;
        currentCharacter = null;
        
        gameObject.SetActive(false);
    }

    void HandleHealthChanged(int currentHealth, int maxHealth)
    {
        float pct = (float)currentHealth / (float)maxHealth;
        foregroundImage.fillAmount = pct;
    }
}
