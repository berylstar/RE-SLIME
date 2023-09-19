using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(GameController))]
public class TestButton : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        GameController generator = (GameController)target;
        if (GUILayout.Button("COIN +"))
        {
            generator.TEST_COINUP();
        }
        if (GUILayout.Button("HP +"))
        {
            generator.TEST_FULLHP();
        }
        if (GUILayout.Button("HP -"))
        {
            generator.TEST_LOWHP();
        }
        if (GUILayout.Button("LIFE +"))
        {
            generator.TEST_LIFEUP();
        }
    }
}
