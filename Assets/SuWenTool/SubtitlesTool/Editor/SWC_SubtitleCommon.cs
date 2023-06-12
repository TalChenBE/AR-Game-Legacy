/*
 * ///////////////////////////////////////////
 * ///Copyright© SuWen All Rights Reserved.///
 * ///////////////////////////////////////////
 */

using System;
using System.IO;
using System.Collections;
using System.Text;
using System.Collections.Generic;

using Object = UnityEngine.Object;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEditor;
using UnityEditorInternal;

using static SuWenTools.Common.Languge;
using static SuWenTools.EditorCoroutineRunner.EditorCoroutines;

namespace SuWenTools
{
	public static class Common
	{
		public class SubtitleColor
		{
			public static Color GUI_COLOR_LIGHT_BLUE => new Color(0.7f, 0.7f, 1, 1);
			public static Color GUI_COLOR_LIGHT_GREEN => new Color(0.7f, 1, 0.7f, 1);
			//--------------------------------------------------------------------------
			public static Color RED => Color.red;
			public static Color ORANGE => new Color(1, .5f, 0);
			public static Color YELLOW => Color.yellow;
			public static Color GREEN => Color.green;
			public static Color BLUE => Color.blue;
			public static Color INDIGO => new Color(.2f, 0, .5f);
			public static Color PURPLE => new Color(.5f, 0, 1);
			public static Color GRAY => Color.gray;

            public static Color[] COLORS()
            {
                var AUTO = EditorGUIUtility.isProSkin ? Color.white : Color.black;
                var AUTO_INVERT = EditorGUIUtility.isProSkin ? Color.black : Color.white;
                return new Color[] { AUTO, RED, ORANGE, YELLOW, GREEN, BLUE, INDIGO, PURPLE, GRAY, AUTO_INVERT };
            }
        }

		public class DisplayController
		{
			public static bool allTrue
			{
				get { return tempAllTrue; }
				set
				{
					if (value)
					{
						tempAllFalse = false;
						tempSubtitle = true;
						tempAutoPlay = true;
						tempTypeWriter = true;
						tempTipLight = true;
					}
					tempAllTrue = value;
				}
			}
			public static bool allFalse
			{
				get { return tempAllFalse; }
				set
				{
					if (value)
					{
						tempAllTrue = false;
						tempSubtitle = false;
						tempAutoPlay = false;
						tempTypeWriter = false;
						tempTipLight = false;
					}
					tempAllFalse = value;
				}
			}
			public static bool subtitle
			{
				get { return tempSubtitle; }
				set
				{
					if (value)
					{
						if (tempAutoPlay && tempTypeWriter && tempTipLight)
							tempAllTrue = true;
						else
							tempAllTrue = false;

						tempAllFalse = false;
					}
					else
					{
						if (!tempAutoPlay && !tempTypeWriter && !tempTipLight)
							tempAllFalse = true;

						tempAllTrue = false;
					}
					tempSubtitle = value;
				}
			}
			public static bool autoPlay
			{
				get { return tempAutoPlay; }
				set
				{
					if (value)
					{
						if (tempSubtitle && tempTypeWriter && tempTipLight)
							tempAllTrue = true;
						else
							tempAllTrue = false;

						tempAllFalse = false;
					}
					else
					{
						if (!tempSubtitle && !tempTypeWriter && !tempTipLight)
							tempAllFalse = true;

						tempAllTrue = false;
					}

					tempAutoPlay = value;
				}
			}
			public static bool typeWriter
			{
				get { return tempTypeWriter; }
				set
				{
					if (value)
					{
						if (tempSubtitle && tempAutoPlay && tempTipLight)
							tempAllTrue = true;
						else
							tempAllTrue = false;

						tempAllFalse = false;
					}
					else
					{
						if (!tempSubtitle && !tempAutoPlay && !tempTipLight)
							tempAllFalse = true;

						tempAllTrue = false;
					}

					tempTypeWriter = value;
				}
			}
			public static bool tipLight
			{
				get { return tempTipLight; }
				set
				{
					if (value)
					{
						if (tempSubtitle && tempAutoPlay && tempTypeWriter)
							tempAllTrue = true;
						else
							tempAllTrue = false;

						tempAllFalse = false;
					}
					else
					{
						if (!tempSubtitle && !tempAutoPlay && !tempTypeWriter)
							tempAllFalse = true;

						tempAllTrue = false;
					}

					tempTipLight = value;
				}
			}
			private static bool tempAllTrue = true, tempAllFalse = false;
			private static bool tempSubtitle = true, tempAutoPlay = true, tempTypeWriter = true, tempTipLight = true;
		}

		public class DisplaySubtitleMode
		{
			public static bool all
			{
				get 
				{
					if (EditorPrefs.HasKey("Subtitle_DisplaySubtitleMode_All"))
						tempAll = EditorPrefs.GetBool("Subtitle_DisplaySubtitleMode_All");

					return tempAll; 
				}
				set 
				{
					if (value) 
					{
						player = true;
						ugui = true;
					}
					tempAll = value;
					EditorPrefs.SetBool("Subtitle_DisplaySubtitleMode_All", tempAll);
				}
			}

			public static bool player
			{
				get 
				{
					if (EditorPrefs.HasKey("Subtitle_DisplaySubtitleMode_Player"))
						tempPlayer = EditorPrefs.GetBool("Subtitle_DisplaySubtitleMode_Player");

					return tempPlayer; 
				}
				set 
				{
					if (value)
					{
						SubtitlePlayer.window?.ShowUtility();
						SubtitlePlayer.window?.Focus();
						if (tempPlayer && tempUGUI)
							tempAll = true;
					}
					else
					{
						tempAll = false;
					}
					tempPlayer = value;
					EditorPrefs.SetBool("Subtitle_DisplaySubtitleMode_Player", tempPlayer);
				}
			}

			public static bool ugui
			{
				get 
				{
					if (EditorPrefs.HasKey("Subtitle_DisplaySubtitleMode_UGUI"))
						tempUGUI = EditorPrefs.GetBool("Subtitle_DisplaySubtitleMode_UGUI");

					return tempUGUI; 
				}
				set
				{
					if (value)
					{
						SubtitlePlayer.window?.Close();
						SubtitlePlayer.ShowWindow();
						InGameGUITextFactory.Refresh();
						if (tempPlayer && tempUGUI)
							tempAll = true;
					}
					else
					{
						tempAll = false;
						InGameGUITextFactory.Clear();
					}
					tempUGUI = value;
					EditorPrefs.SetBool("Subtitle_DisplaySubtitleMode_UGUI", tempUGUI);
				}
			}

			private static bool tempAll = false, tempPlayer = true, tempUGUI = false;
		}

		public class Languge
		{
			public static bool stageEn
			{
				get 
				{ return tempStageEn; }
				set 
				{ 
					if (value) 
						stageCh = false; 
					
					tempStageEn = value; 
				}
			}
			public static bool stageCh
			{
				get
				{ return tempStageCh; }
				set 
				{ 
					if (value) 
						stageEn = false; 

					tempStageCh = value; 
				}
			}
			private static bool tempStageEn, tempStageCh;

			public enum LangugeSelect { Eng, Chi, Jap }
			public static string WINDOW_EDITOR_TITTLE,
				TOOLBAR_FILE, TOOLBAR_FILE_NEW, TOOLBAR_FILE_SAVE, TOOLBAR_FILE_LOAD, TOOLBAR_FILE_CLOSE,
				TOOLBAR_DISPLAY, TOOLBAR_DISPLAY_LAN_ENG, TOOLBAR_DISPLAY_LAN_CHI,
				TOOLBAR_DISPLAY_MODE_DIVIDER, TOOLBAR_DISPLAY_MODE_ALL, TOOLBAR_DISPLAY_MODE_UGUI, TOOLBAR_DISPLAY_MODE_PLEYER,
				TOOLBAR_DISPLAY_PLAYER, TOOLBAR_DISPLAY_ALL_TRUE, TOOLBAR_DISPLAY_ALL_FALSE, 
				TOOLBAR_DISPLAY_ALL_DIVIDER, TOOLBAR_DISPLAY_PLAYER_SUBTITLES, TOOLBAR_DISPLAY_PLAYER_AUTOPLAY, TOOLBAR_DISPLAY_PLAYER_TYPEWRITER, TOOLBAR_DISPLAY_PLAYER_TIPLGIHT,
				TOOLBAR_ABOUT, TOOLBAR_ABOUT_ABOUT, TOOLBAR_ABOUT_HOTKEY, 
				REORDER_SUBTILE, REORDER_DISPLAY, REORDER_COLOR,
				BUTTON_STR_ADD_SUB, BUTTON_STR_CLOSE_SUB_PLAYER, BUTTON_STR_OPEN_SUB_PLAYER,
				LABEL_STR_ONLY_SUB,

				PANEL_SAVE, PANEL_LOAD, FILE_NAME,
				HOTKEY_TITTLE, HOTKEY_COLOR, HOTKEY_FIRST_SUB, HOTKEY_LAST_SUB, HOTKEY_PRE_SUB, HOTKEY_NEXT_SUB, HOTKEY_ENL_SUB, HOTKEY_SHR_SUB, HOTKEY_END_WRITER_MODE, HOTKEY_SPACE_LIGHT,

				WINDOW_PLAYER_TITTLE, WP_NO_ENTERD_SUB, WP_TYPEWRITER, WP_WAIT, WP_SEC, WP_STOP, WP_AUTOPLAY, 
				WP_FROM_BEGIN, WP_WINDOW_TOP, WP_TIPLIGHT_BUTTON, WP_TIPLIGHT_COLOT, WP_TIPLIGHT_FLASH, WP_TIPLIGHT_SEC,

				ABOUT_WIN_TITTLE, ABOUT_WIN_TEXT;
			
			public static void LangugeChoice(LangugeSelect langugeSelect)
			{
				EditorPrefs.SetString("Subtitle_Language", langugeSelect.ToString());
				switch (langugeSelect)
				{
					case LangugeSelect.Eng:
						stageEn = true;
						WINDOW_EDITOR_TITTLE = "Subtitles Editor";
						TOOLBAR_FILE = "File"; TOOLBAR_FILE_NEW = "New Subtitles"; TOOLBAR_FILE_SAVE = "Save Subtitles"; TOOLBAR_FILE_LOAD = "Load Subtitles"; TOOLBAR_FILE_CLOSE = "Close Tool";
						TOOLBAR_DISPLAY = "Display"; TOOLBAR_DISPLAY_LAN_ENG = "Languge/English"; TOOLBAR_DISPLAY_LAN_CHI = "Languge/中文";
						TOOLBAR_DISPLAY_MODE_DIVIDER = "Subtitle Display Mode/"; TOOLBAR_DISPLAY_MODE_ALL = "Subtitle Display Mode/All"; TOOLBAR_DISPLAY_MODE_UGUI = "Subtitle Display Mode/Game View Mode"; TOOLBAR_DISPLAY_MODE_PLEYER = "Subtitle Display Mode/Player Mode";
						TOOLBAR_DISPLAY_ALL_TRUE = "Subtitles Player Display/Show all"; TOOLBAR_DISPLAY_ALL_FALSE = "Subtitles Player Display/Hide all"; 
						TOOLBAR_DISPLAY_ALL_DIVIDER = "Subtitles Player Display/"; 
						TOOLBAR_DISPLAY_PLAYER_SUBTITLES = "Subtitles Player Display/Show Subtitles Controller"; TOOLBAR_DISPLAY_PLAYER_AUTOPLAY = "Subtitles Player Display/Show Auto Player"; TOOLBAR_DISPLAY_PLAYER_TYPEWRITER = "Subtitles Player Display/Show Typewriter"; TOOLBAR_DISPLAY_PLAYER_TIPLGIHT = "Subtitles Player Display/Show Tip Light";
						TOOLBAR_ABOUT = "About"; TOOLBAR_ABOUT_ABOUT = "About Tool"; TOOLBAR_ABOUT_HOTKEY = "HotKey List"; 
						REORDER_SUBTILE = "Subtitles"; REORDER_DISPLAY = "Display(s)"; REORDER_COLOR = "Color";
						BUTTON_STR_ADD_SUB = "Add subtitles"; BUTTON_STR_CLOSE_SUB_PLAYER = "\nClose Subtitles Player\n"; BUTTON_STR_OPEN_SUB_PLAYER = "\nOpen Subtitles Player\n";
						LABEL_STR_ONLY_SUB = "Only subtitles";

						PANEL_SAVE = "Save subtitles(Json)"; PANEL_LOAD = "Load subtitles(Json)"; FILE_NAME = "subtitles";

						HOTKEY_TITTLE = "Subtitles Player Hotkey"; HOTKEY_COLOR = "Change subtitle color"; HOTKEY_FIRST_SUB = "Display first subtitles"; HOTKEY_LAST_SUB = "Display last subtitles";
						HOTKEY_PRE_SUB = "Previous subtitles"; HOTKEY_NEXT_SUB = "Next subtitles";
						HOTKEY_ENL_SUB = "Enlarge subtitles"; HOTKEY_SHR_SUB = "Shrink subtitles"; HOTKEY_END_WRITER_MODE = "Force end typewriter mode"; HOTKEY_SPACE_LIGHT = "Subtitles tip light";

						WINDOW_PLAYER_TITTLE = "Subtitle Player"; WP_NO_ENTERD_SUB = "No subtitles have been entered"; WP_TYPEWRITER = "Typewriter mode";
						WP_WAIT = "Waiting"; WP_SEC = "sec"; WP_STOP = "Stop"; WP_AUTOPLAY = "Auto Play"; WP_FROM_BEGIN = "From beginning";
						WP_WINDOW_TOP = "Window top"; WP_TIPLIGHT_BUTTON = "Tip light"; WP_TIPLIGHT_COLOT = "color"; WP_TIPLIGHT_FLASH = "Flash"; WP_TIPLIGHT_SEC = "sec";

						ABOUT_WIN_TITTLE =  "Subtitles Editor & Player"; 
						ABOUT_WIN_TEXT =	"This is a subtitle tool\n" +
											"provide a quick and easy subtitle solution\n" +
											"while recording video\n" +
											"this editor can display subtitles\n" +
											"explain the current situation\n" +
											"recorded video\n" +
											"you don't need to use post-production software to add subtitles\n" +
											"Is a good helper for meetings, presentations, and message delivery";
						break;

					case LangugeSelect.Chi:
						stageCh = true;
						WINDOW_EDITOR_TITTLE = "字幕編輯工具";
						TOOLBAR_FILE = "檔案"; TOOLBAR_FILE_NEW = "新字幕"; TOOLBAR_FILE_SAVE = "儲存字幕"; TOOLBAR_FILE_LOAD = "載入字幕"; TOOLBAR_FILE_CLOSE = "關閉工具";
						TOOLBAR_DISPLAY = "顯示"; TOOLBAR_DISPLAY_LAN_ENG = "Languge/English"; TOOLBAR_DISPLAY_LAN_CHI = "Languge/中文";
						TOOLBAR_DISPLAY_MODE_DIVIDER = "字幕顯示模式/"; TOOLBAR_DISPLAY_MODE_ALL = "字幕顯示模式/全啟用"; TOOLBAR_DISPLAY_MODE_UGUI = "字幕顯示模式/Game視窗模式"; TOOLBAR_DISPLAY_MODE_PLEYER = "字幕顯示模式/播放器模式";
						TOOLBAR_DISPLAY_ALL_TRUE = "字幕播放器顯示/全部顯示"; TOOLBAR_DISPLAY_ALL_FALSE = "字幕播放器顯示/全部不顯示"; 
						TOOLBAR_DISPLAY_ALL_DIVIDER = "字幕播放器顯示/"; 
						TOOLBAR_DISPLAY_PLAYER_SUBTITLES = "字幕播放器顯示/顯示字幕控制器"; TOOLBAR_DISPLAY_PLAYER_AUTOPLAY = "字幕播放器顯示/顯示自動播放器"; TOOLBAR_DISPLAY_PLAYER_TYPEWRITER = "字幕播放器顯示/顯示打字機"; TOOLBAR_DISPLAY_PLAYER_TIPLGIHT = "字幕播放器顯示/顯示提示光";
						TOOLBAR_ABOUT ="關於"; TOOLBAR_ABOUT_ABOUT ="關於工具"; TOOLBAR_ABOUT_HOTKEY = "快捷鍵清單"; 
						REORDER_SUBTILE = "字幕"; REORDER_DISPLAY = "顯示(sec)"; REORDER_COLOR = "顏色";
						BUTTON_STR_ADD_SUB = "增加字幕數量"; BUTTON_STR_CLOSE_SUB_PLAYER = "\n關閉字幕播放器\n"; BUTTON_STR_OPEN_SUB_PLAYER = "\n開始字幕播放器\n";
						LABEL_STR_ONLY_SUB = "純字幕";

						PANEL_SAVE = "儲存字幕(Json格式)"; PANEL_LOAD = "讀取字幕(Json格式)"; FILE_NAME = "字幕";

						HOTKEY_TITTLE = "字幕播放器快捷鍵"; HOTKEY_COLOR = "快速切換字幕顏色"; HOTKEY_FIRST_SUB = "顯示第一個字幕"; HOTKEY_LAST_SUB = "顯示最後一個字幕"; 
						HOTKEY_PRE_SUB = "上一個字幕"; HOTKEY_NEXT_SUB = "下一個字幕"; 
						HOTKEY_ENL_SUB = "放大字幕"; HOTKEY_SHR_SUB = "縮小字幕"; HOTKEY_END_WRITER_MODE = "強制結束打字機效果"; HOTKEY_SPACE_LIGHT = "字幕提示光";

						WINDOW_PLAYER_TITTLE = "字幕播放器"; WP_NO_ENTERD_SUB = "尚未輸入任何字幕"; WP_TYPEWRITER = "打字機效果";
						WP_WAIT = "等待"; WP_SEC = "秒"; WP_STOP = "停止"; WP_AUTOPLAY = "自動播放"; WP_FROM_BEGIN = "從頭開始";
						WP_WINDOW_TOP = "視窗至頂"; WP_TIPLIGHT_BUTTON = "提示光"; WP_TIPLIGHT_COLOT = "顏色"; WP_TIPLIGHT_FLASH = "閃爍"; WP_TIPLIGHT_SEC = "秒";

						ABOUT_WIN_TITTLE =  "字幕工具與播放器"; 
						ABOUT_WIN_TEXT =	"這是一個字幕工具\n" +
											"提供快速簡單的字幕解決方案\n" +
											"在錄製影片的同時\n" +
											"此編輯器可以顯示字幕\n" +
											"解說當前的情況\n" +
											"錄影完成的影片\n" +
											"則不必再使用後製軟體加上字幕\n" +
											"是開會、簡報、傳遞訊息的好幫手";
						break;

					case LangugeSelect.Jap:
						break;
				}
			}
		}

		public static class JsonTools
		{
			private static string tempSubtitlePath = Application.dataPath.Replace("/Assets", "") + "/TempSubtitle.txt";
			public static void SaveTempSubtitle(List<SWT_Subtitle.SubtitleData> subtitleDataList)
			{
				SaveSubtitleToJson(subtitleDataList, tempSubtitlePath);
			}
			public static List<SWT_Subtitle.SubtitleData> LoadTempSubtitle()
			{
				return LoadSubtitleFromJson(tempSubtitlePath);
			}

			public static void SaveSubtitleToJson(List<SWT_Subtitle.SubtitleData> subtitleDataList, string path = "")//(Subtitle subtitle)
			{
				if (path == "")
					path = EditorUtility.SaveFilePanel(PANEL_SAVE, Application.dataPath, $"{FILE_NAME}.txt", "txt");

				if (path == null || path == "")
					return;

				File.WriteAllText(path, new SWT_Subtitle.SubtitleDataToJson().ToJson(subtitleDataList), Encoding.UTF8);
			}

			public static List<SWT_Subtitle.SubtitleData> LoadSubtitleFromJson(string path = "")
			{
				if (path == "")
					path = EditorUtility.OpenFilePanel(PANEL_LOAD, Application.dataPath, "txt");

				if (!File.Exists(path) || path == "")
					return null;

				var loadData = File.ReadAllText(path);
				var subtitle = JsonUtility.FromJson<SWT_Subtitle.SubtitleDataToJson>(loadData);
				var subtitleDataList = new List<SWT_Subtitle.SubtitleData>();
				for (int i = 0; i < subtitle._strings.Count; i++)
				{
					var subtitleData = new SWT_Subtitle.SubtitleData();
					subtitleData._strings = subtitle._strings[i];
					subtitleData._nextTime = subtitle._nextTime[i];
					subtitleData._colors = subtitle._colors[i];
					subtitleDataList.Add(subtitleData);
				}
				return subtitleDataList;
			}
		}

		public class Protector
		{
			static bool isProtected
			{
				get { return tempIsProtected; }
				set { tempIsProtected = value; EditorPrefs.SetBool("Subtitle_isProtected", isProtected); }
			}
			static bool tempIsProtected = false;

			public static void SaveSubtitleData()
			{
				JsonTools.SaveTempSubtitle(SWT_Subtitle.subtitleDataList);
				isProtected = true;

				SubtitlePlayer.ReLoadSubitles();
			}

			static bool canLoading = false;
			public static void LoadSubtitleData(bool execute = false)
			{
				if (EditorApplication.isCompiling || EditorApplication.isPlayingOrWillChangePlaymode)
				{
					isProtected = EditorPrefs.GetBool("Subtitle_isProtected");
					canLoading = true;
				}

				execute |= (canLoading && isProtected);
				if (execute)
				{
					var tempSubtitleDataList = JsonTools.LoadTempSubtitle();
					if (tempSubtitleDataList == null)
					{
						if (SWT_Subtitle.subtitleDataList.Count < 1)
							SWT_Subtitle.GUIDraw.AddSubtitleData();
					}
					else
					{
						SWT_Subtitle.subtitleDataList = tempSubtitleDataList;
						if (SWT_Subtitle.reorderableList == null)
							SWT_Subtitle.reorderableList = new ReorderableList(SWT_Subtitle.subtitleDataList, SWT_Subtitle.subtitleDataList.GetType());
						else
							SWT_Subtitle.reorderableList.list = tempSubtitleDataList;
					}
					canLoading = false;
					isProtected = false;
				}
			}
		}

		public class Progress
		{
			private float progressStartTime = 0;
			public void Start(float waitTime)
			{
				progressStartTime = time;
			}

			public void Stop()
			{
				progressStartTime = -1;
			}

			public float Time(float waitTime)
			{
				if (progressStartTime > 0)
				{
					var currentTime = time;
					var elapsedTime = currentTime - progressStartTime;
					var o = elapsedTime / waitTime;
					if (o >= 1)
						progressStartTime = -1;

					return o;
				}
				else
					return 0;
			}
		}

		public static class InGameGUITextFactory
		{
			static Canvas canvas;
			static Text text;
			static Outline outline;
			static EventSystem eventSystem;
			
			static string m_message;
			static int m_size;
			static Color m_color, m_bgColor;
			
			static string EventSystemName => nameof(InGameGUITextFactory) + "EventSystem";
			static string CanvasName => nameof(InGameGUITextFactory) + "Canvas";
			static string TextName => nameof(InGameGUITextFactory) + "Text";

			public static void Refresh()
			{
				EditorApplication.QueuePlayerLoopUpdate();
				SceneView.RepaintAll();
			}

			public static void Clear()
			{
				if (text != null)
					Object.DestroyImmediate(text.gameObject);
				
				if (canvas != null)
					Object.DestroyImmediate(canvas.gameObject);
				
				if (eventSystem != null && eventSystem.name == EventSystemName)
					Object.DestroyImmediate(eventSystem.gameObject);

				Refresh();
			}

			public static void Show(string message, int size, Color color)
			{
				if (m_message == message && m_size == size && m_color == color)
					return;

				m_message = message;
				m_size = size;
				m_color = color;

				if (string.IsNullOrWhiteSpace(message))
				{
					Clear();
					return;
				}

				if (eventSystem == null)
				{
					eventSystem = Object.FindObjectOfType<EventSystem>();
				}

				if (eventSystem == null)
				{
					var esgo = new GameObject(EventSystemName) 
					{
						hideFlags = HideFlags.HideAndDontSave
					};
					eventSystem = esgo.AddComponent<EventSystem>();
				}

				if (canvas == null)
				{
					var canvasgo = GameObject.Find(CanvasName);
					if (canvasgo)
					{
						canvas = canvasgo.GetComponent<Canvas>();
					}
				}

				if (text == null)
				{
					var textgo = GameObject.Find(CanvasName + "/" + TextName);
					if (textgo)
						text = textgo.GetComponent<Text>();
				}

				if (outline == null)
				{
					var olgo = GameObject.Find(CanvasName + "/" + TextName);
					if (olgo)
						outline = olgo.GetComponent<Outline>();
				}					

				if (canvas == null)
				{
					var canvasgo = new GameObject(CanvasName) 
					{
						hideFlags = HideFlags.HideAndDontSave
					};
					canvas = canvasgo.AddComponent<Canvas>();
					canvas.sortingOrder = 1000;
					canvas.renderMode = RenderMode.ScreenSpaceOverlay;
					canvasgo.AddComponent<CanvasScaler>();
				}

				if (text == null)
				{
					var textgo = new GameObject(TextName) 
					{
						hideFlags = HideFlags.HideAndDontSave
					};
					textgo.AddComponent<CanvasRenderer>();
					textgo.transform.SetParent(canvas.transform);
					text = textgo.AddComponent<Text>();
					text.alignment = TextAnchor.MiddleCenter;
					text.horizontalOverflow = HorizontalWrapMode.Overflow;
					text.verticalOverflow = VerticalWrapMode.Overflow;
					text.raycastTarget = false;
					text.rectTransform.pivot = new Vector2(0.5f, 0);
					text.rectTransform.anchorMin = Vector2.zero;
					text.rectTransform.anchorMax = new Vector2(1, 0);
					text.rectTransform.anchoredPosition = Vector2.one;
					text.rectTransform.sizeDelta = new Vector2(0, 100);
					text.rectTransform.anchoredPosition = Vector2.zero;
					if (Application.isPlaying)
						text.font = (Font)Resources.GetBuiltinResource(typeof(Font), "Arial.ttf");

					outline = textgo.AddComponent<Outline>();
					outline.effectDistance = Vector2.one;
				}

				text.text = message;
				text.fontSize = size;
				text.color = color;
				outline.effectColor = (Luminance(color) > 0.5f)? Color.black:Color.white;

				Refresh();
			}

			public static void ShowTipLight(Color color, int flashCount, float sec) 
			{
				if (!DisplaySubtitleMode.ugui)
					return;

				StartCoroutine(TipLight(color, flashCount, sec), 0);
			}

			static IEnumerator TipLight(Color color, float flashCount, float sec) 
			{
				var oriColor = outline.effectColor;
				var effectColor = outline.effectColor;
				for (int i = 0; i < flashCount; i++)
				{
					var j = i % 2;
					effectColor = (j == 0)? oriColor : color;
					effectColor.a = 1;
					if(outline != null)
						outline.effectColor = effectColor;

					Refresh();
					yield return new WaitForSeconds(sec);
				}
				effectColor = oriColor;
				effectColor.a = 1;
				if (outline != null)
					outline.effectColor = effectColor;

				Refresh();
			}

			private static float Luminance(Color color) 
			{
				return 0.2125f * color.r + 0.7154f * color.g + 0.0721f * color.b;
			}
		}
	}
}
/*
 * ///////////////////////////////////////////
 * ///Copyright© SuWen All Rights Reserved.///
 * ///////////////////////////////////////////
 */