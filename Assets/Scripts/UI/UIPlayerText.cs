using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class UIPlayerText : MonoBehaviour
{
    TextMeshProUGUI tmText;

    void Awake()
    {
        tmText = GetComponent<TextMeshProUGUI>();
    }



    void DisableText()
    {
        tmText.text = string.Empty;
    }

    internal void HandlePlayerInitialized()
    {
        tmText.text = "Player Joined";

        StartCoroutine(ClearTextAfterDelay());
    }

    IEnumerator ClearTextAfterDelay()
    {
        yield return new WaitForSeconds(2.0f);
        tmText.text = string.Empty;
    }
}