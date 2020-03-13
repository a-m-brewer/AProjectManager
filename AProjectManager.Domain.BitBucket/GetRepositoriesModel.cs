using System;
using System.Collections.Generic;

namespace AProjectManager.Domain.BitBucket
{
    public class Clone
    {
        public string Href { get; set; }
        public string Name { get; set; }
    }

    public class Watchers
    {
        public string Href { get; set; }
    }

    public class Branches
    {
        public string Href { get; set; }
    }

    public class Tags
    {
        public string Href { get; set; }
    }

    public class Commits
    {
        public string Href { get; set; }
    }

    public class Downloads
    {
        public string Href { get; set; }
    }

    public class Source
    {
        public string Href { get; set; }
    }

    public class Html
    {
        public string Href { get; set; }
    }

    public class Avatar
    {
        public string Href { get; set; }
    }

    public class Hooks
    {
        public string Href { get; set; }
    }

    public class Forks
    {
        public string Href { get; set; }
    }

    public class Self
    {
        public string Href { get; set; }
    }

    public class PullRequests
    {
        public string Href { get; set; }
    }

    public class Links
    {
        public List<Clone> Clone { get; set; }
        public Watchers Watchers { get; set; }
        public Branches Branches { get; set; }
        public Tags Tags { get; set; }
        public Commits Commits { get; set; }
        public Downloads Downloads { get; set; }
        public Source Source { get; set; }
        public Html Html { get; set; }
        public Avatar Avatar { get; set; }
        public Hooks Hooks { get; set; }
        public Forks Forks { get; set; }
        public Self Self { get; set; }
        public PullRequests PullRequests { get; set; }
    }

    public class Self2
    {
        public string Href { get; set; }
    }

    public class Html2
    {
        public string Href { get; set; }
    }

    public class Avatar2
    {
        public string Href { get; set; }
    }

    public class Links2
    {
        public Self2 Self { get; set; }
        public Html2 Html { get; set; }
        public Avatar2 Avatar { get; set; }
    }

    public class Project
    {
        public Links2 Links { get; set; }
        public string Type { get; set; }
        public string Uuid { get; set; }
        public string Key { get; set; }
        public string Name { get; set; }
    }

    public class Mainbranch
    {
        public string Type { get; set; }
        public string Name { get; set; }
    }

    public class Self3
    {
        public string Href { get; set; }
    }

    public class Html3
    {
        public string Href { get; set; }
    }

    public class Avatar3
    {
        public string Href { get; set; }
    }

    public class Links3
    {
        public Self3 Self { get; set; }
        public Html3 Html { get; set; }
        public Avatar3 Avatar { get; set; }
    }

    public class Owner
    {
        public string Username { get; set; }
        public string Type { get; set; }
        public string DisplayName { get; set; }
        public string Uuid { get; set; }
        public Links3 Links { get; set; }
    }

    public class Self4
    {
        public string Href { get; set; }
    }

    public class Html4
    {
        public string Href { get; set; }
    }

    public class Avatar4
    {
        public string Href { get; set; }
    }

    public class Links4
    {
        public Self4 Self { get; set; }
        public Html4 Html { get; set; }
        public Avatar4 Avatar { get; set; }
    }

    public class Parent
    {
        public string Name { get; set; }
        public string Type { get; set; }
        public string FullName { get; set; }
        public Links4 Links { get; set; }
        public string Uuid { get; set; }
    }

    public class ApiRepo
    {
        public string Scm { get; set; }
        public string Website { get; set; }
        public bool HasWiki { get; set; }
        public string Uuid { get; set; }
        public Links Links { get; set; }
        public string Description { get; set; }
        public string ForkPolicy { get; set; }
        public string Name { get; set; }
        public Project Project { get; set; }
        public string Language { get; set; }
        public DateTime CreatedOn { get; set; }
        public Mainbranch MainBranch { get; set; }
        public string FullName { get; set; }
        public DateTime UpdatedOn { get; set; }
        public Owner Owner { get; set; }
        public bool HasIssues { get; set; }
        public string Type { get; set; }
        public string Slug { get; set; }
        public bool IsPrivate { get; set; }
        public int Size { get; set; }
        public Parent Parent { get; set; }
    }

    public class GetRepositoriesModel
    {
        public int PageLength { get; set; }
        public string Next { get; set; }
        public List<ApiRepo> Values { get; set; }
        public int Page { get; set; }
        public int Size { get; set; }
    }
}