using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace appmvcnet.Extensions
{
    public static class AppExtends
    {
        public static void AddStatusCodePage(this IApplicationBuilder app) 
        {
            app.UseStatusCodePages(appError => {
                appError.Run(async context => {
                    var response = context.Response;
                    var code = response.StatusCode;

                    var content = @$"<html>
                        <head>
                            <meta charset='UTF-8' />
                            <title>{code}</title>
                        </head>
                        <body>
                            <p>
                                {code} - {(HttpStatusCode)code}
                            </p>
                        </body>
                    </html>";
                    await response.WriteAsync(content);
                });
            });
        }
    }
}