using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace API.COMMON.Library
{
   public class StringLibrary
    {
        public static string PasswordHash(string value)
        {
            MD5 md5 = new MD5CryptoServiceProvider();

            //compute hash from the bytes of text  
            md5.ComputeHash(ASCIIEncoding.ASCII.GetBytes(value));

            //get hash result after compute it  
            byte[] result = md5.Hash;

            StringBuilder strBuilder = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                //change it into 2 hexadecimal digits  
                //for each byte  
                strBuilder.Append(result[i].ToString("x2"));
            }

            return strBuilder.ToString();
        }
        public static string RandCode(int quantity = 4)
        {
            var chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            var stringChars = new char[quantity];
            var random = new Random();
            for (int i = 0; i < stringChars.Length; i++)
            {
                stringChars[i] = chars[random.Next(chars.Length)];
            }
            return new String(stringChars);
        }
        public static string HtmlBodyMail(string username,string code)
        {
            #region body html
            string body = @"<!DOCTYPE html>
                    <html lang='en'>

                    <head>
                        <meta charset='UTF-8'>
                        <meta http-equiv='X-UA-Compatible' content='IE=edge'>
                        <meta name='viewport' content='width=device-width, initial-scale=1.0'>
                        <title>Document</title>
                        <style>                           
                            body {
                                padding: 16px;
                            }
                            .body {
                                border: 1px solid #ccc;
                                width: 100%;
                                margin: auto;
                            }
                            .code {
                                text-align: center;
                                margin: 0 auto;
                                background: rgb(35, 189, 137);
                                border-radius: 50px;
                                font-size: 1.5rem;
                                line-height: 2rem;
                                color: white;
                                min-width: 150px;
                                max-width: 200px;
                            }
                            .code_text {
                                margin: auto;
                                min-width: 150px;
                                max-width: 200px;
                                text-align: center;
                                font-size: 1.5rem;
                                line-height: 2rem;
                            }
                            .header {
                                text-align: center;
                                text-transform: uppercase;
                                background: #00BBA6;
                                color: white;
                                padding: 4px;
                            }
                            .header h1 {
                                line-height: 2.5rem;
                            }
                            .header h4 {
                                line-height: 2rem;
                            }
                            footer {
                                margin-top: 12px;
                                text-align: center;
                                text-transform: uppercase;
                                background: #00BBA6;
                                color: white;
                                padding: 4px;
                            }
                            p {
                                line-height: 2rem;
                                padding: 0 8px;
                            }
                        </style>
                    </head>
                    <body>
                        <div class='body'>
                            <div class='header'>
                                <h1>Công ty nón bảo hiểm việt tin</h1>
                                <h4>Mã xác nhận lấy lại mật khẩu tài khoản" + username + @"</h4>
                            </div>
                            <p>Chào bạn " + username + @",</p>
                            <p>Đây là mã xác nhận kích hoạt tài khoản của bạn. Vui lòng không cung cấp cho người khác.</p>
                            <h4 class='code_text'>Mã xác nhận</h4>
                            <div class='code'>
                                <span>" + code + @"</span>
                            </div>

                            <p>Nếu không phải bạn? Vui lòng bỏ qua email này.</p>
                            <p>Đã có tài khoản?<a href='https://trainwordbook.web.app/auth'>Đăng nhập tại đây</a></p>
                            <p>Xin cảm ơn,</p>
                            <p>learnwordbook@gmail.com</p>
                            <footer class='footer'>
                                <p>Cảm ơn bạn đã đăng kí tài khoản trên <a href='https://trainwordbook.web.app'>Train wordbook</a></p>
                            </footer>
                        </div>
                    </body>

                    </html>";
            #endregion
            return body;
        }
    }
}
