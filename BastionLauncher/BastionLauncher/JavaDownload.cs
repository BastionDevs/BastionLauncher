using Newtonsoft.Json.Linq;
using System;
using System.IO;
using System.Net;
using System.Security.Cryptography;
using System.Windows.Forms;

namespace BastionLauncher
{
    internal class JavaDownload
    {
        public void Download(string javaversion)
        {
            switch (System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE"))
            {
                case "AMD64":
                    break;

                case "ARM64":
                    break;

                case "x86":
                    break;
            }
        }

        public static void Download(string json, string basePath)
        {
            if (json == null)
            {
                throw new ArgumentNullException(nameof(json), "JSON string cannot be null.");
            }

            if (string.IsNullOrEmpty(basePath))
            {
                throw new ArgumentNullException(nameof(basePath), "Base path cannot be null or empty.");
            }

            // Parse the JSON
            var jsonObject = JObject.Parse(json);
            var files = jsonObject["files"] as JObject;

            if (files == null)
            {
                throw new Exception("The 'files' key is missing or is not an object in the JSON.");
            }

            // Create the ProgressForm and show it
            DlProgress progressForm = new DlProgress();
            progressForm.progressBar.Maximum = files.Count; // Set max to the number of files
            progressForm.Show();

            // Initialize WebClient
            using (var client = new WebClient())
            {
                int processedCount = 0;

                foreach (var file in files)
                {
                    var fileDetails = file.Value;
                    string fullPath = Path.Combine(basePath, file.Key);

                    // Handle directories
                    if (fileDetails["type"]?.ToString() == "directory")
                    {
                        if (!Directory.Exists(fullPath))
                        {
                            Directory.CreateDirectory(fullPath);
                            Console.WriteLine($"Directory created: {fullPath}");
                        }
                    }
                    else if (fileDetails["type"]?.ToString() == "file")
                    {
                        var downloads = fileDetails["downloads"];
                        if (downloads != null)
                        {
                            var rawDownload = downloads["raw"];
                            if (rawDownload != null)
                            {
                                var url = rawDownload["url"]?.ToString();
                                var expectedSha1 = rawDownload["sha1"]?.ToString();
                                if (!string.IsNullOrEmpty(url))
                                {
                                    bool needsDownload = true;

                                    // Check if the file exists and the hash matches
                                    if (File.Exists(fullPath))
                                    {
                                        string fileSha1 = CalculateSHA1(fullPath);
                                        if (fileSha1.Equals(expectedSha1, StringComparison.OrdinalIgnoreCase))
                                        {
                                            Console.WriteLine($"{fullPath} is up to date.");
                                            needsDownload = false;
                                        }
                                    }

                                    // Download the file if needed
                                    if (needsDownload)
                                    {
                                        Console.WriteLine($"Downloading {url}");
                                        client.DownloadFile(url, fullPath);
                                        Console.WriteLine($"{fullPath} downloaded.");

                                        // Verify the hash again
                                        string newFileSha1 = CalculateSHA1(fullPath);
                                        if (!newFileSha1.Equals(expectedSha1, StringComparison.OrdinalIgnoreCase))
                                        {
                                            Console.WriteLine($"Hash mismatch for {fullPath} after download.");
                                        }
                                        else
                                        {
                                            Console.WriteLine($"{fullPath} downloaded and verified.");
                                        }
                                    }
                                }
                                else
                                {
                                    Console.WriteLine($"URL is missing for {fullPath}");
                                }
                            }
                            else
                            {
                                Console.WriteLine($"Raw download data is missing for {fullPath}");
                            }
                        }
                        else
                        {
                            Console.WriteLine($"Downloads section is missing for {fullPath}");
                        }
                    }

                    // Update the progress bar after each file/directory is processed
                    processedCount++;
                    progressForm.progressBar.Value = processedCount;

                    progressForm.Text = $"Downloading Java... [{processedCount}/{files.Count}]";

                    // Allow UI to refresh
                    Application.DoEvents();
                }
            }

            // Close the ProgressForm when done
            progressForm.Close();
        }

        // Method to calculate SHA-1 hash
        private static string CalculateSHA1(string filePath)
        {
            using (FileStream fileStream = File.OpenRead(filePath))
            using (SHA1CryptoServiceProvider sha1 = new SHA1CryptoServiceProvider())
            {
                byte[] hashBytes = sha1.ComputeHash(fileStream);
                return BitConverter.ToString(hashBytes).Replace("-", "").ToLower();
            }
        }
        }
    }
