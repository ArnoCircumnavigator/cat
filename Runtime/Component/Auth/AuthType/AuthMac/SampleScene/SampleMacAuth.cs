using Cat.Auth;
using Cat.Logger;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cat.Sample
{
    public class SampleMacAuth : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            MacAuth macAuth = new MacAuth();
            if (macAuth.Verify())
            {
                Debug.Log("验证成功");
            }
            else
            {
                Debug.Log("Mac地址验证失败！");
            }
        }

        // Update is called once per frame
        void Update()
        {

        }
    }
}
