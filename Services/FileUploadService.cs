namespace RecruitmentSystem.Services
{
    public interface IFileUploadService
    {
        Task<FileUploadResult> UploadResumeAsync(IFormFile file);
        bool DeleteResume(string filePath);
        bool ValidateResumeFile(IFormFile file, out string errorMessage);
        string GetFileExtension(string fileName);
        string FormatFileSize(long bytes);
    }

    public class FileUploadResult
    {
        public bool Success { get; set; }
        public string? FileName { get; set; }
        public string? FilePath { get; set; }
        public long FileSize { get; set; }
        public string? FileType { get; set; }
        public string? ErrorMessage { get; set; }
    }

    public class FileUploadService : IFileUploadService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly long _maxFileSize = 5 * 1024 * 1024; // 5MB
        private readonly string[] _allowedExtensions = { ".pdf", ".docx" };
        private readonly string[] _allowedContentTypes = { 
            "application/pdf", 
            "application/vnd.openxmlformats-officedocument.wordprocessingml.document" 
        };

        public FileUploadService(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        public bool ValidateResumeFile(IFormFile file, out string errorMessage)
        {
            errorMessage = string.Empty;

            if (file == null || file.Length == 0)
            {
                errorMessage = "Vui lòng chọn file CV/Resume";
                return false;
            }

            // Kiểm tra kích thước file
            if (file.Length > _maxFileSize)
            {
                errorMessage = $"Kích thước file vượt quá giới hạn cho phép ({FormatFileSize(_maxFileSize)})";
                return false;
            }

            // Kiểm tra định dạng file theo extension
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            if (!_allowedExtensions.Contains(extension))
            {
                errorMessage = "Chỉ chấp nhận file PDF hoặc DOCX";
                return false;
            }

            // Kiểm tra content type
            if (!_allowedContentTypes.Contains(file.ContentType.ToLowerInvariant()))
            {
                errorMessage = "Định dạng file không hợp lệ";
                return false;
            }

            return true;
        }

        public async Task<FileUploadResult> UploadResumeAsync(IFormFile file)
        {
            var result = new FileUploadResult();

            try
            {
                // Validate file
                if (!ValidateResumeFile(file, out string errorMessage))
                {
                    result.Success = false;
                    result.ErrorMessage = errorMessage;
                    return result;
                }

                // Tạo thư mục nếu chưa tồn tại
                var uploadFolder = Path.Combine(_environment.WebRootPath, "uploads", "resumes");
                Directory.CreateDirectory(uploadFolder);

                // Tạo tên file duy nhất
                var uniqueFileName = $"{Guid.NewGuid()}_{Path.GetFileName(file.FileName)}";
                var filePath = Path.Combine(uploadFolder, uniqueFileName);

                // Lưu file
                using (var fileStream = new FileStream(filePath, FileMode.Create))
                {
                    await file.CopyToAsync(fileStream);
                }

                // Trả về kết quả
                result.Success = true;
                result.FileName = file.FileName;
                result.FilePath = $"/uploads/resumes/{uniqueFileName}";
                result.FileSize = file.Length;
                result.FileType = Path.GetExtension(file.FileName).ToLowerInvariant();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = $"Lỗi khi upload file: {ex.Message}";
            }

            return result;
        }

        public bool DeleteResume(string filePath)
        {
            try
            {
                if (string.IsNullOrEmpty(filePath))
                    return false;

                // Chuyển đổi đường dẫn tương đối thành đường dẫn tuyệt đối
                var fullPath = Path.Combine(_environment.WebRootPath, filePath.TrimStart('/').Replace("/", Path.DirectorySeparatorChar.ToString()));

                if (File.Exists(fullPath))
                {
                    File.Delete(fullPath);
                    return true;
                }

                return false;
            }
            catch
            {
                return false;
            }
        }

        public string GetFileExtension(string fileName)
        {
            return Path.GetExtension(fileName).ToLowerInvariant();
        }

        public string FormatFileSize(long bytes)
        {
            string[] sizes = { "B", "KB", "MB", "GB" };
            double len = bytes;
            int order = 0;

            while (len >= 1024 && order < sizes.Length - 1)
            {
                order++;
                len = len / 1024;
            }

            return $"{len:0.##} {sizes[order]}";
        }
    }
}

