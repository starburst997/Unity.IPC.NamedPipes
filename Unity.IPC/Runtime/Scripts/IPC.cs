using IpcLib.Client;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

// ReSharper disable once CheckNamespace
namespace IpcLib
{
    public class IPC : MonoBehaviour
    {
        public static IPC Instance { get; private set; }
        
        // ReSharper disable once UnusedAutoPropertyAccessor.Global
        public static bool Connected { get; private set; }
        
        public const string Tag = "IPC";

        private IpcClient _client;

        // Only useful for debugging
        public bool debug;
        public Text text;
        private bool _hasText;

        public string ipcName = "notessimo-ipc";
        
        public UnityEvent<string> message;
        public UnityEvent connect;
        public UnityEvent disconnect;

        private static string _ipcId;
        public static string GetIPC()
        {
            if (_ipcId != null) return _ipcId;
            
            // Check if we have a ipc id sent from command line
            var args = System.Environment.GetCommandLineArgs();
            var ipcId = "";
            for (var i = 0; i < args.Length; i++) {
                if (args[i] == "-ipc" && i < args.Length - 1) {
                    ipcId = args [i + 1];
                }
            }

            _ipcId = ipcId;
            return ipcId;
        }
        
        private void Awake()
        {
            if (Instance == null)
            {
                Instance = this;
                DontDestroyOnLoad(gameObject);
            }
            else
            {
                Destroy(gameObject);
            }
        }
        
        private void Start()
        {
            gameObject.tag = Tag;
            
            _hasText = text != null;

            var ipcId = GetIPC();
            var ipc = string.IsNullOrEmpty(ipcId) ? ipcName : $"{ipcName}-{ipcId}";
            _client = new IpcClient(ipc);

            Log($"IPC - {ipc}");
            
            _client.Connected += OnConnected;
            _client.Disconnected += OnDisconnected;
            _client.Message += OnMessage;

            _client.Connect();
        }

        public void Send(string data)
        {
            _client.Send(data);
        }

        private void OnMessage(string data)
        {
            message?.Invoke(data);

            Log($"IPC - Message Received {data}");
        }

        private void OnConnected()
        {
            Connected = true;
            
            connect?.Invoke();
            
            Log($"IPC - Connected");
        }

        private void OnDisconnected()
        {
            Connected = false;
            
            disconnect?.Invoke();
            
            Log($"IPC - Disconnected");
        }

        private void Log(string data)
        {
            if (!debug) return;
            
            Debug.Log(data);
            if (_hasText) text.text += $"\n{data}";
        }

        private void OnDestroy()
        {
            _client?.Stop();
        }
    }
}