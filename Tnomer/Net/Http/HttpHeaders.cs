using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Tnomer.Net.Http
{
    public static class HttpHeaders
    {
        public const String Accept  ="Accept"; //	Список допустимых форматов ресурса.	Accept: text/plain
        public const String AcceptCharset	="Accept-Charset"; //	Перечень поддерживаемых кодировок для предоставления пользователю.	Accept-Charset: utf-8
        public const String AcceptEncoding	="Accept-Encoding"; //	Перечень поддерживаемых способов кодирования содержимого сущности при передаче.	Accept-Encoding: <compress | gzip | deflate | sdch| identity>
        public const String AcceptLanguage	="Accept-Language"; //	Список поддерживаемых естественных языков.	Accept-Language: ru
        public const String AcceptRanges	="Accept-Ranges"; //	Перечень единиц измерения диапазонов.	Accept-Ranges: bytes
        public const String Age ="Age"; //	Количество секунд с момента модификации ресурса.	
        public const String Allow   ="Allow"; //	Список поддерживаемых методов.	Allow: OPTIONS, GET, HEAD
        public const String Alternates  ="Alternates"; //	Указание на альтернативные способы представления ресурса.	
        public const String Authorization   ="Authorization"; //	Данные для авторизации.	Authorization: Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==
        public const String CacheControl	="Cache-Control"; /*	Основные директивы для управления кэшированием."Cache-Control: no-cache
                                                                Cache-Control: no-store
                                                                Cache-Control: max-age=3600
                                                                Cache-Control: max-stale=0
                                                                Cache-Control: min-fresh=0
                                                                Cache-Control: no-transform
                                                                Cache-Control: only-if-cached
                                                                Cache-Control: cache-extension"
                                                                */
        public const String Connection  ="Connection"; //	Сведения о проведении соединения.	Connection: close
        public const String ContentBase	="Content-Base"; //	Сведения о постоянном местонахождении ресурса. Убрано в HTTP/1.1v2.	
        public const String ContentDisposition	="Content-Disposition"; /*	Способ распределения сущностей в сообщении при передаче нескольких фрагментов."Content-Disposition: form-data; name=""MessageTitle""
                                                                        Content-Disposition: form-data; name=""AttachedFile1""; filename=""photo-1.jpg"""
                                                                        */
        public const String ContentEncoding	="Content-Encoding"; //	Способ кодирования содержимого сущности при передаче.	
        public const String ContentLanguage	="Content-Language"; //	Один или несколько естественных языков содержимого сущности.	Content-Language: en, ase, ru
        public const String ContentLength	="Content-Length"; //	Размер содержимого сущности в октетах (которые в русском языке обычно называют байтами).	Content-Length: 1348
        public const String ContentLocation	="Content-Location"; //	Альтернативное расположение содержимого сущности.	
        public const String ContentMD5	="Content-MD5"; //	Base64 MD5-хэша сущности для проверки целостности.	Content-MD5: Q2hlY2sgSW50ZWdyaXR5IQ==
        public const String ContentRange	="Content-Range"; //	Байтовые диапазоны передаваемой сущности если возвращается фрагмент. Подробности: Частичные GET.	Content-Range: bytes 88080384-160993791/160993792
        public const String ContentType	="Content-Type"; //	Формат и способ представления сущности.	Content-Type: text/html;charset=utf-8
        public const String ContentVersion	="Content-Version"; //	Информация о текущей версии сущности.	
        public const String Date    ="Date"; //	Дата генерации отклика.	Date: Tue, 15 Nov 1994 08:12:31 GMT
        public const String DerivedFrom	="Derived-From"; //	Информация о текущей версии сущности. [?]	
        public const String ETag    ="ETag"; //	Тег (уникальный идентификатор) версии сущности, используемый при кэшировании.	ETag: "56d-9989200-1132c580"
        public const String Expect  ="Expect"; //	Указывает серверу что клиент ожидает от него дополнительного действия.	Expect: 100-continue
        public const String Expires ="Expires"; //	Дата предполагаемого истечения срока актуальности сущности.	Expires: Tue, 31 Jan 2012 15:02:53 GMT
        public const String From    ="From"; //	Адрес электронной почты ответственного лица со стороны клиента.	From: user@example.com
        public const String Host    ="Host"; //	Доменное имя и порт хоста запрашиваемого ресурса. Необходимо для поддержки виртуального хостинга на серверах.	Host: ru.wikipedia.org
        public const String IfMatch	="If-Match"; //	Список тегов версий сущности. Выполнять метод, если они существуют.	If-Match: "737060cd8c284d8af7ad3082f209582d"
        public const String IfModifiedSince	="If-Modified-Since"; //	Дата. Выполнять метод если сущность изменилась с указанного момента.	If-Modified-Since: Sat, 29 Oct 1994 19:43:31 GMT
        public const String IfNoneMatch	="If-None-Match"; //	Список тегов версий сущности. Выполнять метод если ни одного из них не существует.	If-None-Match: "737060cd8c284d8af7ad3082f209582d"
        public const String IfRange	="If-Range"; //	Список тегов версий сущности или дата для определённого фрагмента сущности.	If-Range: "737060cd8c284d8af7ad3082f209582d"
        public const String IfUnmodifiedSince	="If-Unmodified-Since"; //	Дата. Выполнять метод если сущность не изменилась с указанной даты.	If-Unmodified-Since: Sat, 29 Oct 1994 19:43:31 GMT
        public const String LastModified	="Last-Modified"; //	Дата последней модификации сущности.	
        public const String Link    ="Link"; //	Указывает на логически связанный с сущностью ресурс аналогично тегу <LINK> в HTML.	
        public const String Location    ="Location"; //	URI по которому клиенту следует перейти или URI созданного ресурса.	Location: http:; //example.com/about.html#contacts
        public const String MaxForwards	="Max-Forwards"; //	Максимально допустимое количество переходов через прокси.	Max-Forwards: 10
        public const String MIMEVersion	="MIME-Version"; //	Версия протокола MIME, по которому было сформировано сообщение.	
        public const String Pragma  ="Pragma"; //	Особенные опции выполнения операции.	Pragma: no-cache
        public const String ProxyAuthenticate	="Proxy-Authenticate"; //	Параметры аутентификации на прокси-сервере.	
        public const String ProxyAuthorization	="Proxy-Authorization"; //	Информация для авторизации на прокси-сервере.	Proxy-Authorization: Basic QWxhZGRpbjpvcGVuIHNlc2FtZQ==
        public const String Public  ="Public"; //	Список доступных методов аналогично Allow, но для всего сервера.	
        public const String Range   ="Range"; //	Байтовые диапазоны для запроса фрагментов ресурса. Подробности: Частичные GET.	Range: bytes=50000-99999,250000-399999,500000-
        public const String Referer ="Referer"; //	URI ресурса, после которого клиент сделал текущий запрос.	Referer: http:; //en.wikipedia.org/wiki/Main_Page
        public const String RetryAfter	="Retry-After"; //	Дата или время в секундах после которого можно повторить запрос.	
        public const String Server  ="Server"; //	Список названий и версий веб-сервера и его компонентов с комментариями. Для прокси-серверов поле Via.	Server: Apache/2.2.17 (Win32) PHP/5.3.5
        public const String Title   ="Title"; //	Заголовок сущности.	
        public const String TE  ="TE"; //	Список расширенных способов кодирования при передаче.	TE: trailers, deflate
        public const String Trailer ="Trailer"; //	Список полей, имеющих отношение к кодированию сообщения при передаче.	
        public const String TransferEncoding	="Transfer-Encoding"; //	Список способов кодирования, которые были применены к сообщению для передачи.	Transfer-Encoding: chunked
        public const String Upgrade ="Upgrade"; //	Список предлагаемых клиентом протоколов. Сервер указывает один протокол.	Upgrade: HTTP/2.0, SHTTP/1.3, IRC/6.9, RTA/x11
        public const String URI ="URI"; //	Список URI. В HTTP/1.1 заменено на Location, Content-Location, Varyи Link.	
        public const String UserAgent	="User-Agent"; //	Список названий и версий клиента и его компонентов с комментариями.	User-Agent: Mozilla/5.0 (X11; Linux i686; rv:2.0.1) Gecko/20100101 Firefox/4.0.1
        public const String Vary    ="Vary"; //	Список описывающих ресурс полей из запроса, которые были приняты во внимание.	Vary: Accept-Encoding
        public const String Via ="Via"; //	Список версий протокола, названий и версий прокси-серверов, через которых прошло сообщение.	Via: 1.0 fred, 1.1 nowhere.com (Apache/1.1)
        public const String Warning ="Warning"; //	Код, агент, сообщение и дата, если возникла критическая ситуация.	Warning: 199 Miscellaneous warning
        public const String WWWAuthenticate	="WWW-Authenticate"; //	Параметры аутентификации для выполнения метода к указанному ресурсу.	
    }
}
