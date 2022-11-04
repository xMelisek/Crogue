using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIScript : MonoBehaviour
{
    public void DashClick()
    {
        FindObjectOfType<PlayerBehaviour>().Dash();
    }
}
