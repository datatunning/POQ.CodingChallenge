# Backend WebAPI

Create a .NET Core solution to contain an HTTP endpoint that:

## Instructions

1. Accepts a GET request with three optional query parameters to filter products or
highlight some words (separated by commas in the query parameter) in their description:
Example: /filter?maxprice=20&size=medium&highlight=green,blue

2. Reads the list of all products from the URL (think of this as the database):
<http://www.mocky.io/v2/5e307edf3200005d00858b49>

3. Design the endpoint response so that it contains (in JSON format):
- All products if the request has no parameters
- A filtered subset of products if the request has filter parameters
- A filter object to contain:
  - The minimum and the maximum price of all products in the source URL
  - An array of strings to contain all sizes of all products in the source URL
  - An string array of size ten to contain most common words in the product
descriptions, excluding the most common five
- Add HTML tags to returned product descriptions in order to highlight the words
provided in the highlight parameter.

>Example: “These trousers make a perfect pair with <em>green</em> or <em>blue</em> shirts.”

## What we will look for

* Clean, readable, easy-to-understand code
* Performance, scalability, and security
* Unit tests
* Dependency injection
* Appropriate logging including the full mocky.io response
* Documentation for the users of your API

Hints
* You do not need to implement authorization but look for other potential security
*vulnerabilities
* Feel free to use any open-source libraries
* Feel free to leave notes as code comments
How To Submit
* You should only spend a few hours completing the assignment
* Have the solution ready to compile and run
* Avoid including artifacts (NuGet packages, bin folders) from your local build
* Compress the solution and send us a zip file.
