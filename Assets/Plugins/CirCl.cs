using System;
using System.Net;
using System.Net.Sockets;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CirCl : MonoBehaviour
{
    public Text stateText;
    public static Button[] uiButtons;
    public static int uiIndex = 0;
    public static float uiTime = 0.4f;
    public delegate void OnWiFiInput(int[,] controller);
    public static event OnWiFiInput onWiFiInput;
    public delegate void OnBluetoothInput(int[,] controller);
    public static event OnBluetoothInput onBluetoothInput;

    // wifi connection state
    public static string connectionStateWiFi = "";
    public static bool connectedViaWiFi = false;
    private static bool readingWiFiActive = false;

    // bluetooth connection state
    public static string connectionStateBle = "";
    public static bool connectedViaBluetooth = false;
    private static bool readingBleActive = false;

    // using 8 player inputs from input manager settings
    // horizontal, vertical, button_0, button_1
    public static string[,] controllerArray = new string[,] {
        { "P1_Horizontal", "P1_Vertical", "P1_Button0", "P1_Button1" },
        { "P2_Horizontal", "P2_Vertical", "P2_Button0", "P2_Button1" },
        { "P3_Horizontal", "P3_Vertical", "P3_Button0", "P3_Button1" },
        { "P4_Horizontal", "P4_Vertical", "P4_Button0", "P4_Button1" },
        { "P5_Horizontal", "P5_Vertical", "P5_Button0", "P5_Button1" },
        { "P6_Horizontal", "P6_Vertical", "P6_Button0", "P6_Button1" },
        { "P7_Horizontal", "P7_Vertical", "P7_Button0", "P7_Button1" },
        { "P8_Horizontal", "P8_Vertical", "P8_Button0", "P8_Button1" }
        };

    // using 8 player keycodes from usb interface
    // left, right, down, up, button_0, button_1
    public static KeyCode[,] controllerUsbArray = new KeyCode[,] {
        { KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E, KeyCode.F },
        { KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J, KeyCode.K, KeyCode.L },
        { KeyCode.M, KeyCode.N, KeyCode.O, KeyCode.P, KeyCode.Q, KeyCode.R },
        { KeyCode.S, KeyCode.T, KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X },
        { KeyCode.Alpha1, KeyCode.Alpha2, KeyCode.Alpha3, KeyCode.Alpha4, KeyCode.Alpha5, KeyCode.Alpha6 },
        { KeyCode.Alpha7, KeyCode.Alpha8, KeyCode.Alpha9, KeyCode.Alpha0, KeyCode.Backspace, KeyCode.Tab },
        { KeyCode.F1, KeyCode.F2, KeyCode.F3, KeyCode.F4, KeyCode.F5, KeyCode.F6 },
        { KeyCode.LeftArrow, KeyCode.RightArrow, KeyCode.DownArrow, KeyCode.UpArrow, KeyCode.Space, KeyCode.Return }
        };

    // using 8 player values from wifi/bluetooth interface
    // horizontal, vertical, angle, pitch, roll, button_0 & button_1
    public static int[,] controllerWirelessArray = new int[,] {
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
        };

    private static bool[,] buttonDownArray = new bool[,] {
        { false, false },
        { false, false },
        { false, false },
        { false, false },
        { false, false },
        { false, false },
        { false, false },
        { false, false },
    };
    private static bool[,] buttonUpArray = new bool[,] {
        { false, false },
        { false, false },
        { false, false },
        { false, false },
        { false, false },
        { false, false },
        { false, false },
        { false, false },
    };

    private void Start()
    {
#if UNITY_EDITOR
        StartWiFiProcess();
#else
        StartBleProcess();
# endif
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        uiButtons = FindObjectsOfType<Button>();
        uiIndex = 0;
    }

#region Testing + Debugging
    //int changes = 0;
    //bool test = false;

    //private void Update()
    //{
    //    //stateText.text = controllerWirelessArray[0, 0].ToString();
    //    if (controllerWirelessArray[0, 5] == 16)
    //    {

    //        //packageCounter += 1;
    //        if (controllerWirelessArray[0, 0] == 1 && test == false)
    //        {
    //            test = true;
    //        }
    //        if (controllerWirelessArray[0, 0] == 2 && test == true)
    //        {
    //            test = false;
    //            changes += 1;
    //            stateText.text = changes.ToString() + " | " + packageCounter.ToString();
    //        }
    //        //if(packageTimer < 0)
    //        //{
    //        //    stateText.text += controllerWirelessArray[0, 0].ToString() + "\n";
    //        //    packageTimer = 0.05f;
    //        //}
    //    }
    //    else
    //    {
    //        packageTimer -= Time.deltaTime;
    //    }
    //    //else
    //    //{
    //    //    packageCounter = 0;
    //    //}
    //    if (packageTimer < 0f)
    //    {
    //        packageTimer = 1f;
    //        changes = 0;
    //        packageCounter = 0;
    //        stateText.text = changes.ToString() + " | " + packageCounter.ToString();
    //    }
    //}
#endregion

#region Controller Input
    public static void SelectUiButton(int player)
    {
        if(connectedViaWiFi || connectedViaBluetooth)
        {
            uiTime -= Time.deltaTime;
            if(uiTime < 0f)
            {
                if (controllerWirelessArray[player, 0] == 1)
                {
                    uiTime = 0.4f;
                    uiIndex -= 1;
                    if (uiIndex < 0)
                    {
                        uiIndex = uiButtons.Length - 1;
                    }
                    if (uiButtons[uiIndex].interactable == true)
                    {
                        uiButtons[uiIndex].Select();
                        uiButtons[uiIndex].gameObject.SetActive(true);
                    }
                    else
                    {
                        uiTime = 0f;
                    }
                }
                else if (controllerWirelessArray[player, 0] == 2)
                {
                    uiTime = 0.4f;
                    uiIndex += 1;
                    if (uiIndex > uiButtons.Length - 1)
                    {
                        uiIndex = 0;
                    }
                    if (uiButtons[uiIndex].interactable == true)
                    {
                        uiButtons[uiIndex].Select();
                        uiButtons[uiIndex].gameObject.SetActive(true);
                    }
                    else
                    {
                        uiTime = 0f;
                    }
                }
                else if (controllerWirelessArray[player, 5] == 1)
                {
                    uiTime = 0.4f;
                    if (uiButtons[uiIndex].interactable == true)
                    {
                        uiButtons[uiIndex].onClick.Invoke();
                    }
                    else
                    {
                        uiTime = 0f;
                    }
                }
            }
        }
    }

    public static bool GetButton(int player, int button)
    {
        if(connectedViaWiFi || connectedViaBluetooth)
        {
            if(button == 0)
            {
                if (controllerWirelessArray[player, 5] == 16 || controllerWirelessArray[player, 5] == 17)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            else
            {
                if (controllerWirelessArray[player, 5] == 1 || controllerWirelessArray[player, 5] == 17)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
        }
        else
        {
            return Input.GetKey(controllerUsbArray[player, 4 + button]);
        }
    }

    public static bool GetButtonDown(int player, int button)
    {
        if (connectedViaWiFi || connectedViaBluetooth)
        {
            if (controllerWirelessArray[player, 5] == 0)
            {
                buttonDownArray[player, 0] = false;
                buttonDownArray[player, 1] = false;
                return false;
            }
            else if (controllerWirelessArray[player, 5] == 1 && buttonDownArray[player, 1] == false && button == 1)
            {
                buttonDownArray[player, 0] = false;
                buttonDownArray[player, 1] = true;
                return true;
            }
            else if (controllerWirelessArray[player, 5] == 16 && buttonDownArray[player, 0] == false && button == 0)
            {
                buttonDownArray[player, 0] = true;
                buttonDownArray[player, 1] = false;
                return true;
            }
            else
            {
                return false;
            }

        }
        else
        {
            return Input.GetKeyDown(controllerUsbArray[player, 4 + button]);
        }
    }

    public static bool GetButtonUp(int player, int button)
    {
        if (connectedViaWiFi || connectedViaBluetooth)
        {
            if (controllerWirelessArray[player, 5] == 17)
            {
                buttonUpArray[player, 0] = false;
                buttonUpArray[player, 1] = false;
                return false;
            }
            else if (controllerWirelessArray[player, 5] == 16 && buttonUpArray[player, 1] == false && button == 1)
            {
                buttonUpArray[player, 0] = false;
                buttonUpArray[player, 1] = true;
                return true;
            }
            else if (controllerWirelessArray[player, 5] == 1 && buttonUpArray[player, 0] == false && button == 0)
            {
                buttonUpArray[player, 0] = true;
                buttonUpArray[player, 1] = false;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return Input.GetKeyUp(controllerUsbArray[player, 4 + button]);
        }
    }

    public static float GetAxis(int player, char axis)
    {
        if(axis == 'h')
        {
            if (connectedViaWiFi || connectedViaBluetooth)
            {
                if (controllerWirelessArray[player, 0] == 1)
                {
                    return -1.0f;
                }
                else if (controllerWirelessArray[player, 0] == 2)
                {
                    return 1.0f;
                }
            }
            else
            {
                if (Input.GetKey(controllerUsbArray[player, 0]))
                {
                    return -1.0f;
                }
                else if (Input.GetKey(controllerUsbArray[player, 1]))
                {
                    return 1.0f;
                }
            }
        }
        else if(axis == 'v')
        {
            if (connectedViaWiFi || connectedViaBluetooth)
            {
                if (controllerWirelessArray[player, 1] == 1)
                {
                    return -1.0f;
                }
                else if (controllerWirelessArray[player, 1] == 2)
                {
                    return 1.0f;
                }
            }
            else
            {
                if (Input.GetKey(controllerUsbArray[player, 2]))
                {
                    return -1.0f;
                }
                else if (Input.GetKey(controllerUsbArray[player, 3]))
                {
                    return 1.0f;
                }
            }
        }
        return 0.0f;
    }
#endregion

#region Console WiFi Interface
    //-------------------------------------------------------------------------
    private UdpClient clientData;
    private IPEndPoint ipEndPointData;
    private int portData = 4321;
    private int receiveBufferSize = 72;
    private object obj = null;
    private AsyncCallback AC;
    private int packageCounter = 0;
    private float packageTimer = 1f;

    private string StatusWiFi
    {
        set
        {
            stateText.text = value.ToString();
        }
    }

    private void StartWiFiProcess()
    {
        if (readingWiFiActive == false)
        {
            readingWiFiActive = true;
            InitializeUDPListener();
            //socketThread = new Thread(new ThreadStart(Test));
            //socketThread.IsBackground = true;
            //socketThread.Start();
        }
    }

    private void ReceiveWiFiData(byte[] byte_array)
    {
        for (int i = 0; i < 8; i++)
        {
            controllerWirelessArray[i, 0] = byte_array[i * 9];
            controllerWirelessArray[i, 1] = byte_array[i * 9 + 1];
            controllerWirelessArray[i, 2] = BitConverter.ToInt16(new byte[] { byte_array[i * 9 + 3], byte_array[i * 9 + 2] }, 0);
            controllerWirelessArray[i, 3] = BitConverter.ToInt16(new byte[] { byte_array[i * 9 + 5], byte_array[i * 9 + 4] }, 0);
            controllerWirelessArray[i, 4] = BitConverter.ToInt16(new byte[] { byte_array[i * 9 + 7], byte_array[i * 9 + 6] }, 0);
            controllerWirelessArray[i, 5] = byte_array[i * 9 + 8];
        }
        //packageCounter += 1;
        //onWiFiInput(controllerWirelessArray);
        connectedViaWiFi = true;
    }

    public void InitializeUDPListener()
    {
        ipEndPointData = new IPEndPoint(IPAddress.Any, portData);
        clientData = new UdpClient();
        clientData.Client.ReceiveBufferSize = receiveBufferSize;
        clientData.EnableBroadcast = true;
        clientData.Client.Bind(ipEndPointData);
        AC = new AsyncCallback(ReceivedUDPPacket);
        clientData.BeginReceive(AC, obj);
        StatusWiFi = "";
    }

    void ReceivedUDPPacket(IAsyncResult result)
    {
        //stopwatch.Start();
        byte[] receivedBytes = clientData.EndReceive(result, ref ipEndPointData);

        ReceiveWiFiData(receivedBytes);

        clientData.BeginReceive(AC, obj);
    }


    void OnDestroy()
    {
        if (clientData != null)
        {
            clientData.Close();
        }
    }
#endregion

#region Console BLE Interface
    //-------------------------------------------------------------------------
    private static float timer = 0f;
    private static string _deviceName = "CirCl";
    private static string _serviceUUID = "e80f2064-7a9e-4c5b-91bb-ae5557952a0c";
    private static string _characteristicUUID = "e5282bd6-337a-4244-b4f0-0e73bd4c34b5";
    private static float _timeout = 0f;
    private static States _state = States.Idle;
    private static string _deviceAddress;
    private static bool _foundCharacteristicUUID = false;
    private static bool _rssiOnly = false;
    private enum States
    {
        Idle,
        Scan,
        ScanRSSI,
        ReadRSSI,
        Read,
        Connect,
        RequestMTU,
        Subscribe,
        Waiting,
        Unsubscribe,
        Disconnect,
    }

    private string StatusBle
    {
        set
        {
            //BluetoothLEHardwareInterface.Log(value);
            connectionStateBle = value.ToString();
            stateText.text = value.ToString();
        }
    }

    private void Reset()
    {
        connectedViaBluetooth = false;
        _timeout = 0f;
        _state = States.Idle;
        _deviceAddress = null;
        _foundCharacteristicUUID = false;
    }

    private void SetState(States newState, float timeout)
    {
        _state = newState;
        _timeout = timeout;
    }

    private void ReceiveBleData(byte[] byte_array)
    {
        for (int i = 0; i < 8; i++)
        {
            controllerWirelessArray[i, 0] = byte_array[i * 9];
            controllerWirelessArray[i, 1] = byte_array[i * 9 + 1];
            controllerWirelessArray[i, 2] = BitConverter.ToInt16(new byte[] { byte_array[i * 9 + 3], byte_array[i * 9 + 2] }, 0);
            controllerWirelessArray[i, 3] = BitConverter.ToInt16(new byte[] { byte_array[i * 9 + 5], byte_array[i * 9 + 4] }, 0);
            controllerWirelessArray[i, 4] = BitConverter.ToInt16(new byte[] { byte_array[i * 9 + 7], byte_array[i * 9 + 6] }, 0);
            controllerWirelessArray[i, 5] = byte_array[i * 9 + 8];
        }
        onBluetoothInput(controllerWirelessArray);
    }

    private string FullUUID(string uuid)
    {
        string fullUUID = uuid;
        if (fullUUID.Length == 4)
            fullUUID = "0000" + uuid + "-0000-1000-8000-00805f9b34fb";

        return fullUUID;
    }

    private bool IsEqual(string uuid1, string uuid2)
    {
        if (uuid1.Length == 4)
            uuid1 = FullUUID(uuid1);
        if (uuid2.Length == 4)
            uuid2 = FullUUID(uuid2);

        return (uuid1.ToUpper().Equals(uuid2.ToUpper()));
    }

    private void StartBleProcess()
    {
        Reset();
        BluetoothLEHardwareInterface.Initialize(true, false, () =>
        {
            SetState(States.Scan, 0.1f);
        }, (error) =>
        {
            StatusBle = "";// + error;
        });
        if (readingBleActive == false)
        {
            readingBleActive = true;
            StartCoroutine(ConnectBluetoothInterface());
        }
    }

    IEnumerator ConnectBluetoothInterface()
    {
        while (true)
        {
            timer += Time.deltaTime;
            if (_timeout > 0f)
            {
                _timeout -= Time.deltaTime;
                if (_timeout <= 0f)
                {
                    _timeout = 0f;
                    switch (_state)
                    {
                        case States.Idle:
                            break;
                        case States.Scan:
                            StatusBle = "SCANNING FOR CIRCL ...";
                            BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null, (address, name) =>
                            {
                                if (!_rssiOnly)
                                {
                                    if (name.Contains(_deviceName))
                                    {
                                        StatusBle = "FOUND CIRCL!";
                                        _deviceAddress = address;
                                        SetState(States.Connect, 0.5f);
                                    }
                                }
                            }, (address, name, rssi, bytes) =>
                            {
                                if (name.Contains(_deviceName))
                                {
                                    StatusBle = "FOUND CIRCL!";
                                    _deviceAddress = address;
                                    SetState(States.Connect, 0.5f);
                                }
                            }, false);
                            break;
                        case States.Connect:
                            StatusBle = "CONNECTING TO CIRCL ...";
                            _foundCharacteristicUUID = false;
                            BluetoothLEHardwareInterface.ConnectToPeripheral(_deviceAddress, null, null, (address, serviceUUID, characteristicUUID) =>
                            {
                                StatusBle = "CONNECTED!";
                                BluetoothLEHardwareInterface.StopScan();
                                if (IsEqual(serviceUUID, _serviceUUID))
                                {
                                    StatusBle = "FOUND CIRCL!";
                                    _foundCharacteristicUUID = _foundCharacteristicUUID || IsEqual(characteristicUUID, _characteristicUUID);
                                    if (_foundCharacteristicUUID)
                                    {
                                        connectedViaBluetooth = true;
                                        SetState(States.RequestMTU, 2f);
                                    }
                                }
                            });
                            break;
                        case States.RequestMTU:
                            StatusBle = "REQUESTING MTU ...";
                            BluetoothLEHardwareInterface.RequestMtu(_deviceAddress, 76, (address, newMTU) =>
                            {
                                StatusBle = "MTU SET TO " + newMTU.ToString();

                                SetState(States.Subscribe, 0.1f);
                            });
                            break;
                        case States.Subscribe:
                            StatusBle = "SUBSCRIBING TO CIRCL ...";
                            BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(_deviceAddress, _serviceUUID, _characteristicUUID, (notifyAddress, notifyCharacteristic) =>
                            {
                                BluetoothLEHardwareInterface.ReadCharacteristic(_deviceAddress, _serviceUUID, _characteristicUUID, (characteristic, bytes) =>
                                {
                                    ReceiveBleData(bytes);
                                });
                            }, (address, characteristicUUID, bytes) =>
                            {
                                timer = 0f;
                                ReceiveBleData(bytes);
                            });
                            SetState(States.Waiting, 0.1f);
                            break;
                        case States.Waiting:
                            if (timer > 5f)
                            {
                                SetState(States.Scan, 0.1f);
                            }
                            else
                            {
                                _timeout = 1f;
                            }
                            break;
                        case States.Unsubscribe:
                            BluetoothLEHardwareInterface.UnSubscribeCharacteristic(_deviceAddress, _serviceUUID, _characteristicUUID, null);
                            SetState(States.Disconnect, 4f);
                            break;
                        case States.Disconnect:
                            StatusBle = "DISCONNECTED!";
                            if (connectedViaBluetooth)
                            {
                                BluetoothLEHardwareInterface.DisconnectPeripheral(_deviceAddress, (address) =>
                                {
                                    StatusBle = "DISCONNECTED!";
                                    BluetoothLEHardwareInterface.DeInitialize(() =>
                                    {
                                        connectedViaBluetooth = false;
                                        _state = States.Idle;
                                    });
                                });
                            }
                            else
                            {
                                BluetoothLEHardwareInterface.DeInitialize(() =>
                                {
                                    _state = States.Idle;
                                });
                            }
                            break;
                    }
                }
            }
            yield return new WaitForSeconds(0.001f);
        }
    }
#endregion
}
