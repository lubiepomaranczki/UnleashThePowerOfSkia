using System;

namespace UnleashThePowerOfSkia.Models
{
    public struct PageItem
    {
        public PageItem(string displayName, Type pageType)
        {
            DisplayName = displayName;
            PageType = pageType;
        }
        
        public string DisplayName { get; }
        
        public Type PageType { get; }
    }
}