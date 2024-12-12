using System;
using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class PlayerTracker : MonoBehaviour
{
    CinemachineTargetGroup targetGroup;

    void Awake()
    {
        targetGroup = GetComponent<CinemachineTargetGroup>();

        var players = FindObjectsOfType<Player>();
        foreach (var player in players)
        {
            player.OnCharacterChanged += (character) => Player_OnCharacterChanged(player, character);
        }
    }

    void Player_OnCharacterChanged(Player player, Character character)
    {
        int playerIndex = player.PlayerNumber - 1;
        targetGroup.m_Targets[playerIndex].target = character.transform;
    }
}
