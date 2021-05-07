using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Skillbox.App.Model;
using System;

namespace Skillbox.App.Tools
{
    public class PaymentTypeConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return objectType == typeof(IPaymentData);
        }

        public override object ReadJson(JsonReader reader, Type objectType, object existingValue, JsonSerializer serializer)
        {
            JObject jObject = JObject.Load(reader);
            switch (jObject["Type"].ToObject<PaymentType>())
            {
                case PaymentType.Contract:
                    return serializer.Deserialize<InternPayment>(reader);
                case PaymentType.Percentage:
                    return serializer.Deserialize<ManagerPayment>(reader);
                case PaymentType.PerHour:
                    return serializer.Deserialize<WorkerPayment>(reader);
                default:
                    throw new ArgumentOutOfRangeException("Invalid PaymentType value");
            }
        }

        public override void WriteJson(JsonWriter writer, object value, JsonSerializer serializer)
        {
            serializer.Serialize(writer, value);
        }
    }
}