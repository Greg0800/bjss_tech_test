Feature: API calls to rest interface
@apiScenario: API get call of user details
	Given I want to retrieve user "2"
	Then the first name should be "Janet"
	And the last name should be "Weaver"
	And the id should be "2"

@api
Scenario: API create call for user
	Given I want to create a user with the following details:
			| input fields | Value               |
			| name         | Harold              |
			| job          | Fireworks operative |
	Then the response details should match

@api
Scenario: API update call to update user details
	Given I want to update user "7" with the following details:
			| input fields | Value               |
			| name         | Barney              |
			| job          | Fireworks Engineer  |
	Then the response details should match

@api
Scenario: API delete call to remove user
	Given I want to remove user "7" from the system
	Then the call to get user "7" details should fail


	



