using System;
using System.Collections.Generic;
using System.Text;

namespace SF.Core.Utilities.Results
{


    /// <summary>
    /// rest servisten Idataresult alınca buna dönüştürüp kullanılımalıdır
    /// örnek: apiClient.ExecutePost<GenericDataResult>(...)
    /// </summary>
    public class GenericDataResult<T> : IDataResult<T>
    {
        public GenericDataResult()
        {

        }

        public T Data { get; set; }

        public bool Success { get; set; }

        public string Message { get; set; }
    }

}
