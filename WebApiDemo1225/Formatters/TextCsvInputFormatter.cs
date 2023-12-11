using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.Net.Http.Headers;
using System.Text;
using WebApiDemo1225.Dtos;

namespace WebApiDemo1225.Formatters
{
    public class TextCsvInputFormatter:TextInputFormatter
    {
        public TextCsvInputFormatter()
        {
            SupportedMediaTypes.Add(MediaTypeHeaderValue.Parse("text/csv"));

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

            var logger = serviceProvider.GetRequiredService<ILogger<StudentAddDto>>();

            using var reader = new StreamReader(httpContext.Request.Body, effectiveEncoding);
            string? addLine = null;

            try
            {
                await ReadLineAsync("Fullname - SeriaNo - Age - Score", reader, context, logger);

                addLine = await ReadLineAsync("", reader, context, logger);

                var split = addLine.Split("-");

                var student = new StudentAddDto()
                {
                    Fullname = split[0].Trim(),
                    SeriaNo = split[1].Trim(),
                    Age = int.Parse(split[2].Trim()),
                    Score = double.Parse(split[3].Trim())
                };

                return await InputFormatterResult.SuccessAsync(student);
            }
            catch
            {
                logger.LogError("Read failed: nameLine = {nameLine}", addLine);
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
                logger.LogError(errorMessage);

                throw new Exception(errorMessage);
            }

            return line;
        }
    }
}
