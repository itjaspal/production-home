using api.ModelViews;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace api.Interfaces
{
    interface IUploadFileService
    {
        CommonSearchView<UploadFileView> SearchUploadFile(UploadFileSearchView model);
        void Create(UploadFileView model);
        void Update(UploadFileView model);
        void Delete(UploadFileView model);
        bool CheckDupplicate(string code);
        UploadFileView GetInfo(string code);
    }
}
