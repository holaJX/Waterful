using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.IO;
using Microsoft.Extensions.Options;
using GZ.Platform.Core.Options;
using Waterful.Back.ViewModels;

namespace Waterful.Back.Controllers
{
    public class FileController : LoginControllerBase
    {
        private IOptions<FileOption> _options;
        public FileController(IOptions<FileOption> options)
        {
            _options = options;
        }
        private const string FileFilt = ".gif|.jpg|.jpeg|.png|.bmp";
        #region 单图片上传

        /// <summary>
        /// 服务人员头像上传
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> UploadLogo(IList<IFormFile> files)
        {
            //var filePath = string.Format("/Uploads/Images/{0}/{1}/{2}/", now.ToString("yyyy"), now.ToString("yyyyMM"), now.ToString("yyyyMMdd"));
            return await Upload(files, "WorkerLogo");
        }
        /// <summary>
        ///上传商品图片
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public async Task<JsonResult> UploadProductImage(IList<IFormFile> files)
        {
            return await Upload(files, "ProductImage");
        }

        /// <summary>
        /// 文件上传(仅图片)
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        private async Task<JsonResult> Upload(IList<IFormFile> files, string folder)
        {
            if (string.IsNullOrWhiteSpace(folder))
            {
                return Json(new FileVM { success = false, msg = "项目配置出错" });
            }
            if (files.Count > 0 && files[0] != null)
            {
                try
                {


                    //var uploadfile = Request.Form.Files[0];
                    var uploadfile = files[0];

                    var now = DateTime.Now;
                    //项目路径
                    var rootPath = _options.Value.Path;//Directory.GetCurrentDirectory();
                                                       //目录效验
                    CreateFolder(rootPath + folder);
                    //文件后缀效验
                    var fileExtension = Path.GetExtension(uploadfile.FileName);

                    //图片后缀效验
                    if (fileExtension == null)
                    {
                        return Json(new FileVM { success = false, msg = "上传的文件没有后缀" });
                    }
                    if (FileFilt.IndexOf(fileExtension.ToLower(), StringComparison.Ordinal) <= -1)
                    {
                        return Json(new FileVM { success = false, msg = "请上传jpg、png图片格式文件" });
                    }

                    //判断文件大小
                    long length = uploadfile.Length;
                    if (length > 1024 * 1024 * 2) //2M
                    {
                        return Json(new FileVM { success = false, msg = "上传的文件不能大于2M" });
                    }

                    var strDateTime = DateTime.Now.ToString("yyMMddhhmmssfff"); //取得时间字符串
                    var strRan = Convert.ToString(new Random().Next(100, 999)); //生成三位随机数
                    var saveName = strDateTime + strRan + fileExtension;

                    using (FileStream fs = System.IO.File.Create(Path.Combine(rootPath, folder, saveName)))
                    {
                        await uploadfile.CopyToAsync(fs);
                        fs.Flush();
                    }
                    var url = $"{_options.Value.Url}{folder}/{saveName}";
                    return Json(new FileVM { success = true, msg = "上传成功", url = url });
                }
                catch (Exception e)
                {
                    return Json(new FileVM { success = false, msg = e.ToString() });
                }
            }
            return Json(new FileVM { success = false, msg = "上传失败" });
        }
        #endregion

        #region UMEditor
        /// <summary>
        /// UMEditor 测试上传
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public async Task<JsonResult> UpUMImage(IList<IFormFile> upfile)
        {
            return await UploadUE(upfile, "Test");
        }

        private async Task<JsonResult> UploadUE(IList<IFormFile> files, string folder)
        {
            UMEditorVM vm = new UMEditorVM();
            if (string.IsNullOrWhiteSpace(folder))
            {
                vm.state = "项目配置出错";
                return Json(vm);
            }
            if (files.Count > 0 && files[0] != null)
            {
                try
                {
                    var uploadfile = files[0];

                    var now = DateTime.Now;
                    //项目路径
                    var rootPath = _options.Value.Path;//Directory.GetCurrentDirectory();
                                                       //目录效验
                    CreateFolder(rootPath + folder);

                    //文件后缀效验
                    var fileExtension = Path.GetExtension(uploadfile.FileName);

                    //图片后缀效验
                    if (fileExtension == null)
                    {
                        vm.state = "上传的文件没有后缀";
                        return Json(vm);
                    }
                    if (FileFilt.IndexOf(fileExtension.ToLower(), StringComparison.Ordinal) <= -1)
                    {
                        vm.state = "请上传jpg、png图片格式文件";
                        return Json(vm);
                    }

                    //判断文件大小
                    long length = uploadfile.Length;
                    if (length > 1024 * 1024 * 5)
                    {
                        vm.state = "上传的文件不能大于5M";
                        return Json(vm);
                    }

                    var strDateTime = DateTime.Now.ToString("yyMMddhhmmssfff"); //取得时间字符串
                    var strRan = Convert.ToString(new Random().Next(100, 999)); //生成三位随机数
                    var saveName = strDateTime + strRan + fileExtension;

                    using (FileStream fs = System.IO.File.Create(Path.Combine(rootPath, folder, saveName)))
                    {
                        await uploadfile.CopyToAsync(fs);
                        fs.Flush();
                    }
                    var url = $"{_options.Value.Url}{folder}/{saveName}";

                    vm.url = url;
                    vm.originalName = uploadfile.FileName;
                    vm.name = saveName;
                    vm.size = length.ToString();
                    vm.type = fileExtension;
                    return Json(vm);
                }
                catch (Exception e)
                {
                    vm.state = "未知错误";
                    return Json(vm);
                }
            }
            vm.state = "没有上传任何文件";
            return Json(vm);
        }
        #endregion
        #region common
        /// <summary>
        /// 创建存储文件夹
        /// </summary>
        /// <param name="uploadpath"></param>
        private void CreateFolder(string uploadpath)
        {
            if (!Directory.Exists(uploadpath))
            {
                Directory.CreateDirectory(uploadpath);
            }
        }

        #endregion
    }

}