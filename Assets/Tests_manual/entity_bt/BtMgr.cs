using BehaviourTreeGeneric;
using System.Collections.Generic;

namespace Cat.RuntimeTests_Manual.entity_bt
{
    public class BtMgr
    {
        static BtMgr instance;
        public static BtMgr Instance
        {
            get
            {
                instance ??= new BtMgr();
                return instance;
            }
        }
        Dictionary<string, BTree> bTrees = new Dictionary<string, BTree>();

        public BTree GetBTree(string btkey)
        {
            return bTrees[btkey];
        }

        public void RegisterBTree(string btkey, BTree btree)
        {
            bTrees[btkey] = btree;
        }
    }
}
