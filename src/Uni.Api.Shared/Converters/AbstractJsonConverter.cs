using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace Uni.Api.Shared.Converters
{
    public abstract class AbstractJsonConverter<T> : JsonConverter<T>
    {
        public override bool CanWrite => false;

        protected abstract T Create(Type objectType, JObject jObject);

        protected static bool FieldExists(
            JObject jObject,
            string name,
            JTokenType type
            )
        {
            if (jObject.TryGetValue(name, StringComparison.OrdinalIgnoreCase, out var token))
            {
                return token.Type == type;
            }

            return false;
        }

        public override void WriteJson(
            JsonWriter writer,
            T value,
            JsonSerializer serializer
            )
        {
            throw new NotImplementedException();
        }

        public override T ReadJson(
            JsonReader reader,
            Type objectType,
            T existingValue,
            bool hasExistingValue,
            JsonSerializer serializer
            )
        {
            var jObject = JObject.Load(reader);

            var target = Create(objectType, jObject);

            serializer.Populate(jObject.CreateReader(), target);

            return target;
        }
    }
}
