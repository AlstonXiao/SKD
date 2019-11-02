using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MeshDisplayer : MonoBehaviour
{
	public Text output1;
	public Text output2;

	[Header("Drag mesh to inspect here:")]
	public Mesh mesh;

	void Update ()
	{
		string s = "Mesh name:" + mesh.name + "\n\n";

		s += System.String.Format( "Vertices: {0}\n", mesh.vertexCount);

		for (int i = 0; i < mesh.vertexCount; i++)
		{
			s += System.String.Format ( "{0}. {1}\n", i, mesh.vertices[i].ToString ());
		}

		output1.text = s;

		s = "twitter:@kurtdekker\n\n";
		s += System.String.Format ("{0} Triangles:\n", mesh.triangles.Length / 3);
		
		for (int i = 0; i < mesh.triangles.Length; i += 3)
		{
			s += System.String.Format ( "{0}, {1}, {2}\n",
			                           mesh.triangles[i],
			                           mesh.triangles[i + 1],
			                           mesh.triangles[i + 2]);
		}
	
		output2.text = s;
	}
}
