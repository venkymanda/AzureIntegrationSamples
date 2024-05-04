using PIM_MappingFunctions_Worker.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace PIM_MappingFunctions_Worker.Services
{
    public interface IPIMWorkerService
    {
       
        public  Task<string> SalesOrderConvertandUploadToBlob(List<string> localpo);
    }
}
