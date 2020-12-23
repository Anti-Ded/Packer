using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class My
{
	//Видны ли все части и подчасти таргета из камеры
	public static bool IsVisible(GameObject target)
	{
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(Camera.main);
		Bounds rendererBounds;
		if (null != target.GetComponent<Renderer>())
		{
			rendererBounds = target.GetComponent<Renderer>().bounds;
		}
		else
		{
			rendererBounds = new Bounds(target.transform.position, Vector3.zero);
			Component[] meshes = target.GetComponentsInChildren<MeshFilter>();
			foreach (MeshFilter mesh in meshes)
			{
				rendererBounds.Encapsulate(mesh.GetComponent<Renderer>().bounds);
			}
		}
		if (GeometryUtility.TestPlanesAABB(planes, rendererBounds))
			return true;
		else
			return false;
	}
	public static bool IsVisible(GameObject target, Camera camera)
	{
		Plane[] planes = GeometryUtility.CalculateFrustumPlanes(camera);
		Bounds rendererBounds;
		if (null != target.GetComponent<Renderer>())
		{
			rendererBounds = target.GetComponent<Renderer>().bounds;
		}
		else
		{
			rendererBounds = new Bounds(target.transform.position, Vector3.zero);
			Component[] meshes = target.GetComponentsInChildren<MeshFilter>();
			foreach (MeshFilter mesh in meshes)
			{
				rendererBounds.Encapsulate(mesh.GetComponent<Renderer>().bounds);
			}
		}
		if (GeometryUtility.TestPlanesAABB(planes, rendererBounds))
			return true;
		else
			return false;
	}

	//Плавный переход из одного в другое. Плавный старт и плавный финиш
	public static float Interpolate(float Target, float Start, float End, float TimeTo, bool Smooth = true)
		{
			float range = End - Start;

			if (range > 0 && (Target > Start + range || Target < End - range))
				Target = Start + Time.deltaTime * range / (100 * TimeTo);
			if (range < 0 && (Target < Start + range || Target > End - range))
				Target = Start + Time.deltaTime * range / (100 * TimeTo);

			float Percent = (Target - Start) / range;

			if (Target != End)
			{
				if (Smooth)
					Target += range * Time.deltaTime * Mathf.Min(2f, Mathf.Max(0.27f, Percent * 8f), Mathf.Max(0.27f, (1f - Percent) * 8f)) / TimeTo;
				else
					Target += range * Time.deltaTime / TimeTo;
			}
			if (range > 0 && Target > End)
			{
				Target = End;
				//Percent = 1;
			}
			if (range < 0 && Target < End)
			{
				Target = End;
				//Percent = 1;
			}
			return Target;
		}
	public static float Interpolate(float Target, float Start, float End, float TimeTo, out float Percent, bool Smooth = true)
	{
		float range = End - Start;
		if (range > 0 && (Target > Start + range || Target < End - range))
			Target = Start + Time.deltaTime * range / (100 * TimeTo);
		if (range < 0 && (Target < Start + range || Target > End - range))
			Target = Start + Time.deltaTime * range / (100 * TimeTo);

		Percent = (Target - Start) / range;

		if (Target != End)
		{
			if (Smooth)
				Target += range * Time.deltaTime * Mathf.Min(2f, Mathf.Max(0.27f, Percent * 8f), Mathf.Max(0.27f, (1f - Percent) * 8f)) / TimeTo;
			else
				Target += range * Time.deltaTime / TimeTo;
		}
		if (range > 0 && Target > End)
		{
			Target = End;
			Percent = 1;
		}
		if (range < 0 && Target < End)
		{
			Target = End;
			Percent = 1;
		}
		return Target;
	}

		
	//Ближайший к таргету из списка
	public static Transform Closest(Transform Target, List<Transform> List)
	{
		int index = 0;
		float min = Mathf.Infinity;
		for (int i = 0; i < List.Count; i++)
		{
			float dist = Vector3.Distance(List[i].position, Target.position);
			if (dist < min)
			{
				index = i;
				min = dist;
			}
		}
		return List[index];
	}
	public static GameObject Closest(GameObject Target, List<GameObject> List)
	{
		int index = 0;
		float min = Mathf.Infinity;
		for (int i = 0; i < List.Count; i++)
		{
			float dist = Vector3.Distance(List[i].transform.position, Target.transform.position);
			if (dist < min)
			{
				index = i;
				min = dist;
			}
		}
		return List[index];
	}
	public static Transform Closest(Transform Target, List<Transform> List, out int index)
	{
		index = 0;
		float min = Mathf.Infinity;
		for (int i = 0; i < List.Count; i++)
		{
			float dist = Vector3.Distance(List[i].position, Target.position);
			if (dist < min)
			{
				index = i;
				min = dist;
			}
		}
		return List[index];
	}
	
	public static GameObject Closest(GameObject Target, List<GameObject> List, out int index)
	{
		index = 0;
		float min = Mathf.Infinity;
		for (int i = 0; i < List.Count; i++)
		{
			float dist = Vector3.Distance(List[i].transform.position, Target.transform.position);
			if (dist < min)
			{
				index = i;
				min = dist;
			}
		}
		return List[index];
	}
	
	public static GameObject Closest(this List<GameObject> List, GameObject Target)
	{
		int index = 0;
		float min = Mathf.Infinity;
		for (int i = 0; i < List.Count; i++)
		{
			float dist = Vector3.Distance(List[i].transform.position, Target.transform.position);
			if (dist < min)
			{
				index = i;
				min = dist;
			}
		}
		return List[index];
	}
	public static Transform Closest(this List<Transform> List, Transform Target)
	{
		int index = 0;
		float min = Mathf.Infinity;
		for (int i = 0; i < List.Count; i++)
		{
			float dist = Vector3.Distance(List[i].position, Target.position);
			if (dist < min)
			{
				index = i;
				min = dist;
			}
		}
		return List[index];
	}
	public static GameObject Closest(this List<GameObject> List, GameObject Target, out int index)
	{
		index = 0;
		float min = Mathf.Infinity;
		for (int i = 0; i < List.Count; i++)
		{
			float dist = Vector3.Distance(List[i].transform.position, Target.transform.position);
			if (dist < min)
			{
				index = i;
				min = dist;
			}
		}
		return List[index];
	}
	public static Transform Closest(this List<Transform> List, Transform Target, out int index)
	{
		index = 0;
		float min = Mathf.Infinity;
		for (int i = 0; i < List.Count; i++)
		{
			float dist = Vector3.Distance(List[i].position, Target.position);
			if (dist < min)
			{
				index = i;
				min = dist;
			}
		}
		return List[index];
	}

	//Перемешать список
	public static List<GameObject> Shuffle(List<GameObject> List)
        {
			for (int i = List.Count - 1; i >= 1; i--)
			{
				int j = Random.Range(0,i + 1);
				var temp = List[j];
				List[j] = List[i];
				List[i] = temp;
			}
			return List;
        }
	public static List<int> Shuffle(List<int> List)
	{
		for (int i = List.Count - 1; i >= 1; i--)
		{
			int j = Random.Range(0, i + 1);
			var temp = List[j];
			List[j] = List[i];
			List[i] = temp;
		}
		return List;
	}
	public static List<float> Shuffle(List<float> List)
	{
		for (int i = List.Count - 1; i >= 1; i--)
		{
			int j = Random.Range(0, i + 1);
			var temp = List[j];
			List[j] = List[i];
			List[i] = temp;
		}
		return List;
	}
	public static List<Transform> Shuffle(List<Transform> List)
	{
		for (int i = List.Count - 1; i >= 1; i--)
		{
			int j = Random.Range(0, i + 1);
			var temp = List[j];
			List[j] = List[i];
			List[i] = temp;
		}
		return List;
	}

	//Возвращает нормаль к террайну
	public static Vector3 SampleNormal(Vector3 position)
   		{
        	 Terrain terrain = Terrain.activeTerrain;
	         var terrainLocalPos = position - terrain.transform.position;
    	     var normalizedPos = new Vector2(
        	     Mathf.InverseLerp(0f, terrain.terrainData.size.x, terrainLocalPos.x),
            	 Mathf.InverseLerp(0f, terrain.terrainData.size.z, terrainLocalPos.z)
	         );
    	     var terrainNormal = terrain.terrainData.GetInterpolatedNormal(normalizedPos.x, normalizedPos.y);
        	 return terrainNormal;
     	}
	public static Vector3 SampleNormal(Vector3 position, Terrain terrain)
	{
		var terrainLocalPos = position - terrain.transform.position;
		var normalizedPos = new Vector2(
			Mathf.InverseLerp(0f, terrain.terrainData.size.x, terrainLocalPos.x),
			Mathf.InverseLerp(0f, terrain.terrainData.size.z, terrainLocalPos.z)
		);
		var terrainNormal = terrain.terrainData.GetInterpolatedNormal(normalizedPos.x, normalizedPos.y);
		return terrainNormal;
	}

}