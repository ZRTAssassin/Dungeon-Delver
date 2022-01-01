using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class UICharacterSelectionMenu : MonoBehaviour
{
    [Header("List of Characters")]
    [SerializeField]
    List<UICharacterSelectionPanel> panels = new List<UICharacterSelectionPanel>();

    public List<UICharacterSelectionPanel> Panels => panels;
    
    /*[SerializeField] private UICharacterSelectionPanel farLeftPanel;
    [SerializeField] private UICharacterSelectionPanel leftPanel;
    [SerializeField] private UICharacterSelectionPanel rightPanel;
    [SerializeField] private UICharacterSelectionPanel farRightPanel; 
    public UICharacterSelectionPanel FarLeftPanel => farLeftPanel;
    public UICharacterSelectionPanel LeftPanel => leftPanel;
    public UICharacterSelectionPanel RightPanel => rightPanel;
    public UICharacterSelectionPanel FarRightPanel => farRightPanel; */

    [SerializeField] TextMeshProUGUI startGameText;
    UICharacterSelectionMarker[] markers;
    bool startEnabled;


    void Awake()
    {
        markers = GetComponentsInChildren<UICharacterSelectionMarker>();
    }

    void Update()
    {
        int playerCount = 0;
        int lockedCount = 0;

        foreach (var marker in markers)
        {
            if (marker.IsPlayerIn)
                playerCount++;
            if (marker.IsLockedIn)
                lockedCount++;
        }
        
        startEnabled = playerCount > 0 && playerCount == lockedCount;
        startGameText.gameObject.SetActive(startEnabled);
    }

    public void TryStartGame()
    {
        if(startEnabled)
        {
            GameManager.Instance.Begin();
            gameObject.SetActive(false);
        }
    }
}