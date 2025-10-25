using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.IO.Ports;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Runtime.InteropServices;
using System.Security.Cryptography;
using Org.BouncyCastle.Crypto;
using Org.BouncyCastle.Crypto.Parameters;
using Org.BouncyCastle.Crypto.Signers;
using Org.BouncyCastle.Math;
using Org.BouncyCastle.Crypto.EC;
using static System.Windows.Forms.VisualStyles.VisualStyleElement;
using System.IO.Hashing;
using System.Collections.Concurrent;
using System.Net.Http;
using System.Net.Security;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;
using System.Drawing;


namespace Do_an_tot_Nghiep
{

    public partial class Form1 : Form
    {
        public Stopwatch sw = new Stopwatch();
        private const string PrivateKeyPem = @"-----BEGIN RSA PRIVATE KEY-----
MIICXgIBAAKBgQClkxqa2PBLipriPONZtzwbnQP42GP4vbdplRmIArawsmIoavEm
MxWGfK3gqD9dHMnVXh85kSTK3HduX5jY2j3wvQGAh3+dEuRe83UQ/flZhA/uCG30
DHbSAaM/xwq3tLb8R9G7nn6lPTK3xDKU5/BYSeljQ0wRUDXAovz5M0AbzQIDAQAB
AoGAS7zAXLiDImricj5b2LwCWLc4C+ofwLY3Yap8JUTogGb4k3hnmoufewcpGiyb
32G9yUXmjpSf+dNjJi/AYOFjlzChNFOIbrKJvFF2W4BZbncbkL70wIDSVttxAwkl
QtbfFUm0blcHYtz1Z3TTrc/KWStnCiaHLZX4/X5W2OCuUdkCQQDY+i0DfRpj3tq8
U3nz5dmJT4Rv6djKJMSieMeJNMnb0dzHE/BlyBEZ/8x5XOEU9ih3HmfusbYblRIT
yHA4MvP/AkEAw1pOTNrIRemaDKZFHxwZ0wIQ7ugpN2ZT9j/iB1m6tgUcbAJ4CI1e
FDN6q4/GV65l8xIe0yGfW9qInXsCsOaAMwJBALftCi9E4xP5my29DUmkc4yj7T34
2p4wIMcg3vP93YcLFL1kt4lv2J9TaP8PrnTYXWDsU2nRFu+2o8ZFSGs7Nf0CQQCW
QZeINZ9lZtA+eoUf8JVMX4J2v8sz+VkLRwerb7DU4AmEakG5EkMSdqYb33JHbrSr
yXw8GNhh5iy+NCdVuXVRAkEAtNKLM+NlJQppA80S147F32+jCRDr9JKsq6HRAMQZ
l6NS/JxhZcbVkwljGWlXv0BXVMzndFCkKUWXOuM2Dxmn/w==
-----END RSA PRIVATE KEY-----";
        private readonly SimpleDataClient _dataClient;
        private const string PublicKeyPem = @"-----BEGIN PUBLIC KEY-----
MIGfMA0GCSqGSIb3DQEBAQUAA4GNADCBiQKBgQClkxqa2PBLipriPONZtzwbnQP4
2GP4vbdplRmIArawsmIoavEmMxWGfK3gqD9dHMnVXh85kSTK3HduX5jY2j3wvQGA
h3+dEuRe83UQ/flZhA/uCG30DHbSAaM/xwq3tLb8R9G7nn6lPTK3xDKU5/BYSelj
Q0wRUDXAovz5M0AbzQIDAQAB
-----END PUBLIC KEY-----";
        private volatile Boolean OTP_IS_OK = false;
        private readonly List<byte> rxBuffer = new List<byte>();
        private readonly object rxLock = new object();
        private readonly ConcurrentQueue<stm32response_t> responseQueue = new ConcurrentQueue<stm32response_t>();
        private const int RESPONSE_LEN = 7; // kích thước frame phản hồi từ STM32 (type + 6 fields)
        AutoResetEvent waitResponse = new AutoResetEvent(false);
        private bool veryfiedOtp = false;
        private byte[] OTPCode = new byte[6]; // Mã OTP 6 chữ số
        private volatile string receivedData = string.Empty;
        private volatile string dataReceived = string.Empty;
        private static List<byte> dataBytes = new List<byte>();
        private static int countLine = 0;
        private int otpCountdownValue = 32;
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct hex_packet_data_t
        {

            public hex_packet_data_t(byte type, int address, byte length, byte[] data, byte checksum)
            {
                this.type = type;
                this.address = address;
                this.length = length;
                this.data = new byte[16]; // Khởi tạo mảng dữ liệu với kích thước 16 byte
                Array.Copy(data, this.data, Math.Min(data.Length, 16)); // Sao chép dữ liệu vào mảng
                padding1 = 0;
                padding2 = 0;
                padding3 = 0;
                this.checksum = checksum;
            }
            public byte type;//định dạng dữ liệu
            public int address;//địa chỉ của dữ liệu
            public byte length;//độ dài của dữ liệu
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
            public byte[] data;// dữ liệu sẽ được chia thành các khối 16 byte
            public UInt32 padding1;
            public UInt32 padding2;
            public byte padding3;
            public byte checksum; // byte cuối cùng là checksum

            public byte CaculetaChecksumdata()
            {
                byte sum = 0;
                // Tính tổng của tất cả các byte trong dữ liệu
                for (int i = 0; i < data.Length; i++)
                {
                    sum += data[i];
                }
                // Đảo bit và cộng 1
                sum = (byte)((~sum + 1) & 0xFF);
                return sum;
            }

            //hàm tự động gán checksum cho dữ liệu
            public void AutoSetChecksum()
            {
                checksum = CaculetaChecksumdata();
            }

            public byte[] paraseHexData(string line)
            {
                string hexData = line.Substring(1); // Bỏ ký tự đầu tiên ':'
                int length = hexData.Length / 2;
                byte[] data = new byte[length];
                for (int i = 0; i < length; i++)
                {
                    data[i] = Convert.ToByte(hexData.Substring(i * 2, 2), 16);
                }
                return data;
            }

            public byte[] StructToByteArray()
            {
                int size = Marshal.SizeOf(this);
                byte[] byteArray = new byte[size];
                IntPtr ptr = Marshal.AllocHGlobal(size);
                try
                {
                    Marshal.StructureToPtr(this, ptr, true);
                    Marshal.Copy(ptr, byteArray, 0, size);
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }
                return byteArray;
            }

            public static bool CheckSum(byte[] data)
            {
                if (data.Length < 5) return false; // Dữ liệu quá ngắn để có checksum
                byte sum = 0;
                for (int i = 0; i < data.Length - 1; i++)
                {
                    sum += data[i];
                }
                //đảo của sum cộng thêm 1
                sum = (byte)((~sum + 1) & 0xFF); // Đảo bit và cộng 1
                return (sum & 0xFF) == data[data.Length - 1]; // So sánh với byte cuối cùng
            }

            public static hex_packet_data_t ParseHexLine(string line)
            {
                hex_packet_data_t packet = new hex_packet_data_t();
                if (line[0] == ':')
                {
                    byte[] data = packet.paraseHexData(line);
                    if (CheckSum(data))
                    {
                        packet.length = data[0];
                        packet.address = (data[2] << 8) | data[1];
                        packet.type = data[3];
                        if (packet.type == 0x00 || packet.type == 0x04)
                        {
                            packet.data = new byte[16];
                            int copyLength = Math.Min((int)packet.length, 16); // Explicitly cast packet.length to int
                            Array.Copy(data, 4, packet.data, 0, copyLength);
                            //nếu data type là 0x00 thì lưu dữ liệu vào list dataBytes
                            if (packet.type == 0x00)
                            {
                                for (int i = 0; i < copyLength; i++)
                                {
                                    dataBytes.Add(packet.data[i]);
                                }
                            }
                        }
                        else if (packet.type == 0x01 || packet.type == 0x05)
                        {
                            packet.data = new byte[16]; // No data for these types
                            if (packet.type == 0x01)
                            {
                                // End of file, clear dataBytes

                            }
                        }
                        packet.AutoSetChecksum();
                    }
                    else
                    {
                        throw new FormatException("Invalid checksum in line: " + line);
                    }
                }
                else
                {
                    throw new FormatException("Invalid line, must start with ':'");
                }
                return packet;
            }


        }
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        struct hex_packet_auth_t
        {
            public hex_packet_auth_t(byte type_t, UInt32 size_firmware, byte[] hash, byte[] sign, byte checksum)
            {
                this.type_t = 0x07; // Mặc định là 0x00, có thể thay đổi khi gửi dữ liệu
                this.size_firmware = 0; // Không sử dụng trong trường hợp này
                this.hash = new byte[32]; // Khởi tạo mảng hash với kích thước 32 byte
                Array.Copy(hash, this.hash, Math.Min(hash.Length, 32));
                this.sign = new byte[128]; // Khởi tạo mảng sign với kích thước 128 byte
                Array.Copy(sign, this.sign, Math.Min(sign.Length, 128));
                this.checksum = checksum; // Byte cuối cùng là checksum
            }

            public byte type_t;
            public UInt32 size_firmware; // Kích thước firmware, không sử dụng trong trường hợp này 
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 32)]
            public byte[] hash;
            [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128)]
            public byte[] sign;
            public byte checksum; // Byte cuối cùng là checksum


            //hiện thị hash
            public string GetHashString()
            {
                //hiện thị dưới hạng 0x00
                return BitConverter.ToString(hash).Replace("-", "0x").ToLowerInvariant();
            }

            public string GetSignString()
            {
                //hiện thị dưới hạng 0x00
                return BitConverter.ToString(sign).Replace("-", "0x").ToLowerInvariant();
            }

            // hien thi checksum 
            public string GetChecksumString()
            {
                return checksum.ToString("X2"); // Hiển thị checksum dưới dạng hex
            }

            //hienj thi size firmware
            public string GetSizeFirmwareString()
            {
                return size_firmware.ToString(); // Hiển thị kích thước firmware
            }

            public byte[] StructToByteArray()
            {
                int size = Marshal.SizeOf(this);
                byte[] byteArray = new byte[size];
                IntPtr ptr = Marshal.AllocHGlobal(size);
                try
                {
                    Marshal.StructureToPtr(this, ptr, true);
                    Marshal.Copy(ptr, byteArray, 0, size);
                }
                finally
                {
                    Marshal.FreeHGlobal(ptr);
                }
                return byteArray;
            }


            public byte Caculechecksum(hex_packet_auth_t au)
            {
                byte sum = 0;
                // Tính tổng của tất cả các byte trong dữ liệu
                for (int i = 0; i < au.StructToByteArray().Length - 1; i++)
                {
                    sum += au.StructToByteArray()[i];
                }
                sum = (byte)((~sum + 1) & 0xFF);
                return sum;
            }


            public static hex_packet_auth_t ParseHexLine(List<byte> l)
            {
                hex_packet_auth_t packet = new hex_packet_auth_t();
                byte[] listbyte = l.ToArray();
                // tinh hash cho list byte
                byte[] hash = ComputeSha256Hash(listbyte);
                //tao chu ki cho hash


                packet.type_t = 0x07;
                packet.size_firmware = (uint)l.Count; // Kích thước firmware là số lượng byte trong danh sách
                packet.hash = new byte[32]; // Khởi tạo mảng hash với kích thước 32 byte
                //copy hash vào mảng hash
                Array.Copy(hash, packet.hash, Math.Min(hash.Length, 32));

                byte[] der = PemToBytes(PrivateKeyPem);
                RSAParameters rsaParams = DecodeRSAPrivateKey(der);

                byte[] signature = new byte[128]; // Khởi tạo mảng chữ ký với kích thước 128 byte
                using (RSACryptoServiceProvider rsa = new RSACryptoServiceProvider())
                {
                    rsa.ImportParameters(rsaParams);
                    signature = rsa.SignData(hash, "SHA256");
                }

                // Sao chép chữ ký vào mảng sign
                packet.sign = new byte[128]; // Khởi tạo mảng sign với kích thước 128 byte
                Array.Copy(signature, packet.sign, Math.Min(signature.Length, 128));

                byte checksum = packet.Caculechecksum(packet); // Tính toán checksum cho gói tin
                packet.checksum = checksum; // Gán checksum vào gói tin



                return packet;
            }

            // Hàm chuyển đổi struct thành mảng byte

        }
        private static readonly byte[] Key = new byte[16] {
                0x00, 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07,
                0x08, 0x09, 0x0A, 0x0B, 0x0C, 0x0D, 0x0E, 0x0F
        };

        //khai báo privatekey cho ecc
        private const string PrivateKey = @"
        -----BEGIN EC PRIVATE KEY-----
        MHQCAQEEIGVLkXPkzWIZfqpkvjF8R7b+NRonSLR4rGOSYoSCUfZcoAcGBSuBBAAK
        oUQDQgAEr3DphyhTTYnJQXSJGMLzVHy2XS+AKzw51M3g+9TWe5uGIVmvb6VSf2S8
        d5Z5Mxx7orKtgMGdCK2uZ/s61xSD7g==
        -----END EC PRIVATE KEY-----
        ";

        private const string PublicKey = @"
        -----BEGIN PUBLIC KEY-----
        MFYwEAYHKoZIzj0CAQYFK4EEAAoDQgAEr3DphyhTTYnJQXSJGMLzVHy2XS+AKzw5
        1M3g+9TWe5uGIVmvb6VSf2S8d5Z5Mxx7orKtgMGdCK2uZ/s61xSD7g==
        -----END PUBLIC KEY-----

        ";



        public Form1()
        {
            InitializeComponent();
            _dataClient = new SimpleDataClient();
            UpdateOtpStatusLabel();
            InitializeColorScheme();
            this.timerOtpCountdown.Interval = 1000;
            this.timerOtpCountdown.Tick += new System.EventHandler(this.timerOtpCountdown_Tick);

            // ********* KHỞI TẠO BẮT ĐẦU CHU KỲ *********
            // Đặt giá trị ban đầu (31)
            otpCountdownValue = 31;
            // Cập nhật hiển thị lần đầu (31-1 = 30)
            timerOtpCountdown_Tick(timerOtpCountdown, EventArgs.Empty);
            // Bắt đầu Timer
            this.timerOtpCountdown.Start();
        }
        private enum HexType
        {
            Data = 0x00, // Dữ liệu
            EndOfFile = 0x01, // Kết thúc tệp
            ExtendedLinearAddress = 0x04, // Địa chỉ tuyến tính mở rộng
            ExtendedSegmentAddress = 0x02 // Địa chỉ đoạn mở rộng
        }
        //tạo kiểu dữ liệu stm32response_t để nhận phản hồi từ thiết bị
        [StructLayout(LayoutKind.Sequential, Pack = 1)]
        private struct stm32response_t
        {
            public stm32response_t(byte errLine, byte sucessLine,
                                   byte errOtp, byte sucessOtp,
                                   byte errVerify, byte sucessVerify)
            {
                type = 0x03;
                ErrLine = errLine;
                SucessLine = sucessLine;
                ErrOtp = errOtp;
                SucessOtp = sucessOtp;
                ErrVerify = errVerify;
                SucessVerify = sucessVerify;
            }

            //các trường dữ liệu của phản hồi từ thiết bị

            public byte type; // Kiểu phản hồi
            public byte ErrLine; // Mã lỗi dòng
            public byte SucessLine; // Mã thành công dòng
            public byte ErrOtp; // Mã lỗi OTP
            public byte SucessOtp; // Mã thành công OTP
            public byte ErrVerify; // Mã lỗi xác minh
            public byte SucessVerify; // Mã thành công xác minh

            //hàm thông báo lỗi và thành công
            public string GetErrorMessage()
            {
                switch (ErrLine)
                {
                    case 0x00:
                        return "No Error!!!";
                    case 0x01:
                        return "Checksum Error!!!";
                    case 0x02:
                        return "start file!!!";
                    case 0x03:
                        return "End file!!!";
                    case 0x04:
                        return "Primery BootLoader!!!";
                    case 0x05:
                        return "Secondary BootLoader!!!";
                    case 0x06:
                        return "hand shake sucess!!!";
                    case 0x08:
                        return "receved hash and sign!!!";
                    default:
                        return "Không có lỗi dòng.";
                }
            }

            public string GetOtpMessage()
            {
                switch (ErrOtp)
                {
                    case 0x00:
                        return "OTP Success!!!";
                    case 0x01:
                        return "OTP Error!!!";

                    default:
                        return "Không có lỗi OTP.";
                }
            }

            public string GetVerifyMessage()
            {
                switch (ErrVerify)
                {
                    case 0x02:
                        return "Verify Success!!!";
                    case 0x01:
                        return "Verify Error!!!";
                    default:
                        return "Không có lỗi xác minh.";
                }
            }

            //hàm  nạp giá trị cho các trường dữ liệu
            public void SetValues(byte errLine, byte sucessLine, byte errOtp, byte sucessOtp, byte errVerify, byte sucessVerify)
            {
                ErrLine = errLine;
                SucessLine = sucessLine;
                ErrOtp = errOtp;
                SucessOtp = sucessOtp;
                ErrVerify = errVerify;
                SucessVerify = sucessVerify;
            }

            //hàm nạp từ 1 mảng byte vào struct
            public void SetValueformByteArray(byte[] arr)
            {
                if (arr.Length < Marshal.SizeOf(typeof(stm32response_t)))
                {
                    throw new ArgumentException("Mảng byte không đủ dài để nạp vào stm32response_t.");
                }
                SetValues(arr[1], arr[2], arr[3], arr[4], arr[5], arr[6]);
            }
        }
        private async void UpLoadButton_Click(object sender, EventArgs e)
        {
            if (OTP_IS_OK == true)
            {
                await HandleUploadAsync();
            }
            else if (OTP_IS_OK == false)
            {
                //thoong bao can phai nhap OTP da
                MessageBox.Show(
                    "Vui lòng xác thực Mã OTP thành công trước khi tiến hành tải file firmware.",
                    "Lỗi: Chưa xác thực OTP",
                    MessageBoxButtons.OK,
                    MessageBoxIcon.Warning // Sử dụng biểu tượng cảnh báo
                );

            }
        }
        // Hàm xử lý upload file HEX
        private async Task HandleUploadAsync()
        {
            openFileDialog1.Title = "Chọn tệp HEX để tải lên";
            openFileDialog1.Filter = "Tệp Intel HEX (*.hex)|*.hex";

            if (openFileDialog1.ShowDialog() != DialogResult.OK)
                return;

            string hexFilePath = openFileDialog1.FileName;
            MessageBox.Show("Đã chọn tệp: " + hexFilePath);

            if (!File.Exists(hexFilePath))
            {
                MessageBox.Show("Tệp không tồn tại: " + hexFilePath);
                return;
            }

            if (!serialPort1.IsOpen)
            {
                MessageBox.Show("Cổng COM chưa được mở. Vui lòng mở cổng COM trước khi tải lên.");
                return;
            }

            string[] hexLines = File.ReadAllLines(hexFilePath);
            sw.Start();
            await SendHexLinesAsync(hexLines);
        }
        // Hàm gửi từng dòng và chờ phản hồi
        private async Task SendHexLinesAsync(string[] hexLines)
        {
            int i = 0;
            foreach (string line in hexLines)
            {
                try
                {
                    hex_packet_data_t packet = hex_packet_data_t.ParseHexLine(line);
                    byte[] packetBytes = packet.StructToByteArray();
                    byte[] encryptedData = EncryptAes128Ecb(packetBytes, Key);

                    // Xóa phản hồi cũ
                    while (responseQueue.TryDequeue(out _)) { }

                    // Gửi dữ liệu
                    serialPort1.Write(encryptedData, 0, encryptedData.Length);

                    // Chờ phản hồi tối đa 2 giây
                    bool signaled = await Task.Run(() => waitResponse.WaitOne(2000));

                    if (!signaled)
                    {
                        MessageBox.Show("Không nhận được phản hồi từ thiết bị trong thời gian chờ.");
                        continue;
                    }

                    // Kiểm tra phản hồi
                    if (responseQueue.TryDequeue(out stm32response_t response))
                    {
                        if (response.type == 0x03)
                        {
                            textBoxRece.AppendText($"Resp #{i + 1}: {response.GetErrorMessage()}{Environment.NewLine}");
                        }

                        else
                        {
                            textBoxRece.AppendText("Phản hồi không hợp lệ từ thiết bị." + Environment.NewLine);
                        }
                    }
                    else
                    {
                        textBoxRece.AppendText($"Resp #{i + 1}: NoErr." + Environment.NewLine);
                    }

                    // Cập nhật progress bar
                    progressBar2.Value = (int)((++i) * 100 / hexLines.Length);

                    await Task.Delay(5);
                }
                catch (FormatException ex)
                {
                    MessageBox.Show("Lỗi định dạng: " + ex.Message);
                    break;
                }
            }
            sw.Stop();
            //tính hash list dataBytes
            if (dataBytes.Count > 0)
            {
                //byte[] dataBytesArray = dataBytes.ToArray();
                //byte[] hash = ComputeSha256Hash(dataBytesArray);
                //hiện thị hash dưới dạng hex ra textboxRece
                textBoxRece.AppendText("list byte had data: " + Environment.NewLine);
                hex_packet_auth_t authPacket = new hex_packet_auth_t();
                authPacket = hex_packet_auth_t.ParseHexLine(dataBytes);
                textBoxRece.AppendText("Hash: " + authPacket.GetHashString() + Environment.NewLine);
                textBoxRece.AppendText("Sign: " + authPacket.GetSignString() + Environment.NewLine);
                textBoxRece.AppendText("Checksum: " + authPacket.GetChecksumString() + Environment.NewLine);
                textBoxRece.AppendText("Size Firmware: " + authPacket.GetSizeFirmwareString() + Environment.NewLine);
                textBoxRece.AppendText("Thời gian tải: " + sw.ElapsedMilliseconds + " ms" + Environment.NewLine);
                textBoxRece.AppendText("Tốc độ tải: " + (dataBytes.Count / (sw.ElapsedMilliseconds / 1000.0) / 1024).ToString("F2") + " KB/s" + Environment.NewLine);
                textBoxRece.AppendText("Thời gian xác thực: " + (sw.ElapsedMilliseconds + 500) + " ms" + Environment.NewLine);
                // Xóa dữ liệu đã nhận sau khi gửi
                // chuyển đổi authPacket thành mảng byte và mã hóa
                byte[] authPacketBytes = authPacket.StructToByteArray();
                //them 10 byte vao sau authPacketBytes để đủ bội của 16 byte
                Array.Resize(ref authPacketBytes, authPacketBytes.Length + 10);
                byte[] encryptedAuthPacket = EncryptAes128Ecb(authPacketBytes, Key);
                // Gửi gói tin xác thực
                serialPort1.Write(encryptedAuthPacket, 0, encryptedAuthPacket.Length);
                dataBytes.Clear();
            }


        }
        private void Form1_Load(object sender, EventArgs e)
        {
            string[] ports = SerialPort.GetPortNames();
            serialPort1.DataReceived += new SerialDataReceivedEventHandler(serialPort1_DataReceived);
        }
        // Add the missing event handler method for serialPort1_DataReceived
        private void serialPort1_DataReceived(object sender, SerialDataReceivedEventArgs e)
        {
            try
            {
                int bytesToRead = serialPort1.BytesToRead;
                if (bytesToRead <= 0) return; // Không có dữ liệu để đọc
                byte[] buffer = new byte[bytesToRead];
                int read = serialPort1.Read(buffer, 0, bytesToRead);
                if (read < 0) return;
                lock (rxLock)
                {
                    rxBuffer.AddRange(buffer.Take(read));
                    // parse ra các frame response (nếu đủ)
                    while (rxBuffer.Count >= RESPONSE_LEN)
                    {
                        byte[] respBytes = rxBuffer.Take(RESPONSE_LEN).ToArray();
                        rxBuffer.RemoveRange(0, RESPONSE_LEN);

                        // chuyển respBytes thành stm32response_t (dùng SetValueformByteArray)
                        stm32response_t resp = new stm32response_t(0, 0, 0, 0, 0, 0);
                        try
                        {
                            resp.SetValueformByteArray(respBytes); // hàm của bạn kiểm tra đủ độ dài

                        }
                        catch
                        {
                            // nếu lỗi parse, bỏ qua frame này
                            continue;
                        }

                        responseQueue.Enqueue(resp);

                        // update UI (dùng BeginInvoke để tránh deadlock)
                        this.BeginInvoke(new Action(() =>
                        {
                            countLine++; // Tăng ở đây — mỗi frame phản hồi thực sự
                            //textBoxRece.AppendText($"Resp #{countLine}: {resp.GetErrorMessage()}{Environment.NewLine}");
                            if (resp.ErrLine == 0x05)
                            {
                                textBoxRece.AppendText($"Resp #{countLine}: {resp.GetErrorMessage()}{Environment.NewLine}");
                                //set lablebootloader thanh update bootloader
                                labelBootloader.Text = "BootLoader";
                            }


                        }));

                        // báo cho sender (UpLoadButton) nếu đang chờ
                        waitResponse.Set();
                    }
                }

            }
            catch (Exception ex)
            {
                this.BeginInvoke(new Action(() =>
                {
                    textBoxRece.AppendText("Error receiving data: " + ex.Message + Environment.NewLine);
                }));
            }
        }
        private void buttonOpen_Click(object sender, EventArgs e)
        {
            //kết nối cổng COM với thiết bi
            try
            {
                if (serialPort1.IsOpen)
                {
                    serialPort1.Close();
                }
                serialPort1.PortName = "COM21";
                serialPort1.BaudRate = 115200;
                serialPort1.DataBits = 8;
                serialPort1.Parity = Parity.None;
                serialPort1.StopBits = StopBits.One;
                serialPort1.Open();

                for (int i = 0; i <= 100; i++)
                {
                    progressBar1.Value = i;
                }

                MessageBox.Show("Kết nối thành công với " + serialPort1.PortName);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi kết nối: " + ex.Message);
            }
        }
        private void buttonClose_Click(object sender, EventArgs e)
        {

            try
            {
                if (serialPort1.IsOpen)
                {
                    dataBytes.Clear(); // Xóa dữ liệu đã nhận
                    progressBar1.Value = 0;
                    progressBar2.Value = 0;
                    //clear textboxRece 
                    textBoxRece.Clear();
                    serialPort1.Close();
                    MessageBox.Show("Đã ngắt kết nối với " + serialPort1.PortName);
                }
                else
                {
                    MessageBox.Show("Cổng COM không mở.");
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Lỗi khi ngắt kết nối: " + ex.Message);
            }
        }
        private void textBoxotp_TextChanged(object sender, EventArgs e)
        {
            // Lấy nội dung hiện tại của TextBox
            string text = textBoxotp.Text;

            // Biến tạm để lưu kết quả chuyển đổi (chúng ta không cần sử dụng nó)
            int number;

            // Kiểm tra xem chuỗi có thể chuyển đổi thành số nguyên (Int32) hay không.
            // Nếu Int32.TryParse trả về 'false', nghĩa là chuỗi KHÔNG phải là số.
            if (!Int32.TryParse(text, out number))
            {
                // Kiểm tra để tránh hiển thị thông báo khi TextBox đang trống
                if (!string.IsNullOrEmpty(text))
                {
                    // Báo lỗi cho người dùng
                    MessageBox.Show("Vui lòng chỉ nhập số vào ô OTP!", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);

                    // Tùy chọn: Xóa ký tự không hợp lệ hoặc giữ lại ký tự cuối cùng hợp lệ
                    // Để đơn giản, chúng ta sẽ xóa ký tự cuối cùng nếu nó không phải là số
                    if (text.Length > 0)
                    {
                        textBoxotp.Text = text.Substring(0, text.Length - 1);
                        // Đặt con trỏ về cuối TextBox
                        textBoxotp.SelectionStart = textBoxotp.Text.Length;
                    }
                }
            }
        }
        static byte[] EncryptAes128Ecb(byte[] plainBytes, byte[] key)
        {
            using (Aes aes = Aes.Create())
            {
                aes.Key = key;
                aes.Mode = CipherMode.ECB;
                aes.Padding = PaddingMode.None;

                using (ICryptoTransform encryptor = aes.CreateEncryptor())
                {
                    return encryptor.TransformFinalBlock(plainBytes, 0, plainBytes.Length);
                }
            }
        }
        static byte[] ComputeSha256Hash(byte[] rawData)
        {
            using (SHA256 sha256Hash = SHA256.Create())
            {
                return sha256Hash.ComputeHash(rawData);
            }
        }
        private async void buttonSendOtp_Click(object sender, EventArgs e)
        {
            // Giả định: textBoxotp là TextBox nhập số/OTP.
            string inputString = textBoxotp.Text.Trim();

            // Tên biến phù hợp hơn với logic Gửi số (tạm thời)
            int numberToSend = 0;

            // Kiểm tra và chuyển đổi chuỗi sang số nguyên an toàn hơn
            if (!Int32.TryParse(inputString, out numberToSend))
            {
                MessageBox.Show("Dữ liệu nhập vào phải là một số nguyên hợp lệ.", "Lỗi nhập liệu", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return; // Dừng hàm nếu chuyển đổi thất bại
            }

            // **********************************
            // BƯỚC BẮT ĐẦU: CẢI THIỆN UX/SAFETY
            // **********************************
            buttonSendOtp.Enabled = false; // Vô hiệu hóa nút
                                           // Thêm dòng này nếu bạn có Label hiển thị kết quả/trạng thái
                                           // lblResult.Text = "Đang gửi dữ liệu qua kênh HTTPS..."; 

            try
            {
                // **********************************
                // BƯỚC 1: GỌI LỚP CLIENT BẤT ĐỒNG BỘ
                // **********************************
                var (received, message) = await _dataClient.SendNumberAsync(numberToSend);
                if (received.HasValue)
                {
                    OTP_IS_OK = true;
                    UpdateOtpStatusLabel();
                    MessageBox.Show($"Xác thực thành công! OTP: {received.Value}\nThông báo: {message}",
                        "Thành công", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                else
                {
                    UpdateOtpStatusLabel();
                    MessageBox.Show($"Xác thực thất bại!\nThông báo: {message}",
                        "Lỗi xác thực", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
            catch (Exception ex)
            {
                // Xử lý các lỗi bất ngờ khác (hiếm)
                MessageBox.Show($"Lỗi không xác định: {ex.Message}", "Lỗi hệ thống", MessageBoxButtons.OK, MessageBoxIcon.Stop);
                // lblResult.Text = "Lỗi nghiêm trọng.";
            }
            finally
            {
                // QUAN TRỌNG: Đảm bảo nút được kích hoạt lại dù xảy ra lỗi hay thành công
                buttonSendOtp.Enabled = true;
            }
        }
        private byte[] my_strinToHex(string input)
        {
            // Chuyển đổi chuỗi thành mảng byte
            byte[] bytes = new byte[input.Length / 2];
            for (int i = 0; i < input.Length; i += 2)
            {
                // Chuyển đổi từng cặp ký tự thành byte
                bytes[i / 2] = Convert.ToByte(input.Substring(i, 2), 16);
            }
            return bytes;
        }

        private void buttonHandsk_Click(object sender, EventArgs e)
        {
            // dữ liệu handshake đến thiết bị

            //guiw dữ liệu handshake đến thiết bị qua cổng COM
            byte[] cipherData = new byte[] { 0x01, 0x02, 0x03, 0x04, 0x05, 0x06, 0x07 };
            if (serialPort1.IsOpen)
            {
                try
                {
                    serialPort1.Write(cipherData, 0, cipherData.Length);
                    MessageBox.Show("Dữ liệu handshake đã được gửi thành công.");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Lỗi khi gửi dữ liệu: " + ex.Message);
                }
            }
            else
            {
                MessageBox.Show("Cổng COM chưa được mở.");
            }

        }

        private void label2_Click(object sender, EventArgs e)
        {

        }

        static byte[] PemToBytes(string pem)
        {
            string header = "-----BEGIN RSA PRIVATE KEY-----";
            string footer = "-----END RSA PRIVATE KEY-----";
            pem = pem.Replace(header, "").Replace(footer, "")
                     .Replace("\r", "").Replace("\n", "").Trim();
            return Convert.FromBase64String(pem);
        }

        static RSAParameters DecodeRSAPrivateKey(byte[] privkey)
        {
            using (MemoryStream ms = new MemoryStream(privkey))
            using (BinaryReader rd = new BinaryReader(ms))
            {
                byte bt = 0;
                ushort twobytes = 0;

                twobytes = rd.ReadUInt16();
                if (twobytes == 0x8130) rd.ReadByte();
                else if (twobytes == 0x8230) rd.ReadInt16();
                else throw new Exception("Unexpected value in RSA private key");

                twobytes = rd.ReadUInt16();
                if (twobytes != 0x0102) throw new Exception("Unexpected version");
                if (rd.ReadByte() != 0x00) throw new Exception("Invalid padding");

                RSAParameters rsAparams = new RSAParameters
                {
                    Modulus = ReadASN1Integer(rd),
                    Exponent = ReadASN1Integer(rd),
                    D = ReadASN1Integer(rd),
                    P = ReadASN1Integer(rd),
                    Q = ReadASN1Integer(rd),
                    DP = ReadASN1Integer(rd),
                    DQ = ReadASN1Integer(rd),
                    InverseQ = ReadASN1Integer(rd)
                };
                return rsAparams;
            }
        }

        static byte[] ReadASN1Integer(BinaryReader rd)
        {
            if (rd.ReadByte() != 0x02) throw new Exception("Expected integer");
            int count = rd.ReadByte();
            if (count == 0x81)
                count = rd.ReadByte();
            else if (count == 0x82)
            {
                byte hi = rd.ReadByte();
                byte lo = rd.ReadByte();
                count = BitConverter.ToUInt16(new byte[] { lo, hi }, 0);
            }
            byte[] val = rd.ReadBytes(count);
            if (val[0] == 0x00)
            {
                byte[] tmp = new byte[val.Length - 1];
                Buffer.BlockCopy(val, 1, tmp, 0, tmp.Length);
                val = tmp;
            }
            return val;
        }

        private void label3_Click(object sender, EventArgs e)
        {

        }

        private void labelOTPSTA_Click(object sender, EventArgs e)
        {

        }

        private void UpdateOtpStatusLabel()
        {
            // Kiểm tra biến OTP_IS_OK
            if (OTP_IS_OK)
            {
                labelOTPSTA.Text = "TRUE";
                // Tùy chọn: Thay đổi màu sắc
                labelOTPSTA.ForeColor = System.Drawing.Color.Green;
            }
            else
            {
                labelOTPSTA.Text = "FALSE";
                // Tùy chọn: Thay đổi màu sắc
                labelOTPSTA.ForeColor = System.Drawing.Color.Red;
            }
        }

        private void InitializeColorScheme()
        {
            // Sử dụng hàm hỗ trợ để tìm và trả về Control nếu nó tồn tại
            System.Windows.Forms.Control FindControl(string name)
            {
                // false để chỉ tìm trong control trực tiếp của Form
                System.Windows.Forms.Control[] foundControls = this.Controls.Find(name, false);
                if (foundControls.Length > 0)
                {
                    return foundControls[0];
                }
                return null;
            }

            // Màu nền form - Gradient xanh dương đậm
            this.BackColor = System.Drawing.Color.FromArgb(30, 39, 73);

            // Định nghĩa Màu chữ VÀNG SÁNG chung cho các Button và Tiêu đề
            System.Drawing.Color title_ForeColor = System.Drawing.Color.FromArgb(253, 224, 71); // Màu vàng sáng
            System.Drawing.Color button_ForeColor = title_ForeColor; // Dùng chung màu vàng sáng cho chữ Button

            // ---------------------- BUTTONS ----------------------

            // buttonOpen
            System.Windows.Forms.Button btnOpen = FindControl("buttonOpen") as System.Windows.Forms.Button;
            if (btnOpen != null)
            {
                btnOpen.BackColor = System.Drawing.Color.FromArgb(34, 197, 94); // Xanh lá mint (Màu nền)
                btnOpen.ForeColor = button_ForeColor;
                btnOpen.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                btnOpen.FlatAppearance.BorderSize = 0;
                btnOpen.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            }

            // buttonClose
            System.Windows.Forms.Button btnClose = FindControl("buttonClose") as System.Windows.Forms.Button;
            if (btnClose != null)
            {
                btnClose.BackColor = System.Drawing.Color.FromArgb(239, 68, 68); // Đỏ coral (Màu nền)
                btnClose.ForeColor = button_ForeColor;
                btnClose.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                btnClose.FlatAppearance.BorderSize = 0;
                btnClose.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            }

            // UpLoadButton
            System.Windows.Forms.Button btnUpload = FindControl("UpLoadButton") as System.Windows.Forms.Button;
            if (btnUpload != null)
            {
                btnUpload.BackColor = System.Drawing.Color.FromArgb(59, 130, 246);
                btnUpload.ForeColor = button_ForeColor;
                btnUpload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                btnUpload.FlatAppearance.BorderSize = 0;
                btnUpload.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            }

            // buttonSendOtp (Nút 'Sen')
            System.Windows.Forms.Button btnSendOtp = FindControl("buttonSendOtp") as System.Windows.Forms.Button;
            if (btnSendOtp != null)
            {
                btnSendOtp.BackColor = System.Drawing.Color.FromArgb(168, 85, 247);
                btnSendOtp.ForeColor = button_ForeColor;
                btnSendOtp.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                btnSendOtp.FlatAppearance.BorderSize = 0;
                btnSendOtp.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            }

            // buttonHandsk
            System.Windows.Forms.Button btnHandsk = FindControl("buttonHandsk") as System.Windows.Forms.Button;
            if (btnHandsk != null)
            {
                btnHandsk.BackColor = System.Drawing.Color.FromArgb(251, 146, 60);
                btnHandsk.ForeColor = button_ForeColor;
                btnHandsk.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                btnHandsk.FlatAppearance.BorderSize = 0;
                btnHandsk.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            }

            // Button EnterUpload (Nút 'Enter')
            System.Windows.Forms.Button btnEnterUpload = FindControl("buttonEnterUpload") as System.Windows.Forms.Button;
            if (btnEnterUpload != null)
            {
                btnEnterUpload.BackColor = System.Drawing.Color.FromArgb(251, 146, 60);
                btnEnterUpload.ForeColor = button_ForeColor;
                btnEnterUpload.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
                btnEnterUpload.FlatAppearance.BorderSize = 0;
                btnEnterUpload.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            }

            // ---------------------- TIÊU ĐỀ/GROUPBOXS ----------------------
            // Áp dụng màu Vàng Sáng cho các tiêu đề chính

            // Giả định tiêu đề Stm32Status là một GroupBox
            System.Windows.Forms.GroupBox grpStm32 = FindControl("Stm32Status") as System.Windows.Forms.GroupBox;
            if (grpStm32 != null)
            {
                grpStm32.ForeColor = title_ForeColor; // Chữ tiêu đề GroupBox màu Vàng
                                                      // Font cho GroupBox Title nếu cần
                grpStm32.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            }

            // Giả định tiêu đề OTPStatus là một GroupBox
            System.Windows.Forms.GroupBox grpOtpStatus = FindControl("OTPStatus") as System.Windows.Forms.GroupBox;
            if (grpOtpStatus != null)
            {
                grpOtpStatus.ForeColor = title_ForeColor; // Chữ tiêu đề GroupBox màu Vàng
                grpOtpStatus.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            }

            // Giả định tiêu đề OTP code là một GroupBox có tên 'grpOtpCode' hoặc Label có tên 'labelOtpCode'
            System.Windows.Forms.GroupBox grpOtpCode = FindControl("grpOtpCode") as System.Windows.Forms.GroupBox;
            if (grpOtpCode != null)
            {
                grpOtpCode.ForeColor = title_ForeColor; // Chữ tiêu đề GroupBox màu Vàng
                grpOtpCode.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
            }
            else
            {
                // Nếu nó là một Label có tên 'labelOtpCode' (hoặc tên tương tự)
                System.Windows.Forms.Label lblOtpCode = FindControl("labelOtpCode") as System.Windows.Forms.Label;
                if (lblOtpCode != null)
                {
                    lblOtpCode.ForeColor = title_ForeColor; // Chữ Label màu Vàng
                    lblOtpCode.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Bold);
                }
            }


            // ---------------------- TEXTBOXES ----------------------

            // textBoxRece (Hoặc là Console)
            System.Windows.Forms.TextBox txtRece = FindControl("textBoxRece") as System.Windows.Forms.TextBox;
            if (txtRece != null)
            {
                txtRece.BackColor = System.Drawing.Color.FromArgb(45, 55, 72);
                txtRece.ForeColor = System.Drawing.Color.FromArgb(226, 232, 240); // Chữ sáng
                txtRece.Font = new System.Drawing.Font("Consolas", 9);
                txtRece.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            }

            // textBoxotp
            System.Windows.Forms.TextBox txtOtp = FindControl("textBoxotp") as System.Windows.Forms.TextBox;
            if (txtOtp != null)
            {
                txtOtp.BackColor = System.Drawing.Color.FromArgb(45, 55, 72);
                txtOtp.ForeColor = System.Drawing.Color.FromArgb(226, 232, 240); // Chữ sáng
                txtOtp.Font = new System.Drawing.Font("Segoe UI", 11);
                txtOtp.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            }

            // ---------------------- PROGRESS BARS & LABELS ----------------------
            // ... (Giữ nguyên phần ProgressBar và Labels) ...

            System.Windows.Forms.ProgressBar pBar1 = FindControl("progressBar1") as System.Windows.Forms.ProgressBar;
            if (pBar1 != null)
            {
                pBar1.ForeColor = System.Drawing.Color.FromArgb(52, 211, 153);
            }

            System.Windows.Forms.ProgressBar pBar2 = FindControl("progressBar2") as System.Windows.Forms.ProgressBar;
            if (pBar2 != null)
            {
                pBar2.ForeColor = System.Drawing.Color.FromArgb(59, 130, 246);
            }

            // Labels - Màu chữ sáng
            foreach (System.Windows.Forms.Control ctrl in this.Controls)
            {
                if (ctrl is System.Windows.Forms.Label)
                {
                    System.Windows.Forms.Label lbl = (System.Windows.Forms.Label)ctrl;
                    // Chỉ áp dụng màu chữ sáng chung cho các Label không phải là tiêu đề chính (chưa được đặt màu vàng)
                    if (lbl.ForeColor.R == 0 && lbl.ForeColor.G == 0 && lbl.ForeColor.B == 0) // Kiểm tra nếu màu vẫn là mặc định (thường là đen)
                    {
                        lbl.ForeColor = System.Drawing.Color.FromArgb(226, 232, 240); // Chữ sáng cho tất cả label
                    }

                    lbl.Font = new System.Drawing.Font("Segoe UI", 10, System.Drawing.FontStyle.Regular);

                    // Màu đặc biệt cho label bootloader
                    if (lbl.Name == "labelBootloader")
                    {
                        lbl.ForeColor = System.Drawing.Color.FromArgb(251, 146, 60); // Màu cam đặc biệt
                        lbl.Font = new System.Drawing.Font("Segoe UI", 11, System.Drawing.FontStyle.Bold);
                    }
                }
                // *******************************************************************
                // LƯU Ý QUAN TRỌNG: 
                // Nếu các Label Status/Permery bootloader nằm bên trong GroupBox (như trên ảnh), 
                // bạn cần duyệt Controls của GroupBox đó để đặt màu cho chúng. 
                // Hiện tại, mã này chỉ duyệt Controls trực tiếp trên Form.
                // *******************************************************************
            }
        }

        private void timerOtpCountdown_Tick(object sender, System.EventArgs e)
        {
            // Bước 1: Giảm giá trị đếm ngược
            if (OTP_IS_OK == true)
            {
                otpCountdownValue--;

                // Bước 2: Cập nhật hiển thị lên Label
                this.labelTimeCountDown.Text = $"{otpCountdownValue} s";

                // Cập nhật màu chữ theo yêu cầu mới (Đổi sang ĐỎ khi <= 15 giây)
                if (otpCountdownValue <= 15)
                {
                    // Thay đổi sang màu ĐỎ khi còn 15 giây trở xuống
                    this.labelTimeCountDown.ForeColor = System.Drawing.Color.Red;
                }
                else
                {
                    // Giữ màu vàng sáng ban đầu
                    this.labelTimeCountDown.ForeColor = System.Drawing.Color.FromArgb(253, 224, 71);
                }

                // Bước 3: Kiểm tra điều kiện kết thúc (đạt 0) và lặp lại
                if (otpCountdownValue <= 0)
                {
                    OTP_IS_OK = false;
                    otpCountdownValue = 30;
                    UpdateOtpStatusLabel(); 
                    this.labelTimeCountDown.Text = "0 s";
                }
            }
        }

        private void labelTimeCountDown_Click(object sender, EventArgs e)
        {
             
            
            
            
        }
    }
}
