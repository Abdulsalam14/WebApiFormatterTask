using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;
using WebApiDemo1225.Dtos;
using WebApiDemo1225.Entities;

namespace WebApiDemo1225.Formatters
{
    public class VCardOutputFormatter : TextOutputFormatter
    {
        public VCardOutputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/vcard"));
            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }
        public override Task WriteResponseBodyAsync(OutputFormatterWriteContext context, Encoding selectedEncoding)
        {
            var response = context.HttpContext.Response;
            var sb = new StringBuilder();
            if (context.Object is IEnumerable<StudentDto> list)
            {
                foreach (var item in list)
                {
                    FormatVCard(sb, item);
                }
            }
            else if (context.Object is StudentDto item)
            {
                FormatVCard(sb, item);
            }

            return response.WriteAsync(sb.ToString());
        }

        private void FormatVCard(StringBuilder sb, StudentDto item)
        {
            sb.AppendLine("BEGIN:VCARD");
            sb.AppendLine("VERSION:2.1");
            sb.AppendLine($"FN:{item.Fullname}");
            sb.AppendLine($"SNO:{item.SeriaNo}");
            sb.AppendLine($"AGE:{item.Age}");
            sb.AppendLine($"SC:{item.Score}");
            sb.AppendLine($"UID:{item.Id}");
            sb.AppendLine("END:VCARD");
        }
    }
}
