      Remote Command System using TCP Socket
      
Team Members:
Trương Tuấn Dũng – MSSV: 079206042161
Phạm Tuân – MSSV: 089206007430


      Giới thiệu đề tài

Đề tài xây dựng một hệ thống điều khiển máy tính từ xa dựa trên mô hình Client–Server, sử dụng giao thức TCP Socket trong C#.
Hệ thống cho phép client gửi các lệnh hệ thống đến server, server thực thi lệnh và trả kết quả về cho client theo thời gian thực.


        Mục tiêu
      
Hiểu và áp dụng lập trình socket TCP
Xây dựng ứng dụng theo mô hình Client–Server
Thực thi lệnh từ xa
Xử lý nhiều client cùng lúc
Tăng cường bảo mật cơ bản
Xây dựng giao diện GUI bằng Avalonia


      Công nghệ sử dụng
      
C# (.NET)
TCP Socket (System.Net.Sockets)
Đa luồng (Thread)
Thực thi tiến trình (System.Diagnostics)
Avalonia UI (giao diện)
Git & GitHub


      Kiến trúc hệ thống
      
Client (Console / GUI Avalonia)
            ↓
        TCP Socket
            ↓
          Server
            ↓
     Thực thi lệnh hệ thống
            ↓
        Trả kết quả về Client


        Các chức năng chính

1. Kết nối Client–Server
   
Server lắng nghe tại một IP và Port
Client nhập IP + Port để kết nối
Hiển thị trạng thái kết nối

2. Xác thực người dùng (Authentication)
   
Server yêu cầu đăng nhập trước khi sử dụng
Client gửi:
username|password
Tài khoản mặc định:
Username: admin
Password: 1234

3. Thực thi lệnh từ xa

Client có thể gửi các lệnh:

dir
ipconfig
tasklist
whoami
hostname

Server:

sử dụng cmd.exe để chạy lệnh
lấy output hoặc error
gửi kết quả về client

4. Hỗ trợ nhiều client (đa luồng)
   
Server xử lý nhiều client cùng lúc
mỗi client chạy trên 1 thread riêng
không ảnh hưởng lẫn nhau

5. Bảo mật lệnh
Chặn các lệnh nguy hiểm:
shutdown, format, del, powershell...
Phản hồi:
[BLOCKED] → lệnh nguy hiểm
[REJECTED] → lệnh không hợp lệ

6. Ghi log hệ thống

Server ghi lại:

thời điểm client kết nối/ngắt kết nối
trạng thái đăng nhập
lệnh đã thực thi
lệnh bị chặn / bị từ chối

7. Client Console

giao diện dòng lệnh
dùng để test hệ thống
hỗ trợ login và gửi lệnh

8. Client GUI (Avalonia)
   
giao diện thân thiện
có:
nhập IP, Port
username/password
nhập command
hiển thị kết quả
có trạng thái kết nối


      ▶️ Hướng dẫn chạy chương trình
Bước 1: Chạy Server
dotnet run

Bước 2: Chạy Client
Cách 1: Console
Run/donet run

Cách 2: GUI Avalonia
Chạy project:

remoteCommandClientAvalonia
Bước 3: Kết nối

Nhập:

IP: 127.0.0.1
Port: 8080
Username: admin
Password: 1234

Bước 4: Test lệnh
dir
whoami
tasklist
powershell → bị chặn
exit

      📈 Các giai đoạn phát triển
1.Kết nối TCP cơ bản
2.Gửi / nhận dữ liệu
3.Thực thi lệnh
4.Trả kết quả
5.Gửi nhiều lệnh liên tục
6.Xử lý nhiều client
7.Xác thực người dùng
8.Bảo mật lệnh
9.Ghi log hệ thống
10.Xây dựng GUI Avalonia
