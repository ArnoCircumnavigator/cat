using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Cat.RuntimeTests_Manual.entity_bt
{
    public class EntityObject : MonoBehaviour
    {
        public string typeString = string.Empty;

        public void Awake()
        {
            this.typeString = this.gameObject.name;
        }
    }
}