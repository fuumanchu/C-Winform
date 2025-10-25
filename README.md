Secure Firmware Update Tool – C# (Windows Forms)

Ứng dụng được phát triển bằng C# WinForms nhằm nạp firmware định dạng Intel HEX lên vi điều khiển STM32 thông qua giao tiếp UART (COM). Công cụ tích hợp nhiều cơ chế bảo mật như mã hóa AES, xác thực OTP và xác minh chữ ký số RSA nhằm đảm bảo quá trình cập nhật firmware an toàn và chống sửa đổi trái phép.

+)Chức năng chính
-Xử lý và truyền file Intel HEX

Parse và xử lý các bản ghi HEX: Data (0x00), End Of File (0x01), Extended Linear Address (0x04)

Chia nhỏ dữ liệu và truyền theo gói có cấu trúc rõ ràng

Theo dõi tiến trình bằng Progress Bar và Log chi tiết theo thời gian thực

-Giao tiếp bảo mật với STM32

Mã hóa dữ liệu bằng AES-128 ECB trước khi truyền

Mỗi gói tin kèm checksum để kiểm tra toàn vẹn dữ liệu

Cơ chế handshake 2 chiều:

STM32 giải mã, tính lại checksum và phản hồi ACK/NACK

Tool chỉ gửi gói tiếp theo khi ACK hợp lệ → giảm lỗi nạp

-Chữ ký số & xác minh tính xác thực firmware

Tạo hash SHA-256 cho dữ liệu firmware

Ký hash bằng khóa riêng RSA (PEM)

Gửi chữ ký và hash tới STM32 để xác minh bằng khóa công khai
→ đảm bảo firmware đúng nguồn và chưa bị can thiệp

-Xác thực OTP chống truy cập trái phép

Nhập OTP trước khi nạp firmware

Giao tiếp với server qua HTTPS

Hiển thị trạng thái OTP (Valid/Invalid) và bộ đếm ngược 30s

-Xử lý lỗi và bảo vệ thiết bị

Thông báo cụ thể các lỗi: sai OTP, sai checksum, sai chữ ký, mất kết nối COM

Dừng nạp khi phát hiện bất thường để bảo vệ firmware hiện tại