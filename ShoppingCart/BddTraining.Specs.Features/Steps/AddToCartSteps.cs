﻿using System.Linq;
using BddTraining.Common;
using BddTraining.DomainModel;
using BddTraining.DomainModel.Commands;
using BddTraining.Specs.Features.Steps.Utility;
using BddTraining.RequestHandlers.Interfaces;
using FluentAssertions;
using TechTalk.SpecFlow;
using TechTalk.SpecFlow.Assist;

namespace BddTraining.Specs.Features.Steps
{
    [Binding]
    public class AddToCartSteps
    {
        private Product _product;
        private ShoppingCart _shoppingCart;

        [Given(@"I have the following product:")]
        public void GivenIHaveTheFollowingProduct(Table table)
        {
            var command = table.CreateInstance<CreateProductCmd>();
            var createProductCmdHandler = DependencyResolver.Resolve<ICreateProductCmdHandler>();
            _product = createProductCmdHandler.Handle(command);
        }
        
        [When(@"I add the product to my cart")]
        public void WhenIAddTheProductToMyCart()
        {
            var cmdHandler = DependencyResolver.Resolve<IAddToCartCmdHandler>();
            var addToCartCmd = new AddToCartCmd(null, _product.ID, 1);
            _shoppingCart = cmdHandler.Handle(addToCartCmd);
        }
        
        [Then(@"My cart item should look like the following:")]
        public void ThenMyCartItemShouldLookLikeTheFollowing(Table table)
        {
            var expectedItem = table.CreateInstance<CartItemInput>();
            var cartItem = _shoppingCart.CartItems.First();

            cartItem.Name.ShouldBeEquivalentTo(expectedItem.Name);
            cartItem.Price.ShouldBeEquivalentTo(expectedItem.Price);
            cartItem.Quantity.ShouldBeEquivalentTo(expectedItem.Quantity);
        }
    }
}
