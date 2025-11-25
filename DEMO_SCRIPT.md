# 🎬 KỊCH BẢN DEMO - HỆ THỐNG QUẢN LÝ SỰ KIỆN

## 📋 CHECKLIST TRƯỚC KHI DEMO
- [ ] Chạy `dotnet run` và đảm bảo server đang chạy
- [ ] Mở browser ở localhost:7077
- [ ] Chuẩn bị 2 tab: Tab User (thường) + Tab Admin (ẩn danh/incognito)
- [ ] Đóng tất cả notifications/alerts để màn hình gọn
- [ ] Zoom browser 100% (Ctrl+0)

---

## 🎯 PHẦN 1: GIỚI THIỆU & TRANG CHỦ (2 phút)

### 🎬 [Mở trang chủ]
💬 **"Xin chào các thầy cô và các bạn. Em xin được trình bày đồ án Hệ Thống Quản Lý Sự Kiện."**

### 🎬 [Scroll từ trên xuống dưới trang chủ]
💬 **"Đây là giao diện trang chủ của hệ thống. Trang chủ hiển thị các sự kiện sắp diễn ra với đầy đủ thông tin:"**
- ✅ Tên sự kiện, thời gian, địa điểm
- ✅ Hình ảnh minh họa
- ✅ Nghệ sĩ tham gia
- ✅ Phạm vi giá vé

### 🎬 [Hover vào card sự kiện]
💬 **"Giao diện được thiết kế responsive với hiệu ứng hover mượt mà."**

### 🎬 [Click vào nút "Xem Chi Tiết" của một sự kiện]

---

## 🎯 PHẦN 2: CHI TIẾT SỰ KIỆN & ĐẶT VÉ (3 phút)

### 🎬 [Trang chi tiết sự kiện]
💬 **"Đây là trang chi tiết sự kiện, hiển thị đầy đủ thông tin:"**

### 🎬 [Scroll và chỉ vào từng phần]
1. **Header Section**
   💬 "Phần đầu hiển thị banner, tên sự kiện, nghệ sĩ và thời gian"

2. **Thông tin sự kiện**
   💬 "Thông tin chi tiết về sự kiện, địa điểm, và mô tả"

3. **Phần Loại Vé** ⭐ QUAN TRỌNG
   💬 **"Phần này hiển thị các loại vé với giá và số lượng còn lại được tính real-time từ database."**
   💬 **"Mỗi khi có người đặt vé, số lượng sẽ tự động trừ đi. Em sẽ demo điều này ở phần sau."**

### 🎬 [Chọn số lượng vé]
💬 **"Người dùng có thể chọn nhiều loại vé khác nhau."**
- Chọn VIP: 2 vé
- Chọn Regular: 3 vé

### 🎬 [Nhập thông tin khách hàng]
💬 **"Hệ thống yêu cầu thông tin liên hệ:"**
- Tên: Nguyễn Văn A
- Email: nguyenvana@gmail.com
- SĐT: 0123456789

### 🎬 [Click "Đặt Vé Ngay"]
💬 **"Khi nhấn đặt vé, hệ thống sẽ:"**
- ✅ Kiểm tra số lượng vé còn lại
- ✅ Tạo đơn hàng
- ✅ Trừ số lượng vé trong database
- ✅ Chuyển đến trang xác nhận

### 🎬 [Trang Confirmation]
💬 **"Đây là trang xác nhận đơn hàng với:"**
- ✅ Thông tin khách hàng
- ✅ Chi tiết vé đã đặt
- ✅ Tổng tiền
- ✅ **Mã QR Code cho mỗi loại vé**

### 🎬 [Chỉ vào QR Code]
💬 **"Hệ thống tự động generate QR code duy nhất cho mỗi đơn hàng, có thể dùng để check-in tại sự kiện."**

---

## 🎯 PHẦN 3: ĐĂNG KÝ & ĐĂNG NHẬP (2 phút)

### 🎬 [Click "Đăng Ký" trên navbar]
💬 **"Bây giờ em sẽ demo chức năng đăng ký tài khoản."**

### 🎬 [Trang đăng ký - chỉ vào giao diện]
💬 **"Giao diện đăng ký được thiết kế split-screen hiện đại với:"**
- ✅ Bên trái: Branding với gradient animation
- ✅ Bên phải: Form đăng ký

### 🎬 [Điền form đăng ký]
- Email: demo@test.com
- Password: Demo123!
- Confirm: Demo123!

### 🎬 [Click "Đăng Ký Ngay"]
💬 **"Hệ thống có validation:"**
- ✅ Email hợp lệ
- ✅ Mật khẩu tối thiểu 6 ký tự
- ✅ Password match

### 🎬 [Sau khi đăng ký thành công]
💬 **"Tài khoản được tạo và tự động đăng nhập, được assign vai trò User."**

---

## 🎯 PHẦN 4: QUẢN LÝ TÀI KHOẢN (2 phút)

### 🎬 [Click vào tên user trên navbar → "Quản Lý Tài Khoản"]
💬 **"Đây là trang quản lý tài khoản cá nhân với giao diện dashboard chuyên nghiệp."**

### 🎬 [Chỉ vào Sidebar]
💬 **"Sidebar navigation bao gồm:"**
- Hồ Sơ
- Email
- Mật Khẩu
- Xác Thực Hai Yếu Tố
- Dữ Liệu Cá Nhân

### 🎬 [Tab Hồ Sơ]
💬 **"Trang hồ sơ cho phép cập nhật thông tin cá nhân."**
- Thêm số điện thoại: 0987654321
- Click "Lưu Thay Đổi"

### 🎬 [Tab Mật Khẩu]
💬 **"Trang đổi mật khẩu có:"**
- ✅ Form đổi mật khẩu an toàn
- ✅ Tips về mật khẩu mạnh

### 🎬 [Tab 2FA]
💬 **"Hệ thống hỗ trợ xác thực hai yếu tố để tăng bảo mật."**

---

## 🎯 PHẦN 5: XEM VÉ ĐÃ ĐẶT (1 phút)

### 🎬 [Click "Vé Của Tôi" trên navbar]
💬 **"Đây là trang quản lý vé đã đặt:"**

### 🎬 [Chỉ vào danh sách đơn hàng]
💬 **"Hiển thị tất cả vé đã đặt với:"**
- ✅ Thông tin sự kiện
- ✅ Loại vé, số lượng
- ✅ Trạng thái đơn hàng
- ✅ Tổng tiền

### 🎬 [Click "Xem Chi Tiết" một đơn hàng]
💬 **"Chi tiết đơn hàng có đầy đủ thông tin và QR code để check-in."**

---

## 🎯 PHẦN 6: ADMIN - QUẢN LÝ NGHỆ SĨ (2 phút)

### 🎬 [Đăng xuất → Đăng nhập Admin]
💬 **"Bây giờ em sẽ đăng nhập với tài khoản Admin để demo các chức năng quản trị."**
- Email: admin@admin.com
- Password: Admin@123

### 🎬 [Hover vào "Quản Trị" trên navbar]
💬 **"Menu quản trị có đầy đủ các module quản lý."**

### 🎬 [Click "Nghệ Sĩ"]
💬 **"Đây là trang quản lý nghệ sĩ với giao diện bảng danh sách."**

### 🎬 [Click "Thêm Nghệ Sĩ Mới"]
💬 **"Bây giờ em sẽ demo tính năng validation tránh trùng tên."**

### 🎬 [Điền form - Tên trùng]
💬 **"Em sẽ thử tạo nghệ sĩ với tên đã tồn tại:"**
- Tên: Sơn Tùng M-TP (tên đã có trong DB)
- Thể loại: Pop
- Click "Lưu"

### 🎬 [Lỗi validation hiện ra]
💬 **"Hệ thống tự động kiểm tra và báo lỗi: 'Tên nghệ sĩ này đã tồn tại. Vui lòng chọn tên khác.'"**

### 🎬 [Sửa lại tên khác]
- Tên: Nghệ Sĩ Demo Test
- Thể loại: Rock
- Click "Lưu"

### 🎬 [Thành công]
💬 **"Nghệ sĩ mới được thêm thành công với thông báo xanh."**

---

## 🎯 PHẦN 7: ADMIN - QUẢN LÝ ĐỊA ĐIỂM (1 phút)

### 🎬 [Click "Địa Điểm" trong menu Admin]
💬 **"Tương tự, module quản lý địa điểm cũng có validation tránh trùng."**

### 🎬 [Click "Thêm Địa Điểm"]
- Thử tên trùng: "Nhà hát Hòa Bình"
- Lỗi validation hiện ra

💬 **"Validation giúp đảm bảo dữ liệu không bị duplicate trong hệ thống."**

---

## 🎯 PHẦN 8: ADMIN - QUẢN LÝ SỰ KIỆN & LOẠI VÉ (3 phút)

### 🎬 [Chỉ vào Sidebar bên trái]
💬 **"Đây là Admin Panel với sidebar navigation bao gồm:"**
- Sự Kiện
- Nghệ Sĩ
- Địa Điểm
- **Loại Vé** ⭐
- Đơn Hàng

### 🎬 [Click "Loại Vé" trong sidebar]
💬 **"Module Loại Vé quản lý tất cả các loại vé của tất cả sự kiện."**

### 🎬 [Chỉ vào cột "Số Lượng Còn Lại"]
💬 **"Đây là số lượng vé real-time - tự động cập nhật khi có người đặt."**
💬 **"Ví dụ sự kiện này còn X vé VIP, Y vé Regular."**

### 🎬 [Note số lượng, sau đó click "Sự Kiện" trong sidebar]
💬 **"Bây giờ quay lại module Sự Kiện."**

### 🎬 [Click một sự kiện để xem details]
💬 **"Mỗi sự kiện có thông tin chi tiết, nghệ sĩ, và các loại vé."**

---

## 🎯 PHẦN 9: ADMIN - QUẢN LÝ ĐỚN HÀNG (2 phút)

### 🎬 [Click "Đơn Hàng" trong menu Admin]
💬 **"Module quản lý đơn hàng hiển thị tất cả đơn đặt vé trong hệ thống."**

### 🎬 [Chỉ vào đơn hàng vừa tạo]
💬 **"Đây chính là đơn hàng chúng ta vừa tạo lúc nãy."**

### 🎬 [Click "Chỉnh Sửa"]
💬 **"Admin có thể cập nhật trạng thái đơn hàng:"**
- Chờ duyệt → Đã xác nhận → Hoàn thành

### 🎬 [Click nút "Export Excel" ở trên]
💬 **"Hệ thống cho phép xuất báo cáo Excel toàn bộ đơn hàng với đầy đủ thông tin."**

### 🎬 [File Excel được tải xuống]
💬 **"File Excel có định dạng đẹp với header màu, dữ liệu đầy đủ để Admin dễ dàng quản lý và báo cáo."**

---

## 🎯 PHẦN 10: DEMO REAL-TIME TICKET QUANTITY (3 phút) ⭐ QUAN TRỌNG

### 🎬 [Mở 2 tabs browser cạnh nhau]
💬 **"Bây giờ em sẽ demo tính năng real-time tracking số lượng vé."**

### Tab 1: Admin - Quản lý loại vé
### Tab 2: User - Trang đặt vé

### 🎬 [Tab Admin - Note số lượng hiện tại]
💬 **"Ở tab Admin, số lượng vé VIP còn lại là X vé."**

### 🎬 [Tab User - Đặt vé]
💬 **"Em sẽ đặt 2 vé VIP từ tài khoản User."**
- Chọn 2 vé VIP
- Điền thông tin
- Click "Đặt Vé Ngay"

### 🎬 [Quay lại Tab Admin - Refresh]
💬 **"Refresh lại trang Admin, số lượng vé VIP đã giảm từ X xuống còn Y vé."**
💬 **"Điều này chứng tỏ hệ thống đã tự động trừ số lượng vé trong database ngay khi có đơn hàng mới."**

### 🎬 [Tab User - Thử đặt quá số lượng]
💬 **"Bây giờ em sẽ thử đặt số lượng vé lớn hơn số còn lại."**
- Chọn Y+1 vé (nhiều hơn số còn lại)
- Click "Đặt Vé"

### 🎬 [Lỗi hiện ra]
💬 **"Hệ thống từ chối với thông báo: 'Không đủ vé hoặc loại vé không tồn tại!'"**
💬 **"Validation này đảm bảo không bao giờ bán quá số lượng vé có sẵn."**

---

## 🎯 PHẦN 11: TỔNG KẾT (1 phút)

### 🎬 [Quay lại trang chủ]
💬 **"Tóm lại, em đã trình bày Hệ Thống Quản Lý Sự Kiện với các tính năng chính:"**

### ✅ **Chức năng User:**
- Xem danh sách sự kiện
- Xem chi tiết và đặt vé
- Quản lý tài khoản cá nhân
- Xem vé đã đặt với QR code

### ✅ **Chức năng Admin:**
- Quản lý nghệ sĩ, địa điểm, sự kiện
- Quản lý loại vé và đơn hàng
- Export báo cáo Excel
- **Validation tránh trùng dữ liệu**

### ✅ **Điểm nổi bật:**
- **Real-time tracking số lượng vé**
- QR code cho mỗi vé
- Giao diện hiện đại, responsive
- Authentication & Authorization
- Validation toàn diện

💬 **"Em xin cảm ơn các thầy cô và các bạn đã lắng nghe. Em xin hết phần trình bày!"**

---

## 📝 LƯU Ý QUAN TRỌNG KHI DEMO

### ✅ DO:
- Nói rõ ràng, không quá nhanh
- Chỉ rõ từng phần trên màn hình
- Nhấn mạnh các tính năng đặc biệt (real-time, validation, QR code)
- Luôn giải thích "TẠI SAO" có tính năng này

### ❌ DON'T:
- Không click lung tung
- Không đi quá nhanh
- Không bỏ qua lỗi (nếu có lỗi, giải thích và handle)
- Không quên demo phần VALIDATION và REAL-TIME

### 🎯 NẾU BỊ HỎI:

**Q: "Làm sao đảm bảo không bán quá số lượng vé?"**
A: "Em có validation ở 2 level:
1. Client-side: Kiểm tra input
2. Server-side: Kiểm tra database trước khi tạo đơn hàng
3. Transaction để đảm bảo consistency"

**Q: "QR code có unique không?"**
A: "Có ạ, mỗi QR code chứa Order ID unique, có thể dùng để check-in và tránh fake vé."

**Q: "Tại sao cần validation tên trùng?"**
A: "Để tránh duplicate data, dễ quản lý, và tránh nhầm lẫn khi user search."

**Q: "Hệ thống có handle concurrency không?"**
A: "Có ạ, em dùng Entity Framework với transaction để đảm bảo khi nhiều user đặt vé cùng lúc, số lượng vẫn chính xác."

---

## ⏱️ TIMELINE TỔNG (21 phút)

1. Giới thiệu & Trang chủ: 2 phút
2. Chi tiết & Đặt vé: 3 phút
3. Đăng ký/Đăng nhập: 2 phút
4. Quản lý tài khoản: 2 phút
5. Xem vé đã đặt: 1 phút
6. Admin - Nghệ sĩ: 2 phút
7. Admin - Địa điểm: 1 phút
8. Admin - Sự kiện & Loại Vé: 3 phút ⭐
9. Admin - Đơn hàng: 2 phút
10. **Demo Real-time: 3 phút** ⭐
11. Tổng kết: 1 phút

---

## 🔥 TIP GIẢM STRESS

1. **Thở sâu trước khi bắt đầu** - 3 hơi thở
2. **Cầm nước** - Để uống khi cần
3. **Nhìn vào màn hình, không phải người** - Tập trung vào demo
4. **Nếu quên** - Nhìn script này (mở ở tab khác)
5. **Nếu lỗi xảy ra** - Giữ bình tĩnh, F5 refresh
6. **Mỉm cười** - Tự tin là được rồi!

---

**CHÚC BẠN DEMO THÀNH CÔNG! 🎉**
