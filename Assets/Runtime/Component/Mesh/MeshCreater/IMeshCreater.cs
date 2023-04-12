using System;
using UnityEngine;

namespace MeshUtils
{
    public interface IMeshCreater : IDisposable
    {
        GameObject CreatMeshGameObject();
    }

}
