﻿using System;
using System.Collections.Generic;
using Baseline;
using Microsoft.AspNetCore.Http;

namespace Jasper.Http.Routing
{


    public class RouteTree
    {
        private readonly IDictionary<string, Node> _all = new Dictionary<string, Node>();
        private readonly IDictionary<string, Route> _leaves = new Dictionary<string, Route>();
        private readonly Node _root;
        private Route _home;

        public RouteTree(string verb)
        {
            HttpVerb = verb;
            _root = new Node("");
            _all.Add(string.Empty, _root);
        }

        public string HttpVerb { get; }

        public void AddRoute(Route route)
        {
            if (string.IsNullOrEmpty(route.Pattern))
            {
                _home = route;
            }

            _leaves.SmartAdd(route.Pattern, route);
            var node = getNode(route.NodePath);
            node.AddLeaf(route);
        }

        private Node getNode(string nodePath)
        {
            if (_all.ContainsKey(nodePath)) return _all[nodePath];

            var node = new Node(nodePath);
            _all.Add(node.Route, node);

            if (node.ParentRoute != null)
            {
                var parent = getNode(node.ParentRoute);
                parent.AddChild(node);
            }



            return node;
        }

        public Route Select(string route)
        {
            if (string.IsNullOrEmpty(route.Trim())) return _home;

            var segments = ToSegments(route);

            return Select(segments);
        }

        public Route Select(string[] segments)
        {
            return _root.Select(segments, 0);
        }

        public static string[] ToSegments(string route)
        {
            return route.Trim().TrimStart('/').TrimEnd('/').Split('/');
        }

    }
}
