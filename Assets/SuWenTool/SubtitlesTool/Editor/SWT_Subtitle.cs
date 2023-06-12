/*
 * ///////////////////////////////////////////
 * ///Copyright© SuWen All Rights Reserved.///
 * ///////////////////////////////////////////
 */

using System;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEditor;
using UnityEditorInternal;

using static SuWenTools.Common;
using static SuWenTools.Common.Languge;
using static SuWenTools.Common.SubtitleColor;
using static SuWenTools.EditorCoroutineRunner.EditorCoroutines;
using Progress = SuWenTools.Common.Progress;

namespace SuWenTools
{
	public class SWT_Subtitle : EditorWindow
	{
		readonly static string toolVersion = "1.00";

		public static SWT_Subtitle window;
		public static bool subtitlePlayerIsOpen = false;

		public static ReorderableList reorderableList;
		public static List<SubtitleData> subtitleDataList = new List<SubtitleData>();

		public static GUIStyle styleTittle;
		static GUIStyle styleSubtitle;
		
		[Serializable]
		public class SubtitleData
		{
			[SerializeField] internal String _strings;
			[SerializeField] internal float _nextTime;
			[SerializeField] internal Color _colors;
		}

		[Serializable]
		public class SubtitleDataToJson
		{
			[SerializeField] internal List<String> _strings = new List<string>();
			[SerializeField] internal List<float> _nextTime = new List<float>();
			[SerializeField] internal List<Color> _colors = new List<Color>();

			public string ToJson(List<SubtitleData> subtitleDataList)
			{
				if (subtitleDataList == null)
					return null;
				
				// SyncData
				_strings.Clear();
				_nextTime.Clear();
				_colors.Clear();
				foreach (var data in subtitleDataList)
				{
					_strings.Add(data._strings);
					_nextTime.Add(data._nextTime);
					_colors.Add(data._colors);
				}
				return JsonUtility.ToJson(this, true);
			}
		}

		public static LangugeSelect languageSelect;
		[MenuItem("Window/SuWen Tool/Subtitles Tool")]
		public static void ShowWindow()
		{
			var hasLanguageKey = EditorPrefs.HasKey("Subtitle_Language");
			var langugePrefs = hasLanguageKey ? EditorPrefs.GetString("Subtitle_Language") : "Eng";
			languageSelect = (LangugeSelect)Enum.Parse(typeof(LangugeSelect), langugePrefs);
			if (hasLanguageKey)
				LangugeChoice(languageSelect);

            if (WINDOW_EDITOR_TITTLE == null)
				LangugeChoice(LangugeSelect.Eng);

			window = GetWindow<SWT_Subtitle>();
			window.titleContent = new GUIContent(WINDOW_EDITOR_TITTLE);
			window.minSize = new Vector2(350, 350);
			window.maxSize = new Vector2(2000, 2000);

			if (subtitleDataList != null)
			{
				reorderableList = new ReorderableList(subtitleDataList, subtitleDataList.GetType());
				Protector.LoadSubtitleData(true);
			}

            if (!EditorPrefs.HasKey("Subtitle_DisplaySubtitleMode_All") ||
                !EditorPrefs.HasKey("Subtitle_DisplaySubtitleMode_Player") ||
                !EditorPrefs.HasKey("Subtitle_DisplaySubtitleMode_UGUI"))
            {
                DisplaySubtitleMode.all = false;
                DisplaySubtitleMode.player = true;
                DisplaySubtitleMode.ugui = false;
            }
        }

        private void OnEnable()
        {
			InGameGUITextFactory.Clear();
		}

		private void OnDisable()
		{
			if (SubtitlePlayer.window != null && subtitlePlayerIsOpen && !EditorApplication.isCompiling)
				SubtitlePlayer.window?.Close();

			InGameGUITextFactory.Clear();
		}

		void OnGUI()
		{
			if (styleTittle == null)
			{
				styleTittle = new GUIStyle();
				styleTittle.fontSize = 20;
			}

			if (styleSubtitle == null)
			{
				styleSubtitle = new GUIStyle(GUI.skin.textField);
				styleTittle.fontSize = 15;
			}

			EditorGUI.BeginDisabledGroup(SubtitlePlayer.window && SubtitlePlayer.autoPlay);
			
			GUIDraw.ToolBar();
			GUILayout.Space(5);

			GUIDraw.SubtitleEditor();
			GUIDraw.GUIReOrderableList();
			GUILayout.Space(-5);
			GUIDraw.AddButton();
			GUILayout.Space(50);

			EditorGUI.EndDisabledGroup();
			
			GUIDraw.SubtitlePlayerButton();
			GUILayout.Space(5);
			//Repaint();

			Protector.LoadSubtitleData();
		}

		public class GUIDraw
		{
			private static Event e = Event.current;
			public static void ToolBar()
			{
				var bgWidth = (SWT_Subtitle.window == null) ? 0 : SWT_Subtitle.window.position.width;
				var buttonWidth = GUILayout.Width(50);
				GUILayout.BeginHorizontal(EditorStyles.toolbar, GUILayout.Width(bgWidth));
				if (GUILayout.Button(TOOLBAR_FILE, EditorStyles.toolbarButton, buttonWidth))
				{
					var menuFile = new GenericMenu();
					menuFile.AddItem(new GUIContent(TOOLBAR_FILE_NEW), false, () => { ToolBarItemButtonFunc(ToolBarItemButton.FILE_NEW); });
					menuFile.AddSeparator("");
					menuFile.AddItem(new GUIContent(TOOLBAR_FILE_SAVE), false, () => { ToolBarItemButtonFunc(ToolBarItemButton.FILE_SAVE); });
					menuFile.AddItem(new GUIContent(TOOLBAR_FILE_LOAD), false, () => { ToolBarItemButtonFunc(ToolBarItemButton.FILE_LOAD); });
					menuFile.AddSeparator("");
					menuFile.AddItem(new GUIContent(TOOLBAR_FILE_CLOSE), false, () => { ToolBarItemButtonFunc(ToolBarItemButton.FILE_CLOSE); });
					menuFile.ShowAsContext();
				}
				else if (GUILayout.Button(TOOLBAR_DISPLAY, EditorStyles.toolbarButton, buttonWidth))
				{
					var menuDisplay = new GenericMenu();
					menuDisplay.AddItem(new GUIContent(TOOLBAR_DISPLAY_LAN_ENG), stageEn, () => { ToolBarItemButtonFunc(ToolBarItemButton.DISPLAY_LANGUGE_EN); });
					menuDisplay.AddItem(new GUIContent(TOOLBAR_DISPLAY_LAN_CHI), stageCh, () => { ToolBarItemButtonFunc(ToolBarItemButton.DISPLAY_LANGUGE_CH); });
					menuDisplay.AddSeparator("");
                    menuDisplay.AddItem(new GUIContent(TOOLBAR_DISPLAY_MODE_ALL), DisplaySubtitleMode.all, () => { ToolBarItemButtonFunc(ToolBarItemButton.DISPLAY_MODE_ALL); });
                    menuDisplay.AddSeparator(TOOLBAR_DISPLAY_MODE_DIVIDER);
                    menuDisplay.AddItem(new GUIContent(TOOLBAR_DISPLAY_MODE_UGUI), DisplaySubtitleMode.ugui, () => { ToolBarItemButtonFunc(ToolBarItemButton.DISPLAY_MODE_UGUI); });
                    menuDisplay.AddItem(new GUIContent(TOOLBAR_DISPLAY_MODE_PLEYER), DisplaySubtitleMode.player, () => { ToolBarItemButtonFunc(ToolBarItemButton.DISPLAY_MODE_PLAYER); });
                    menuDisplay.AddItem(new GUIContent(TOOLBAR_DISPLAY_ALL_TRUE), DisplayController.allTrue, () => { ToolBarItemButtonFunc(ToolBarItemButton.DISPLAY_ALL_TRUE); });
					menuDisplay.AddItem(new GUIContent(TOOLBAR_DISPLAY_ALL_FALSE), DisplayController.allFalse, () => { ToolBarItemButtonFunc(ToolBarItemButton.DISPLAY_ALL_FALSE); });
					menuDisplay.AddSeparator(TOOLBAR_DISPLAY_ALL_DIVIDER);
					menuDisplay.AddItem(new GUIContent(TOOLBAR_DISPLAY_PLAYER_SUBTITLES), DisplayController.subtitle, () => { ToolBarItemButtonFunc(ToolBarItemButton.DISPLAY_SUBTITLES); });
					menuDisplay.AddItem(new GUIContent(TOOLBAR_DISPLAY_PLAYER_AUTOPLAY), DisplayController.autoPlay, () => { ToolBarItemButtonFunc(ToolBarItemButton.DISPLAY_AUTOPLAY); });
					menuDisplay.AddItem(new GUIContent(TOOLBAR_DISPLAY_PLAYER_TYPEWRITER), DisplayController.typeWriter, () => { ToolBarItemButtonFunc(ToolBarItemButton.DISPLAY_TYPEWRITER); });
					menuDisplay.AddItem(new GUIContent(TOOLBAR_DISPLAY_PLAYER_TIPLGIHT), DisplayController.tipLight, () => { ToolBarItemButtonFunc(ToolBarItemButton.DISPLAY_TIPLIGHT); });
					
					menuDisplay.ShowAsContext();
				}
				else if (GUILayout.Button(TOOLBAR_ABOUT, EditorStyles.toolbarButton, buttonWidth))
				{
					var menuAbout = new GenericMenu();
					menuAbout.AddItem(new GUIContent(TOOLBAR_ABOUT_ABOUT), false, () => { ToolBarItemButtonFunc(ToolBarItemButton.ABOUT_ABOUT); });
					menuAbout.AddSeparator("");
					menuAbout.AddItem(new GUIContent(TOOLBAR_ABOUT_HOTKEY), false, () => { ToolBarItemButtonFunc(ToolBarItemButton.ABOUT_HOTKEY); });
					//menuAbout.AddItem(new GUIContent("Editor Prefs Delete All"), false, () => { EditorPrefs.DeleteAll(); Debug.Log("delete all"); });
					menuAbout.ShowAsContext();
				}
				GUILayout.EndHorizontal();
			}

			enum ToolBarItemButton 
			{ 
				FILE_NEW, FILE_SAVE, FILE_LOAD, FILE_CLOSE,
				DISPLAY_MODE_ALL, DISPLAY_MODE_UGUI, DISPLAY_MODE_PLAYER, 
				DISPLAY_ALL_TRUE, DISPLAY_ALL_FALSE, DISPLAY_SUBTITLES, DISPLAY_AUTOPLAY, DISPLAY_TYPEWRITER, DISPLAY_TIPLIGHT,
				ABOUT_ABOUT, ABOUT_HOTKEY, DISPLAY_LANGUGE_EN, DISPLAY_LANGUGE_CH 
			}
			private static void ToolBarItemButtonFunc(ToolBarItemButton toolBarItemButton)
			{
				switch (toolBarItemButton)
				{
					case ToolBarItemButton.FILE_NEW:
						subtitleDataList = new List<SubtitleData>();
						reorderableList.list = subtitleDataList;
						AddSubtitleData();
						Protector.SaveSubtitleData();
						SubtitlePlayer.progressSubtitleIndex = 0;
						SubtitlePlayer.ReLoadSubitles();
						break;

					case ToolBarItemButton.FILE_SAVE:
						JsonTools.SaveSubtitleToJson(subtitleDataList);
						break;

					case ToolBarItemButton.FILE_LOAD:
						var tempSubtitleDataList = JsonTools.LoadSubtitleFromJson();
						if (tempSubtitleDataList == null)
							return;

						subtitleDataList = tempSubtitleDataList;
						reorderableList.list = tempSubtitleDataList;
						Protector.SaveSubtitleData();
						break;

					case ToolBarItemButton.FILE_CLOSE: 
						window.Close();
						break;

					case ToolBarItemButton.DISPLAY_LANGUGE_EN:
						LangugeChoice(LangugeSelect.Eng);
						break;

					case ToolBarItemButton.DISPLAY_LANGUGE_CH:
						LangugeChoice(LangugeSelect.Chi);
						break;

					case ToolBarItemButton.DISPLAY_MODE_ALL:
						DisplaySubtitleMode.all = !DisplaySubtitleMode.all;
						break;

					case ToolBarItemButton.DISPLAY_MODE_UGUI:
						DisplaySubtitleMode.ugui = !DisplaySubtitleMode.ugui;
						break;

					case ToolBarItemButton.DISPLAY_MODE_PLAYER:
						DisplaySubtitleMode.player = !DisplaySubtitleMode.player;
						break;

					case ToolBarItemButton.DISPLAY_ALL_TRUE:
						DisplayController.allTrue = !DisplayController.allTrue;
						ShowAndFocusSubtitlesPlayer();
						break;

					case ToolBarItemButton.DISPLAY_ALL_FALSE:
						DisplayController.allFalse = !DisplayController.allFalse;
						ShowAndFocusSubtitlesPlayer();
						break;

					case ToolBarItemButton.DISPLAY_SUBTITLES:
						DisplayController.subtitle = !DisplayController.subtitle;
						ShowAndFocusSubtitlesPlayer();
						break;

					case ToolBarItemButton.DISPLAY_AUTOPLAY:
						DisplayController.autoPlay = !DisplayController.autoPlay;
						ShowAndFocusSubtitlesPlayer();
						break;

					case ToolBarItemButton.DISPLAY_TYPEWRITER:
						DisplayController.typeWriter = !DisplayController.typeWriter;
						ShowAndFocusSubtitlesPlayer();
						break;

					case ToolBarItemButton.DISPLAY_TIPLIGHT:
						DisplayController.tipLight = !DisplayController.tipLight;
						ShowAndFocusSubtitlesPlayer();
						break;

					case ToolBarItemButton.ABOUT_ABOUT:
						AboutWindow.Init(toolVersion, ABOUT_WIN_TITTLE, ABOUT_WIN_TEXT, window.position);
						break;

					case ToolBarItemButton.ABOUT_HOTKEY:
						EditorUtility.DisplayDialog("", 
									$"----------------------------\n" +
									$"{HOTKEY_TITTLE}\n" +
									$"----------------------------\n" +
									$"\n" +
									$"0 ~ 9：{HOTKEY_COLOR}\n" +
									$"\n" +
									$"Home：{HOTKEY_FIRST_SUB}\n" +
									$"End   ：{HOTKEY_LAST_SUB}\n" +
									$"\n" +
									$"ArrowUp/PageUp：{HOTKEY_PRE_SUB}\n" +
									$"ArrowDn/PageDn：{HOTKEY_NEXT_SUB}\n" +
									$"\n" +
									$"＋：{HOTKEY_ENL_SUB}\n" +
									$"－：{HOTKEY_SHR_SUB}\n" +
									$"\n" +
									$"Enter：{HOTKEY_END_WRITER_MODE}\n" +
									$"\n" +
									$"Space：{HOTKEY_SPACE_LIGHT}", 
									"OK");
						break;
				}
			}

			static void ShowAndFocusSubtitlesPlayer() 
			{
				ShowSubtitlesPlayer();
				SubtitlePlayer.window?.Focus();
			}

			static Vector2 scrollVec;
			public static void SubtitleEditor()
			{
				GUILayout.BeginVertical(GUI.skin.box);

				scrollVec = GUILayout.BeginScrollView(scrollVec, false, true);
				if (reorderableList != null)
				{
					EditorGUI.BeginChangeCheck();
					try { reorderableList.DoLayoutList();} catch { }
					if (EditorGUI.EndChangeCheck())
						Protector.SaveSubtitleData();
				}
				else
					ShowWindow();
				GUILayout.EndScrollView();

				GUILayout.EndVertical();
			}

			public static void GUIReOrderableList()
			{
				if (window == null || reorderableList == null)
					return;

				var windowRect = window.position;
				var mWidth = windowRect.width;
				var nextTimeWidth = 65f;
				var colorWidth = 60f;
				var stringWidth = mWidth - nextTimeWidth - colorWidth - 40;
				reorderableList.drawHeaderCallback = (r) =>
				{
					EditorGUI.LabelField(new Rect(r.x, r.y, stringWidth, r.height), REORDER_SUBTILE, EditorStyles.toolbarButton);
					EditorGUI.LabelField(new Rect(stringWidth, r.y, nextTimeWidth, r.height), REORDER_DISPLAY, EditorStyles.toolbarButton);
					EditorGUI.LabelField(new Rect(stringWidth + nextTimeWidth, r.y, colorWidth, r.height), REORDER_COLOR, EditorStyles.toolbarButton);
				};
				reorderableList.drawElementCallback = (r, i, isActive, isFocused) =>
				{
					subtitleDataList[i]._strings = EditorGUI.TextField(new Rect(r.x, r.y, stringWidth - 20, r.height), subtitleDataList[i]._strings);
					subtitleDataList[i]._nextTime = EditorGUI.FloatField(new Rect(stringWidth, r.y, nextTimeWidth, r.height), subtitleDataList[i]._nextTime);
					subtitleDataList[i]._colors = EditorGUI.ColorField(new Rect(stringWidth + nextTimeWidth, r.y, colorWidth, r.height), subtitleDataList[i]._colors);
				};
				reorderableList.onRemoveCallback = (o) => 
				{ 
					o.list.RemoveAt(o.index);
					Protector.SaveSubtitleData();
				};
				reorderableList.onMouseDragCallback = (o) => 
				{
					Protector.SaveSubtitleData(); 
				};

				if (reorderableList.count == 1) reorderableList.displayRemove = false;
				else reorderableList.displayRemove = true;
				reorderableList.displayAdd = false;
			}

			public static void AddButton()
			{
				GUILayout.BeginHorizontal(GUI.skin.box);

				GUILayout.FlexibleSpace();
				GUILayout.Label(BUTTON_STR_ADD_SUB);
				EditorGUI.BeginChangeCheck();
				var width = GUILayout.Width(40);
				if (GUILayout.Button("+1", EditorStyles.miniButtonLeft,   width)) AddSubtitleData(); 
				if (GUILayout.Button("+5", EditorStyles.miniButtonMid,    width)) AddSubtitleData(5);
				if (GUILayout.Button("+10", EditorStyles.miniButtonRight, width)) AddSubtitleData(10);
				if (EditorGUI.EndChangeCheck())
					Protector.SaveSubtitleData();

				GUILayout.EndHorizontal();
			}

			public static void AddSubtitleData(int count = 1)
			{
				for (int i = 0; i < count; i++)
				{
					var subtitleData = new SubtitleData();
					subtitleData._strings = "";
					subtitleData._nextTime = 2.0f;
					subtitleData._colors = EditorGUIUtility.isProSkin ? Color.white : Color.black;
					subtitleDataList.Add(subtitleData);
				}
				reorderableList.list = subtitleDataList;
			}

			public static void SubtitlePlayerButton()
			{
				var tempGuiColor = GUI.color;
				if (subtitlePlayerIsOpen)
				{
					GUI.color = GUI_COLOR_LIGHT_BLUE;
					if (GUILayout.Button(BUTTON_STR_CLOSE_SUB_PLAYER))
					{
						subtitlePlayerIsOpen = false;
						SubtitlePlayer.window?.Close();
						if (!DisplaySubtitleMode.ugui)
							InGameGUITextFactory.Clear();
					}
				}
				else
				{
					GUI.color = GUI_COLOR_LIGHT_GREEN;
					if (GUILayout.Button(BUTTON_STR_OPEN_SUB_PLAYER))
						ShowSubtitlesPlayer();
				}
				GUI.color = tempGuiColor;
			}

			static void ShowSubtitlesPlayer() 
			{
				if (!subtitlePlayerIsOpen)
				{
					subtitlePlayerIsOpen = true;
					SubtitlePlayer.ReLoadSubitles();
					SubtitlePlayer.ShowWindow();
				}
			}
		}
	}
	
	public class SubtitlePlayer : EditorWindow
	{
		public static SubtitlePlayer window;
		GUIStyle styleSubtitle = new GUIStyle();
		GUIStyle styleSubtitleMin = new GUIStyle();
		public static List<SWT_Subtitle.SubtitleData> subtitleDataList = SWT_Subtitle.subtitleDataList;
		public static bool autoPlay = false;
		bool autoReStart = true;
		Progress progress_Typewriter = new Progress();
		Progress progress_AutoPlay = new Progress();
		Progress progress_TipLight = new Progress();
		public static int progressSubtitleIndex = 0;
		Rect rectTypewriter = new Rect(0, 0, 0, 0);
		bool isTypewriter = true;
		bool typewriterOnce = false;
		bool typewriterEnding = false;
		string currentWriter = "";
		float writerWaitTime = 0.15f;
		bool canUseArrow = true;
		float tipLightTime = 0.5f;
		Color tipLightColor = YELLOW;
		Rect rectSubtitle;
		bool tipLighting = false;
		KeyCode keySpace = KeyCode.Space;
		KeyCode[,] keyCodeNumbers = new KeyCode[,]
		{
			{ KeyCode.Alpha0, KeyCode.Keypad0 }, { KeyCode.Alpha1, KeyCode.Keypad1 },
			{ KeyCode.Alpha2, KeyCode.Keypad2 }, { KeyCode.Alpha3, KeyCode.Keypad3 },
			{ KeyCode.Alpha4, KeyCode.Keypad4 }, { KeyCode.Alpha5, KeyCode.Keypad5 },
			{ KeyCode.Alpha6, KeyCode.Keypad6 }, { KeyCode.Alpha7, KeyCode.Keypad7 },
			{ KeyCode.Alpha8, KeyCode.Keypad8 }, { KeyCode.Alpha9, KeyCode.Keypad9 }
		};

		public static void ShowWindow()
		{
			window = ScriptableObject.CreateInstance(typeof(SubtitlePlayer)) as SubtitlePlayer;
			window.minSize = new Vector2(250, 50);
			window.maxSize = new Vector2(1500, 500);

			window.titleContent = new GUIContent(WINDOW_PLAYER_TITTLE);
			window.ShowUtility();
			window.Focus();
		}

		void OnEnable()
		{
			SWT_Subtitle.subtitlePlayerIsOpen = true;

			styleSubtitle.fontSize = 25;
			styleSubtitle.alignment = TextAnchor.MiddleCenter;

			styleSubtitleMin.fontSize = 10;
			styleSubtitleMin.alignment = TextAnchor.MiddleCenter;
			styleSubtitleMin.normal.textColor = EditorGUIUtility.isProSkin ? new Color(1, 1, 1, 0.5f) : new Color(0, 0, 0, 0.5f);
		}

		private void OnDisable()
		{
			SWT_Subtitle.subtitlePlayerIsOpen = false;
			currentSubtitleIndex = 0;
			tempJ = 0;

			StopAllCoroutines(this);
			InGameGUITextFactory.Clear();
		}

		bool alwaysTop = false;
		public static int currentSubtitleIndex = 0;
		Event e;
		Rect[] allGUIRect = new Rect[5];
		private void OnGUI()
		{
			if (EditorApplication.isCompiling)
				window.Close();

			if (subtitleDataList.Count == 0)
			{
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal();
				GUILayout.FlexibleSpace();
				GUILayout.BeginHorizontal(GUI.skin.box);
				GUILayout.Label(WP_NO_ENTERD_SUB == null ? "" : WP_NO_ENTERD_SUB);
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();
				GUILayout.EndHorizontal();
				GUILayout.FlexibleSpace();

				if (SWT_Subtitle.subtitleDataList.Count != 0)
					subtitleDataList = SWT_Subtitle.subtitleDataList;

				return;
			}

			e = Event.current;

			GUILayout.BeginVertical();
			GUISubtitle();
			allGUIRect[0] = DisplaySubtitleMode.player ? GUILayoutUtility.GetLastRect() : Rect.zero;
			GUILayout.EndVertical();
			
			GUILayout.Space(10);

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUITurnPageButton();
			allGUIRect[1] = GUILayoutUtility.GetLastRect();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
			
			GUILayout.Space(10);

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUIAutoPlay();
			allGUIRect[2] = GUILayoutUtility.GetLastRect();
			GUITypewriter();
			allGUIRect[3] = GUILayoutUtility.GetLastRect();
			GUITipLight();
			allGUIRect[4] = GUILayoutUtility.GetLastRect();
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			if (alwaysTop)
				window?.Focus();

			if (e.isMouse)
			{
                foreach (var rect in allGUIRect)   
					if (!rect.Contains(e.mousePosition))
					{
						GUIUtility.hotControl = 0;
						GUIUtility.keyboardControl = 0;
						EditorGUIUtility.editingTextField = false;
					}
			}
			Repaint();
		}

		void GUIProgressBar(Rect r, bool display = false, float currentValue = 0.5f)
		{
			EditorGUI.DrawRect(new Rect(r.x, r.y - 5, r.width, 5), GRAY); // backgrund.
			if (display)
				EditorGUI.DrawRect(new Rect(r.x, r.y - 5, r.width * currentValue, 5), Color.white);// progress.
		}

		void GUITypewriter()
		{
			if (DisplayController.typeWriter)
			{
				GUILayout.BeginVertical(GUI.skin.box);

				isTypewriter = GUILayout.Toggle(isTypewriter, WP_TYPEWRITER);

				GUILayout.BeginHorizontal();
				GUILayout.Label(WP_WAIT);
				writerWaitTime = EditorGUILayout.FloatField(writerWaitTime, GUILayout.Width(30));
				GUILayout.Label(WP_SEC);
				GUILayout.EndHorizontal();

				GUILayout.EndVertical();
				rectTypewriter = GUILayoutUtility.GetLastRect();
				GUIProgressBar(rectTypewriter, isTypewriter, progress_Typewriter.Time(writerWaitTime));
			}
        }

		void GUISubtitle()
		{
			SubtitleColorChange();

			if (DisplaySubtitleMode.player)
			{
				GUILayout.Label((currentSubtitleIndex - 1 <= 0) ? "---" : subtitleDataList[currentSubtitleIndex - 1]._strings, styleSubtitleMin);
				GUILayout.BeginHorizontal(GUI.skin.box);
			}

			styleSubtitle.normal.textColor = subtitleDataList[currentSubtitleIndex]._colors;
			var text = isTypewriter ? Typewriter(subtitleDataList) : subtitleDataList[currentSubtitleIndex]._strings;
			
			if(DisplaySubtitleMode.player)
				GUILayout.TextField(text, styleSubtitle);

			if(DisplaySubtitleMode.ugui)
				InGameGUITextFactory.Show(text, styleSubtitle.fontSize, styleSubtitle.normal.textColor);

			if (DisplaySubtitleMode.player)
			{
				GUILayout.EndHorizontal();
				rectSubtitle = GUILayoutUtility.GetLastRect();

				GUILayout.Label((currentSubtitleIndex + 1 >= subtitleDataList.Count) ? "---" : subtitleDataList[currentSubtitleIndex + 1]._strings, styleSubtitleMin);
				GUILayout.FlexibleSpace();
			}
		}

		void GUITurnPageButton()
		{
			if (DisplayController.subtitle)
			{
				EditorGUI.BeginDisabledGroup(autoPlay);
				GUILayout.BeginHorizontal(GUI.skin.box);
			}
			var keyUp = (e.type == EventType.KeyUp) && (!EditorGUIUtility.editingTextField);
			var keyBoardUp = e.keyCode == KeyCode.UpArrow || e.keyCode == KeyCode.PageUp;
			var upArrow = keyUp && (keyBoardUp) && (canUseArrow);
			if (CustomButton("▲") || upArrow)
				if (currentSubtitleIndex - 1 >= 0)
				{
					StopAllCoroutines(this);
					currentSubtitleIndex--;
					SyncPlayerValue();
				}

			var keyBoardDown = e.keyCode == KeyCode.DownArrow || e.keyCode == KeyCode.PageDown;
			var downArrow = keyUp && (keyBoardDown) && (canUseArrow);
			if (CustomButton("▼") || downArrow)
				if (currentSubtitleIndex + 1 < subtitleDataList.Count)
				{
					StopAllCoroutines(this);
					currentSubtitleIndex++;
					SyncPlayerValue();
				}

			if (keyUp && (e.keyCode == KeyCode.Home))
			{
				currentSubtitleIndex = 0;
				SyncPlayerValue();
			}

			if (keyUp && (e.keyCode == KeyCode.End))
			{
				currentSubtitleIndex = subtitleDataList.Count - 1;
				SyncPlayerValue();
			}

			GUILayout.Space(25);

			var numPlus = (e.keyCode == KeyCode.KeypadPlus) || (e.keyCode == KeyCode.Equals);  //e = Event.current;
			numPlus |= (e.type == EventType.ScrollWheel) && e.delta.y < 0;
			//numPlus &= keyUp;
			if (CustomButton("+") || numPlus)
				if (styleSubtitle.fontSize < 128)
					styleSubtitle.fontSize++;

			var numSub = (e.keyCode == KeyCode.KeypadMinus) || (e.keyCode == KeyCode.Minus);
			numSub |= (e.type == EventType.ScrollWheel) && e.delta.y > 0;
			//numSub &= keyUp;
			if (CustomButton("-") || numSub)
				if (styleSubtitle.fontSize > 8)
					styleSubtitle.fontSize--;

			if (DisplayController.subtitle)
			{
				GUILayout.EndHorizontal();
				rectTypewriter = GUILayoutUtility.GetLastRect();
				GUIProgressBar(rectTypewriter, true, ((float)progressSubtitleIndex / ((float)subtitleDataList.Count - 1)));
				EditorGUI.EndDisabledGroup();
			}
		}

		void GUIAutoPlay()
		{
			if (DisplayController.autoPlay)
			{
				GUILayout.BeginVertical(GUI.skin.box);
				if (autoPlay)
				{
					if (GUILayout.Button(WP_STOP))
					{
						autoPlay = false;
						canUseArrow = true;
						StopAllCoroutines(this);
					}
				}
				else
				{
					if (GUILayout.Button(WP_AUTOPLAY))
					{
						autoPlay = true;
						canUseArrow = false;
						StartCoroutine(AutoPlay(), this);
					}
				}
				autoReStart = GUILayout.Toggle(autoReStart, WP_FROM_BEGIN);
				alwaysTop = GUILayout.Toggle(alwaysTop, WP_WINDOW_TOP);

				GUILayout.EndVertical();
				rectTypewriter = GUILayoutUtility.GetLastRect();
				GUIProgressBar(rectTypewriter, autoPlay, progress_AutoPlay.Time(subtitleDataList[currentSubtitleIndex]._nextTime));
			}
		}

		void GUITipLight() 
		{
			var tipLightButton = false;
			if (DisplayController.tipLight)
			{
				GUILayout.BeginVertical(GUI.skin.box);

				tipLightButton = GUILayout.Button(WP_TIPLIGHT_BUTTON);

				GUILayout.BeginHorizontal();
				GUILayout.Label(WP_TIPLIGHT_COLOT);
				tipLightColor = EditorGUILayout.ColorField(tipLightColor, GUILayout.Width(45));
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				GUILayout.Label(WP_TIPLIGHT_FLASH);
				tipLightTime = EditorGUILayout.FloatField(tipLightTime, GUILayout.Width(30));
				GUILayout.Label(WP_TIPLIGHT_SEC);
				GUILayout.EndHorizontal();

				GUILayout.EndVertical();
				var rectTipLight = GUILayoutUtility.GetLastRect();
				GUIProgressBar(rectTipLight, true, progress_TipLight.Time(tipLightTime));
			}

			if (tipLighting)
			{
				var oriColor = GUI.color;
				GUI.color = tipLightColor;
				GUI.Box(rectSubtitle, "");
				GUI.color = oriColor;
			}

			tipLightButton |= (e.type == EventType.KeyDown) && (e.keyCode == keySpace) && (EditorGUIUtility.editingTextField == false);
			if (tipLightButton)
				TipLight();
		}

		public bool CustomButton(string s, float width = 25)
		{
			if (DisplayController.subtitle)
				return GUILayout.Button(s, GUILayout.Width(width));
			else
				return false;
		}

		void SubtitleColorChange()
		{
			if (EditorGUIUtility.editingTextField)
				return;

			for (int i = 0; i < COLORS().Length; i++)
            {
				var keyBoardNumber = (e.keyCode == keyCodeNumbers[i, 0]) || (e.keyCode == keyCodeNumbers[i, 1]);
				keyBoardNumber &= (e.type == EventType.KeyUp);
				if (keyBoardNumber)
					subtitleDataList[currentSubtitleIndex]._colors = COLORS()[i];
			}
		}

		string Typewriter(List<SWT_Subtitle.SubtitleData> subtitleDataList)
		{
			var str = subtitleDataList[currentSubtitleIndex]._strings;
			var length = str.Length;

			// End
			var keyBoardEnter = (e.keyCode == KeyCode.Return) || (e.keyCode == KeyCode.KeypadEnter);
			if (currentWriter.Length >= length || keyBoardEnter )
				if (!typewriterEnding)
				{
					typewriterEnding = true;
					StopCoroutine(Typewriter(writerWaitTime, str), this);
					progress_Typewriter.Stop();
				}
			
			// Output
			if (typewriterEnding)
				return str;
			else
			{
				if(!typewriterOnce)
					StartCoroutine(Typewriter(writerWaitTime, str), this);

				typewriterOnce = true;
				return currentWriter;
			}
		}

		void TypewriterRemove() 
		{
			typewriterOnce = false;
			typewriterEnding = false;
			currentWriter = "";
			StopCoroutine(Typewriter(writerWaitTime, ""), this);
		}

		void TipLight() 
		{
			var flashCount = 6;
			var sec = tipLightTime / flashCount;
			if (!tipLighting)
			{
				if(DisplaySubtitleMode.player)
					StartCoroutine(TipLight(sec, flashCount), this);

				if(DisplaySubtitleMode.ugui)
					InGameGUITextFactory.ShowTipLight(tipLightColor, flashCount, sec);
			}
		}

		void SyncPlayerValue() 
		{
			progressSubtitleIndex = currentSubtitleIndex;
			tempJ = currentSubtitleIndex;
			TypewriterRemove();
		}

		public static void ReLoadSubitles()
		{
			subtitleDataList = SWT_Subtitle.subtitleDataList;
			currentSubtitleIndex = 0;
		}

		int tempJ = 0;
		IEnumerator AutoPlay()
		{
			var tempCount = subtitleDataList.Count - 1;
			for (int j = (autoReStart || currentSubtitleIndex == tempCount) ? 0 : tempJ; j < subtitleDataList.Count; j++)
			{
				tempJ = j;
				currentSubtitleIndex = j;
				progressSubtitleIndex = j;
				var nextTime = subtitleDataList[currentSubtitleIndex]._nextTime; 
				progress_AutoPlay.Start(nextTime);

				if (currentSubtitleIndex == tempCount)
				{
					autoPlay = false;
					progress_AutoPlay.Stop();
				}

				if (j != subtitleDataList.Count)
					TypewriterRemove();

				yield return new WaitForSeconds(nextTime);

				progress_AutoPlay.Stop();
			}
			canUseArrow = true;
		}

		IEnumerator Typewriter(float sec, string text)
		{
			foreach (var c in text)
			{
				progress_Typewriter.Start(sec);

				yield return new WaitForSeconds(sec);
				currentWriter += c;

				progress_Typewriter.Stop();
			}
		}

		IEnumerator TipLight(float sec, float flashCount)
		{
			tipLighting = true;
			progress_TipLight.Start(sec * flashCount);
			for (int i = 0; i < flashCount; i++)
			{
				yield return new WaitForSeconds(sec);
				var tempI = ((i + 1) % 2) * 0.5f;
				tipLightColor.a = tempI;
			}
			tipLighting = false;
			progress_TipLight.Stop();
		}
	}

	internal class AboutWindow : EditorWindow
	{
		public static AboutWindow window;
		public static Vector2 windowSize = new Vector2(520, 300);
		public static GUIStyle styleTittle, styleText, sytleURL;
		static string _version, _title, _text;
		Event e = null;

		void StyleSetting()
		{
			if (styleTittle == null)
			{
				styleTittle = new GUIStyle();
				styleTittle.fontSize = 30;
			}

			if (styleText == null)
			{
				styleText = new GUIStyle();
				styleText.fontSize = 14;
				styleText.alignment = TextAnchor.UpperCenter;
			}

			if (sytleURL == null)
			{
				sytleURL = new GUIStyle();
				sytleURL.fontSize = 13;
				sytleURL.normal.textColor = Color.blue;
			}
		}

		public static void Init(string version, string aboutTitle, string aboutText, Rect targetWinPos)
		{
			window = CreateInstance(typeof(AboutWindow)) as AboutWindow;
			window.titleContent = new GUIContent(aboutTitle);
			window.minSize = windowSize;
			window.maxSize = windowSize;
			window.ShowUtility();
			window.Focus();

			_version = version;
			_title = aboutTitle;
			_text = aboutText;

			var aboutWinPos = window.position;
			var aboutOffset_W = targetWinPos.x - ((aboutWinPos.width - targetWinPos.width) / 2);
			var aboutOffset_H = targetWinPos.y + (targetWinPos.height / 2) - (aboutWinPos.height / 2);
			window.position = new Rect(aboutOffset_W, aboutOffset_H, windowSize.x, windowSize.y);
			window.ShowUtility();
			window.Focus();
		} 

		void OnGUI()
		{
			if (focusedWindow?.ToString() != " (SuWenTools.AboutWindow)" || EditorApplication.isCompiling) 
			{
				window?.Close();
				window = null;
			}

			if (e == null)
				e = Event.current;

			StyleSetting();

			GUITittle();
			GUIDescription();

			GUILayout.FlexibleSpace();
            GUILayout.BeginHorizontal();
            GUILayout.EndHorizontal();
            var r = GUILayoutUtility.GetLastRect();
			GUI.Box(new Rect(r.x, r.y, r.width, 1),"");
			GUILayout.FlexibleSpace();

			GUICopyright();
			SuWenInformation();
			Repaint();
		}

		void GUITittle() 
		{
			GUILayout.Space(20);

			GUILayout.BeginVertical(GUI.skin.box);

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label(_title, styleTittle);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label($"Version : {_version}");
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();

			GUILayout.EndVertical();

			GUILayout.Space(20);
		}

		void GUIDescription() 
		{
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			
			GUILayout.Label(_text, styleText);
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}

        readonly string[,] URL_INFO = new string[,]
        {
             { "Facebook", "https://www.facebook.com/su.wen.142/" },
             { "Youtube", "https://www.youtube.com/channel/UCeK0f9Lqj5oVgyjtGHSj9sw" },
             { "Blog", "https://fiend945.wixsite.com/suwen" }
        };
        void SuWenInformation() 
		{
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
            for (int i = 0; i < 3; i++)
            {
				GUILayout.Space(5);
                URLString(URL_INFO[i, 0], URL_INFO[i, 1]);
				GUILayout.Space(5);
            }
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}

		void GUICopyright() 
		{
			GUILayout.BeginHorizontal();
			GUILayout.FlexibleSpace();
			GUILayout.Label("Copyright© SuWen All Rights Reserved.");
			GUILayout.FlexibleSpace();
			GUILayout.EndHorizontal();
		}

		void URLString(string text, string url = null)
		{
			if (url == null || url == "")
				GUILayout.Label(text);
			else
			{
				if (GUILayout.Button(text, sytleURL))
					try
					{
						if (url.Contains("http://") || url.Contains("https://"))
							Application.OpenURL(url);
					}
					catch (Exception e)
					{
						Debug.LogError(e);
					}
				var r = GUILayoutUtility.GetLastRect();
				if (r.Contains(e.mousePosition))
				{
					var oriColor = GUI.color;
					GUI.color = new Color(0, 0, 1, 0.5f);
					GUI.Box(new Rect(r.x, r.y, r.width, r.height), "");
					GUI.color = oriColor;
				}
			}
		}
	}
}
/*
 * ///////////////////////////////////////////
 * ///Copyright© SuWen All Rights Reserved.///
 * ///////////////////////////////////////////
 */