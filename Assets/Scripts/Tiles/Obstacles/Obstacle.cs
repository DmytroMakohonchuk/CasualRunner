//using System;
//using System.Collections;
//using System.Collections.Generic;
//using UnityEditor;
//using UnityEngine;
//using static Obstacle;

//public class Obstacle : MonoBehaviour
//{
//    [SerializeField]
//    private ObstaclesTypes obstacleType;

//    public ObstaclesTypes ObstacleType => obstacleType;

//    // Start is called before the first frame update
//    void Start()
//    {
        
//    }

//    [Flags]
//    public enum ObstaclesTypes
//    {
//        jumpover = 0,
//        bendover = 1,
//        runover = 2
//    }

//    // Update is called once per frame
//    void Update()
//    {
//    }
//}

//[CustomPropertyDrawer(typeof(ObstaclesTypes))]
//public class EnumFlagsAttributeDrawer : PropertyDrawer
//{
//    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
//    {
//        property.intValue = EditorGUI.MaskField(position, label, property.intValue, property.enumNames);
//    }
//}
