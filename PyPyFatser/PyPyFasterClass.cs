//#define ENABLE_TEST
using System;
using UnityEngine;
using MelonLoader;
using UnityEngine.Video;
using VRC.Core;
using VRC.SDKBase;
using UnityEngine.UI;
using System.Collections;
using Il2CppSystem.Text.RegularExpressions;
using Il2CppSystem.IO;
using System.Net;

namespace PyPyFatser
{
    public class PyPyFasterClass : MelonMod
    {
        const string sceneName = "W_DanceV2";
        const string bluePrintID = "wrld_f20326da-f1ac-45fc-a062-609723b097b1";
        const string urlPattern = @"[^/\\]*$";
        const string content1 = "storage";
        const string content2 = "llss.io";
        const string content3 = "jd.pypy.moe";
        readonly string cachePath;
        VideoPlayer videoPlayerInstance;
        Text textInstance;
        VideoPlayer VideoPlayerInstance
        {
            set
            {
                OutputLog("Set videoPlayerInstance=" + value, true, ConsoleColor.White, (value != null || !active) ? 0 : 2);
                videoPlayerInstance = value;
            }
            get
            {
                return videoPlayerInstance;
            }
        }
        Text TextInstance
        {
            set
            {
                OutputLog("Set textInstance=" + value, true, ConsoleColor.White, (value != null || !active) ? 0 : 2);
                textInstance = value;
            }
            get
            {
                return textInstance;
            }
        }
        object routine;
        object rountine2;
        bool active;
        string lastRecordedText;
        string LastRecordedText
        {
            set
            {
                if (lastRecordedText != value)
                {
                    OutputLog("Text Changed," + lastRecordedText + "->" + value, true);
                    if (active && VideoPlayerInstance != null)
                    {

                    }
                }
                lastRecordedText = value;
            }
            get
            {
                return lastRecordedText;
            }
        }
        string lastRecordedURL;
        string LastRecordedURL
        {
            set
            {
                if (lastRecordedURL != value)
                {
                    OutputLog("URL Changed," + lastRecordedURL + "->" + value, true);
                    if (active && VideoPlayerInstance != null)
                    {
                        if (value == null || value == "" || cachePath == null || cachePath == "")
                        {

                        }
                        else if (value.Contains(content1) && value.Contains(content2))
                        {
                            var match = Regex.Match(value, urlPattern);
                            OutputLog("Matching File:" + match.Value, true);
                            if (File.Exists(cachePath + "\\" + match.Value))
                            {
                                MelonCoroutines.Start(WaittingForNextFramePlayUrl(cachePath + "\\" + match.Value));                           
                            }
                            else
                            {
                                OutputLog("DL File:" + match.Value, false, ConsoleColor.Green);
                                WebClient webClient = new WebClient();
                                webClient.DownloadFileTaskAsync(value, cachePath + "\\" + match.Value);
                            }
                        }
                        else if(value.Contains(content3))
                        {
                            var match = Regex.Match(value, urlPattern);
                            OutputLog("Matching File:" + match.Value, true);
                            if (File.Exists(cachePath + "\\" + match.Value))
                            {
                                MelonCoroutines.Start(WaittingForNextFramePlayUrl(cachePath + "\\" + match.Value));                              
                            }
                            else
                            {
                                OutputLog("DL File:" + match.Value, false, ConsoleColor.Green);
                                WebClient webClient = new WebClient();
                                webClient.DownloadFileTaskAsync(value, cachePath + "\\" + match.Value);
                            }
                        }
                    }
                }
                lastRecordedURL = value;
            }
            get
            {
                return lastRecordedURL;
            }
        }

        MelonPreferences_Category category;

        public PyPyFasterClass()
        {
            if (MelonPreferences.GetCategory("PyPyFaster") == null)
            {
                category = MelonPreferences.CreateCategory("PyPyFaster");
                category.CreateEntry("Cache Path", Environment.CurrentDirectory + "\\PyPyCache");
                MelonPreferences.Save();
            }
            else
            {
                category.LoadFromFile();
                if (category.GetEntry<string>("Cache Path") == null)
                {
                    category.CreateEntry("Cache Path", Environment.CurrentDirectory + "\\PyPyCache");
                    MelonPreferences.Save();
                }
            }
            cachePath = category.GetEntry<string>("Cache Path").Value;
            if (!Directory.Exists(cachePath))
                Directory.CreateDirectory(cachePath);
        }
        public override void OnSceneWasLoaded(int buildIndex, string sceneName)
        {
            active = false;
            if (buildIndex == -1 && sceneName == PyPyFasterClass.sceneName)
            {
                routine = MelonCoroutines.Start(WaittingForObjectInstantiated());
            }
            else
            {
                if (routine != null)
                {
                    MelonCoroutines.Stop(routine);
                    routine = null;
                }
                VideoPlayerInstance = null;
                TextInstance = null;
            }

        }

        public override void OnUpdate()
        {
#if ENABLE_TEST
            if (true)
            {
                if (Input.GetKeyDown(KeyCode.T))
                {
                    OutputLog("Get videoPlayerInstance=" + VideoPlayerInstance, true, ConsoleColor.White, VideoPlayerInstance != null ? 0 : 2);
                    OutputLog("Get videoPlayerInstance=" + TextInstance, true, ConsoleColor.White, TextInstance != null ? 0 : 2);
                    OutputLog(category.GetEntry<string>("Cache Path").Value, true);
                }
                if (Input.GetKey(KeyCode.Y))
                {
                    OutputLog(VideoPlayerInstance.url, true, ConsoleColor.White, VideoPlayerInstance != null ? 0 : 2);
                    OutputLog(TextInstance.text, true, ConsoleColor.White, TextInstance != null ? 0 : 2);
                }
            }
#endif

            if (active && TextInstance != null)
            {
                LastRecordedText = TextInstance.text;
            }

            if (active && VideoPlayerInstance != null)
            {
                LastRecordedURL = VideoPlayerInstance.url;
                if(GameObject.Find("videoerror")!=null&& rountine2==null)
                {
                    OutputLog("videoerror,", false,ConsoleColor.Yellow);
                    rountine2 = MelonCoroutines.Start(TryToRebornPlayer());
                }
            }
        }

        IEnumerator WaittingForObjectInstantiated()
        {
            float count = 0;
            bool allowSetValue = true;
            while (active == false)
            {
                if (VRC_SceneDescriptor.Instance.gameObject != null)
                {
                    if (VRC_SceneDescriptor.Instance.gameObject.GetComponent<PipelineManager>() != null)
                    {
                        if (VRC_SceneDescriptor.Instance.gameObject.GetComponent<PipelineManager>().blueprintId == bluePrintID)
                        {
                            OutputLog("Joined PyPyDance!", false, ConsoleColor.Blue);
                            lastRecordedText = null;
                            active = true;
                        }
                        break;
                    }
                }
                yield return null;
            }
            if (active)
            {
                while (GameObject.Find("videoplayer") == null || GameObject.Find("videotitle") == null ||
                    GameObject.Find("videoplayer").GetComponent<VideoPlayer>() == null || GameObject.Find("videotitle").GetComponent<Text>() == null)
                {
                    if (count > 10)
                    {
                        OutputLog("Mod Failure or Try To Reload World", false, default, 1);
                        allowSetValue = false;
                        break;
                    }
                    yield return null;
                    count += Time.deltaTime;
                }
                if (allowSetValue)
                {
                    VideoPlayerInstance = GameObject.Find("videoplayer").GetComponent<VideoPlayer>();
                    TextInstance = GameObject.Find("videotitle").GetComponent<Text>();
                }
            }

            if (routine != null)
                routine = null;
        }
        IEnumerator WaittingForNextFramePlayUrl(string url)
        {
            yield return null;
            VideoPlayerInstance.url = url;
           // VideoPlayerInstance.Play();
            OutputLog("Load Local File:" + url, false, ConsoleColor.Blue);
        }
        IEnumerator TryToRebornPlayer()
        {
            OutputLog("StartCoroutines:TryToRebornPlayer", false, ConsoleColor.Yellow);
            while (GameObject.Find("videoerror") != null)
            {
                OutputLog("TryToRebornPlayer:GameObject.Find(\"videoerror\") != null", true, ConsoleColor.Yellow);
                if (VideoPlayerInstance == null)
                    break;
                VideoPlayerInstance.gameObject.SetActive(false);
                VideoPlayerInstance.gameObject.SetActive(true);
                yield return new WaitForSecondsRealtime(2);
            }      
            rountine2 = null;
            OutputLog("EndCoroutines:TryToRebornPlayer", false, ConsoleColor.Yellow);
        }


        public void OutputLog(object message, bool debug = false, ConsoleColor consoleColor = ConsoleColor.White, int logType = 0)
        {
            if (message == null)
            {
                message = "null";
            }
            if (debug)
            {
#if ENABLE_TEST
                message = "debug:" + message;
                switch (logType)
                {
                    case 0:
                        LoggerInstance.Msg(consoleColor, message);
                        break;
                    case 1:
                        LoggerInstance.Warning(message);
                        break;
                    case 2:
                        LoggerInstance.Error(message);
                        break;
                    default:
                        LoggerInstance.Msg(consoleColor, message);
                        break;
                }
#endif
            }
            else
            {
                switch (logType)
                {
                    case 0:
                        LoggerInstance.Msg(consoleColor, message);
                        break;
                    case 1:
                        LoggerInstance.Warning(message);
                        break;
                    case 2:
                        LoggerInstance.Error(message);
                        break;
                    default:
                        LoggerInstance.Msg(consoleColor, message);
                        break;
                }
            }
        }
    }


}
