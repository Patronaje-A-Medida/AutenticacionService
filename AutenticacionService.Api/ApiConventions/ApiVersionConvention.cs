using Microsoft.AspNetCore.Mvc.ApplicationModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AutenticacionService.Api.ApiConventions
{
    public class ApiVersionConvention : IControllerModelConvention
    {
        public void Apply(ControllerModel controller)
        {
            var controllerNameSpace = controller.ControllerType.Namespace;
            var apiVersion = controllerNameSpace.Split(".").Last().ToLower();
            controller.ApiExplorer.GroupName = apiVersion;
        }
    }
}
