using UnityEngine;
using System.Collections;

public class Comment3D : MonoBehaviour
{

	[TextArea]
	public string text;

	public Vector3 offset;

	private TextMesh tMesh;

	void Update()
	{
		if (tMesh == null)
		{
			tMesh = new GameObject("Comment3D of " + this.name).AddComponent<TextMesh>();
			tMesh.transform.SetParent(this.transform, false);
		}
		tMesh.text = text;
		tMesh.alignment = TextAlignment.Center;
		tMesh.anchor = TextAnchor.MiddleCenter;
		tMesh.characterSize = .1f;
		tMesh.fontSize = 50;
		tMesh.transform.localPosition = offset;
		if (Camera.main != null)
		{
			tMesh.transform.LookAt(Camera.main.transform);
			tMesh.transform.Rotate(0, 180, 0);
		}
	}

}

public static class CommentUtils
{
	public static void Print3D(this MonoBehaviour x, object txt)
	{
		x.Print3D(txt, Vector3.zero);
	}

	public static void Print3D(this MonoBehaviour x, object txt, Vector3 offset)
	{
		Comment3D c = x.GetComponent<Comment3D>();
		if (c == null)
			c = x.gameObject.AddComponent<Comment3D>();
		if (txt == null)
			txt = "Null";
		c.text = txt.ToString();
		c.offset = offset;

	}
}