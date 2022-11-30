using System.Collections;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] private GameObject hiddenPanel;

    public void HideButtonPress()
    {
        hiddenPanel.SetActive(!hiddenPanel.activeSelf);
    }
}