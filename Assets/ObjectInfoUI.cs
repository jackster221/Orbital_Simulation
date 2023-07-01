using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ObjectInfoUI : MonoBehaviour
{
    public GameObject infoPanelGameObject;
    public TextMeshProUGUI NameText;
    public TextMeshProUGUI MassText;


    private void Awake()
    {
        infoPanelGameObject.SetActive(false);
    }

    public void UpdateInfo(CelestialBody celestialBody)
    {
        if (celestialBody == null)
        {
            Debug.LogError("CelestialBody is null");
            return;
        }

        NameText.text = celestialBody.Name;
        MassText.text = celestialBody.Mass.ToString("F2");

        // Set the position of the panel.
        infoPanelGameObject.transform.position = celestialBody.transform.position;
        infoPanelGameObject.SetActive(true);
    }

    public void HideInfo()
    {
        infoPanelGameObject.SetActive(false);
    }
}
