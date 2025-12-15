using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Core.Model
{
    public class ApiResponse<T> where T : class
    {
        public Dictionary<string, string> Errors { get; set; }
        public T? Data { get; set; }
        public bool Success => Errors == null || !Errors.Any();

        public static ApiResponse<T> SuccessResult(T? data)
        {
            return new ApiResponse<T>
            {
                Errors = [],
                Data = data,
            };
        }
        public static ApiResponse<T> ErrorResult(Dictionary<string, string> errors)
        {
            return new ApiResponse<T>
            {
                Errors = errors,
                Data = null,
            };
        }

        public static ApiResponse<T> ErrorResult(string error)
        {
            return new ApiResponse<T>
            {
                Errors = new Dictionary<string, string>
                {
                    ["general"] = error
                },
                Data = null,
            };
        }
    }
}
