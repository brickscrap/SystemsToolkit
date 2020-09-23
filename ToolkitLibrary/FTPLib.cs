using FluentFTP;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ToolkitLibrary
{
    public class FTPLib
    {
        // Test sys: "SUPERVISOR", "7428F3DBB"
        // "FTP_E01", "F5D1F22E3"
        public static void GetListing()
        {
            using (var conn = new FtpClient("10.33.27.131", "SUPERVISOR", "7428F3DBB"))
            {
                foreach (var item in conn.GetListing("/", FtpListOption.Auto))
                {
                    switch (item.Type)
                    {

                        case FtpFileSystemObjectType.Directory:

                            Console.WriteLine("Directory!  " + item.FullName);
                            // Console.WriteLine("Modified date:  " + conn.GetModifiedTime(item.FullName));

                            break;

                        case FtpFileSystemObjectType.File:

                            Console.WriteLine("File!  " + item.FullName);
                            //Console.WriteLine("File size:  " + conn.GetFileSize(item.FullName));
                            //Console.WriteLine("Modified date:  " + conn.GetModifiedTime(item.FullName));
                            //Console.WriteLine("Chmod:  " + conn.GetChmod(item.FullName));

                            break;

                        case FtpFileSystemObjectType.Link:
                            break;
                    }
                }
            }
        }

        public static async Task GetListingAsync()
        {
            var token = new CancellationToken();
            using (var conn = new FtpClient("10.33.27.131", "SUPERVISOR", "7428F3DBB"))
            {
                await conn.ConnectAsync(token);

                // get a recursive listing of the files & folders in a specific folder
                foreach (var item in await conn.GetListingAsync("/htdocs", FtpListOption.Recursive, token))
                {
                    switch (item.Type)
                    {

                        case FtpFileSystemObjectType.Directory:

                            Console.WriteLine("Directory!  " + item.FullName);
                            Console.WriteLine("Modified date:  " + await conn.GetModifiedTimeAsync(item.FullName, FtpDate.Original, token));

                            break;

                        case FtpFileSystemObjectType.File:

                            Console.WriteLine("File!  " + item.FullName);
                            Console.WriteLine("File size:  " + await conn.GetFileSizeAsync(item.FullName, token));
                            Console.WriteLine("Modified date:  " + await conn.GetModifiedTimeAsync(item.FullName, FtpDate.Original, token));
                            Console.WriteLine("Chmod:  " + await conn.GetChmodAsync(item.FullName, token));

                            break;

                        case FtpFileSystemObjectType.Link:
                            break;
                    }
                }

            }
        }
    }
}
