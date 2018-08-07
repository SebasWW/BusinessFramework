using System;
//using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;
using System.Linq;
using System.Net;

namespace Tnomer.Net.Http.Model
{


    public class ResponseModel<TModel>
    {
        public MetaModel Metadata { get; set; } = new MetaModel();
        public TModel Data { get; set; }

        public ResponseModel() { }

        public ResponseModel(TModel model)
        {
            Data = model;
        }
    }
}

//200: статус Ok.Указывает на удачное выполнение запроса

//201: статус Created. Указывает на успешное создание объекта, как правило, используется в запросах POST

//204: статус NoContent - запрос прошел успешно, например, после удаления

//400: статус BadRequest - ошибка при выполнении запроса

//401: статус Unathorized - пользователь не авторизован

//403: статус Forbidden - доступ запрещен

//404: статус NotFound - ресурс не найден