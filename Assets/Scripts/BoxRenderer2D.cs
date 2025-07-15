using UnityEngine;

public class BoxRenderer2D : MonoBehaviour {

    public Vector2 boxSize = new Vector2(5, 3);
    public GameObject linePrefab;
	public Color tint = Color.white;

    private LineRenderer[] lines = new LineRenderer[4];

    void Awake() {
        if (linePrefab == null) {
            Debug.LogError("Line Renderer Prefab is not assigned.");
            return;
        }
        CreateBoxFromPrefab();
    }

    void OnValidate() {
        UpdateBox();
    }

    private void CreateBoxFromPrefab() {
        // Destroy any old lines if they exist
        foreach (Transform child in transform) {
            DestroyImmediate(child.gameObject);
        }

        for (int i = 0; i < 4; i++) {
            GameObject instance = Instantiate(linePrefab, transform);
            LineRenderer lr = instance.GetComponent<LineRenderer>();
			lr.transform.parent = transform;

            if (lr == null) {
				Debug.LogError("Prefab does not have a LineRenderer component.");
				return;
			}
            lr.useWorldSpace = false;
			lr.startColor = tint;
			lr.endColor = tint;
            lines[i] = lr;
        }
        UpdateBox();
    }

    private void UpdateBox() {
		if (lines[0] == null) {
			return;
		}
        Vector2 halfSize = boxSize * 0.5f;
        Vector3 topLeft     = new Vector3(-halfSize.x,  halfSize.y, 0);
        Vector3 topRight    = new Vector3( halfSize.x,  halfSize.y, 0);
        Vector3 bottomRight = new Vector3( halfSize.x, -halfSize.y, 0);
        Vector3 bottomLeft  = new Vector3(-halfSize.x, -halfSize.y, 0);

        Vector3[][] positions = new Vector3[][] {
            new Vector3[] { topLeft,     topRight },    // Top
            new Vector3[] { topRight,    bottomRight }, // Right
            new Vector3[] { bottomRight, bottomLeft },  // Bottom
            new Vector3[] { bottomLeft,  topLeft }      // Left
        };

        for (int i = 0; i < 4; i++) {
            var lr = lines[i];
            lr.positionCount = 2;
            lr.SetPositions(positions[i]);
        }
    }
	
	#if UNITY_EDITOR
	void OnDrawGizmos() {
		Gizmos.color = Color.yellow;
		Vector2 halfSize = boxSize * 0.5f;

		Vector3 topLeft = transform.position + new Vector3(-halfSize.x,  halfSize.y, 0);
		Vector3 topRight = transform.position + new Vector3( halfSize.x,  halfSize.y, 0);
		Vector3 bottomRight = transform.position + new Vector3( halfSize.x, -halfSize.y, 0);
		Vector3 bottomLeft = transform.position + new Vector3(-halfSize.x, -halfSize.y, 0);

		Gizmos.DrawLine(topLeft, topRight);
		Gizmos.DrawLine(topRight, bottomRight);
		Gizmos.DrawLine(bottomRight, bottomLeft);
		Gizmos.DrawLine(bottomLeft, topLeft);
	}
	#endif
}
