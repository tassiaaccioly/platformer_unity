using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HudLife : MonoBehaviour
{
    // static classes can be accessed from anywhere by other classes;
    public static HudLife instance;

    [SerializeField]
    private List<GameObject> _listLife;

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(instance);
    }

    public void RefreshLife(int value)
    {
        for(int i = 1; i <= _listLife.Count; i++)
        {
            if(i <= value)
            {
                _listLife[i - 1].SetActive(true);
            }
            else
            {
                _listLife[i - 1].SetActive(false);
            }
        }
    }
}


//int i = 0;

//foreach (var life in _listLife)
//{
//    i++;
//}
