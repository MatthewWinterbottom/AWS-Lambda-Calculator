using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Alexa.NET.Helpers;
using Alexa.NET.Response;
using Alexa.NET.Request.Type;
using Alexa.NET;
using Amazon.Lambda.Core;
using Alexa.NET.Request;

// Assembly attribute to enable the Lambda function's JSON input to be converted into a .NET class.
[assembly: LambdaSerializer(typeof(Amazon.Lambda.Serialization.Json.JsonSerializer))]

namespace AWSLambdaHelloWorld
{
    public class Function
    {


        public SkillResponse FunctionHandler(SkillRequest input, ILambdaContext context)
        {
            //Alexa Skills Variables
            var requestType = input.GetRequestType();
            var speech = new SsmlOutputSpeech();
            var finalResponse = ResponseBuilder.Tell("");

            //Calculator Variables
            string oper;
            int firstNumber;
            int secondNumber;
            int result = 0;

            if (requestType == typeof(IntentRequest))
            {
                var intentRequest = input.Request as IntentRequest;

                if (intentRequest.Intent.Name.Equals("CalculateIntent"))
                {
                    firstNumber = Convert.ToInt32(intentRequest.Intent.Slots["firstnumber"].Value);
                    secondNumber = Convert.ToInt32(intentRequest.Intent.Slots["secondnumber"].Value);
                    oper = intentRequest.Intent.Slots["operator"].Value;
                    
                    switch (oper)
                    {
                        case "add":
                        case "plus": 
                            result = Calculator.add(firstNumber, secondNumber);
                            break;

                        case "subtract":
                        case "minus":
                            result = Calculator.minus(firstNumber, secondNumber);
                            break;

                        case "multiply":
                        case "times":
                            result = Calculator.times(firstNumber, secondNumber);
                            break;

                        case "divide":
                            result = Calculator.divide(firstNumber, secondNumber);
                            break;

                        default:
                            break;
                    }

                    return SpeechHandler.handleSpeech(firstNumber, secondNumber, oper, result);

                }

                speech.Ssml = "<speak>I did not recognise that intent</speak>";
                finalResponse = ResponseBuilder.Tell(speech);
                return finalResponse;
            }

            return finalResponse;

        }
    }

    static class SpeechHandler
    {
        public static SkillResponse handleSpeech(int firstnumber, int secondnumber, string oper, int result)
        {
            var speech = new SsmlOutputSpeech();
            speech.Ssml = $"<speak>{firstnumber} {oper} {secondnumber} equals {result}</speak>";

            return ResponseBuilder.Tell(speech);
        }
    }

    static class Calculator
    {
        public static int add(int num1, int num2)
        {
            return num1 + num2;
        }

        public static int minus(int num1, int num2)
        {
            return num1 - num2;
        }

        public static int times(int num1, int num2)
        {
            return num2 * num1;
        }

        public static int divide(int num1, int num2)
        {
            return num1 / num2;
        }
    }
}
