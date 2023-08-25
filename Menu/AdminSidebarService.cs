using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;
using Microsoft.AspNetCore.Mvc.Routing;

namespace appmvcnet.Menu
{
    public class AdminSidebarService
    {
        private readonly IUrlHelper UrlHelper;
        public List<SidebarItem> Items { get; set; } = new List<SidebarItem>();

        public AdminSidebarService(IUrlHelperFactory factory, IActionContextAccessor action)
        {
            UrlHelper = factory.GetUrlHelper(action.ActionContext);
            Items.Add(new SidebarItem() { Type = SidebarItemType.Divider });
            Items.Add(new SidebarItem() { Type = SidebarItemType.Heading, Title = "General" });

            Items.Add(new SidebarItem()
            {
                Type = SidebarItemType.NavItem,
                Controller = "DbManage",
                Action = "Index",
                Area = "Database",
                Title = "Manage Database",
                AwesomeIcon = "fas fa-database"
            });
            Items.Add(new SidebarItem() { Type = SidebarItemType.Divider });

            Items.Add(new SidebarItem()
            {
                Type = SidebarItemType.NavItem,
                Title = "Roles & Members",
                AwesomeIcon = "far fa-folder",
                collapseID = "role",
                Items = new List<SidebarItem>() {
                        new SidebarItem() {
                                Type = SidebarItemType.NavItem,
                                Controller = "Role",
                                Action = "Index",
                                Area = "Identity",
                                Title = "Roles"
                        },
                         new SidebarItem() {
                                Type = SidebarItemType.NavItem,
                                Controller = "Role",
                                Action = "Create",
                                Area = "Identity",
                                Title = "Create Role"
                        },
                        new SidebarItem() {
                                Type = SidebarItemType.NavItem,
                                Controller = "User",
                                Action = "Index",
                                Area = "Identity",
                                Title = "Members"
                        },
                    },
            });
            Items.Add(new SidebarItem() { Type = SidebarItemType.Divider });

            Items.Add(new SidebarItem()
            {
                Type = SidebarItemType.NavItem,
                Title = "Manage posts",
                AwesomeIcon = "far fa-folder",
                collapseID = "blog",
                Items = new List<SidebarItem>() {
                        new SidebarItem() {
                                Type = SidebarItemType.NavItem,
                                Controller = "Category",
                                Action = "Index",
                                Area = "Blog",
                                Title = "Categories"
                        },
                         new SidebarItem() {
                                Type = SidebarItemType.NavItem,
                                Controller = "Category",
                                Action = "Create",
                                Area = "Blog",
                                Title = "Create category"
                        },
                        new SidebarItem() {
                                Type = SidebarItemType.NavItem,
                                Controller = "Post",
                                Action = "Index",
                                Area = "Blog",
                                Title = "Posts"
                        },
                        new SidebarItem() {
                                Type = SidebarItemType.NavItem,
                                Controller = "Post",
                                Action = "Create",
                                Area = "Blog",
                                Title = "Create post"
                        },
                    },
            });
            Items.Add(new SidebarItem() { Type = SidebarItemType.Divider });
            Items.Add(new SidebarItem()
            {
                Type = SidebarItemType.NavItem,
                Title = "Manage products",
                AwesomeIcon = "far fa-folder",
                collapseID = "product",
                Items = new List<SidebarItem>() {
                        new SidebarItem() {
                                Type = SidebarItemType.NavItem,
                                Controller = "CategoryProduct",
                                Action = "Index",
                                Area = "Product",
                                Title = "Product Categories"
                        },
                         new SidebarItem() {
                                Type = SidebarItemType.NavItem,
                                Controller = "CategoryProduct",
                                Action = "Create",
                                Area = "Product",
                                Title = "Create product category"
                        },
                        new SidebarItem() {
                                Type = SidebarItemType.NavItem,
                                Controller = "ProductManage",
                                Action = "Index",
                                Area = "Product",
                                Title = "Products"
                        },
                        new SidebarItem() {
                                Type = SidebarItemType.NavItem,
                                Controller = "ProductManage",
                                Action = "Create",
                                Area = "Product",
                                Title = "Create product"
                        },
                    },
            });
        }

        public string renderHtml()
        {
            var html = new StringBuilder();

            foreach (var item in Items)
            {
                html.Append(item.RenderHtml(UrlHelper));
            }

            return html.ToString();
        }

        public void SetActive(string Controller, string Action, string Area)
        {
            foreach (var item in Items)
            {
                if (item.Controller == Controller && item.Action == Action && item.Area == Area)
                {
                    item.IsActive = true;
                    return;
                }
                else
                {
                    if (item.Items != null)
                    {
                        foreach (var children in item.Items)
                        {
                            if ((children.Controller == Controller) && (children.Action == Action) && (children.Area == Area))
                            {
                                children.IsActive = true;
                                item.IsActive = true;
                                return;
                            }
                        }
                    }
                }
            }
        }
    }
}