using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIManager : MonoBehaviour
{
    public List<Sprite> HealthIcons = new List<Sprite>();
    public Image HealthBar;

    public void UpdateLife(int health) {
        HealthBar.sprite = HealthIcons[health];

    }
}
