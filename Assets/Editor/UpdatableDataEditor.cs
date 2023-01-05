using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof(UpdatableData), true)]
public class UpdatableDataEditor : Editor {

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();

		UpdatableData data = (UpdatableData)target;

		// allows for real time updates of the mesh plane will in the editor
		if (GUILayout.Button ("Update")) {
			data.NotifyOfUpdatedValues ();
			EditorUtility.SetDirty (target);
		}
	}
	
}
