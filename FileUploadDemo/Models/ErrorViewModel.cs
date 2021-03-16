using System;
using System.Collections.Generic;

namespace FileUploadDemo.Models
{
    public class ErrorViewModel
    {
        public string RequestId { get; set; }

        public bool ShowRequestId => !string.IsNullOrEmpty(RequestId);
    }

    public class ViewImagesViewModel
    {
        public List<Image> Images { get; set; }
    }
}
