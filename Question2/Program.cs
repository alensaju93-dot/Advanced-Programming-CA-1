using System;
using System.Collections.Generic;
using System.Linq;

namespace ExtensionExplorer
{
    class FileType
    {
        public string Extension { get; set; }
        public string Description { get; set; }
    }

    class Program
    {
        static void Main()
        {
            List<FileType> types = LoadFileTypes();

            Console.WriteLine("=== File Extension Lookup Tool ===");

            bool active = true;
            while (active)
            {
                Console.Write("\nEnter extension (or type QUIT): ");
                string input = Console.ReadLine();

                if (string.IsNullOrWhiteSpace(input))
                {
                    Console.WriteLine("Input required.");
                    continue;
                }

                input = input.Trim();

                if (input.Equals("quit", StringComparison.OrdinalIgnoreCase))
                {
                    Console.WriteLine("Program terminated.");
                    active = false;
                    continue;
                }

                if (!input.StartsWith("."))
                    input = "." + input;

                var result = types.FirstOrDefault(t =>
                    t.Extension.Equals(input, StringComparison.OrdinalIgnoreCase));

                if (result != null)
                {
                    Console.WriteLine($"Extension : {result.Extension}");
                    Console.WriteLine($"Type      : {result.Description}");
                }
                else
                {
                    Console.WriteLine("Extension not found in the database.");
                }
            }
        }

        static List<FileType> LoadFileTypes()
        {
            return new List<FileType>
            {
                new FileType { Extension = ".mp4",  Description = "MPEG-4 video file" },
                new FileType { Extension = ".mp3",  Description = "Compressed audio file" },
                new FileType { Extension = ".wav",  Description = "Wave audio format" },
                new FileType { Extension = ".avi",  Description = "AVI video container" },
                new FileType { Extension = ".mkv",  Description = "Matroska multimedia file" },
                new FileType { Extension = ".mov",  Description = "QuickTime movie" },
                new FileType { Extension = ".jpg",  Description = "JPEG image" },
                new FileType { Extension = ".png",  Description = "PNG image file" },
                new FileType { Extension = ".gif",  Description = "GIF image format" },
                new FileType { Extension = ".pdf",  Description = "Portable Document Format" },
                new FileType { Extension = ".docx", Description = "Microsoft Word document" },
                new FileType { Extension = ".xlsx", Description = "Microsoft Excel spreadsheet" },
                new FileType { Extension = ".pptx", Description = "PowerPoint presentation" },
                new FileType { Extension = ".txt",  Description = "Text document" },
                new FileType { Extension = ".csv",  Description = "Comma-separated values file" },
                new FileType { Extension = ".html", Description = "HTML web page" },
                new FileType { Extension = ".css",  Description = "Style sheet file" },
                new FileType { Extension = ".py",   Description = "Python source code" },
                new FileType { Extension = ".cs",   Description = "C# source file" },
                new FileType { Extension = ".json", Description = "JSON structured data" }
            };
        }
    }
}