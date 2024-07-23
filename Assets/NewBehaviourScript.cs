using Cat;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewBehaviourScript : MonoBehaviour
{
    Loger loger;
    // Start is called before the first frame update
    void Start()
    {
        Loger.FileLogSwitch = true;
        loger = new Loger($"{this.gameObject.name}");
    }
    int i = 0;
    // Update is called once per frame
    void Update()
    {
        loger.Log($"update.log{i}");
        loger.LogWarning($"update.warning{i}");
        loger.LogError($"update.error{i}");
        i++;
    }
}
