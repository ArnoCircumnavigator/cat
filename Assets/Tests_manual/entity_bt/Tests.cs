using BehaviourTreeGeneric;
using Newtonsoft.Json.Linq;
using System.IO;
using UnityEngine;
using UnityEngine.Assertions;

namespace Cat.RuntimeTests_Manual.entity_bt
{
    public class Tests : MonoBehaviour
    {
        // Start is called before the first frame update
        void Start()
        {
            string path = Path.Combine(Application.dataPath, "Tests", "Test_BehaviourTree", "bt1.json");
            string jsonString = File.ReadAllText(path);
            JObject json = JObject.Parse(jsonString);
            BTree btree = new BTree();
            btree.Load(json.ToString());


            EntityObject[] entities = FindObjectsOfType<EntityObject>();

            //foreach (EntityObject entity in entities)
            //{
            //    BtMgr.Instance.RegisterBTree(entity.typeString, );
            //}
        }
    }
}