﻿using System.Collections.Generic;

namespace AventStack.ExtentReports.Reporter.Filter
{
    public class ContextFilter
    {
        private static readonly ContextFilter Filter = new ContextFilter();

        public HashSet<Status> Status { get; set; }
        public HashSet<string> Author { get; set; }
        public HashSet<string> Category { get; set; }
        public HashSet<string> Device { get; set; }

        public static ContextFilterBuilder Builder => new ContextFilterBuilder(Filter);

        public class ContextFilterBuilder
        {
            private readonly ContextFilter _filter;

            public ContextFilterBuilder(ContextFilter filter)
            {
                _filter = filter;
            }

            public ContextFilterBuilder Status(Status[] status)
            {
                foreach (Status s in status)
                {
                    _filter.Status.Add(s);
                }

                return this;
            }

            public ContextFilterBuilder Author(string[] author)
            {
                foreach (string s in author)
                {
                    _filter.Author.Add(s);
                }

                return this;
            }

            public ContextFilterBuilder Category(string[] category)
            {
                foreach (string s in category)
                {
                    _filter.Category.Add(s);
                }

                return this;
            }

            public ContextFilterBuilder Device(string[] device)
            {
                foreach (string s in device)
                {
                    _filter.Device.Add(s);
                }

                return this;
            }

            public ContextFilter Build()
            {
                return _filter;
            }
        }
    }
}
