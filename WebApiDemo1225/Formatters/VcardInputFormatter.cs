using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;
using WebApiDemo1225.Dtos;

namespace WebApiDemo1225.Formatters
{
    public class VcardInputFormatter : TextInputFormatter
    {
        public VcardInputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/vcard"));

            SupportedEncodings.Add(Encoding.UTF8);
            SupportedEncodings.Add(Encoding.Unicode);
        }

        protected override bool CanReadType(Type type)
            => type == typeof(StudentAddDto);

        public override async Task<InputFormatterResult> ReadRequestBodyAsync(
            InputFormatterContext context, Encoding effectiveEncoding)
        {
            var httpContext = context.HttpContext;
            var serviceProvider = httpContext.RequestServices;

            var logger = serviceProvider.GetRequiredService<ILogger<VcardInputFormatter>>();

            using var reader = new StreamReader(httpContext.Request.Body, effectiveEncoding);
            string? nameLine = null;

            /*
             *  
             *  sb.AppendLine($"FN:{item.Fullname}");
            sb.AppendLine($"SNO:{item.SeriaNo}");
            sb.AppendLine($"AGE:{item.Age}");
            sb.AppendLine($"SC:{item.Score}");
            sb.AppendLine($"UID:{item.Id}");
             */

            try
            {
                await ReadLineAsync("BEGIN:VCARD", reader, context, logger);
                await ReadLineAsync("VERSION:", reader, context, logger);

                var fullname = await ReadLineAsync("FN:", reader, context, logger);
                var seriaNO = await ReadLineAsync("SNO:", reader, context, logger);
                var age = await ReadLineAsync("AGE:", reader, context, logger);
                var score = await ReadLineAsync("SC:", reader, context, logger);
                var obj = new StudentAddDto
                {
                    SeriaNo = seriaNO.Split(':')[1],
                    Age = int.Parse(age.Split(':')[1]),
                    Score = double.Parse(score.Split(':')[1]),
                    Fullname = fullname.Split(':')[1],
                };

                await ReadLineAsync("END:VCARD", reader, context, logger);


                return await InputFormatterResult.SuccessAsync(obj);
            }
            catch
            {
                return await InputFormatterResult.FailureAsync();
            }
        }

        private static async Task<string> ReadLineAsync(
            string expectedText, StreamReader reader, InputFormatterContext context,
            ILogger logger)
        {
            var line = await reader.ReadLineAsync();

            if (line is null || !line.StartsWith(expectedText))
            {
                var errorMessage = $"Looked for '{expectedText}' and got '{line}'";

                context.ModelState.TryAddModelError(context.ModelName, errorMessage);

                throw new Exception(errorMessage);
            }

            return line;
        }
    }
}
