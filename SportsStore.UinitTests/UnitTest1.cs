﻿using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using SportsStore.Domain.Abstract;
using SportsStore.Domain.Concrete;
using SportsStore.Domain.Entities;
using System.Linq;
using System.Collections.Generic;
using SportsStore.WebUI.Controllers;
using System.Web.Mvc;
using SportsStore.WebUI.Models;
using SportsStore.WebUI.HtmlHelpers;

namespace SportsStore.UinitTests
{
    [TestClass]
    public class UnitTest1
    {
        [TestMethod]
        public void Can_Paginate()
        {
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(o => o.Products).Returns(new Product[] {
                new Product { ProductID=1,Name="P1"},
                new Product { ProductID=2,Name="P2"},
                new Product { ProductID=3,Name="P3"},
                new Product { ProductID=4,Name="P4"},
                new Product { ProductID=5,Name="P5"},
                new Product { ProductID=6,Name="P6"}
            });
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            ProductsListViewModel result = (ProductsListViewModel)controller.List(2).Model;

            Product[] prodArray = result.Products.ToArray(); 
            Assert.IsTrue(prodArray.Length == 2);
            Assert.AreEqual(prodArray[0].Name, "P4");
            Assert.AreEqual(prodArray[1].Name, "P5");
        }
        [TestMethod]
        public void Can_Generate_Page_Links()
        {
            HtmlHelper myHelper = null;

            PagingInfo pagingInfo = new PagingInfo
            {
                CurrentPage=2,
                TotalItems=28,
                ItemsPerPage=10,
            };

            Func<int, string> pageUrlDelegate = i => "page" + i;

            MvcHtmlString result = myHelper.PageLinks(pagingInfo, pageUrlDelegate);

            Assert.AreEqual(@"<a class=""btn btn-default"" href=""Page1"">1</a>"+@"<a class=""btn btn-default btn-primary selected"" href=""Page1"">2</a>"+ @"<a class=""btn btn-default"" href=""Page3"">3</a>", result.ToString());
        }
        [TestMethod]
        public void Can_Send_Pagination_View_Model()
        {
            Mock<IProductsRepository> mock = new Mock<IProductsRepository>();
            mock.Setup(o => o.Products).Returns(new Product[] {
                new Product { ProductID=1,Name="P1"},
                new Product { ProductID=2,Name="P2"},
                new Product { ProductID=3,Name="P3"},
                new Product { ProductID=4,Name="P4"},
                new Product { ProductID=5,Name="P5"},
                new Product { ProductID=6,Name="P6"}
            });
            ProductController controller = new ProductController(mock.Object);
            controller.PageSize = 3;

            ProductsListViewModel result = (ProductsListViewModel)controller.List(2).Model;

            PagingInfo pageInfo = result.PagingInfo;
            Assert.AreEqual(pageInfo.CurrentPage, 2);
            Assert.AreEqual(pageInfo.ItemsPerPage, 3);
            Assert.AreEqual(pageInfo.TotalItems,5);
            Assert.AreEqual(pageInfo.TotalPages, 2);
        }
    }
}
