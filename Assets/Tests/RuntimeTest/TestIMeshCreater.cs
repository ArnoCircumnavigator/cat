using MeshUtils;
using NUnit.Framework;
using System.Collections;
using UnityEngine;
using UnityEngine.TestTools;

namespace Cat.RuntimeTests
{
    public class TestIMeshCreater
    {
        IMeshCreater creater;

        // A Test behaves as an ordinary method
        // SetUp : some action before entry this Testing
        [SetUp]
        public void TestSetup()
        {
            int gridSizeX = 10; // 网格的行数
            int gridSizeY = 10; // 网格的列数
            float cellSize = 1.0f; // 每个单元格的大小

            MeshFilter meshFilter;
            MeshRenderer meshRenderer;
            Mesh mesh;
            // 计算网格的总大小
            float totalSizeX = gridSizeX * cellSize;
            float totalSizeY = gridSizeY * cellSize;

            // 计算网格的半宽和半高
            float halfSizeX = totalSizeX * 0.5f;
            float halfSizeY = totalSizeY * 0.5f;

            // 生成顶点
            Vector3[] vertices = new Vector3[(gridSizeX + 1) * (gridSizeY + 1)];
            for (int y = 0; y <= gridSizeY; y++)
            {
                for (int x = 0; x <= gridSizeX; x++)
                {
                    float xPos = x * cellSize - halfSizeX;
                    float yPos = y * cellSize - halfSizeY;
                    vertices[y * (gridSizeX + 1) + x] = new Vector3(xPos, 0, yPos);
                }
            }

            // 生成三角形
            int[] triangles = new int[gridSizeX * gridSizeY * 6];
            int t = 0;
            int v = 0;
            for (int y = 0; y < gridSizeY; y++)
            {
                for (int x = 0; x < gridSizeX; x++)
                {
                    triangles[t++] = v;
                    triangles[t++] = v + (gridSizeX + 1);
                    triangles[t++] = v + 1;
                    triangles[t++] = v + 1;
                    triangles[t++] = v + (gridSizeX + 1);
                    triangles[t++] = v + (gridSizeX + 1) + 1;
                    v++;
                }
                v++;
            }

            // 设置网格的UV
            Vector2[] uv = new Vector2[vertices.Length];
            for (int i = 0; i < uv.Length; i++)
            {
                uv[i] = new Vector2(vertices[i].x / totalSizeX, vertices[i].z / totalSizeY);
            }

            //默认着色器的材质球
            var material = new Material(Shader.Find("Standard"));

            creater = new MeshCreater(vertices, triangles, uv, material);

        }

        [Test, Order(1)]
        public void Test1()
        {
            var gameObject = creater.CreatMeshGameObject();
            Assert.IsNotNull(gameObject);
        }

        [Test, Order(2)]
        public void Test2()
        {
            creater.Dispose();
            creater = null;
            Assert.IsNull(creater);
        }

        // TearDown : some action after every test
        [TearDown]
        public void TestTerDown()
        {
            Assert.IsTrue(true);
        }

        [UnityTest]
        public IEnumerator TestRuning()
        {
            yield return null;
            //static void action()
            //{
            //    Debug.Log("Runing1");
            //}
            //var ie = new IncrementalIEnumerator(KeyCode.S, KeyCode.P, new WaitForFixedUpdate(), action);
            //yield return ie.Execute();
            //yield return ie.Execute();
        }
    }
}
