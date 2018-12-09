using System;
using Newtonsoft.Json.Linq;
using Uni.Api.Shared.Responses;

namespace Uni.Api.Shared.Converters
{
    public class PersonConverter : AbstractJsonConverter<PersonResponseModel>
    {
        protected override PersonResponseModel Create(Type objectType, JObject jObject)
        {
            if (FieldExists(jObject, nameof(StudentResponseModel.GroupId), JTokenType.Integer))
                return new StudentResponseModel();
 
            if (FieldExists(jObject, nameof(TeacherResponseModel.FacultyId), JTokenType.Integer))
                return new TeacherResponseModel();
 
            throw new InvalidOperationException();
        }
    }
}
