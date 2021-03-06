﻿Feature: Item purchase process
	As a registered user
	I want to order multiple items
	And I want to add a comment to my order

Background: 
	Given I have logged in to the site with email "greigbaird@techtest.com" and password "BJSSTest"

@purchase
Scenario: Customer purchases 2 items
	And I navigate to the "women" navbar item
	And I quick view product "3"
	Then I should see the product details for product "3"
	When I add that product to my cart with the following details:
			| Order Amendments |  Value |
			| quantity         |	1   |
			| size             |    M   | 
	Then I should see a confirmation popup
	And I should see the size as "M" on the confirmation popup
	And I continue shopping 
	And I quick view product "1"
	Then I should see the product details for product "1"
	When I add that product to my cart with the following details:
			| Order Amendments |  Value |
			| quantity         |	1   |
			| size             |    S   | 
	Then I should see a confirmation popup
	And I should see the size as "S" on the confirmation popup
	And I proceed to checkout
	And the details for all orders should be correct in the cart 
	And I proceed to payment and complete my order

@purchase
Scenario: Customer reviews previous order and adds message
	And I go to my order history
	And I select my most recent order and view its details
	When I add a message to item "1" in the order
	Then I should see that message has been added


@purchase
Scenario: Customer reviews previous order and adds message to invalid product
	And I go to my order history
	And I select my most recent order and view its details
	When I add a message to item "7" in the order
	Then I should see that message has been added