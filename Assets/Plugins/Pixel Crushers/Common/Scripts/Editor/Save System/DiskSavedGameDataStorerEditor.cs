﻿// Copyright (c) Pixel Crushers. All rights reserved.

using UnityEngine;
using UnityEditor;
using UnityEditorInternal;
using System.Collections.Generic;
using System.IO;

namespace PixelCrushers
{

    [CustomEditor(typeof(DiskSavedGameDataStorer), true)]
    public class DiskSavedGameDataStorerEditor : Editor
    {

        private const int MaxSlots = 100;

        private List<string> m_files;
        private ReorderableList m_list;

        protected virtual void OnEnable()
        {
            var storer = target as DiskSavedGameDataStorer;

            // Get active files:
            m_files = new List<string>();
            int slotNum = 0;
            var filename = storer.GetSavedGameInfoFilename();
            if (!string.IsNullOrEmpty(filename) && File.Exists(filename))
            {
                try
                {
                    using (StreamReader streamReader = new StreamReader(filename))
                    {
                        int safeguard = 0;
                        while (!streamReader.EndOfStream && safeguard < 999)
                        {
                            var sceneName = streamReader.ReadLine().Replace("<cr>", "\n");
                            m_files.Add(storer.GetSaveGameFilename(slotNum) + ": " + sceneName);
                            slotNum++;
                            safeguard++;
                        }
                    }
                }
                catch (System.Exception)
                {
                    Debug.Log("Save System: DiskSavedGameDataStorer - Error reading file: " + filename);
                }
            }

            // Setup editor list:
            m_list = new ReorderableList(m_files, typeof(string), false, true, false, false);
            m_list.drawHeaderCallback = OnDrawHeader;
            m_list.drawElementCallback = OnDrawElement;
        }

        public override void OnInspectorGUI()
        {
            base.OnInspectorGUI();
            DrawSavedGameList();
        }

        protected virtual void DrawSavedGameList()
        {
            if (m_list != null) m_list.DoLayoutList();
            if (GUILayout.Button(new GUIContent("Clear Saved Games", "Delete all saved game files.")))
            {
                if (EditorUtility.DisplayDialog("Clear Saved Games", "Delete all saved game files?", "OK", "Cancel"))
                {
                    ClearSavedGames();
                }
            }
        }

        private void OnDrawHeader(Rect rect)
        {
            EditorGUI.LabelField(rect, "Saved Games");
        }

        private void OnDrawElement(Rect rect, int index, bool isActive, bool isFocused)
        {
            if (!(0 <= index && index < m_files.Count)) return;
            var key = m_files[index];
            //int buttonWidth = 48;
            //var keyRect = new Rect(rect.x, rect.y + 1, rect.width - buttonWidth, EditorGUIUtility.singleLineHeight);
            //var showRect = new Rect(rect.x + rect.width - buttonWidth, rect.y + 1, buttonWidth, EditorGUIUtility.singleLineHeight);
            EditorGUI.BeginDisabledGroup(true);
            EditorGUI.TextField(rect, key); //EditorGUI.TextField(keyRect, key);
            EditorGUI.EndDisabledGroup();
            //EditorGUI.BeginDisabledGroup(string.Equals(key, EmptySlotString));
            //if (GUI.Button(showRect, "Show"))
            //{
            //    Debug.Log(key + ": " + PlayerPrefs.GetString(key));
            //}
            //EditorGUI.EndDisabledGroup();
        }

        //private void OnRemoveElement(ReorderableList list)
        //{
        //    if (!(0 <= list.index && list.index < m_files.Count)) return;
        //    var key = m_files[list.index];
        //    if (EditorUtility.DisplayDialog("Delete Saved Game", "Delete saved game " + key + "?", "OK", "Cancel"))
        //    {
        //        PlayerPrefs.DeleteKey(key);
        //        m_files[list.index] = EmptySlotString;
        //    }
        //}


        private void ClearSavedGames()
        {
            var storer = target as DiskSavedGameDataStorer;
            for (int i = m_files.Count - 1; i >= 0; i--)
            {
                storer.DeleteSavedGameData(i);
            }
            m_files.Clear();
            Repaint();
        }

    }

}
