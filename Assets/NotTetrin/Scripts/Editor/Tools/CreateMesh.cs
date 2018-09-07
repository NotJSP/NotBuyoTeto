using System.Linq;
using UnityEngine;
using UnityEditor;
using System.Collections;

public class CreateMesh : MonoBehaviour {
    public class MinoI {
        private static Vector3[] vertices = new Vector3[] {
            new Vector3(-2.0f, -0.5f, 0.0f),
            new Vector3(-2.0f, 0.5f, 0.0f),
            new Vector3(2.0f, -0.5f, 0.0f),
            new Vector3(2.0f, 0.5f, 0.0f)
        };

        private static int[] triangles = new int[] {
            0, 1, 2,
            2, 1, 3
        };

        private static Vector2[] uv = new Vector2[] {
            new Vector2(0.0f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 0.0f),
            new Vector2(1.0f, 1.0f)
        };

        [MenuItem(@"Tools/Create Mesh/Mino I")]
        public static void Create() => CreateMesh.Create(@"Mino_I", vertices, triangles, uv);
    }

    public class MinoJ {
        private static Vector3[] vertices = new Vector3[] {
            new Vector3(1.5f, -1.0f, 0.0f),
            new Vector3(0.5f, 0.0f, 0.0f),
            new Vector3(0.5f, -1.0f, 0.0f),
            new Vector3(1.5f, 1.0f, 0.0f),
            new Vector3(-1.5f, 1.0f, 0.0f),
            new Vector3(-1.5f, 0.0f, 0.0f)
        };

        private static int[] triangles = new int[] {
            0, 2, 1,
            0, 1, 3,
            3, 1, 4,
            1, 5, 4
        };

        private static Vector2[] uv = new Vector2[] {
            new Vector2(1.0f, 0.0f),
            new Vector2(0.6667f, 0.5f),
            new Vector2(0.6667f, 0.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(0.0f, 0.5f)
        };

        [MenuItem(@"Tools/Create Mesh/Mino J")]
        public static void Create() => CreateMesh.Create(@"Mino_J", vertices, triangles, uv);
    }

    public class MinoL {
        private static Vector3[] vertices = new Vector3[] {
            new Vector3(-1.5f, -1.0f, 0.0f),
            new Vector3(-0.5f, 0.0f, 0.0f),
            new Vector3(-0.5f, -1.0f, 0.0f),
            new Vector3(-1.5f, 1.0f, 0.0f),
            new Vector3(1.5f, 1.0f, 0.0f),
            new Vector3(1.5f, 0.0f, 0.0f)
        };

        private static int[] triangles = new int[] {
            0, 1, 2,
            0, 3, 1,
            3, 4, 1,
            1, 4, 5
        };

        private static Vector2[] uv = new Vector2[] {
            new Vector2(0.0f, 0.0f),
            new Vector2(0.3333f, 0.5f),
            new Vector2(0.3333f, 0.0f),
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(1.0f, 0.5f)
        };

        [MenuItem(@"Tools/Create Mesh/Mino L")]
        public static void Create() => CreateMesh.Create(@"Mino_L", vertices, triangles, uv);
    }

    public class MinoO {
        private static Vector3[] vertices = new Vector3[] {
            new Vector3(-1.0f, 1.0f, 0.0f),
            new Vector3(1.0f, 1.0f, 0.0f),
            new Vector3(-1.0f, -1.0f, 0.0f),
            new Vector3(1.0f, -1.0f, 0.0f)
        };

        private static int[] triangles = new int[] {
            0, 1, 2,
            2, 1, 3
        };

        private static Vector2[] uv = new Vector2[] {
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 1.0f),
            new Vector2(0.0f, 0.0f),
            new Vector2(1.0f, 0.0f)
        };

        [MenuItem(@"Tools/Create Mesh/Mino O")]
        public static void Create() => CreateMesh.Create(@"Mino_O", vertices, triangles, uv);
    }

    public class MinoS {
        private static Vector3[] vertices = new Vector3[] {
            new Vector3(-1.5f, -1.0f, 0.0f),
            new Vector3(-1.5f, 0.0f, 0.0f),
            new Vector3(0.5f, -1.0f, 0.0f),
            new Vector3(0.5f, 0.0f, 0.0f),
            new Vector3(-0.5f, 0.0f, 0.0f),
            new Vector3(-0.5f, 1.0f, 0.0f),
            new Vector3(1.5f, 0.0f, 0.0f),
            new Vector3(1.5f, 1.0f, 0.0f)
        };

        private static int[] triangles = new int[] {
            0, 1, 2,
            2, 1, 3,
            4, 5, 6,
            6, 5, 7
        };

        private static Vector2[] uv = new Vector2[] {
            new Vector2(0.0f, 0.0f),
            new Vector2(0.0f, 0.5f),
            new Vector2(0.6667f, 0.0f),
            new Vector2(0.6667f, 0.5f),
            new Vector2(0.3333f, 0.5f),
            new Vector2(0.3333f, 1.0f),
            new Vector2(1.0f, 0.5f),
            new Vector2(1.0f, 1.0f)
        };

        [MenuItem(@"Tools/Create Mesh/Mino S")]
        public static void Create() => CreateMesh.Create(@"Mino_S", vertices, triangles, uv);
    }

    public class MinoT {
        private static Vector3[] vertices = new Vector3[] {
            new Vector3(-0.5f, -1.0f, 0.0f),
            new Vector3(-0.5f, 0.0f, 0.0f),
            new Vector3(0.5f, -1.0f, 0.0f),
            new Vector3(0.5f, 0.0f, 0.0f),
            new Vector3(-1.5f, 0.0f, 0.0f),
            new Vector3(-1.5f, 1.0f, 0.0f),
            new Vector3(1.5f, 0.0f, 0.0f),
            new Vector3(1.5f, 1.0f, 0.0f)
        };

        private static int[] triangles = new int[] {
            0, 1, 2,
            2, 1, 3,
            4, 5, 6,
            6, 5, 7
        };

        private static Vector2[] uv = new Vector2[] {
            new Vector2(0.3333f, 0.0f),
            new Vector2(0.3333f, 0.5f),
            new Vector2(0.6667f, 0.0f),
            new Vector2(0.6667f, 0.5f),
            new Vector2(0.0f, 0.5f),
            new Vector2(0.0f, 1.0f),
            new Vector2(1.0f, 0.5f),
            new Vector2(1.0f, 1.0f)
        };

        [MenuItem(@"Tools/Create Mesh/Mino T")]
        public static void Create() => CreateMesh.Create(@"Mino_T", vertices, triangles, uv);
    }

    public class MinoZ {
        private static Vector3[] vertices = new Vector3[] {
            new Vector3(1.5f, -1.0f, 0.0f),
            new Vector3(1.5f, 0.0f, 0.0f),
            new Vector3(-0.5f, -1.0f, 0.0f),
            new Vector3(-0.5f, 0.0f, 0.0f),
            new Vector3(0.5f, 0.0f, 0.0f),
            new Vector3(0.5f, 1.0f, 0.0f),
            new Vector3(-1.5f, 0.0f, 0.0f),
            new Vector3(-1.5f, 1.0f, 0.0f)
        };

        private static int[] triangles = new int[] {
            0, 2, 1,
            2, 3, 1,
            4, 6, 5,
            6, 7, 5
        };

        private static Vector2[] uv = new Vector2[] {
            new Vector2(1.0f, 0.0f),
            new Vector2(1.0f, 0.5f),
            new Vector2(0.3333f, 0.0f),
            new Vector2(0.3333f, 0.5f),
            new Vector2(0.6667f, 0.5f),
            new Vector2(0.6667f, 1.0f),
            new Vector2(0.0f, 0.5f),
            new Vector2(0.0f, 1.0f)
        };

        [MenuItem(@"Tools/Create Mesh/Mino Z")]
        public static void Create() => CreateMesh.Create(@"Mino_Z", vertices, triangles, uv);
    }

    private static string RootPath = @"Assets/NotTetrin";

    public static void Create(string name, Vector3[] vertices, int[] triangles, Vector2[] uv) {
        // 選択オブジェクト
        var selectObj = Selection.activeGameObject;

        // テクスチャ取得
        var texture = selectObj?.GetComponent<SpriteRenderer>()?.sprite.texture;        
        if (texture == null) {
            EditorUtility.DisplayDialog(@"エラー", @"SpriteRendererの設定されているゲームオブジェクトを選択してくだしあ。", @"OK");
            return;
        }

        // マテリアルの作成
        var material = new Material(Shader.Find(@"Unlit/Texture"));
        material.mainTexture = texture;
        var materialPath = $"{RootPath}/Materials/{name}.mat";
        AssetDatabase.CreateAsset(material, materialPath);

        // メッシュの作成
        var mesh = new Mesh {
            name = name,
            vertices = vertices,
            triangles = triangles,
            uv = uv
        };
        mesh.RecalculateNormals();
        mesh.RecalculateBounds();
        var meshPath = $"{RootPath}/Meshes/{name}.asset";
        AssetDatabase.CreateAsset(mesh, meshPath);

        // セーブ
        AssetDatabase.SaveAssets();

        // ゲームオブジェクトの作成
        var obj = new GameObject(name);
        obj.AddComponent<MeshFilter>().sharedMesh = mesh;
        obj.AddComponent<MeshRenderer>().materials = new Material[] { material };
        var collider = obj.AddComponent<PolygonCollider2D>();
        var points = new Vector2[vertices.Length];
        for (int i = 0; i < vertices.Length; i++) {
            points[i] = new Vector2(vertices[i].x, vertices[i].y);
        }
        collider.SetPath(0, points);
    }
}