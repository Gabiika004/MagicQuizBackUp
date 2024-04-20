using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;
using MagicQuizDesktop.Models;
using System.Text;
using System.Windows;
using System;
using System.Net.Http.Headers;
using System.Diagnostics;

public class QuizApiService
{
    private readonly HttpClient _httpClient;
    private readonly string _baseUrl = "http://127.0.0.1:8000/api"; // Cseréld le a saját URL-edre

    public QuizApiService()
    {
        _httpClient = new HttpClient();
    }

    private ApiResponse<T> HandleException<T>(Exception ex)
    {
        Debug.WriteLine("Exception caught: " + ex.Message);
        return new ApiResponse<T>
        {
            Success = false,
            Message = "Error: " + ex.Message,
            StatusCode = System.Net.HttpStatusCode.InternalServerError // Default, feltételezve, hogy a hiba a szerver oldali problémára utal
        };
    }   
    private ApiResponseWithNoData HandleExceptionWithNoData(Exception ex)
    {
        Debug.WriteLine("Exception caught: " + ex.Message);
        return new ApiResponseWithNoData
        {
            Message = "Error: " + ex.Message,
            StatusCode = System.Net.HttpStatusCode.InternalServerError // Default, feltételezve, hogy a hiba a szerver oldali problémára utal
        };
    }

    public async Task<ApiResponse<T>> PostAsync<T>(string uri, object data, string authToken = "")
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            var content = new StringContent(JsonConvert.SerializeObject(data), Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}{uri}", content);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            var apiResponse = new ApiResponse<T>
            {
                StatusCode = response.StatusCode,
                Message = ApiResponse<T>.ParseErrorMessage(jsonResponse),
                Success = response.IsSuccessStatusCode
            };

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Received response: " + jsonResponse);
                apiResponse.Data = JsonConvert.DeserializeObject<T>(jsonResponse);
            }
            else
            {
                Debug.WriteLine($"Failed POST request: {response.StatusCode}\n{jsonResponse}");
            }

            return apiResponse;
        }
        catch (Exception ex)
        {
            return HandleException<T>(ex);
        }
    }

    public async Task<ApiResponseWithNoData> PostAsyncWithNoData(string uri, string authToken)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            var content = new StringContent(string.Empty, Encoding.UTF8, "application/json");
            var response = await _httpClient.PostAsync($"{_baseUrl}{uri}", content);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            var apiResponse = new ApiResponseWithNoData
            {
                StatusCode = response.StatusCode,
                Message = ApiResponseWithNoData.ParseErrorMessage(jsonResponse),
            };

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Received response: " + jsonResponse);
            }
            else
            {
                Debug.WriteLine($"Failed POST request: {response.StatusCode}\n{jsonResponse}");
            }

            return apiResponse;
        }
        catch (Exception ex)
        {

            return HandleExceptionWithNoData(ex);
        }
    }
    public async Task<ApiResponse<T>> GetAsync<T>(string uri, string authToken)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            var response = await _httpClient.GetAsync($"{_baseUrl}{uri}");
            var jsonResponse = await response.Content.ReadAsStringAsync();

            var apiResponse = new ApiResponse<T>
            {
                StatusCode = response.StatusCode,
                Message = ApiResponse<T>.ParseErrorMessage(jsonResponse),
                Success = response.IsSuccessStatusCode
            };

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Received GET response: " + jsonResponse);
                try
                {
                    apiResponse.Data = JsonConvert.DeserializeObject<T>(jsonResponse);
                }
                catch (JsonReaderException jex)
                {
                    Debug.WriteLine("JSON reading error in GET: " + jex.Message);
                    apiResponse.Message = "JSON reading error: " + jex.Message;
                }
                catch (Exception ex)
                {
                    Debug.WriteLine("General deserialization error in GET: " + ex.Message);
                    apiResponse.Message = "General deserialization error: " + ex.Message;
                }
            }
            else
            {
                Debug.WriteLine($"Failed GET request: {response.StatusCode}\n{jsonResponse}");
            }

            return apiResponse;
        }
        catch (Exception ex)
        {

            return HandleException<T>(ex);
        }
    }

    public async Task<ApiResponse<T>> PutAsync<T>(string uri, object data, string authToken)
    {
        try
        {       
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);

            var json = JsonConvert.SerializeObject(data);
            var content = new StringContent(json, Encoding.UTF8, "application/json");
            var response = await _httpClient.PutAsync($"{_baseUrl}{uri}", content);
            var jsonResponse = await response.Content.ReadAsStringAsync();

            var apiResponse = new ApiResponse<T>
            {
                StatusCode = response.StatusCode,
                Message = ApiResponse<T>.ParseErrorMessage(jsonResponse),
                Success = response.IsSuccessStatusCode
            };

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Received response from PUT: " + jsonResponse);
                apiResponse.Data = JsonConvert.DeserializeObject<T>(jsonResponse);
            }
            else
            {
                Debug.WriteLine($"Failed PUT request: {response.StatusCode}\n{jsonResponse}");
            }

            return apiResponse;
        }
        catch (Exception ex)
        {

            return HandleException<T>(ex);
        }
    }

    public async Task<ApiResponseWithNoData> DeleteAsync(string uri, string authToken)
    {
        try
        {
            _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", authToken);
            var response = await _httpClient.DeleteAsync($"{_baseUrl}{uri}");
            var responseContent = await response.Content.ReadAsStringAsync();

            var apiResponse = new ApiResponseWithNoData
            {
                StatusCode = response.StatusCode,
                Message = ApiResponseWithNoData.ParseErrorMessage(responseContent)
            };

            if (response.IsSuccessStatusCode)
            {
                Debug.WriteLine("Received response: " + responseContent);
            }
            else
            {
                Debug.WriteLine($"Failed POST request: {response.StatusCode}\n{responseContent}");
            }

            return apiResponse;
        }
        catch (Exception ex)
        {

            return HandleExceptionWithNoData(ex);
        }
    }
}
