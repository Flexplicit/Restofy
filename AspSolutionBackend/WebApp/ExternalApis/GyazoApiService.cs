using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Helpers;
using Newtonsoft.Json;
using WebApp.SpecialExternalApiHelpers;

namespace WebApp.ExternalApis
{
    /// <summary>
    /// This is used in the backend to store images in an external cloud server via gyazo
    /// </summary>
    public class GyazoApiService
    {
        // TODO: Make it more secure
        private string BaseToken { get; set; } = "8309ce809a2f90bf37879153073b0c53f379c92e475cb580ae00df4eb5ae2df5";


        /// <summary>
        /// Uploads an encoded base64 image to Gyazo and returns a short .jpg/png/jpeg link
        /// Action only takes 200-300ms and helps us to avoid adding bloated blobs/base64 strings into database.
        /// </summary>
        /// <param name="base64EncodedImage"></param>
        /// <exception cref="Exception"></exception>
        public async Task<GyazoResponse> UploadImageViaApi(string base64EncodedImage)
        {
            const string baseUrl = @"https://upload.gyazo.com/api/upload";
            var returnResponse = new GyazoResponse();

            if (Regex.IsMatch(base64EncodedImage, @"^https:\/\/i.gyazo.com\/[^\.]+.(?:png|jpg|jpeg)"))
            {
                return new GyazoResponse()
                {
                    IsSuccessfulResponse = true,
                    Url = base64EncodedImage
                };
            }


            var repairedImage = Base64Operations.RepairBase64StringToCorrectFormat(base64EncodedImage);
            var correctPartOfImageBase64 = repairedImage.Split(",");
            if (correctPartOfImageBase64.Length != 2) //  should be [data:image/jpeg], [decodedBase64]
            {
                returnResponse.Message = "Bad image raw data";
                return returnResponse;
                // throw new Exception("Bad image raw data");
            }

            var imageType = correctPartOfImageBase64[0]; // TODO: we can log it for diagnostics
            var repairedImageRawDecodedBase64 = correctPartOfImageBase64[1];

            var imageInBytes = Base64Operations.ValidateBase64(repairedImageRawDecodedBase64);
            if (imageInBytes.Length < 3)
            {
                returnResponse.Message = "Not a picture file";
                return returnResponse;
            }

            using (var httpClient = new HttpClient()) // Client that actually send the post
            {
                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue
                    ("Bearer", BaseToken);
                using (var form = new MultipartFormDataContent()) // Contains my multipart/form-data body content
                {
                    using (var
                        imageContent = new ByteArrayContent(imageInBytes)) // image raw binary in multipartformdata
                    {
                        imageContent.Headers.ContentType = MediaTypeHeaderValue.Parse("multipart/form-data");
                        form.Add(imageContent, "imagedata", "jpeg"); // file name must match server's
                        HttpResponseMessage response = await httpClient.PostAsync(baseUrl, form);
                        if (response.IsSuccessStatusCode)
                        {
                            returnResponse.Url = JsonConvert
                                .DeserializeObject<Dictionary<string, string>>(response
                                    .Content
                                    .ReadAsStringAsync().Result)["url"];
                            returnResponse.IsSuccessfulResponse = true;
                            return returnResponse;
                        }

                        Console.WriteLine(await response.Content.ReadAsStringAsync());
                    }
                }
            }
            returnResponse.Message = "Problem with converting bytes into blob";
            return returnResponse;
        }
    }
}