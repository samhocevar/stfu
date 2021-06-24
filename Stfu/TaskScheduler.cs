//
//  Stfu — Sam’s Tiny Framework Utilities
//
//  Copyright © 2013—2021 Sam Hocevar <sam@hocevar.net>
//
//  This program is free software. It comes without any warranty, to
//  the extent permitted by applicable law. You can redistribute it
//  and/or modify it under the terms of the Do What the Fuck You Want
//  to Public License, Version 2, as published by the WTFPL Task Force.
//  See http://www.wtfpl.net/ for more details.
//

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text;
using System.Xml;

namespace Stfu
{
    public static class TaskScheduler
    {
        /// <summary>
        /// Return whether a task with this name exists
        /// </summary>
        public static bool HasTask(string task_name)
            => !string.IsNullOrEmpty(RunSchTasks($"/query /tn {task_name} /xml"));

        /// <summary>
        /// Install a task with the given name and command line
        /// </summary>
        public static bool InstallTask(string task_name, string command,
                                       bool elevated = false, string author = null)
        {
            var source = $@"{Environment.SystemDirectory}\Tasks\{task_name}";
            var tmp = Path.GetTempFileName();

            try
            {
                // Create a scheduled task, then edit the resulting XML with some
                // features that the command line tool does not support, and reload
                // the XML file.
                RunSchTasks($"/tn {task_name} /f /create /sc onlogon /tr {command}");

                var doc = new XmlDocument();
                doc.Load(source);

                var fixer = new XmlFixer();
                if (elevated)
                {
                    // Make sure we use a GroupId, not a UserId; we can’t use the SYSTEM
                    // account because it is not allowed to open GUI programs. We use the
                    // built-in BUILTIN\Users group instead.
                    fixer.Renames.Add("UserId", "GroupId");
                    fixer.Replaces.Add("RunLevel", "HighestAvailable"); // run with higest privileges
                    fixer.Replaces.Add("GroupId", GetLocalUserGroupName());
                    fixer.Removes.Add("LogonType"); // This tag is only legal for UserId.
                }

                if (author != null)
                {
                    fixer.Replaces.Add("Author", author);
                }

                fixer.FixElement(doc.DocumentElement);
                doc.Save(tmp);

                RunSchTasks($"/tn {task_name} /f /create /xml \"{tmp}\"");
                File.Delete(tmp);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }

        private class XmlFixer
        {
            public void FixElement(XmlElement node)
            {
                // Rename node if necessary
                if (Renames.TryGetValue(node.Name, out string new_name))
                {
                    var tmp = node.OwnerDocument.CreateElement(new_name, node.NamespaceURI);
                    foreach (XmlNode child in node.ChildNodes)
                        tmp.AppendChild(child.CloneNode(true));
                    node.ParentNode.InsertBefore(tmp, node);
                    node.ParentNode.RemoveChild(node);
                    node = tmp;
                }

                // Replace node content if necessary
                if (Replaces.TryGetValue(node.Name, out string content))
                    node.InnerText = content;

                // Recurse
                if (node.HasChildNodes && node.FirstChild is XmlElement first_child)
                    FixElement(first_child);

                // Process next sibling
                if (node.NextSibling is XmlElement sibling)
                    FixElement(sibling);

                // Remove node if necessary (make sure to do this after sibling)
                if (Removes.Contains(node.Name))
                    node.ParentNode.RemoveChild(node);
            }

            public readonly HashSet<string> Removes = new HashSet<string>();

            public readonly Dictionary<string, string> Renames = new Dictionary<string, string>();

            public readonly Dictionary<string, string> Replaces = new Dictionary<string, string>()
            {
                { "ExecutionTimeLimit", "PT0S" },          // allow to run indefinitely
                { "MultipleInstancesPolicy", "Parallel" }, // allow multiple instances
                { "DisallowStartIfOnBatteries", "false" },
                { "StopIfGoingOnBatteries", "false" },
                { "StopOnIdleEnd", "false" },
                { "StartWhenAvailable", "false" },
                { "RunOnlyIfNetworkAvailable", "false" },
            };
        }

        private static string GetLocalUserGroupName()
        {
            var user_name = new StringBuilder();
            var domain_name = new StringBuilder();
            user_name.EnsureCapacity(128);
            domain_name.EnsureCapacity(128);
            var user_size = (uint)user_name.Capacity;
            var domain_size = (uint)domain_name.Capacity;
            // Build SID S-1-5-32-545 (“Users” group)
            byte[] sid = new byte[]
            {
                1, // Revision
                2, // SubAuthorityCount
                0, 0, 0, 0, 0, 5, // IdentifierAuthority = SECURITY_NT_AUTHORITY (5)
                32, 0, 0, 0, // SECURITY_BUILTIN_DOMAIN_RID (32)
                33, 2, 0, 0, // DOMAIN_ALIAS_RID_USERS (545)
            };

            if (!NativeMethods.LookupAccountSid(null, sid, user_name, ref user_size, domain_name,
                                                ref domain_size, out SID_NAME_USE sid_use))
                return @"BUILTIN\Users";

            return $@"{domain_name}\{user_name}";
        }


        private static string RunSchTasks(string args)
        {
            var pi = new ProcessStartInfo()
            {
                FileName = "schtasks.exe",
                Arguments = args,
                UseShellExecute = false,
                RedirectStandardOutput = true,
                WindowStyle = ProcessWindowStyle.Hidden,
            };
            var p = Process.Start(pi);
            p.WaitForExit();
            var exit_code = p.ExitCode;
            var stdout = p.StandardOutput.ReadToEnd();
            return exit_code == 0 ? stdout : null;
        }
    }
}
