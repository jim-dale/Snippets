using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using Microsoft.Build.Framework;
using Microsoft.Build.Utilities;

namespace GetSnippetTask
{
    public sealed class GetSnippet : Task, IDisposable
    {
        [Required]
        [SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "Required for MSBuild tasks")]
        public ITaskItem[] Inputs { get; set; }

        [Required]
        public string SnippetNamespace { get; set; }

        [Required]
        public string BaseUrl { get; set; }

        [Output]
        [SuppressMessage("Performance", "CA1819:Properties should not return arrays", Justification = "Required for MSBuild tasks")]
        public ITaskItem[] Outputs { get; private set; }

        public override bool Execute()
        {
            var outputs = new List<ITaskItem>();

            Log.LogMessage(MessageImportance.High, $"SnippetNamespace: \"{SnippetNamespace}\"");
            Log.LogMessage(MessageImportance.High, $"BaseUrl: \"{BaseUrl}\"");

            foreach (var item in Inputs)
            {
                Log.LogMessage(MessageImportance.Low, $"ItemSpec: \"{item.ItemSpec}\"");
                foreach (var name in item.MetadataNames)
                {
                    var value = item.GetMetadata(name as string);

                    Log.LogMessage(MessageImportance.High, $"\t{name}={value}");
                }

                var outputFile = item.ItemSpec;
                var remotePath = item.GetMetadata("RemotePath");

                if (string.IsNullOrWhiteSpace(outputFile) == false
                    && string.IsNullOrWhiteSpace(remotePath) == false)
                {
                    var relativeDir = item.GetMetadata("RelativeDir");
                    var version = item.GetMetadata("Version");
                    var baseUrl = GetString(item.GetMetadata("BaseUrl"), BaseUrl);
                    var ns = GetString(item.GetMetadata("SnippetNamespace"), SnippetNamespace);

                    if (string.IsNullOrWhiteSpace(version)
                        || string.IsNullOrWhiteSpace(baseUrl)
                        || string.IsNullOrWhiteSpace(ns))
                    {
                        //Log error
                        continue;
                    }

                    if (TryCreateOutputDirectory(relativeDir) == false)
                    {
                        continue;
                    }

                    var currentVersion = GetVersionFromSourceFile(outputFile);

                    Log.LogMessage(MessageImportance.Low, $"Comparing versions \"{version}\" (requested) to \"{currentVersion}\" (current)");

                    // Versions don't match so download the specified version of the snippet
                    if (string.Equals(currentVersion, version, StringComparison.OrdinalIgnoreCase) == false)
                    {
                        Uri url = new Uri($"{baseUrl}/{version}/{remotePath}");
                        Log.LogMessage(MessageImportance.Normal, $"Downloading file \"{url}\" to \"{outputFile}\"");

                        if (TryProcessSnippetRequest(url, outputFile, version, ns) == false)
                        {
                            continue;
                        }
                    }

                    outputs.Add(new TaskItem(outputFile));
                }
            }
            if (outputs.Count > 0)
            {
                Outputs = outputs.ToArray();
            }

            return (Log.HasLoggedErrors == false);
        }

        private bool TryCreateOutputDirectory(string path)
        {
            bool result = true;

            try
            {
                if (string.IsNullOrWhiteSpace(path) == false)
                {
                    Directory.CreateDirectory(path);
                }
            }
            catch (Exception ex)
            when (ex is IOException
                || ex is UnauthorizedAccessException
                || ex is PathTooLongException
                || ex is DirectoryNotFoundException
                || ex is NotSupportedException)
            {
                Log.LogErrorFromException(ex);
                result = false;
            }

            return result;
        }

        private bool TryProcessSnippetRequest(Uri url, string path, string version, string ns)
        {
            bool result = true;

            try
            {
                var client = GetHttpClient();

                var contents = client.GetStringAsync(url).GetAwaiter().GetResult();

                if (string.IsNullOrWhiteSpace(contents) == false)
                {
                    contents = AddSnippetHeader(contents, version);

                    contents = contents.Replace("__Snippets__", ns);

                    File.WriteAllText(path, contents);
                }
            }
            catch (Exception ex)
            when (ex is InvalidOperationException
                || ex is HttpRequestException
                || ex is System.Threading.Tasks.TaskCanceledException)
            {
                Log.LogErrorFromException(ex);
                result = false;
            }

            return result;
        }

        private static string GetString(string first, string second)
        {
            return (string.IsNullOrWhiteSpace(first)) ? second : first;
        }

        private static string GetVersionFromSourceFile(string path)
        {
            string result = default;

            if (File.Exists(path))
            {
                var contents = File.ReadAllText(path);

                var match = Regex.Match(contents, @"^// Version: (.+)$", RegexOptions.Multiline);
                if (match.Success && match.Groups.Count > 1)
                {
                    result = match.Groups[1].Value.Trim();
                }
            }

            return result;
        }

        private static string AddSnippetHeader(string value, string version)
        {
            var result = new StringBuilder(value.Length + 100);

            result.Append("// Version: ");
            result.AppendLine(version);
            result.AppendLine("// Auto-generated file. DO NOT MODIFY.");
            result.AppendLine();
            result.Append(value);

            return result.ToString();
        }

        private HttpClient _client;
        private HttpClient GetHttpClient()
        {
            if (_client == null)
            {
                _client = new HttpClient();
            }

            return _client;
        }

        [SuppressMessage("Design", "CA1063:Implement IDisposable Correctly", Justification = "No native resource")]
        public void Dispose()
        {
            _client?.Dispose();
        }
    }
}
