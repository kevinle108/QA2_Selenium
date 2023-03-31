# **QA Analyst | Course 2** | Final Project

### Automated UI Testing - C#

### Website: [4qrcode.com](https://4qrcode.com/)

A website that lets you generate and scan QR codes for various purposes (ie. links, texts, events, etc.)

## Test Results
![Test Results](https://github.com/kevinle108/QA2_Selenium/blob/master/tests_passed.png)

## Project Requirements

Create a suite of automated UI tests for publicly accessible web applications. You may use any online web application you choose, and you are not required to write all of your tests against the same application. You may write your tests in the same project as your API tests from the midterm, or you may create a new one. You may use any Nuget packages, test frameworks, or assertion libraries you choose, but your solution must be in C#. Create at least 15 automated UI tests. Your tests should each be unique and test a different use case of the web application. Your tests should also cover all of the following criteria:

- [x] 1. Each test must contain an assertion.

- [x] 2. Include at least 1 negative test.
    - `Negative_Test_Incomplete_Event()` & `Negative_Test_Phone_Number_Rejects_Letters()`

- [x] 3. Include at least one test for each of the following complex page controls: 

- [x] - A file upload
    - `File_Uploads_Successfully()` & `Generate_Download_And_Verify_QR_Code()`

- [x] - A date picker
    - `Date_Picker_Widget_Input()` & `Generate_Download_And_Verify_QR_Code()`

- [x] - A hover-over or tooltip
    - `Tooltip_Appears_On_Hover()`

- [x] 4. Include at least one test that performs an action that opens a new tab or window, and then continues the test in that new tab or window.
    - `ScannerLink_Opens_In_NewTab()` & `Generate_Download_And_Verify_QR_Code()`
- [x] 5. Utilize a Page Object Model. Create at least 3 Page Object Model classes that represent web pages under test.
    - `HomePage.cs`, `ContactPage.cs`, and `ScanPage.cs`

Ensure that at the end of your tests, any and all open browser windows or sessions are closed and disposed of properly.

Build your code and run your tests. All tests should run and all assertions should pass in order to receive credit. If your code requires any setup in order to build or run your tests, include a readme file with instructions to build and run your code. Check your completed tests into a GitHub repository, and submit that link as your turn-in.
