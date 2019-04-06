using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Optimization;

namespace Web.Transformers
{
    public class CssRewriteUrlTransformFixed : IItemTransform
    {
        private static string RebaseUrlToAbsolute(string baseUrl, string url, string prefix, string suffix)
        {
            if (string.IsNullOrWhiteSpace(url)
                || string.IsNullOrWhiteSpace(baseUrl)
                || url.StartsWith("/", StringComparison.OrdinalIgnoreCase)
                || url.StartsWith("http://") || url.StartsWith("https://"))
            {
                return url;
            }

            if (url.StartsWith("data:"))
            {
                // Keep the prefix and suffix quotation chars as is in case they are needed (e.g. non-base64 encoded svg)
                return prefix + url + suffix;
            }

            if (!baseUrl.EndsWith("/", StringComparison.OrdinalIgnoreCase))
            {
                baseUrl += "/";
            }

            return $"'{VirtualPathUtility.ToAbsolute(baseUrl + url)}'";
        }

        private static string ConvertUrlsToAbsolute(string baseUrl, string content)
        {
            if (string.IsNullOrWhiteSpace(content))
            {
                return content;
            }

            var regex = new Regex("url\\((?<prefix>['\"]?)(?<url>[^)]+?)(?<suffix>['\"]?)\\)");

            return regex.Replace(content, match => "url(" + RebaseUrlToAbsolute(baseUrl, match.Groups["url"].Value, match.Groups["prefix"].Value, match.Groups["suffix"].Value) + ")");
        }

        public string Process(string includedVirtualPath, string input)
        {
            if (includedVirtualPath == null)
            {
                throw new ArgumentNullException(nameof(includedVirtualPath));
            }
            if (includedVirtualPath.Length < 1 || includedVirtualPath[0] != '~')
            {
                throw new ArgumentException("includedVirtualPath must be valid ( i.e. have a length and start with ~ )");
            }

            var directory = VirtualPathUtility.GetDirectory(includedVirtualPath);

            return ConvertUrlsToAbsolute(directory, input);
        }
    }
}