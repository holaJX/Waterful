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
        #region ��ͼƬ�ϴ�

        /// <summary>
        /// ������Աͷ���ϴ�
        /// </summary>
        /// <returns></returns>
        public async Task<JsonResult> UploadLogo(IList<IFormFile> files)
        {
            //var filePath = string.Format("/Uploads/Images/{0}/{1}/{2}/", now.ToString("yyyy"), now.ToString("yyyyMM"), now.ToString("yyyyMMdd"));
            return await Upload(files, "WorkerLogo");
        }
        /// <summary>
        ///�ϴ���ƷͼƬ
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        public async Task<JsonResult> UploadProductImage(IList<IFormFile> files)
        {
            return await Upload(files, "ProductImage");
        }

        /// <summary>
        /// �ļ��ϴ�(��ͼƬ)
        /// </summary>
        /// <param name="files"></param>
        /// <returns></returns>
        private async Task<JsonResult> Upload(IList<IFormFile> files, string folder)
        {
            if (string.IsNullOrWhiteSpace(folder))
            {
                return Json(new FileVM { success = false, msg = "��Ŀ���ó���" });
            }
            if (files.Count > 0 && files[0] != null)
            {
                try
                {


                    //var uploadfile = Request.Form.Files[0];
                    var uploadfile = files[0];

                    var now = DateTime.Now;
                    //��Ŀ·��
                    var rootPath = _options.Value.Path;//Directory.GetCurrentDirectory();
                                                       //Ŀ¼Ч��
                    CreateFolder(rootPath + folder);
                    //�ļ���׺Ч��
                    var fileExtension = Path.GetExtension(uploadfile.FileName);

                    //ͼƬ��׺Ч��
                    if (fileExtension == null)
                    {
                        return Json(new FileVM { success = false, msg = "�ϴ����ļ�û�к�׺" });
                    }
                    if (FileFilt.IndexOf(fileExtension.ToLower(), StringComparison.Ordinal) <= -1)
                    {
                        return Json(new FileVM { success = false, msg = "���ϴ�jpg��pngͼƬ��ʽ�ļ�" });
                    }

                    //�ж��ļ���С
                    long length = uploadfile.Length;
                    if (length > 1024 * 1024 * 2) //2M
                    {
                        return Json(new FileVM { success = false, msg = "�ϴ����ļ����ܴ���2M" });
                    }

                    var strDateTime = DateTime.Now.ToString("yyMMddhhmmssfff"); //ȡ��ʱ���ַ���
                    var strRan = Convert.ToString(new Random().Next(100, 999)); //������λ�����
                    var saveName = strDateTime + strRan + fileExtension;

                    using (FileStream fs = System.IO.File.Create(Path.Combine(rootPath, folder, saveName)))
                    {
                        await uploadfile.CopyToAsync(fs);
                        fs.Flush();
                    }
                    var url = $"{_options.Value.Url}{folder}/{saveName}";
                    return Json(new FileVM { success = true, msg = "�ϴ��ɹ�", url = url });
                }
                catch (Exception e)
                {
                    return Json(new FileVM { success = false, msg = e.ToString() });
                }
            }
            return Json(new FileVM { success = false, msg = "�ϴ�ʧ��" });
        }
        #endregion

        #region UMEditor
        /// <summary>
        /// UMEditor �����ϴ�
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
                vm.state = "��Ŀ���ó���";
                return Json(vm);
            }
            if (files.Count > 0 && files[0] != null)
            {
                try
                {
                    var uploadfile = files[0];

                    var now = DateTime.Now;
                    //��Ŀ·��
                    var rootPath = _options.Value.Path;//Directory.GetCurrentDirectory();
                                                       //Ŀ¼Ч��
                    CreateFolder(rootPath + folder);

                    //�ļ���׺Ч��
                    var fileExtension = Path.GetExtension(uploadfile.FileName);

                    //ͼƬ��׺Ч��
                    if (fileExtension == null)
                    {
                        vm.state = "�ϴ����ļ�û�к�׺";
                        return Json(vm);
                    }
                    if (FileFilt.IndexOf(fileExtension.ToLower(), StringComparison.Ordinal) <= -1)
                    {
                        vm.state = "���ϴ�jpg��pngͼƬ��ʽ�ļ�";
                        return Json(vm);
                    }

                    //�ж��ļ���С
                    long length = uploadfile.Length;
                    if (length > 1024 * 1024 * 5)
                    {
                        vm.state = "�ϴ����ļ����ܴ���5M";
                        return Json(vm);
                    }

                    var strDateTime = DateTime.Now.ToString("yyMMddhhmmssfff"); //ȡ��ʱ���ַ���
                    var strRan = Convert.ToString(new Random().Next(100, 999)); //������λ�����
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
                    vm.state = "δ֪����";
                    return Json(vm);
                }
            }
            vm.state = "û���ϴ��κ��ļ�";
            return Json(vm);
        }
        #endregion
        #region common
        /// <summary>
        /// �����洢�ļ���
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