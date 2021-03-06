﻿using System.Collections;
using UnityEngine;
using UnityEditor;
using UnityEngine.UI;

namespace MagiCloud.KGUI
{
    [CustomEditor(typeof(KGUI_ButtonObject))]
    [CanEditMultipleObjects]
    public class KGUIButtonObjectEditor : Editor
    {
        private GUIStyle style;

        public SerializedProperty onClick;  //鼠标点击

        public SerializedProperty onEnter;  //鼠标移入
        public SerializedProperty onExit;   //鼠标移出
        public SerializedProperty onDown;   //鼠标按下
        public SerializedProperty onUp;     //鼠标抬起

        public SerializedProperty onDownStay; //按下持续
        public SerializedProperty onUpRange;

        public SerializedProperty onGroupReset;

        public SerializedProperty spriteRenderer;
        public SerializedProperty image;

        public SerializedProperty normalSprite, enterSprite, pressedSprite, disableSprite;
        public SerializedProperty normalObject, enterObject, pressedObject, disableObject;

        public SerializedProperty buttonType;

        public SerializedProperty AudioClip;

        public KGUI_ButtonObject button;

        public SerializedProperty BindObject;
        public SerializedProperty Panel;

        private void OnEnable()
        {

            button = serializedObject.targetObject as KGUI_ButtonObject;

            onClick = serializedObject.FindProperty("onClick");
            onEnter = serializedObject.FindProperty("onEnter");
            onExit = serializedObject.FindProperty("onExit");
            onDown = serializedObject.FindProperty("onDown");
            onUp = serializedObject.FindProperty("onUp");
            onDownStay = serializedObject.FindProperty("onDownStay");
            onUpRange = serializedObject.FindProperty("onUpRange");

            onGroupReset = serializedObject.FindProperty("onGroupReset");

            buttonType = serializedObject.FindProperty("buttonType");

            spriteRenderer = serializedObject.FindProperty("spriteRenderer");
            image = serializedObject.FindProperty("image");

            normalSprite = serializedObject.FindProperty("normalSprite");
            enterSprite = serializedObject.FindProperty("enterSprite");
            pressedSprite = serializedObject.FindProperty("pressedSprite");
            disableSprite = serializedObject.FindProperty("disableSprite");

            normalObject = serializedObject.FindProperty("normalObject");
            enterObject = serializedObject.FindProperty("enterObject");
            pressedObject = serializedObject.FindProperty("pressedObject");
            disableObject = serializedObject.FindProperty("disableObject");

            AudioClip = serializedObject.FindProperty("audioClip");

            BindObject = serializedObject.FindProperty("bindObject");
            Panel = serializedObject.FindProperty("panel");
        }

        public override void OnInspectorGUI()
        {
            if (style == null)
            {
                style = new GUIStyle(GUI.skin.name);
                style.normal.textColor = GUI.skin.label.normal.textColor;
                style.fontStyle = FontStyle.Bold;
                style.alignment = TextAnchor.UpperLeft;
            }

            EditorGUILayout.BeginVertical(GUILayout.Width(500));

            GUILayout.Space(10);
            EditorGUILayout.LabelField("常用属性", style);

            button.buttonType = (ButtonType)EditorGUILayout.EnumPopup("Button类型：", button.buttonType);

            switch (button.buttonType)
            {
                case ButtonType.Image:

                    EditorGUI.BeginChangeCheck();

                    EditorGUILayout.PropertyField(image, true, null);
                    EditorGUILayout.PropertyField(normalSprite, true, null);
                    EditorGUILayout.PropertyField(enterSprite, true, null);

                    if (button.pressedSprite == null)
                    {
                        button.pressedSprite = button.enterSprite;
                    }

                    EditorGUILayout.PropertyField(pressedSprite, true, null);

                    if (button.disableSprite == null)
                    {
                        button.disableSprite = button.normalSprite;
                    }

                    EditorGUILayout.PropertyField(disableSprite, true, null);

                    break;
                case ButtonType.Object:
                    EditorGUI.BeginChangeCheck();

                    EditorGUILayout.PropertyField(normalObject, true, null);
                    EditorGUILayout.PropertyField(enterObject, true, null);

                    if (button.pressedObject == null)
                    {
                        button.pressedObject = button.enterObject;
                    }

                    EditorGUILayout.PropertyField(pressedObject, true, null);

                    if (button.disableObject == null)
                    {
                        button.disableObject = button.normalObject;
                    }

                    EditorGUILayout.PropertyField(disableObject, true, null);

                    break;
                case ButtonType.SpriteRenderer:
                    EditorGUI.BeginChangeCheck();

                    EditorGUILayout.PropertyField(spriteRenderer, true, null);

                    EditorGUILayout.PropertyField(normalSprite, true, null);
                    EditorGUILayout.PropertyField(enterSprite, true, null);

                    if (button.pressedSprite == null)
                    {
                        button.pressedSprite = button.enterSprite;
                    }

                    EditorGUILayout.PropertyField(pressedSprite, true, null);

                    if (button.disableSprite == null)
                    {
                        button.disableSprite = button.normalSprite;
                    }

                    EditorGUILayout.PropertyField(disableSprite, true, null);
                    break;
                default:
                    EditorGUI.BeginChangeCheck();
                    break;
            }

            GUILayout.Space(10);

            button.IsEnable = EditorGUILayout.Toggle("是否启用(IsEnable)", button.IsEnable);

            button.IsStartAudio = EditorGUILayout.Toggle("启动音频：", button.IsStartAudio);

            if (button.IsStartAudio)
            {
                EditorGUILayout.PropertyField(AudioClip, true, null);
                button.AddAudio();
            }
            else
            {
                button.DestroyAudio();
            }

            GUILayout.Space(20);

            button.zValue = EditorGUILayout.FloatField("相对摄像机Z轴值：", button.zValue);
            button.maxCount = EditorGUILayout.IntField("最大数：", button.maxCount);
            EditorGUILayout.PropertyField(BindObject, true, null);
            EditorGUILayout.PropertyField(Panel, true, null);

            EditorGUILayout.LabelField("Button事件", style);
            EditorGUILayout.PropertyField(onClick, true, null);

            EditorGUILayout.PropertyField(onEnter, true, null);
            EditorGUILayout.PropertyField(onExit, true, null);

            EditorGUILayout.PropertyField(onDown, true, null);
            EditorGUILayout.PropertyField(onUp, true, null);

            EditorGUILayout.PropertyField(onDownStay, true, null);
            EditorGUILayout.PropertyField(onUpRange, true, null);

            EditorGUILayout.EndVertical();

            serializedObject.ApplyModifiedProperties();
        }
    }
}
