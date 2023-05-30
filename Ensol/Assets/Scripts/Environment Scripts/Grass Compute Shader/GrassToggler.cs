using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrassToggler : MonoBehaviour
{
    [SerializeField] private OptionsMenu optionsMenu;
    [SerializeField] private GameObject grass;

    private void Start()
    {
        optionsMenu.OnGrassActivatedChange.AddListener(ToggleGrass);
    }

    private void ToggleGrass(bool active)
    {
        grass.SetActive(active);
    }
}
