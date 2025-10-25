using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Do_an_tot_Nghiep
{
    internal class SimpleDataClient
    {
        private const string ServerUrl = "https://192.168.0.102:5000/api/v1/echo_number";

        private readonly HttpClient _httpClient;

        // Cấu trúc phản hồi từ Server Flask
        private class EchoResponse
        {
            public string status { get; set; }
            public int received { get; set; }
            public bool processed { get; set; } // Sửa: processed là bool, không phải int
            public string message { get; set; } // Thêm message để xử lý thông báo lỗi
        }

        public SimpleDataClient()
        {
            // Cấu hình để bỏ qua lỗi chứng chỉ tự ký (-k của curl)
            var handler = new HttpClientHandler
            {
                ServerCertificateCustomValidationCallback = (req, cert, chain, policy) => true
            };
            _httpClient = new HttpClient(handler);
            _httpClient.Timeout = TimeSpan.FromSeconds(10);
        }

        /// <summary>
        /// Gửi một số nguyên lên Server qua HTTPS và nhận về số đã xử lý (+1).
        /// </summary>
        public async Task<(int? received, string message)> SendNumberAsync(int numberToSend)
        {
            // 1. Tạo JSON payload: {"number": 123}
            string jsonPayload = $"{{\"number\": {numberToSend}}}";
            var content = new StringContent(jsonPayload, Encoding.UTF8, "application/json");
            try
            {
                // 2. Gửi yêu cầu POST
                HttpResponseMessage response = await _httpClient.PostAsync(ServerUrl, content);
                string responseBody = await response.Content.ReadAsStringAsync();
                if (response.IsSuccessStatusCode) // HTTP 200 OK
                {
                    var result = JsonConvert.DeserializeObject<EchoResponse>(responseBody);
                    if (result.status == "success")
                    {
                        return (result.received, result.message); // Trả về OTP và message
                    }
                    else
                    {
                        Console.WriteLine($"LỖI Logic: {responseBody}");
                        return (null, result.message);
                    }
                }
                else // Xử lý lỗi (401, 403, v.v.)
                {
                    var errorResult = JsonConvert.DeserializeObject<EchoResponse>(responseBody);
                    Console.WriteLine($"LỖI Server: {response.StatusCode}. Chi tiết: {responseBody}");
                    return (null, errorResult?.message ?? $"Lỗi server: {response.StatusCode}");
                }
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"LỖI KẾT NỐI HTTPS: {ex.Message}");
                return (null, $"Lỗi kết nối: {ex.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"LỖI KHÁC: {ex.Message}");
                return (null, $"Lỗi không xác định: {ex.Message}");
            }
        }
    }
}
