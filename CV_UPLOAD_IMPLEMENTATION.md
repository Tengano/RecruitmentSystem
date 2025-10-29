# Hướng dẫn triển khai quản lý CV/Resume

## ✅ Các tính năng đã hoàn thành:

### 1. **Giới hạn kích thước file upload**
- Giới hạn: **5MB**
- Validation ở cả client và server side

### 2. **Validate định dạng file**
- Chỉ cho phép: **PDF và DOCX**
- Kiểm tra cả extension và content type
- Hiển thị thông báo lỗi rõ ràng

### 3. **Lưu metadata của file**
- `TenFileCV`: Tên file gốc
- `DuongDanCV`: Đường dẫn lưu trữ
- `KichThuocFile`: Kích thước file (bytes)
- `LoaiFile`: Loại file (.pdf hoặc .docx)
- `NgayUploadCV`: Ngày giờ upload

### 4. **Chức năng download/xem CV**
- Download CV: `AdminController.DownloadCV()`
- Xem CV trực tiếp (PDF): `AdminController.ViewCV()`
- Hiển thị thông tin file trong ApplicationDetails
- Nút download/view trong danh sách Applications

### 5. **Tự động xóa file khi xóa đơn**
- Xóa file khi xóa đơn ứng tuyển
- Xóa tất cả CV khi xóa việc làm
- Action riêng để xóa đơn ứng tuyển

---

## 📋 Các file đã tạo/cập nhật:

### Models
- ✅ `Models/Application.cs` - Thêm fields metadata CV

### Services
- ✅ `Services/FileUploadService.cs` - Service xử lý upload file
  - Validate file (size, extension, content type)
  - Upload file
  - Delete file
  - Format file size

### Controllers
- ✅ `Controllers/JobsController.cs`
  - Sử dụng FileUploadService
  - Validate file trước khi upload
  - Lưu metadata

- ✅ `Controllers/AdminController.cs`
  - `DownloadCV()` - Tải CV
  - `ViewCV()` - Xem CV
  - `DeleteApplication()` - Xóa đơn ứng tuyển
  - Tự động xóa CV khi xóa Job/Application

### Views
- ✅ `Views/Jobs/Apply.cshtml` - Cập nhật form upload
- ✅ `Views/Admin/Applications.cshtml` - Thêm cột CV và nút download
- ✅ `Views/Admin/ApplicationDetails.cshtml` - Hiển thị thông tin CV chi tiết
- ✅ `Views/Admin/DeleteApplication.cshtml` - Trang xác nhận xóa

### Migrations
- ✅ `Migrations/20250130000000_AddCVMetadataToApplication.cs`

### Configuration
- ✅ `Program.cs` - Đăng ký FileUploadService

---

## 🚀 Các bước triển khai:

### Bước 1: Dừng ứng dụng nếu đang chạy

### Bước 2: Chạy migration
```bash
dotnet ef database update
```

### Bước 3: Kiểm tra thư mục uploads
Đảm bảo thư mục `/wwwroot/uploads/resumes` tồn tại (sẽ tự động tạo khi upload)

### Bước 4: Build và chạy ứng dụng
```bash
dotnet build
dotnet run
```

---

## 🧪 Testing:

### Test Upload CV:
1. Vào trang "Tìm việc làm"
2. Chọn một công việc và click "Ứng tuyển"
3. Điền thông tin và upload file CV (PDF hoặc DOCX, < 5MB)
4. Submit form

### Test Validation:
- Upload file không đúng định dạng (JPG, PNG...) → Hiển thị lỗi
- Upload file > 5MB → Hiển thị lỗi
- Không chọn file → Hiển thị lỗi (required)

### Test Download/View CV:
1. Đăng nhập admin
2. Vào "Đơn ứng tuyển"
3. Click nút Download hoặc Xem (nếu là PDF)

### Test Xóa CV:
1. Xóa đơn ứng tuyển → File CV tự động xóa
2. Xóa việc làm → Tất cả CV của đơn liên quan bị xóa

---

## 📊 Thông tin validation:

| Tiêu chí | Giá trị |
|----------|---------|
| Kích thước tối đa | 5MB |
| Định dạng cho phép | PDF, DOCX |
| Bắt buộc upload | Có |
| Content Types | `application/pdf`, `application/vnd.openxmlformats-officedocument.wordprocessingml.document` |

---

## 🔒 Bảo mật:

1. ✅ Validate kích thước file
2. ✅ Validate extension
3. ✅ Validate content type
4. ✅ Tên file unique (GUID prefix)
5. ✅ Lưu trữ ngoài wwwroot/uploads
6. ✅ Xóa file khi xóa record

---

## 💡 Lưu ý:

- File được lưu tại: `/wwwroot/uploads/resumes/`
- Tên file format: `{GUID}_{TenFileGoc}`
- CV chỉ admin mới xem/tải được
- Tự động xóa file vật lý khi xóa record
- Hiển thị thông tin file đầy đủ (tên, size, type, ngày upload)

---

## 🎨 UI Features:

- Badge hiển thị trạng thái CV trong danh sách
- Card hiển thị thông tin CV chi tiết
- Buttons: View (PDF only) và Download
- Icon phân biệt loại file
- Responsive design

---

## 🔧 Troubleshooting:

### Nếu migration fail:
```bash
dotnet ef migrations remove
dotnet ef migrations add AddCVMetadataToApplication
dotnet ef database update
```

### Nếu không upload được:
- Kiểm tra quyền write thư mục wwwroot/uploads
- Kiểm tra IIS/Server upload file size limit

### Nếu không download được:
- Kiểm tra file tồn tại
- Kiểm tra đường dẫn file trong database

---

**Hoàn tất! Hệ thống quản lý CV/Resume đã được triển khai đầy đủ.** ✨

