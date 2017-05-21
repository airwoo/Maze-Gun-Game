using UnityEngine;
using System.Collections;
using UnityEditor;

[CustomEditor (typeof(MapGenerator))]		//we must say which class or which script this is an editor for
public class MapEditor : Editor {				

	public override void OnInspectorGUI()
	{
		base.OnInspectorGUI ();			//call base method to draw all the default stuff

		MapGenerator map = target as MapGenerator;							//reference to our mapGenerator script   //target is the object that the MapEditor is editor of	//cast as MapGenerator script

		map.GenerateMap ();			//each frame we can say map.GeneratorMap
	}
}
