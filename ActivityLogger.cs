using System;
using System.Collections.Generic;
using System.Linq;

namespace CyberSecurityChatbot_PART_2
{
    public static class ActivityLogger
    {
        private static List<string> _log = new List<string>();

        public static void Log(string action)
        {
            string entry =
                DateTime.Now.ToString("[HH:mm] ") + action;

            _log.Add(entry);
        }

        public static string GetRecentLog(int count = 10)
        {
            if (_log.Count == 0)
            {
                return "No activity has been recorded yet.";
            }

            var recentEntries =
                _log.Skip(Math.Max(0, _log.Count - count)).ToList();

            string result =
                "Here's a summary of recent actions:\n\n";

            for (int i = 0; i < recentEntries.Count; i++)
            {
                result += $"{i + 1}. {recentEntries[i]}\n";
            }

            return result;
        }

        public static string GetFullLog()
        {
            if (_log.Count == 0)
            {
                return "No activity has been recorded yet.";
            }

            string result = "Full Activity Log:\n\n";

            for (int i = 0; i < _log.Count; i++)
            {
                result += $"{i + 1}. {_log[i]}\n";
            }

            return result;
        }

        public static int GetCount()
        {
            return _log.Count;
        }
    }
}