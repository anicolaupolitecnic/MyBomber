using System.Collections;
using System.Diagnostics.Tracing;
using UnityEngine;

public class Explode: MonoBehaviour {
	public LayerMask playerLayer;
	public GameObject miniBlock;

	public int cubesPerAxis = 8;
    public float delay = 1f;
	public float force = 300f;
	public float radius = 2f;
	public float lifeTime = 3f;
	private Material mat;
	private Transform transf;

	public void DestroyCube() {
		gameObject.GetComponent<Collider>().enabled = false;
		mat = gameObject.GetComponent<Renderer>().material;
		transf = gameObject.transform;

		gameObject.GetComponent<Renderer>().enabled = false;
		for (int x = 0; x < cubesPerAxis; x++) {
			for (int y = 0; y< cubesPerAxis; y++) {
				for (int z = 0; z < cubesPerAxis; z++) {
					CreateCube (new Vector3(x, y, z));
				}
			}
		}
		StartCoroutine(DestroyCubeAfterTime(gameObject, lifeTime + 0.1f));
	}

	private IEnumerator DestroyCubeAfterTime(GameObject cube, float delay) {
        yield return new WaitForSeconds(delay);
        Destroy(cube);
    }
	
	void CreateCube (Vector3 coordinates) {
		GameObject cube = GameObject.CreatePrimitive(PrimitiveType.Cube);
		cube.GetComponent<Collider>().enabled = false;
		Renderer rd = cube.GetComponent<Renderer>();
		rd.material = mat;//GetComponent<Renderer>().material;
		cube.transform.localScale = transf.localScale / cubesPerAxis;
		
		Vector3 firstCube = transf.position - transf.localScale/ 2 + cube.transform. localScale / 2;
		cube.transform.position = firstCube + Vector3.Scale(coordinates, cube.transform.localScale);
		
		Rigidbody rb = cube.AddComponent<Rigidbody>();
		rb.AddExplosionForce(force, this.gameObject.transform.position, radius);

		StartCoroutine(DestroyCubeAfterTime(cube, lifeTime));
	}
}