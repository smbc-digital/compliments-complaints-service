using System;
using System.Net.Http;
using System.Text.Json;
using StockportGovUK.AspNetCore.Gateways.Response;

namespace compliments_complaints_service.Utils.Parsers
{
    public static class HttpResponseMessageParser
    {
        public static HttpResponse<T> Parse<T>(this HttpResponseMessage responseMessage)
        {
            T deserializedObject;

            try
            {
                var content = responseMessage.Content.ReadAsStringAsync().Result;
                deserializedObject = JsonSerializer.Deserialize<T>(content);
            }
            catch (Exception)
            {
                deserializedObject = default(T);
            }

            return new HttpResponse<T>
            {
                ResponseContent = deserializedObject,
                Headers = responseMessage.Headers,
                IsSuccessStatusCode = responseMessage.IsSuccessStatusCode,
                ReasonPhrase = responseMessage.ReasonPhrase,
                StatusCode = responseMessage.StatusCode,
                Version = responseMessage.Version
            };
        }
    }
}