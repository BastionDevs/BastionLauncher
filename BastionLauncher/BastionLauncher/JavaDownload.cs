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
        public static void DownloadJava(string javaversion)
        {
            if (OSCompat(javaversion))
            {
                string json = Util.JREMasterList["windows-"+OSArch()][javaversion][0]["manifest"]["url"].ToString();
                new WebClient().DownloadString(json);
                MessageBox.Show(json);
            } else
            {
                MessageBox.Show("Required Java version is not compatible with your device!", "Bastion Launcher", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        static bool OSCompat(string version)
        {
            switch (System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE"))
            {
                case "AMD64":
                    return true;

                case "ARM64":
                    if (version == "java-runtime-delta" || version == "java-runtime-gamma" || version == "java-runtime-gamma-snapshot") { 
                    return true; } else { return false; }

                case "x86":
                    if (version != "java-runtime-delta")
                    {
                        return true;
                    }
                    else { return false; }

                default:
                    return false;
            }
        }

        static string OSArch()
        {
            switch (System.Environment.GetEnvironmentVariable("PROCESSOR_ARCHITECTURE"))
            {
                case "AMD64":
                    return "x64";

                case "ARM64":
                    return "arm64";

                case "x86":
                    return "x86";

                default:
                    return "x86";
            }
        }

        public static void DownloadFiles(string json, string basePath)
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

                    progressForm.Text = $"Downloading Java Runtime... [{processedCount}/{files.Count} Files]";

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
