#r "Newtonsoft.Json"
#r "Twilio"
#r "Microsoft.Azure.WebJobs.Extensions.Twilio"

using System.Net;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Primitives;
using Newtonsoft.Json;
using Microsoft.Azure.WebJobs.Extensions.Twilio;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

public static async Task<IActionResult> Run(HttpRequest req, IAsyncCollector<CreateMessageOptions> message, ILogger log)
{
    log.LogInformation("C# HTTP trigger function processed a request.");

    string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
    log.LogInformation("body: {requestBody}", requestBody);
    dynamic data = JsonConvert.DeserializeObject(requestBody);
    string gameName = data?.value1 ?? "noGameName";
    string player = data?.value2 ?? "noPlayerName";
    string turnNum =  data?.value3 ?? "noTurnNumber";

    // You must initialize the CreateMessageOptions variable with the "To" phone number.
    CreateMessageOptions smsText = new CreateMessageOptions(new PhoneNumber("+17788407906"));
    // A dynamic message can be set instead of the body in the output binding. In this example, we use
    // the order information to personalize a text message.
    string body = $"{player}'s turn #{turnNum} in {gameName}";
    smsText.Body = body;
    log.LogInformation($"sending sms: {body}");
    await message.AddAsync(smsText);

    string responseMessage = "This HTTP triggered function executed successfully";

    return new OkObjectResult(responseMessage);
}
