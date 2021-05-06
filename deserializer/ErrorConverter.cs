using System;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace deserializer
{
    public class ErrorConverter : JsonConverter<Error>
    {
        public override Error Read(ref Utf8JsonReader reader, Type typeToConvert, JsonSerializerOptions options)
        {
            string? errorType = null;
            var messages = new List<string>();
            string? stackTrace = null;

            if (reader.TokenType != JsonTokenType.StartObject)
            {
                throw new JsonException($"First token wasn't of type JsonTokenType.StartObject, it was {reader.TokenType}.");
            }

            while (reader.Read())
            {
                switch (reader.TokenType)
                {
                    case JsonTokenType.EndObject:
                        return ConstructError(errorType, messages, stackTrace);
                    case JsonTokenType.PropertyName:
                    {
                        var propertyName = reader.GetString();
                        if (string.IsNullOrEmpty(propertyName)) continue; // here is check for null, warning is incorrect

                        reader.Read();

                        switch (propertyName)
                        {
                            case "errorType":
                            case "ErrorType":
                                errorType = reader.GetString();
                                break;
                            case "messages":
                            case "Messages":
                                messages = PopulateList(reader);
                                break;
                            case "stackTrace":
                            case "StackTrace":
                                stackTrace = reader.GetString();
                                break;
                            case "error":
                            case "Error":
                                messages = new List<string> {reader.GetString()};
                                break;  
                        }

                        break;
                    }
                    case JsonTokenType.None:
                        break;
                    case JsonTokenType.StartObject:
                        break;
                    case JsonTokenType.StartArray:
                        break;
                    case JsonTokenType.EndArray:
                        break;
                    case JsonTokenType.Comment:
                        break;
                    case JsonTokenType.String:
                        break;
                    case JsonTokenType.Number:
                        break;
                    case JsonTokenType.True:
                        break;
                    case JsonTokenType.False:
                        break;
                    case JsonTokenType.Null:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException(nameof(reader));
                }
            }

            return ConstructError(errorType, messages, stackTrace);
        }

        private Error ConstructError(string? errorType, IList<string> messages, string? stackTrace)
        {
            
            if (errorType == null)
            {
                return new Error.ServiceUnavailableError(messages, stackTrace);
            }
            
            if (string.IsNullOrEmpty(errorType)  ||
                string.IsNullOrEmpty(stackTrace) ||
                messages.Count == 0)
            {
                throw new JsonException("Failed to deserialize error.");
            }
            
            var error = errorType switch
            {
                nameof(Error.BadRequestError)         => Error.BadRequest(messages, stackTrace),
                nameof(Error.ServiceUnavailableError) => Error.ServiceUnavailable(messages, stackTrace),
                nameof(Error.NotFoundError)           => Error.NotFound(messages, stackTrace),
                nameof(Error.UnauthorizedError)       => Error.Unauthorized(messages, stackTrace),
                _ => throw new NotImplementedException("Error type is not implemented in deserializer.")
            };
            
            return error;
        }

        private List<string> PopulateList(in Utf8JsonReader reader)
        {
            var list = new List<string>();

            if (reader.TokenType != JsonTokenType.StartArray)
            {
                throw new JsonException("Failed to deserialize array.");
            }

            while (reader.Read())
            {
                if (reader.TokenType == JsonTokenType.EndArray)
                {
                    break;
                }

                var item = reader.GetString();
                if (!string.IsNullOrEmpty(item)) 
                {
                    list.Add(item);    
                } 

            }
            
            return list;
        }
        
        public override void Write(Utf8JsonWriter writer, Error value, JsonSerializerOptions options)
        {
            throw new NotImplementedException("Don't use this converter for serializing.");
        }
    }
}