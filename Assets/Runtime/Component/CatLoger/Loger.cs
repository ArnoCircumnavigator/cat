//using System;
//using System.Collections.Generic;
//using System.IO;
//using System.Linq;
//using System.Text;
//using System.Threading;
//using UnityEngine;

//namespace Cat
//{
//    /// <summary>
//    /// 日志组件 注:此模块在编辑器模式的非play期间,不会将日志写入文件
//    /// </summary>
//    public class Loger : ILoger
//    {
//        /// <summary>
//        /// Unity Console日志？
//        /// </summary>
//        public static bool UnityDebugSwitch = true;

//        static Timer timer = null;
//        static bool fileLogSwitch = false;
//        /// <summary>
//        /// 文件日志？
//        /// </summary>
//        public static bool FileLogSwitch
//        {
//            get
//            {
//                return fileLogSwitch;
//            }
//            set
//            {
//                if (!value.Equals(fileLogSwitch))
//                {
//                    if (value)//从否 变为 真
//                    {
//                        try { timer?.Dispose(); timer = null; } catch { }
//                        //创建定时器
//                        timer = new Timer(new TimerCallback(LogToFile), null, 0, 100);

//#if UNITY_EDITOR
//                        UnityEditor.EditorApplication.playModeStateChanged += (e) =>
//                        {
//                            if (e == UnityEditor.PlayModeStateChange.ExitingPlayMode)
//                            {
//                                timer?.Dispose();
//                                timer = null;
//                            }
//                        };
//#endif
//                    }
//                    else//从 真变否
//                    {
//                        //释放定时器
//                        timer?.Dispose();
//                        timer = null;
//                    }
//                }
//                fileLogSwitch = value;
//            }
//        }

//        static string fileLogPath = string.Empty;
//        /// <summary>
//        /// 文件路径(如果路径不存在，Loger会尝试创建)
//        /// </summary>
//        public static string FileLogPath
//        {
//            get
//            {
//                if (string.IsNullOrEmpty(fileLogPath))
//                    fileLogPath = Path.Combine(new DirectoryInfo(Application.dataPath).Parent.FullName, "catlogs");
//                return fileLogPath;
//            }
//            set
//            {
//                fileLogPath = value;
//            }
//        }

//        /// <summary>
//        /// 所需日志级别
//        /// </summary>
//        public static LogType LogLevel = LogType.Info;

//        //提高写文件日志的效率
//        static readonly Queue<string> messages_waiting_to_file = new Queue<string>();

//        //日志前缀，用于区分模块
//        readonly string prefix;

//        public Loger(object prefix)
//        {
//            this.prefix = $"[{prefix}] ";
//        }

//        public void Log(object message, UnityEngine.Object content = null)
//        {
//            Log(LogType.Info, message);
//        }

//        public void LogWarning(object message, UnityEngine.Object content = null)
//        {
//            Log(LogType.Warning, message);
//        }

//        public void LogError(object message, UnityEngine.Object content = null)
//        {
//            Log(LogType.Error, message);
//        }

//        void Log(LogType type, object message, UnityEngine.Object content = null)
//        {
//            if (type < LogLevel)
//            {
//                return;
//            }

//            if (UnityDebugSwitch)
//            {
//                switch (type)
//                {
//                    case LogType.Info:
//                        Debug.Log(message, content);
//                        break;
//                    case LogType.Warning:
//                        Debug.LogWarning(message, content);
//                        break;
//                    case LogType.Error:
//                        Debug.LogError(message, content);
//                        break;
//                }
//            }

//            if (FileLogSwitch)
//            {
//                //包装message
//                string head = $"------------------------------------\r\n{DateTime.Now:yyyy-MM-dd HH:mm:ss fff}  [{type}]\r\n";
//                string content_string = head + prefix + message.ToString() + "\n\n";

//                //使用队列，保证强有序
//                messages_waiting_to_file.Enqueue(content_string);
//            }
//        }

//        static void LogToFile(object source)
//        {
//            //Debug.Log("Log to file");
//            ThreadPool.QueueUserWorkItem((_) =>
//            {
//                lock (messages_waiting_to_file)
//                {
//                    if (!Directory.Exists(FileLogPath))
//                    {
//                        try
//                        {
//                            Directory.CreateDirectory(FileLogPath);
//                        }
//                        catch (Exception e)
//                        {
//                            Debug.LogError($"日志组件无法创建文件夹{FileLogPath},Exception:{e}");
//                            return;
//                        }
//                    }
//                    string path = Path.Combine(FileLogPath, DateTime.Now.ToString("yyyy-MM-dd HH") + ".log");
//                    FileMode mode = (File.Exists(path) ? FileMode.Append : FileMode.Create);
//                    using FileStream fileStream = new FileStream(path, mode, FileAccess.Write, FileShare.None);
//                    while (messages_waiting_to_file.Any())
//                    {
//                        var content_string = messages_waiting_to_file.Dequeue();
//                        byte[] array = Encoding.UTF8.GetBytes(content_string);
//                        fileStream.Write(array, 0, array.Length);
//                    }
//                    fileStream.Flush();
//                }
//            });
//        }
//    }
//}