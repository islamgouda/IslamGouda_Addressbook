using System.Drawing;
using System.Drawing.Imaging;


namespace api.Helper
{
    public static class FileHelper
    {
        private static IWebHostEnvironment _hostingEnvironment;

        public static bool IsInitialized { get; private set; }

        // for dependeny injection 
        public static void Initialize(IWebHostEnvironment hostEnvironment)
        {
            if (IsInitialized)
                throw new InvalidOperationException("Object already initialized");

            _hostingEnvironment = hostEnvironment;
            IsInitialized = true;
        }
        public static string SaveStudentImageFromBase64(string base64Img)
        {
            try
            {
                if (!IsInitialized)
                    throw new InvalidOperationException("Object is not initialized");
                var path = Path.GetFullPath(Path.Combine(_hostingEnvironment.WebRootPath, "images\\addresss\\"));
                if (string.IsNullOrEmpty(base64Img))
                {
                    return "";
                }
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                string[] Objects = base64Img.Split(',');
                string Base64String = Objects[1];
                var img = LoadImage(Base64String);
                var fileName = DateTime.Now.ToString("yyyyMMddHHmmssffff") + "." + img.RawFormat;
                using (Bitmap tempImage = new Bitmap(img))
                {
                    tempImage.Save(path + fileName, img.RawFormat);
                }

                return "images\\addresss\\" + fileName;
            }
            catch
            (Exception ex)
            {
                throw ex;
            }
        }

        private static Image LoadImage(string base64Img)
        {

            byte[] bytes = Convert.FromBase64String(base64Img);

            Image image;
            using (MemoryStream ms = new MemoryStream(bytes))
            {
                image = Image.FromStream(ms);
            }

            return image;
        }
    }
}
