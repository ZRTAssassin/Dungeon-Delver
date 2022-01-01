using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UICharacterSelectionMarker : MonoBehaviour
{
    [Header("Index")] 
    [SerializeField]
    int _selectedCharacterIndex;

    
    [Header("Player marker info")]
    [SerializeField]
    Player _player;
    [SerializeField] Image _markerImage;
    [SerializeField] Image _lockImage;

    UICharacterSelectionMenu _menu;
    bool _initializing;
    bool _initialized;

    public bool IsLockedIn { get; private set; }
    public bool IsPlayerIn { get { return _player.HasController; } }

    void Awake()
    {
        _menu = GetComponentInParent<UICharacterSelectionMenu>();
        _markerImage.gameObject.SetActive(false);
        _lockImage.gameObject.SetActive(false);
    }

    void Update()
    {
        if (IsPlayerIn == false)
            return;

        if (!_initializing)
            StartCoroutine(Initialize());

        if (!_initialized)
            return;
        //check for player controls and selection + locking character
        if (!IsLockedIn)
        {
            MoveToCharacterPanel();

            // former way to set the transform and the panels
            /*if (player.Controller.horizontal > 0.5)
            {
                MoveToCharacterPanel(menu.RightPanel);
            }
            else if (player.Controller.horizontal < -0.5)
            {
                MoveToCharacterPanel(menu.LeftPanel);
            }
            //add ability to go to far left and right panels*/
            if (_player.Controller.attackPressed)
            {
                StartCoroutine(LockCharacter());
            }
            
        }
        else if (IsLockedIn && _player.Controller.cancelPressed)
        {
            //Debug.Log("Hey fucker!"); 
            StartCoroutine(UnlockCharacter());
        }

        else
        {
            if (_player.Controller.attackPressed)
            {
                _menu.TryStartGame();
            }
        }
    }

    IEnumerator LockCharacter()
    {
        yield return new WaitForSeconds(0.2f);
        _lockImage.gameObject.SetActive(true);
        // adding in player character prefab
        _player.CharacterPrefab = _menu.Panels[_selectedCharacterIndex].CharacterPrefab;
        IsLockedIn = true;
    }

    IEnumerator UnlockCharacter()
    {
        yield return new WaitForSeconds(0.2f);
        _lockImage.gameObject.SetActive(false);
        _player.CharacterPrefab = null;
        IsLockedIn = false;
    }

    void MoveToCharacterPanel()
    {
        if (_selectedCharacterIndex > 0 && _player.Controller.leftBumperPressed)
        {
            _selectedCharacterIndex--;
            transform.position = _menu.Panels[_selectedCharacterIndex].transform.position;
            Debug.Log(_selectedCharacterIndex);
        }
        if (_selectedCharacterIndex < _menu.Panels.Count - 1 && _player.Controller.rightBumperPressed)
        {
            _selectedCharacterIndex++;
            transform.position = _menu.Panels[_selectedCharacterIndex].transform.position;
            Debug.Log(_selectedCharacterIndex);
        }
    }

    //old way to move character panels
    /*private void MoveToCharacterPanel(UICharacterSelectionPanel panel)
    {
        transform.position = panel.transform.position;
        
       player.CharacterPrefab = panel.CharacterPrefab;
    }*/

    IEnumerator Initialize()
    {
        _initializing = true;
        //MoveToCharacterPanel(menu.Panels[selectedCharacterIndex]);
        _selectedCharacterIndex = 0;
        transform.position = _menu.Panels[_selectedCharacterIndex].transform.position;

        yield return new WaitForSeconds(0.5f);

        _markerImage.gameObject.SetActive(true);
        _initialized = true;
    }
}
