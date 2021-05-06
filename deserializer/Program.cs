using System;
using System.Collections.Generic;
using System.Text.Json;

namespace deserializer
{
    class Program
    {
        static void Main(string[] args)
        {
            var json =
                @"{""ErrorType"":""BadRequestError"",""StatusCode"":400,""StackTrace"":"" STACKTRACEFOO "",""Messages"":[""User does not exist""]}";

            var foo = JsonSerializer.Deserialize<Foo>(json);

            var bar = JsonSerializer.Serialize(new Foo
            {
                Messages = new List<string> {"User does not exist"},
                ErrorType = "BacRequestError",
                StackTrace =  "something",
                StatusCode =  400
            });

            var zxc = JsonSerializer.Deserialize<Foo>(bar);

            var serializeOptions = new JsonSerializerOptions();
            serializeOptions.Converters.Add(new ErrorConverter());
            serializeOptions.WriteIndented = true;
            var error = JsonSerializer.Deserialize<Error>(json, serializeOptions);
            
            Console.WriteLine(foo.StatusCode);
        }
    }
    
    public class Foo 
    {
        public string ErrorType { get; set; }
        public int StatusCode { get; set; }
        public string StackTrace { get; set; }
        public List<string> Messages { get; set; }
    }
}