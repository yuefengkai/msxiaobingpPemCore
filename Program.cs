using System;
using System.IO;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;

namespace msxiaobingCore
{
    class Program
    {
        static void Main(string[] args)
        {
            var path = Path.GetDirectoryName(System.Reflection.Assembly.GetEntryAssembly().Location);
            //Console.WriteLine(path);

            var files = new DirectoryInfo(path).GetFiles("*.jpg");

            foreach (var file in files)
            {   
                var poemPath=$"{path}/{Path.GetFileNameWithoutExtension(file.FullName)}.json";

                if(File.Exists(poemPath)){
                    continue;
                }

                Console.WriteLine($"{file.FullName} poem is start");

                var base64String = ImageToBase64String(file.FullName);

                var poem = ComposePoem(base64String);

                File.WriteAllTextAsync(poemPath, poem);
                
                Console.WriteLine($"{file.FullName} poem is Ok");
            }

            Console.WriteLine("all is Ok");
        }

        //写诗
        static string ComposePoem(string imageBase64)
        {
            string url = "https://poem.msxiaobing.com/api/upload";//小冰上传图片地址    

            HttpRequestClient httpRequestClient = new HttpRequestClient();
            httpRequestClient.SetFieldValue("userid", new Guid().ToString("x"));//发送数据  
            httpRequestClient.SetFieldValue("text", "");//发送数据    
            httpRequestClient.SetFieldValue("guid", new Guid().ToString("x"));//发送数据    
            httpRequestClient.SetFieldValue("image", imageBase64.Split(',')[1]);//发送数据                                       
            string responseText = string.Empty;
            bool uploaded = httpRequestClient.Upload(url, out responseText);

            return responseText;
        }

         //图片转为base64编码的字符串  
        static string ImageToBase64String(string Imagefilename)
        {
            try
            {
                using (var image = Image.Load(Imagefilename))
                {
                    return image.ToBase64String(ImageFormats.Jpeg);
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.ToString());
                return null;
            }
        }
    }
}
