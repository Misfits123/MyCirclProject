using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class CirCl : MonoBehaviour
{
    public Text debug;
    public delegate void OnBluetoothInput(int[,] controller);
    public static event OnBluetoothInput onBluetoothInput;

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

    // using 8 player values from bluetooth interface
    // horizontal, vertical, angle, pitch, roll, button_0 & button_1
    public static int[,] controllerBleArray = new int[,] {
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 },
        { 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }
        };

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

    private static float timer = 0f;
    private static string _deviceName = "CirCl";
    private static string _serviceUUID = "e80f2064-7a9e-4c5b-91bb-ae5557952a0c";
    private static string _characteristicUUID = "e5282bd6-337a-4244-b4f0-0e73bd4c34b5";
    private static bool readingActive = false;

    private static bool _connected = false;
    private static float _timeout = 0f;
    private static States _state = States.Idle;
    private static string _deviceAddress;
    private static bool _foundCharacteristicUUID = false;
    private static bool _rssiOnly = false;
    private static bool[,] button_down = new bool[,] {
        { false, false },
        { false, false },
        { false, false },
        { false, false },
        { false, false },
        { false, false },
        { false, false },
        { false, false },
    };
    private static bool[,] button_up = new bool[,] {
        { false, false },
        { false, false },
        { false, false },
        { false, false },
        { false, false },
        { false, false },
        { false, false },
        { false, false },
    };
    private static bool return_it_once = true;

    private string StatusMessage
    {
        set
        {
            BluetoothLEHardwareInterface.Log(value);
            debug.text = value.ToString();
        }
    }

    public static bool GetButton(int player, int button, bool use_bluetooth)
    {
        if(use_bluetooth)
        {
            if(button == 0)
            {
                if (controllerBleArray[player, 5] == 16 || controllerBleArray[player, 5] == 17)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
            else
            {
                if (controllerBleArray[player, 5] == 1 || controllerBleArray[player, 5] == 17)
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        else
        {
            return Input.GetKey(controllerUsbArray[player, 4 + button]);
        }
    }

    public static bool GetButtonDown(int player, int button, bool use_bluetooth)
    {
        if (use_bluetooth)
        {
            if (controllerBleArray[player, 5] == 0)
            {
                button_down[player, 0] = false;
                button_down[player, 1] = false;
                return false;
            }
            else if (controllerBleArray[player, 5] == 1 && button_down[player, 1] == false && button == 1)
            {
                button_down[player, 0] = false;
                button_down[player, 1] = true;
                return true;
            }
            else if (controllerBleArray[player, 5] == 16 && button_down[player, 0] == false && button == 0)
            {
                button_down[player, 0] = true;
                button_down[player, 1] = false;
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

    public static bool GetButtonUp(int player, int button, bool use_bluetooth)
    {
        if (use_bluetooth)
        {
            if (controllerBleArray[player, 5] == 17)
            {
                button_down[player, 0] = false;
                button_down[player, 1] = false;
                return false;
            }
            else if (controllerBleArray[player, 5] == 16 && button_down[player, 1] == false && button == 1)
            {
                button_down[player, 0] = false;
                button_down[player, 1] = true;
                return true;
            }
            else if (controllerBleArray[player, 5] == 1 && button_down[player, 0] == false && button == 0)
            {
                button_down[player, 0] = true;
                button_down[player, 1] = false;
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

    public static float GetAxis(int player, char axis, bool use_bluetooth)
    {
        if(axis == 'h')
        {
            if (use_bluetooth)
            {
                if (controllerBleArray[player, 0] == 1)
                {
                    return -1.0f;
                }
                else if (controllerBleArray[player, 0] == 2)
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
            if (use_bluetooth)
            {
                if (controllerBleArray[player, 1] == 1)
                {
                    return -1.0f;
                }
                else if (controllerBleArray[player, 1] == 2)
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

    private void Reset()
    {
        _connected = false;
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

    private void Start()
    {
        StartProcess();
    }

    private void ReceiveData(byte[] byte_array)
    {
        //int joy_id = byte_array[0];
        if(byte_array[0] == 1)
        {
            controllerBleArray[0, 0] = byte_array[1];
            controllerBleArray[0, 1] = byte_array[2];
            controllerBleArray[0, 2] = BitConverter.ToInt16(new byte[] { byte_array[4], byte_array[3] }, 0);
            controllerBleArray[0, 3] = BitConverter.ToInt16(new byte[] { byte_array[6], byte_array[5] }, 0);
            controllerBleArray[0, 4] = BitConverter.ToInt16(new byte[] { byte_array[8], byte_array[7] }, 0);
            controllerBleArray[0, 5] = byte_array[9];
            controllerBleArray[1, 0] = byte_array[10];
            controllerBleArray[1, 1] = byte_array[11];
            controllerBleArray[1, 2] = BitConverter.ToInt16(new byte[] { byte_array[13], byte_array[12] }, 0);
            controllerBleArray[1, 3] = BitConverter.ToInt16(new byte[] { byte_array[15], byte_array[14] }, 0);
            controllerBleArray[1, 4] = BitConverter.ToInt16(new byte[] { byte_array[17], byte_array[16] }, 0);
            controllerBleArray[1, 5] = byte_array[18];
        }
        if (byte_array[0] == 2)
        {
            controllerBleArray[2, 0] = byte_array[1];
            controllerBleArray[2, 1] = byte_array[2];
            controllerBleArray[2, 2] = BitConverter.ToInt16(new byte[] { byte_array[4], byte_array[3] }, 0);
            controllerBleArray[2, 3] = BitConverter.ToInt16(new byte[] { byte_array[6], byte_array[5] }, 0);
            controllerBleArray[2, 4] = BitConverter.ToInt16(new byte[] { byte_array[8], byte_array[7] }, 0);
            controllerBleArray[2, 5] = byte_array[9];
            controllerBleArray[3, 0] = byte_array[10];
            controllerBleArray[3, 1] = byte_array[11];
            controllerBleArray[3, 2] = BitConverter.ToInt16(new byte[] { byte_array[13], byte_array[12] }, 0);
            controllerBleArray[3, 3] = BitConverter.ToInt16(new byte[] { byte_array[15], byte_array[14] }, 0);
            controllerBleArray[3, 4] = BitConverter.ToInt16(new byte[] { byte_array[17], byte_array[16] }, 0);
            controllerBleArray[3, 5] = byte_array[18];
        }
        if (byte_array[0] == 3)
        {
            controllerBleArray[4, 0] = byte_array[1];
            controllerBleArray[4, 1] = byte_array[2];
            controllerBleArray[4, 2] = BitConverter.ToInt16(new byte[] { byte_array[4], byte_array[3] }, 0);
            controllerBleArray[4, 3] = BitConverter.ToInt16(new byte[] { byte_array[6], byte_array[5] }, 0);
            controllerBleArray[4, 4] = BitConverter.ToInt16(new byte[] { byte_array[8], byte_array[7] }, 0);
            controllerBleArray[4, 5] = byte_array[9];
            controllerBleArray[5, 0] = byte_array[10];
            controllerBleArray[5, 1] = byte_array[11];
            controllerBleArray[5, 2] = BitConverter.ToInt16(new byte[] { byte_array[13], byte_array[12] }, 0);
            controllerBleArray[5, 3] = BitConverter.ToInt16(new byte[] { byte_array[15], byte_array[14] }, 0);
            controllerBleArray[5, 4] = BitConverter.ToInt16(new byte[] { byte_array[17], byte_array[16] }, 0);
            controllerBleArray[5, 5] = byte_array[18];
        }
        if (byte_array[0] == 4)
        {
            controllerBleArray[6, 0] = byte_array[1];
            controllerBleArray[6, 1] = byte_array[2];
            controllerBleArray[6, 2] = BitConverter.ToInt16(new byte[] { byte_array[4], byte_array[3] }, 0);
            controllerBleArray[6, 3] = BitConverter.ToInt16(new byte[] { byte_array[6], byte_array[5] }, 0);
            controllerBleArray[6, 4] = BitConverter.ToInt16(new byte[] { byte_array[8], byte_array[7] }, 0);
            controllerBleArray[6, 5] = byte_array[9];
            controllerBleArray[7, 0] = byte_array[10];
            controllerBleArray[7, 1] = byte_array[11];
            controllerBleArray[7, 2] = BitConverter.ToInt16(new byte[] { byte_array[13], byte_array[12] }, 0);
            controllerBleArray[7, 3] = BitConverter.ToInt16(new byte[] { byte_array[15], byte_array[14] }, 0);
            controllerBleArray[7, 4] = BitConverter.ToInt16(new byte[] { byte_array[17], byte_array[16] }, 0);
            controllerBleArray[7, 5] = byte_array[18];
        }
        onBluetoothInput(controllerBleArray);
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

    private void StartProcess()
    {
        Reset();
        BluetoothLEHardwareInterface.Initialize(true, false, () =>
        {

            SetState(States.Scan, 0.1f);

        }, (error) =>
        {

            StatusMessage = "STATUS";// + error;
        });
        if (readingActive == false)
        {
            readingActive = true;
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
                            StatusMessage = "SCANNING...";// FOR " + _deviceName;
                            BluetoothLEHardwareInterface.ScanForPeripheralsWithServices(null, (address, name) =>
                            {
                                if (!_rssiOnly)
                                {
                                    if (name.Contains(_deviceName))
                                    {
                                        StatusMessage = "FOUND!";// + name;
                                        _deviceAddress = address;
                                        SetState(States.Connect, 0.5f);
                                    }
                                }
                            }, (address, name, rssi, bytes) =>
                            {
                                if (name.Contains(_deviceName))
                                {
                                    StatusMessage = "FOUND!";// + name;
                                    _deviceAddress = address;
                                    SetState(States.Connect, 0.5f);
                                }
                            }, false);
                            break;
                        case States.Connect:
                            StatusMessage = "CONNECTING...";
                            _foundCharacteristicUUID = false;
                            BluetoothLEHardwareInterface.ConnectToPeripheral(_deviceAddress, null, null, (address, serviceUUID, characteristicUUID) =>
                            {
                                StatusMessage = "CONNECTED!";
                                BluetoothLEHardwareInterface.StopScan();
                                if (IsEqual(serviceUUID, _serviceUUID))
                                {
                                    StatusMessage = "FOUND!";
                                    _foundCharacteristicUUID = _foundCharacteristicUUID || IsEqual(characteristicUUID, _characteristicUUID);
                                    if (_foundCharacteristicUUID)
                                    {
                                        _connected = true;
                                        SetState(States.Subscribe, 2f);
                                    }
                                }
                            });
                            break;
                        case States.Subscribe:
                            StatusMessage = "SUBSCRIBING...";
                            BluetoothLEHardwareInterface.SubscribeCharacteristicWithDeviceAddress(_deviceAddress, _serviceUUID, _characteristicUUID, (notifyAddress, notifyCharacteristic) =>
                            {
                                BluetoothLEHardwareInterface.ReadCharacteristic(_deviceAddress, _serviceUUID, _characteristicUUID, (characteristic, bytes) =>
                                {
                                    ReceiveData(bytes);
                                });
                            }, (address, characteristicUUID, bytes) =>
                            {
                                timer = 0f;
                                ReceiveData(bytes);
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
                            StatusMessage = "DISCONNECTED!";
                            if (_connected)
                            {
                                BluetoothLEHardwareInterface.DisconnectPeripheral(_deviceAddress, (address) =>
                                {
                                    StatusMessage = "DISCONNECTED!";
                                    BluetoothLEHardwareInterface.DeInitialize(() =>
                                    {
                                        _connected = false;
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
}
